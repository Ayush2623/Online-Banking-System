namespace BussinessLayer.Interfaces
{
    public interface ISmtpService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}