namespace PRN232_EbayClone.Infrastructure.Mail;

public sealed class EmailConfiguration
{
    public required string SmtpServer { get; set; }
    public required int SmtpPort { get; set; }
    public required string SenderName { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}
