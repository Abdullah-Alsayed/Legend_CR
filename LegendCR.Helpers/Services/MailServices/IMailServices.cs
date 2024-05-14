namespace LegendCR.Helpers.Services.MailServices;

public interface IMailServices
{
    Task<bool> SendAsync(EmailDto request);
}
