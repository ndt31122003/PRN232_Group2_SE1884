using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using PRN232_EbayClone.Application.Research.Abstractions;
using PRN232_EbayClone.Application.Research.Dtos;

namespace PRN232_EbayClone.Infrastructure.Research;

internal sealed class InMemorySourcingInsightsRepository : ISourcingInsightsRepository
{
    private static readonly IReadOnlyList<SourcingCategoryDto> Categories = new List<SourcingCategoryDto>
    {
        new("action-figures", "Action Figures", "in Action Figures & Accessories", "Good opportunity", 0.86m, 826_860, 1_181_922, 0.70m, 2.41m, 7, 0.37m, 94.31m),
        new("audio-books", "Audio Books", "in Books, Comics & Magazines", "Good opportunity", 0.74m, 3_994, 112_168, 0.04m, 1.37m, 7, 0.24m, 100m),
        new("bears", "Bears", "in Bears, Clothing & Accessories", "Emerging", 0.72m, 24_459, 45_083, 0.54m, 2.20m, 7, 0.09m, 100m),
        new("belts", "Belts", "in Women's Accessories", "Emerging", 0.69m, 17_231, 314_964, 0.05m, 0.25m, 7, 0.49m, 100m),
        new("cassettes", "Cassettes", "in Music", "Watchlist", 0.65m, 18_348, 111_141, 0.17m, 1.29m, 7, 0m, 92.76m),
        new("casual-shirts", "Casual Shirts", "in Shirts", "Good opportunity", 0.81m, 95_523, 255_677, 0.37m, 1.86m, 7, 0.59m, 98.14m),
        new("contemporary", "Contemporary", "in Sheet Music & Song Books", "Niche", 0.60m, 3_504, 45_384, 0.08m, 1.28m, 9, 0.14m, 100m),
        new("doilies", "Doilies", "in Kitchen Linens & Textiles", "Watchlist", 0.67m, 2_648, 6_306, 0.42m, 4.11m, 6, 0m, 91.27m),
        new("dolls", "Dolls & Doll Playsets", "in Dolls, Clothing & Accessories", "Good opportunity", 0.84m, 290_466, 303_275, 0.96m, 2.57m, 6, 0.16m, 95.45m),
        new("hats-men", "Hats", "in Men's Accessories", "Competitive", 0.61m, 114_209, 592_381, 0.19m, 0.98m, 7, 0.41m, 94.90m),
        new("hats-boys", "Hats", "in Boys' Accessories", "Watchlist", 0.58m, 1_485, 21_643, 0.07m, 0.79m, 7, 0.39m, 100m),
        new("jeans", "Jeans", "in Men's Clothing", "Good opportunity", 0.83m, 47_012, 114_956, 0.41m, 2.27m, 8, 0.33m, 98.29m),
        new("jumpers", "Jumpers", "in Men's Clothing", "Emerging", 0.70m, 33_416, 92_488, 0.36m, 1.23m, 7, 0.08m, 100m),
        new("coffee-machines", "Coffee Machines", "in Small Kitchen Appliances", "Good opportunity", 0.93m, 102_486, 71_224, 1.44m, 4.12m, 7, 0.41m, 96.52m),
        new("smartwatches", "Smart Watches", "in Wearable Technology", "Competitive", 0.63m, 478_230, 561_009, 0.85m, 2.68m, 9, 0.52m, 89.74m)
    };

    private readonly ConcurrentDictionary<Guid, HashSet<string>> _savedByUser = new();

    public Task<IReadOnlyList<SourcingCategoryDto>> GetAllCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Categories);
    }

    public Task<IReadOnlyCollection<string>> GetSavedCategoryIdsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var set = _savedByUser.GetOrAdd(userId, _ => new HashSet<string>(StringComparer.OrdinalIgnoreCase));
        lock (set)
        {
            return Task.FromResult<IReadOnlyCollection<string>>(set.ToList());
        }
    }

    public Task SaveCategoryAsync(Guid userId, string categoryId, CancellationToken cancellationToken = default)
    {
        var set = _savedByUser.GetOrAdd(userId, _ => new HashSet<string>(StringComparer.OrdinalIgnoreCase));
        lock (set)
        {
            if (set.Count >= 150)
            {
                return Task.CompletedTask;
            }

            set.Add(categoryId);
        }

        return Task.CompletedTask;
    }

    public Task RemoveCategoryAsync(Guid userId, string categoryId, CancellationToken cancellationToken = default)
    {
        if (_savedByUser.TryGetValue(userId, out var set))
        {
            lock (set)
            {
                set.Remove(categoryId);
            }
        }

        return Task.CompletedTask;
    }
}
