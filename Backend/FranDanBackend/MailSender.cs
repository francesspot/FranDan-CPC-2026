using FranDanBackend.Models;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
namespace FranDanBackend
{
    public class LoginInfo
    {
        public string email { get; set; }
        public string password { get; set; }
        public LoginInfo() { }
        public static LoginInfo getFromConfig(string configPath)
        {
            string json;
            using (StreamReader r = new StreamReader(configPath))
            {
                json = r.ReadToEnd();
            }
            LoginInfo? loginInfo = JsonSerializer.Deserialize<LoginInfo>(json);
            if (loginInfo == null)
            {
                throw new Exception("Could not load email config.");
            }
            return loginInfo;
        }
        public MailAddress getEmail()
        {
            return new MailAddress(email, "FranDan CPC 2026");
        }

    }
    public static class MailSender
    {
        private static LoginInfo sendingMail = LoginInfo.getFromConfig("config.json");
        public static void sendCode(User user)
        {
            //sendingMail = LoginInfo.getFromConfig("config.json");
            MailMessage message = new MailMessage();
            message.From = sendingMail.getEmail();
            message.To.Add(user.email);
            message.Subject = $"Verification code for FranDan.";
            message.Body = $"Hello {user.username},\n" +
                $"We are sending you verification code:\n" +
                $"{user.verifier.code}\n" +
                $"Paste it on our website to verify email.\n" +
                "This email was generated automatically. Don't answear.\n" +
                "FranDan team\n";
            message.IsBodyHtml = false;
            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(sendingMail.email, sendingMail.password);
                try
                {
                    client.Send(message);
                    Console.WriteLine("Yes!");
                }
                catch (Exception ex)
                {
                    throw new Exception("An error accured while sending email.");
                }
            }
        }
        public static void sendFriendRequest(User fromUser, User toUser)
        {
            MailMessage message = new MailMessage();
            message.From = sendingMail.getEmail();
            message.To.Add(toUser.email);
            message.Subject = $"You have new friend invitation from {fromUser.username}.";
            message.Body = $"Hello {toUser.username},\n" +
                $"{fromUser.username} send you a friend invitation.\n" +
                "Log in to accept or reject it.\n" +
                "This email was generated automatically.\n" +
                " Don't answear.\n" +
                "FranDan team\n";
            message.IsBodyHtml = false;
            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(sendingMail.email, sendingMail.password);
                try
                {
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    throw new Exception("An error accured while sending email.");
                }
            }
        }
        public static void sendPlanRequest(User fromUser, User toUser, Plan plan)
        {
            MailMessage message = new MailMessage();
            message.From = sendingMail.getEmail();
            message.To.Add(toUser.email);
            message.Subject = $"You have recived an invitation to join plan {plan.title} from {fromUser.username}.";
            message.Body = $"Hello {toUser.username},\n" +
                $"{fromUser.username} send you an invitation to join a plan.\n" +
                "Title:\n" +
                $"{plan.title}\n" +
                $"Category:\n" +
                $"{plan.category}\n" +
                $"Description:\n" +
                $"{plan.description}\n" +
                $"The plan is sheduled on {plan.startTime.ToString("G")}.\n" +
                "Log in to accept or reject it.\n" +
                "This email was generated automatically.\n" +
                "Don't answear.\n" +
                "FranDan team\n";
            message.IsBodyHtml = false;
            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(sendingMail.email, sendingMail.password);
                try
                {
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    throw new Exception("An error accured while sending email.");
                }
            }
        }
    }
}
