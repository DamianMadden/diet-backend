namespace draft_ml.UnitTests;

public class CyclePlanTests(CyclePlanFixture fixture) : IClassFixture<CyclePlanFixture>
{
    [Fact]
    public async Task When_Cycle_Planned_Then_MealCountCorrect()
    {
        var targetVec = new Vector(new ReadOnlyMemory<float>([20.5f, 15.6f, 16.7f]));
        var plan = await fixture._cyclePlan.PlanCycle(targetVec, 1, 2, 0);
        Assert.Equal(2, plan[0].Meals.Count());
    }
}
