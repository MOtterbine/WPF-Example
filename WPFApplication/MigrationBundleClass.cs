using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace OS.WPFJamme
{

    public interface ILogger
    {
        void WriteToLog(string logText, LogLevels level);
        bool Enabled { get; set; }
        string ProcessModuleName { get; }
        LogLevels LogLevel { get; set; }
    }

    public enum LogLevels
    {
        Error,
        Warn,
        Information,
        Debug
    }

    public interface IConnectable
    {
        event System.Data.StateChangeEventHandler StateChanged;
        bool Connected { get; }
        bool Connect();
        bool Disconnect();
    }

    /// <summary>
    /// IReportInfo is a standardized way to extract information from any given class via a simple string.
    /// Mainly used for user logging and is typically user-readable text.
    /// </summary>
    public interface IReportInfo
    {
        /// <summary>
        /// Class-specific information usually based on the last operation performed.
        /// </summary>
        string InfoText { get; }
    }
    public class Emailer
    {
        /// <summary>
        /// Sends an email according to input parameters. If logger is null, then an exception is thrown rather than returning 'false'
        /// </summary>
        /// <param name="smtpServer">SMTP server addres</param>
        /// <param name="smtpPort">SMTP server port</param>
        /// <param name="fromEmail">sending email address</param>
        /// <param name="toEmail">colon-separated list of email recipients</param>
        /// <param name="subject">email subject line</param>
        /// <param name="body">body text</param>
        /// <param name="logger">ILogger object. Can be null, if so, then function will throw exception containing would-be log information</param>
        /// <returns></returns>
        public static bool SendEmail(string smtpServer, int smtpPort, string fromEmail, string toEmail, string subject, string body, bool useSSL)
        {
            try
            {
                string[] emails = toEmail.Split(new char[] { ';' });
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(fromEmail);
                foreach (string emailAddress in emails)
                {
                    if (!string.IsNullOrEmpty(emailAddress))
                    {
                        mail.To.Add(emailAddress);
                    }
                }
                SmtpClient client = new SmtpClient(smtpServer, smtpPort);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                // Not using authentication
                client.UseDefaultCredentials = false;

                // When using authentication
                //client.UseDefaultCredentials = true;
                //System.Net.NetworkCredential creds = new System.Net.NetworkCredential("<Some Username>", "<Some Password>");
                //client.Credentials = creds;

                // Use secure socket layer?
                client.EnableSsl = useSSL;

                mail.Subject = subject;
                mail.Body = body;
                client.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed to send email to {0} using server: {1} on port {2} - {3}", toEmail, smtpServer, smtpPort, ex.Message), ex);
            }
        }

        /// <summary>
        /// Sends and email where the smtp server requires authentication
        /// </summary>
        /// <param name="smtpServer"></param>
        /// <param name="smtpPort"></param>
        /// <param name="fromEmail"></param>
        /// <param name="toEmail"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool SendEmail(string smtpServer, int smtpPort, string fromEmail, string toEmail, string subject, string body, bool useSSL, string userName, string password)
        {
            try
            {
                string[] emails = toEmail.Split(new char[] { ';' });
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(fromEmail);
                foreach (string emailAddress in emails)
                {
                    if (!string.IsNullOrEmpty(emailAddress))
                    {
                        mail.To.Add(emailAddress);
                    }
                }
                SmtpClient client = new SmtpClient(smtpServer, smtpPort);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                // When using authentication
                client.UseDefaultCredentials = true;
                System.Net.NetworkCredential creds = new System.Net.NetworkCredential(userName, password);
                client.Credentials = creds;

                // Use secure socket layer?
                client.EnableSsl = useSSL;

                mail.Subject = subject;
                mail.Body = body;
                client.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed to send email to {0} using server: {1} on port {2} - {3}", toEmail, smtpServer, smtpPort, ex.Message), ex);
            }
        }

    }


}
