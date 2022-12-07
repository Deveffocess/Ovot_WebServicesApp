
using OVOT_SERVICE.Bll;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;


namespace OVOT_SERVICE
{
    public class EmailCommon
    {
        
        public void SendHTMLFormatedMail(string strToAddress, string strSubject, string strBody)
        {
            string strUserName = ConfigurationManager.AppSettings["UserName"].ToString();
            string strPassword = ConfigurationManager.AppSettings["Password"].ToString();
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(strUserName, "OVOT - Issue Forward");
                mailMessage.Subject = strSubject;
                mailMessage.Body = strBody;
                mailMessage.IsBodyHtml = true;
                string[] Mailto = strToAddress.Split(',');
                foreach (string MultiMailto in Mailto)
                {
                    if (MultiMailto != "")
                    {
                        mailMessage.To.Add(new MailAddress(MultiMailto.Trim()));
                    }
                }
                SmtpClient smtp = new SmtpClient();
                smtp.Host = ConfigurationManager.AppSettings["Host"];
                smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                NetworkCred.UserName =  strUserName;
                NetworkCred.Password = strPassword;
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = int.Parse(ConfigurationManager.AppSettings["Port"]);
                smtp.Send(mailMessage);
            }
        }

        public void SendMailWithFormated(EmailBll bll, string strMailType, string Type)
        {
            SetBllData(bll, strMailType);

            //string body = string.Empty;

            //using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(RegThanks.HTMLTemplate)))
            //{
            //    body = reader.ReadToEnd();
            //}

            string body = "";
            string HTMLTemplate = "MailTemplate/MailCommon.html";
            string filePath = Path.Combine(HttpRuntime.AppDomainAppPath, HTMLTemplate);
            body = System.IO.File.ReadAllText(filePath);

            body = body.Replace("{Body}", bll.P_MailBody);

            body = body.Replace("<userName>", bll.P_UserName);
            body = body.Replace("<<IssueType>>", bll.P_IssueType);
            body = body.Replace("<<ForwardBy>>", bll.P_EntryBy);
            
            
           
            try
            {
                SendHTMLFormatedMail(bll.P_Mailto, bll.P_Mailsubject, body);
                CDal dal = new CDal();
               // dal.InsertMailDetails(bll.P_MailType, bll.P_Mailsubject, bll.P_MailBody, bll.P_UserCode, bll.P_Mailto);
            }
            catch (Exception ex)
            {

                // insertlog.get("EmailDal.aspx", ex.ToString(), "SendAndSaveQueryMail-SendHTMLFormatedMail");

            }

            try
            {

            }
            catch (Exception ex)
            {
                //insertlog.get("EmailDal.aspx", ex.ToString(), "SendAndSaveQueryMail-insertMailDetails");
            }
        }

        private void SetBllData(EmailBll bll, string strMailType)
        {
            DataSet ds = new DataSet();
            CDal dal = new CDal();

            ds = dal.GetMailMaster(strMailType);
            if (ds.Tables[0].Rows.Count > 0)
            {
                
                    bll.P_MailBody = Convert.ToString(ds.Tables[0].Rows[0]["Body"]);
                    bll.P_Mailsubject = Convert.ToString(ds.Tables[0].Rows[0]["Subject"]);
                   // bll.P_Mailto = Convert.ToString(ds.Tables[0].Rows[0]["Mailto"]);
            }
        }
    }
}