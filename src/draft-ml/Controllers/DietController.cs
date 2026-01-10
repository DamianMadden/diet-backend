using draft_ml.Controllers.Models;
using draft_ml.Data;
using draft_ml.Db;
using draft_ml.Exceptions;
using draft_ml.Extensions;
using draft_ml.Functions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace draft_ml.Controllers
{
    [ApiController]
    [Authorize]
    public class DietController(DietDbContext data, ILogger<DietController> logger) : ControllerBase
    {
        [HttpGet("meals")]
        [ProducesResponseType(typeof(GetMealsResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<GetMealsResponse>> GetDietPlan(
            [FromQuery] Guid planId,
            [FromQuery] int count = 5,
            [FromQuery] int page = 0
        )
        {
            try
            {
                var userId = GetUserId();

                var mealTarget = await data
                    .Users.Where(u => u.Id == userId)
                    .Select(u => u.MealTarget)
                    .SingleOrDefaultAsync();

                if (mealTarget is null)
                {
                    return NotFound();
                }

                var planTarget = await data
                    .Plans.Where(p => p.Id == planId)
                    .Select(p => p.MealTarget)
                    .SingleOrDefaultAsync();

                if (planTarget is null)
                {
                    // Initialise plan
                    return NotFound();
                }

                var targetVector = mealTarget.Add(planTarget);

                var meals = await data.GetMealAsync(targetVector, count, page);

                var response = new GetMealsResponse
                {
                    Meals = meals
                        .Select(m => new MealSummary
                        {
                            Id = m.Id,
                            Name = m.Name,
                            ThumbnailUrl = m.ThumbnailUrl,
                            Description = m.Description,
                        })
                        .ToList(),
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Error pulling next meal options");
            }
        }

        [HttpPost("planmeal")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetMealsResponse>> PostPlanMeal(
            [FromBody] PostPlanMealRequest request
        )
        {
            try
            {
                var userId = GetUserId();

                var selection = await data
                    .Users.Where(u => u.Id == userId)
                    .Select(u => new
                    {
                        u.MealTarget,
                        Plan = u.Plans.FirstOrDefault(p => p.Id == request.PlanId),
                    })
                    .FirstOrDefaultAsync();

                if (selection?.MealTarget is null || selection?.Plan is null)
                {
                    logger.LogError("Plan or Meal Target not found {PlanId}", request.PlanId);
                    return NotFound();
                }

                var plan = selection.Plan;

                var meal = await data.Meals.FindAsync(request.MealId);

                if (meal is null)
                {
                    logger.LogError("Meal not found {MealId}", request.MealId);
                    return NotFound();
                }

                // Record plan meal
                data.PlanMeals.Add(
                    new PlanMeal
                    {
                        PlanId = plan.Id,
                        MealId = request.MealId,
                        Quantity = request.Quantity,
                        // TODO: Meal timings
                        Timestamp = DateTime.Now,
                    }
                );

                // Revise meal target
                plan.MealTarget = selection
                    .MealTarget.Add(plan.MealTarget!)
                    .Subtract(meal.Nutrients);

                data.Plans.Update(plan);

                await data.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Error pulling next meal options");
            }
        }

        [HttpGet("profile")]
        [ProducesResponseType(typeof(UserProfile), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserProfile>> GetProfile()
        {
            try
            {
                var userId = GetUserId();
                var user =
                    await data.Users.FindAsync(userId)
                    ?? throw new NotFoundException("User not found");

                return Ok(
                    new UserProfile
                    {
                        PlanId = user.PrimaryPlanId,
                        Height = user.Height,
                        Weight = user.Weight,
                        Gender = user.Gender,
                        DateOfBirth = user.DateOfBirth,
                        ActivityLevel = user.ActivityLevel,
                        Goal = user.Goal,
                    }
                );
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Error pulling next meal options");
            }
        }

        [HttpPost("profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserProfile>> SetProfile([FromBody] UserProfile request)
        {
            try
            {
                var userId = GetUserId();
                var user =
                    await data.Users.FirstOrDefaultAsync(u => u.Id == userId)
                    ?? throw new NotFoundException("User not found");

                user.Height = request.Height;
                user.Weight = request.Weight;
                user.Gender = request.Gender;
                user.DateOfBirth = request.DateOfBirth;
                user.ActivityLevel = request.ActivityLevel;
                user.Goal = request.Goal;
                user.ActivityResistanceCoefficient = request.ActivityResistanceCoefficient;

                // Calculate user's meal target
                user.MealTarget = DietFunctions.calculateMealTarget(user, DateTime.UtcNow);

                if (request.PlanId is null)
                {
                    request.PlanId = Guid.NewGuid();
                    data.Add(
                        new Plan
                        {
                            Id = (Guid)request.PlanId,
                            Name = "primary",
                            Meals = [],
                            MealTarget = user.MealTarget,
                        }
                    );

                    data.Add(new UserPlan { UserId = userId, PlanId = (Guid)request.PlanId });

                    user.PrimaryPlanId = (Guid)request.PlanId;
                }

                await data.SaveChangesAsync();

                return Ok(new { plan_id = (Guid)request.PlanId });
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Error pulling next meal options");
            }
        }

        // TODO: Consider removing due to new design
        [HttpPost("DietPlan")]
        [ProducesResponseType(typeof(GetDietPlanResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<GetDietPlanResponse>> GetDietPlan(
            [FromBody] GetDietPlanRequest request
        )
        {
            Vector targetVector = new Vector(new ReadOnlyMemory<float>(request.Target));

            try
            {
                CyclePlan plan = new CyclePlan(data);
                var dayPlans = await plan.PlanCycle(
                    targetVector,
                    request.CycleLength,
                    request.Meals,
                    request.Snacks
                );

                return Ok(new GetDietPlanResponse { DayPlans = dayPlans });
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Error constructing diet plan");
            }
        }

        private Guid GetUserId()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("id"))?.Value;

            if (Guid.TryParse(userId, out var userGuid))
            {
                return userGuid;
            }

            throw new InvalidClaimsException("User ID is missing or invalid");
        }

        private ActionResult HandleException(Exception ex, string logMessage)
        {
            logger.LogError(ex, logMessage);

            return ex switch
            {
                InvalidClaimsException => Unauthorized(new { error = ex.Message }),
                NotFoundException => NotFound(new { error = ex.Message }),
                _ => Problem(ex.Message, null, 500),
            };
        }
    }
}
