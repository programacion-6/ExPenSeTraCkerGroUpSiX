using MimeKit;
using RestApi.Services;

namespace RestApi.Tests;

public class EmailTest
{
    [Fact]
    public void SendMeMail()
    {
        var email = new MimeMessage();
        email.To.Add(new MailboxAddress("Tacos", "lynn9316@awgarstone.com"));
        email.Subject = "Testing out email sending";
        email.Body = new TextPart("plain")
        {
            Text = "Hello World!"
        };
        
        Assert.True(EmailNotifier.SendEmail(email));
    }
}