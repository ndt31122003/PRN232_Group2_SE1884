using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Categories.Entities;

public sealed class CategorySpecific(Guid id) : Entity<Guid>(id)
{
    private CategorySpecific(
       string name,
       bool isRequired,
       bool allowMultiple,
       HashSet<string> values) : this(Guid.NewGuid())
    {
        Name = name;
        IsRequired = isRequired;
        AllowMultiple = allowMultiple;
        _values = values;
    }
    public string Name { get; private set; } = null!;
    public bool IsRequired { get; private set; } = false;
    public bool AllowMultiple { get; private set; } = false;

    private readonly HashSet<string> _values = [];
    public IReadOnlyCollection<string> Values => _values;

    public static Result<CategorySpecific> Create(
        string name,
        bool isRequired,
        bool allowMultiple,
        IEnumerable<string> values)
    {
        return new CategorySpecific(name, isRequired, allowMultiple, values.ToHashSet());
    }
}