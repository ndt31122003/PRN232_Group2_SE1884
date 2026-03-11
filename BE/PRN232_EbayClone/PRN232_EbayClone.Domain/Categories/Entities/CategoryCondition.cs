using PRN232_EbayClone.Domain.Conditions.Entities;

namespace PRN232_EbayClone.Domain.Categories.Entities;

public sealed class CategoryCondition
{
    public Guid CategoryId { get; private set; }
    public Guid ConditionId { get; private set; }
    public Condition Condition { get; private set; } = null!;
    private CategoryCondition() { }
    public CategoryCondition(Guid categoryId, Guid conditionId)
    {
        CategoryId = categoryId;
        ConditionId = conditionId;
    }
}
