namespace PRN232_EbayClone.Application.Abstractions.Mail;

public interface ITemplateRenderer
{
    Task<string> RenderAsync(string templateName, object model, CancellationToken cancellationToken = default);
}
