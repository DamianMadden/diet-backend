using draft_ml.Controllers.Models;
using draft_ml.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace draft_ml.Controllers
{
    [ApiController]
    public class DietPlanController(IDietDataService data, ILogger<DietPlanController> logger) : ControllerBase
    {
        [HttpPost("DietPlan")]
        [ProducesResponseType(typeof(GetDietPlanResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<GetDietPlanResponse>> GetDietPlan([FromBody] GetDietPlanRequest request)
        {
            Vector targetVector = new Vector(new ReadOnlyMemory<float>(request.Target));

            try
            {
                CyclePlan plan = new CyclePlan(data);
                var dayPlans = await plan.PlanCycle(targetVector, request.CycleLength, request.Meals, request.Snacks);

                return Ok(new GetDietPlanResponse
                {
                    DayPlans = dayPlans
                });
            }
            catch (Exception ex)
            {
                // TODO: More specificity
                // return Problem(ex.Message, null, 500, null, null);
                throw;
            }
        }
    }
}
