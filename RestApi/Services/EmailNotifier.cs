using MailKit.Net.Smtp;
using MimeKit;

namespace RestApi.Services;

public class EmailNotifier
{
   private static readonly string? EmailAddress = Environment.GetEnvironmentVariable("EmailAddress");
   private static readonly string? EmailPassword = Environment.GetEnvironmentVariable("EmailPassword");
   private static readonly string SenderName = "GroupSix";
   
   public static bool SendEmail(MimeMessage email)
   {
      if (EmailPassword is null || EmailAddress is null)
      {
         return false;
      }

      email.From.Add(new MailboxAddress(SenderName, EmailAddress));
      using var smtp = new SmtpClient();
      smtp.Connect("smtp.zoho.com", 465, true);
      smtp.Authenticate(EmailAddress, EmailPassword);
      smtp.Send(email);
      smtp.Disconnect(true);

      return true;
   }
}