using EfiritPro.Retail.Packages.Errors.Models;
using Models;
using Persistence;
using System.Net.Mail;

namespace EmailTestModule.Services;

public class ResetPasswordEmailService
{
    private readonly EmailTestDbContext _emailDb;
    private readonly string _defaultSenderEmail;
    private readonly string _defaultSenderPassword;

    public ResetPasswordEmailService(EmailTestDbContext emailDb)
    {
        _emailDb = emailDb;

        _defaultSenderEmail = Environment.GetEnvironmentVariable("DEFAULT_SENDER_EMAIL") ??
                              throw new InvalidOperationException($"string {nameof(_defaultSenderEmail).ToUpper()} not found.");
        _defaultSenderPassword = Environment.GetEnvironmentVariable("DEFAULT_SENDER_PASSWROD") ??
                              throw new InvalidOperationException($"string {nameof(_defaultSenderPassword).ToUpper()} not found.");
    }

    public async Task<ServiceAnswer<ResetPasswordMessage>> Send(string? to, string msg)
    {
        string recipient = to ?? _defaultSenderEmail;

        if(string.IsNullOrEmpty(msg))
        {
            return new ServiceAnswer<ResetPasswordMessage>()
            {
                Ok = false,
                Errors = new[] { 
                    new ServiceFieldError()
                    {
                        Fields = new[] { "message" },
                        Message = "Сообщение не должно быть пустым."
                    } 
                }
            };
        }

        return await Send(recipient: recipient, msg);
    }

    public async Task<ServiceAnswer<ResetPasswordMessage>> Send(string recipient, string msg, string empty = "")
    {
        ResetPasswordMessage message;

        try
        {
            SmtpClient mySmtpClient = new SmtpClient("smtp.mail.ru");

            mySmtpClient.UseDefaultCredentials = false;
            System.Net.NetworkCredential basicAuthenticationInfo = new
               System.Net.NetworkCredential(_defaultSenderEmail, _defaultSenderPassword);

            mySmtpClient.Credentials = basicAuthenticationInfo;
            mySmtpClient.EnableSsl = true;

            // add from,to mailaddresses
            MailAddress from = new MailAddress("tester.efir.it@mail.ru", "Efirit team");
            MailAddress sender = new MailAddress(recipient, recipient);
            MailMessage myMail = new System.Net.Mail.MailMessage(from, sender);

            // add ReplyTo
            MailAddress replyTo = new MailAddress("reply@example.com");
            myMail.ReplyToList.Add(replyTo);

            // set subject and encoding
            myMail.Subject = "Test message";
            myMail.SubjectEncoding = System.Text.Encoding.UTF8;

            // set body-message and encoding
            myMail.Body = $"<b>{msg}</b>.";
            myMail.BodyEncoding = System.Text.Encoding.UTF8;
            // text or html
            myMail.IsBodyHtml = true;

            mySmtpClient.Send(myMail);

            message = new ResetPasswordMessage()
            {
                To = recipient,
                Date = DateTime.UtcNow
            };

            _emailDb.ResetPasswordMessages.Add(message);
            await _emailDb.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return new ServiceAnswer<ResetPasswordMessage>()
            { 
                Ok = false,
                Errors = new[] { ex.Message }
            };
        }

        return new ServiceAnswer<ResetPasswordMessage>()
        {
            Ok = true,
            Answer = message
        };
    }

    //public async Task<ServiceAnswer<CacheMessage>> WriteInCache(ResetPasswordMessage email)
    //{

    //}
}
