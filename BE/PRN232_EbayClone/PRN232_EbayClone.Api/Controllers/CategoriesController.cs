using PRN232_EbayClone.Application.Categories.Queries;

namespace PRN232_EbayClone.Api.Controllers;

[Route("api/categories")]
public sealed class CategoriesController(ISender sender) : ApiController(sender)
{
    [HttpGet]
    public Task<IActionResult> GetCategories(Guid? parentId,CancellationToken cancellationToken)
        => SendAsync(new GetCategoriesQuery(parentId), cancellationToken);

    [HttpGet("{id:guid}")]
    public Task<IActionResult> GetCategoryDetail(Guid id, CancellationToken cancellationToken)
        => SendAsync(new GetCategoryDetailQuery(id), cancellationToken);
}
