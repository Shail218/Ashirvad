using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using context = System.Web.HttpContext;

namespace Ashirvad.Logger
{
    [Serializable]
    public enum Severity
    {
        Info,
        Warning,
        Error,
        Fatal
    }

    public static class EventLogger
    {
        private static log4net.ILog Log { get; set; }

        private readonly static string EmailID = "";

        static EventLogger()
        {
            Log = log4net.LogManager.GetLogger(typeof(EventLogger));
            EmailID = Convert.ToString(ConfigurationManager.AppSettings["EventLogEmail"]);
        }

        private static bool Send(string toEmail, string cc, string bcc, string subject, string message, bool isHtml, string fromAddress = "", string displayName = "")
        {
            try
            {
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();

                if (string.IsNullOrEmpty(fromAddress))
                    fromAddress = "DoNotReply@test.com";

                if (string.IsNullOrEmpty(displayName))
                    displayName = "Ashirvad-Alert";

                foreach (var item in EmailID.Split(';'))
                {
                    mail.To.Add(item);
                }

                if (!string.IsNullOrEmpty(cc))
                    mail.CC.Add(cc);
                if (!string.IsNullOrEmpty(bcc))
                    mail.Bcc.Add(bcc);
                mail.From = new System.Net.Mail.MailAddress(fromAddress, displayName, Encoding.UTF8);
                mail.Subject = subject;
                mail.Body = message;
                mail.Priority = System.Net.Mail.MailPriority.High;
                mail.DeliveryNotificationOptions = System.Net.Mail.DeliveryNotificationOptions.OnFailure;
                mail.IsBodyHtml = isHtml;
                ////mail.Attachments.Add(new Attachment())

                System.Net.NetworkCredential basicAuthentication = new System.Net.NetworkCredential("test@gmail.com", "test123");
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.UseDefaultCredentials = false;
                client.Credentials = basicAuthentication;

                client.EnableSsl = true;
                client.Send(mail);
                client.Dispose();
                mail.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                //EventLogger.WriteEvent(Severity.Fatal, ex);
                SendErrorToText(ex);
                return false;
            }
        }

        public static void WriteEvent(Severity severity, Exception ex)
        {
            string message = "";
            if (ex != null)
            {
                message = "StackTrace: " + ex.StackTrace + ", Message:" + ex.Message + ", InnerException: " + ex.InnerException;

            }

            switch (severity)
            {
                case Severity.Warning:
                    Log.Warn(ex);
                    break;
                case Severity.Error:
                    Send("", "", "", "Ashirvad Live Software Error: ", message, false);
                    Log.Error(ex);
                    break;
                case Severity.Fatal:
                    Send("", "", "", "Ashirvad Live Software Fatal: ", message, false);
                    Log.Fatal(ex);
                    break;
                default:
                    break;
            }
        }

        public static void WriteEvent(Severity severity, string description)
        {
            switch (severity)
            {
                case Severity.Info:
                    Log.Info(description);
                    break;
                case Severity.Warning:
                    Log.Warn(description);
                    break;
                case Severity.Error:
                    Send("", "", "", "Ashirvad Live Software Error: ", description, false);
                    Log.Error(description);
                    break;
                case Severity.Fatal:
                    Send("", "", "", "Ashirvad Live Software Fatal: ", description, false);
                    Log.Fatal(description);
                    break;
                default:
                    break;
            }
        }

        public static void WriteEvent(Severity severity, string description, Exception ex)
        {
            switch (severity)
            {
                case Severity.Info:
                    Log.Info(description, ex);
                    break;
                case Severity.Warning:
                    Log.Warn(description, ex);
                    break;
                case Severity.Error:
                    Log.Error(description, ex);
                    break;
                case Severity.Fatal:
                    Log.Fatal(description, ex);
                    break;
                default:
                    break;
            }
        }

        private static String ErrorlineNo, Errormsg, extype, exurl, hostIp, ErrorLocation, HostAdd;

        public static void SendErrorToText(Exception ex)
        {
            var line = Environment.NewLine + Environment.NewLine;

            ErrorlineNo = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            Errormsg = ex.GetType().Name.ToString();
            extype = ex.GetType().ToString();
            //exurl = context.Current.Request.Url.ToString();
            ErrorLocation = ex.Message.ToString();

            try
            {
                string filepath = context.Current.Server.MapPath("~/Logs/");  //Text File Path

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);

                }
                filepath = filepath + DateTime.Today.ToString("dd-MM-yy") + ".txt";   //Text File Name
                if (!File.Exists(filepath))
                {


                    File.Create(filepath).Dispose();

                }
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    string error = "Log Written Date:" + " " + DateTime.Now.ToString() + line + "Error Line No :" + " " + ErrorlineNo + line + "Error Message:" + " " + Errormsg + line + "Exception Type:" + " " + extype + line + "Error Location :" + " " + ErrorLocation + line + " Error Page Url:" + " " + exurl + line + "User Host IP:" + " " + hostIp + line;
                    sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                    sw.WriteLine("-------------------------------------------------------------------------------------");
                    sw.WriteLine(line);
                    sw.WriteLine(error);
                    sw.WriteLine("--------------------------------*End*------------------------------------------");
                    sw.WriteLine(line);
                    sw.Flush();
                    sw.Close();

                }

            }
            catch (Exception e)
            {
                e.ToString();

            }
        }


        public static void SendErrorToText(string log)
        {
            try
            {
                string filepath = context.Current.Server.MapPath("~/Logs/");  //Text File Path

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);

                }
                filepath = filepath + DateTime.Today.ToString("dd-MM-yy") + ".txt";   //Text File Name
                if (!File.Exists(filepath))
                {


                    File.Create(filepath).Dispose();

                }
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(log);
                    sw.Flush();
                    sw.Close();

                }

            }
            catch (Exception e)
            {
                e.ToString();

            }
        }
    }
}
