using SandeshAgarbathiAPI.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace SandeshAgarbathiAPI.Models
{
    public class CommomModal
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DBConStr"].ConnectionString;

      public string sendMail(string mailPriority,string mTo,string subject, string mailcontent, string mCC, string mBCC,Boolean IsenableSsl,Boolean BodyHtml)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string mailSentStatus = "";

            Boolean enableSsl = IsenableSsl;
            Boolean IsBodyHtml = BodyHtml;


            // email settings

            string smtpUsername = "", smtpPassword = "", smtpPort = "", smtClient = "", senderEmailId = "";
            SqlCommand readcommand2 = new SqlCommand("select * from EmailSetting ", connection);
            connection.Open();
            SqlDataReader rdr2 = readcommand2.ExecuteReader();
            while (rdr2.Read())
            {
                smtClient = rdr2.GetString(0);
                smtpUsername = rdr2.GetString(1);
                smtpPassword = rdr2.GetString(2);
                smtpPort = rdr2.GetString(3);
                senderEmailId = rdr2.GetString(4);

            }

            connection.Close();
           

            MailMessage mailMsg = new MailMessage();
            mailMsg.Subject = subject;
            //  mailMsg.Body = data[8];
            mailMsg.Body = mailcontent;
            mailMsg.From = new MailAddress(senderEmailId);

            if (mailPriority.Equals("High"))
            {
                mailMsg.Priority = MailPriority.High;
            }
            else
            {
                mailMsg.Priority = MailPriority.Normal;
            }

           

            if (!String.IsNullOrEmpty(mTo))
            {
                String[] to = mTo.Split(';');
                for (int i = 0; i < to.Length; i++)
                    if (!String.IsNullOrEmpty(to[i]))
                        mailMsg.To.Add(new MailAddress(to[i]));
            }

            if (!String.IsNullOrEmpty(mBCC))
            {
                String[] arrBCC = mBCC.Split(';');
                for (int i = 0; i < arrBCC.Length; i++)
                    if (!String.IsNullOrEmpty(arrBCC[i]))
                        mailMsg.Bcc.Add(new MailAddress(arrBCC[i]));
            }

            if (!String.IsNullOrEmpty(mCC))
            {
                String[] arrBCC = mCC.Split(';');
                for (int i = 0; i < arrBCC.Length; i++)
                    if (!String.IsNullOrEmpty(arrBCC[i]))
                        mailMsg.Bcc.Add(new MailAddress(arrBCC[i]));
            }

            mailMsg.IsBodyHtml = IsBodyHtml;
            try
            {

                SmtpClient smtpClient = new SmtpClient(smtClient);
                NetworkCredential networkCredential = new NetworkCredential();
                networkCredential.UserName = smtpUsername;
                networkCredential.Password = smtpPassword;
                smtpClient.Credentials = networkCredential;
                smtpClient.Port = Convert.ToInt32(smtpPort);


                smtpClient.EnableSsl = enableSsl;

                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtpClient.Send(mailMsg);
                mailSentStatus = "Mail Send Successfully";

            }
            catch (SmtpException ex)
            {
                mailSentStatus="Error Sending Mail" + ex;
            }

            return mailSentStatus;
        }
    }
}