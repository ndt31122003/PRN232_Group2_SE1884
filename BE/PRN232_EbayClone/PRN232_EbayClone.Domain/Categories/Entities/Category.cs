using PRN232_EbayClone.Domain.Conditions.Entities;
using PRN232_EbayClone.Domain.Listings.ValueObjects;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Categories.Entities;

public sealed class Category(Guid id) : AggregateRoot<Guid>(id)
{
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public Guid? ParentId { get; private set; } = null;
    public bool IsLeaf => _children.Count == 0;

    private readonly HashSet<Category> _children = [];
    public IReadOnlyCollection<Category> Children => _children;

    private readonly HashSet<CategorySpecific> _categorySpecifics = [];
    public IReadOnlyCollection<CategorySpecific> CategorySpecifics => _categorySpecifics;

    private readonly HashSet<CategoryCondition> _categoryConditions = [];
    public IReadOnlyCollection<CategoryCondition> CategoryConditions => _categoryConditions;

    public static Result<Category> Create(
        string name,
        string description,
        List<CategorySpecific>? categorySpecifics = default)
    {
        var category = new Category(Guid.NewGuid())
        {
            Name = name,
            Description = description
        };
        if (categorySpecifics is not null)
        {
            foreach (var categorySpecific in categorySpecifics)
            {
                category.AddCategorySpecific(categorySpecific);
            }
        }
        return Result.Success(category);
    }

    public Result AddCategorySpecific(CategorySpecific categorySpecific)
    {
        _categorySpecifics.Add(categorySpecific);
        return Result.Success();
    }

    public Result AddCondition(Condition condition)
    {
        _categoryConditions.Add(new CategoryCondition(Id, condition.Id));
        return Result.Success();
    }

    public static Result ValidateSpecifics<TSpecific>(
        IEnumerable<TSpecific> requestSpecifics,
        IEnumerable<CategorySpecific> categorySpecifics)
    {
        var context = typeof(TSpecific).Name;
        foreach (var catSpec in categorySpecifics)
        {
            var match = requestSpecifics.FirstOrDefault(x =>
                x switch
                {
                    ItemSpecific i => i.Name == catSpec.Name,
                    VariationSpecific v => v.Name == catSpec.Name,
                    _ => false
                });

            if (catSpec.IsRequired && match is null)
            {
                return Error.Failure(
                    $"{context}.Required",
                    $"{context} '{catSpec.Name}' is required."
                );
            }

            if (match is not null)
            {
                var values = match switch
                {
                    ItemSpecific i => i.Values ?? [],
                    VariationSpecific v => v.Values ?? [],
                    _ => []
                };

                if (!catSpec.AllowMultiple && values.Count() > 1)
                {
                    return Error.Failure(
                        $"{context}.SingleValue",
                        $"{context} '{catSpec.Name}' allows only a single value."
                    );
                }
            }
        }
        return Result.Success();
    }
}