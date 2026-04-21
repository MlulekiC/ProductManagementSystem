using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Utilities
{
    public static class Email
    {
        public static async Task<Boolean> SendEmailAsync(string URL, string username)
        {
            bool sent = false;
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("ProductMS Support", "MlulekiM2000@gmail.com"));
            message.To.Add(new MailboxAddress("Recipient Name", username));
            message.Subject = "Email Confirmation for ProductMS";

            message.Body = new TextPart("plain")
            {
                Text = $"Hi {message.To}.\n\n" +
                $"Please click on the following link to finish setting up you ProductMS Account: {URL}.\n\n" +
                $"Best Regards,\n" +
                $"ProductMS Support Team"
            };

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

                // Authenticate google credentials
                await client.AuthenticateAsync("MlulekiM2000@gmail.com", "jquq xwtp qesp dusd");

                string str = await client.SendAsync(message);
                if (str.Contains("2.0.0 OK"))
                {
                    sent = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
            return sent;
        }
    }
}