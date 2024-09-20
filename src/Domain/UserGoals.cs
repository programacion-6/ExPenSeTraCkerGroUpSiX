namespace ExpenseTrackerGrup6.src.Domain;

public class UserGoals
{
    public required Guid UserId { get; set; }
    public required decimal ExpectBudgeting { get; set; }
    public required decimal ActualBudgeting { get; set; }
    public required decimal ExpectIncomeGoal { get; set; }
    public required decimal ActualIncomeGoal { get; set; }
}
