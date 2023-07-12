using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using OVOT_SERVICE.Bll;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace OVOT_SERVICE
{
    /// <summary>
    /// Summary description for Index
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Index : System.Web.Services.WebService
    {
        CDal dal = new CDal();
        DataSet ds = new DataSet();
        JavaScriptSerializer js = new JavaScriptSerializer();
        DataSet dsError = new DataSet();


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void ValidateLogin(string MobileNo)
        {
            ds = dal.GETLOGIN(MobileNo);

            Context.Response.Clear();
            Context.Response.ContentType = "application/json";

            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataColumn dc = new DataColumn("message", typeof(System.String));
                    dc.DefaultValue = "Success";
                    ds.Tables[0].Columns.Add(dc);
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    rows = DataTable2JSON(ds);
                    this.Context.Response.Write(js.Serialize(new { LoginDetails = rows }));
                }
                else
                {
                    Common[] LoginDetails = new Common[]
                    {
                        new Common()
                        {
                            message="Fail"
                        }
                    };

                    Context.Response.Write(js.Serialize(new { LoginDetails }));
                }
            }
            catch (Exception ex)
            {

            }

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetUserDetails(string UserId)
        {
            ds = dal.GetUserDetails(UserId);

            Context.Response.Clear();
            Context.Response.ContentType = "application/json";

            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string AppStatus = ConfigurationManager.AppSettings["AppStatus"].ToString();
                    DataColumn dca = new DataColumn("AppStatus", typeof(System.String));
                    dca.DefaultValue = AppStatus;
                    ds.Tables[0].Columns.Add(dca);
                    DataColumn dc = new DataColumn("message", typeof(System.String));
                    dc.DefaultValue = "Success";
                    ds.Tables[0].Columns.Add(dc);
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    rows = DataTable2JSON(ds);
                    this.Context.Response.Write(js.Serialize(new { UserDetails = rows }));
                }
                else
                {
                    Common[] UserDetails = new Common[]
                    {
                        new Common()
                        {
                            message="Fail"
                        }
                    };

                    Context.Response.Write(js.Serialize(new { UserDetails }));
                }
            }
            catch (Exception ex)
            {

            }

        }


        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public void ValidateProductSRNO(string ProdSRNO)
        //{
        //    ds = dal.ValidateProduct(ProdSRNO);

        //    Context.Response.Clear();
        //    Context.Response.ContentType = "application/json";

        //    try
        //    {
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            DataColumn dc = new DataColumn("message", typeof(System.String));
        //            dc.DefaultValue = "Success";

        //            if(ds.Tables[0].Rows[0][0].ToString() != "0")
        //            {
        //                dc.DefaultValue = "Fail";
        //            }
        //            ds.Tables[0].Columns.Add(dc);
        //            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        //            rows = DataTable2JSON(ds);
        //            this.Context.Response.Write(js.Serialize(new { ProductSerial = rows }));
        //        }
        //        else
        //        {
        //            Common[] ProductSerial = new Common[]
        //            {
        //                new Common()
        //                {
        //                    message="Fail"
        //                }
        //            };

        //            Context.Response.Write(js.Serialize(new { ProductSerial }));
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //}

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void ValidateInvoiceNo(string InvoiceNo)
        {
            ds = dal.ValidateInvoiceNo(InvoiceNo);

            Context.Response.Clear();
            Context.Response.ContentType = "application/json";

            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataColumn dc = new DataColumn("message", typeof(System.String));
                    dc.DefaultValue = "Success";

                    if (ds.Tables[0].Rows[0][0].ToString() != "0")
                    {
                        dc.DefaultValue = "Fail";
                    }
                    ds.Tables[0].Columns.Add(dc);
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    rows = DataTable2JSON(ds);
                    this.Context.Response.Write(js.Serialize(new { invoiceNo = rows }));
                }
                else
                {
                    Common[] invoiceNo = new Common[]
                    {
                        new Common()
                        {
                            message="Fail"
                        }
                    };

                    Context.Response.Write(js.Serialize(new { invoiceNo }));
                }
            }
            catch (Exception ex)
            {

            }

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void InsertCustomerInvoice(string InvoiceNo, string ProductCode, string Customer, string MobileNo, string ISDCode, string FileName, string IPAdd)
        {
            // ds = dal.CheckDuplicate("customerinvoice", "ProductCode", ProductCode);


            //if (ds.Tables[0].Rows[0][0].ToString() == "0")
            //{


            ds = dal.GetISDDetails(ISDCode);
            string Dealer = null;
            string ProductSrNo = null;
            string Model = null;
            string IncentiveAmt = null;
            if (ds.Tables[0].Rows.Count > 0)
            {
                Dealer = Convert.ToString(ds.Tables[0].Rows[0]["DealerName"]);
            }

            ds = dal.GetProductDetails(ProductCode);
            if (ds.Tables[0].Rows.Count > 0)
            {

                ProductSrNo = ds.Tables[0].Rows[0]["SerialNo"].ToString();
                Model = ds.Tables[0].Rows[0]["Model"].ToString();
                IncentiveAmt = ds.Tables[0].Rows[0]["IncentiveAmt"].ToString();
            }

            ds = dal.InsertCustomerInvoice(InvoiceNo, ProductCode, ProductSrNo, Model, Dealer, Customer, MobileNo, ISDCode, IncentiveAmt, FileName, ISDCode, IPAdd);



            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Context.Response.Clear();
                    Context.Response.ContentType = "application/json";

                    DataColumn dc = new DataColumn("message", typeof(System.String));
                    dc.DefaultValue = "Success";
                    ds.Tables[0].Columns.Add(dc);

                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    rows = DataTable2JSON(ds);

                    try
                    {
                        this.Context.Response.Write(js.Serialize(new { InvoiceUploadCode = rows }));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    Common[] met = new Common[]
                    {
                        new Common()
                        {
                            message="Fail"
                        }
                    };
                    //return new JavaScriptSerializer().Serialize(emp);
                    Context.Response.Write(js.Serialize(new { met }));
                }

                //  ds = dal.InsertLog("InsertCustomerInvoice");
            }
            catch (Exception ex)
            {
                // dsError = dal.ErrorLog(ex.Message, ex.GetType().Name, ex.TargetSite.Name, ex.StackTrace, "WebService", "InsertMemberNotReceivedAction", UserCode);
            }
            //}
            //else
            //{

            //    Context.Response.Clear();
            //    Context.Response.ContentType = "application/json";
            //    ds.Tables[0].Columns.Remove("count(*)");
            //    ds.AcceptChanges();
            //    DataColumn dc = new DataColumn("UploadCode", typeof(System.String));
            //    dc.DefaultValue = "UPL0000000";
            //    ds.Tables[0].Columns.Add(dc);
            //    DataColumn dc2 = new DataColumn("message", typeof(System.String));
            //    dc2.DefaultValue = "Success";
            //    ds.Tables[0].Columns.Add(dc2);

            //    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            //    rows = DataTable2JSON(ds);

            //    try
            //    {
            //        this.Context.Response.Write(js.Serialize(new { InvoiceUploadCode = rows }));
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}


        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void UpdateISDDetails(string ISDCode, string EmailId, string AdharNo, string PANNO, string BankName, string BankACNo, string UPINo, string IFSCCode, string UserCode, string IPAdd)
        {

            try
            {


                ds = dal.UpdateISDDetails(ISDCode, EmailId, AdharNo, PANNO, BankName, BankACNo, UPINo, IFSCCode, UserCode, IPAdd);


                if (ds.Tables[0].Rows.Count > 0)
                {
                    Context.Response.Clear();
                    Context.Response.ContentType = "application/json";

                    DataColumn dc = new DataColumn("message", typeof(System.String));
                    dc.DefaultValue = "Success";
                    ds.Tables[0].Columns.Add(dc);

                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    rows = DataTable2JSON(ds);

                    try
                    {
                        this.Context.Response.Write(js.Serialize(new { ISDCode = rows }));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    Common[] met = new Common[]
                    {
                        new Common()
                        {
                            message="Fail"
                        }
                    };
                    //return new JavaScriptSerializer().Serialize(emp);
                    Context.Response.Write(js.Serialize(new { met }));
                }


            }
            catch (Exception ex)
            {
                // dsError = dal.ErrorLog(ex.Message, ex.GetType().Name, ex.TargetSite.Name, ex.StackTrace, "WebService", "InsertMemberNotReceivedAction", UserCode);
            }

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void UpdateISDDAgreementOld(string ISDCode, string AcceptanceStatus, string UserCode, string IPAdd)
        {

            try
            {


                ds = dal.UpdateISDAggreement(ISDCode, AcceptanceStatus, UserCode, IPAdd);


                if (ds.Tables[0].Rows.Count > 0)
                {
                    Context.Response.Clear();
                    Context.Response.ContentType = "application/json";

                    DataColumn dc = new DataColumn("message", typeof(System.String));
                    dc.DefaultValue = "Success";
                    ds.Tables[0].Columns.Add(dc);

                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    rows = DataTable2JSON(ds);

                    try
                    {
                        this.Context.Response.Write(js.Serialize(new { ISDCode = rows }));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    Common[] met = new Common[]
                    {
                        new Common()
                        {
                            message="Fail"
                        }
                    };
                    //return new JavaScriptSerializer().Serialize(emp);
                    Context.Response.Write(js.Serialize(new { met }));
                }


            }
            catch (Exception ex)
            {
                // dsError = dal.ErrorLog(ex.Message, ex.GetType().Name, ex.TargetSite.Name, ex.StackTrace, "WebService", "InsertMemberNotReceivedAction", UserCode);
            }

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void UpdateISDDAgreement(string ISDCode, string AcceptanceStatus, string UserCode, string IPAdd)
        {

            try
            {

                //GenrateAgreementandSendMail(ISDCode);
                ds = dal.UpdateISDAggreement(ISDCode, AcceptanceStatus, UserCode, IPAdd);
                

                if (ds.Tables[0].Rows.Count > 0)
                {
                    Context.Response.Clear();
                    Context.Response.ContentType = "application/json";

                    DataColumn dc = new DataColumn("message", typeof(System.String));
                    dc.DefaultValue = "Success";
                    ds.Tables[0].Columns.Add(dc);

                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    rows = DataTable2JSON(ds);

                    try
                    {
                        this.Context.Response.Write(js.Serialize(new { ISDCode = rows }));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    GenrateAgreementandSendMail(ISDCode);


                }
                else
                {
                    Common[] met = new Common[]
                    {
                        new Common()
                        {
                            message="Fail"
                        }
                    };
                    //return new JavaScriptSerializer().Serialize(emp);
                    Context.Response.Write(js.Serialize(new { met }));
                }


            }
            catch (Exception ex)
            {
                // dsError = dal.ErrorLog(ex.Message, ex.GetType().Name, ex.TargetSite.Name, ex.StackTrace, "WebService", "InsertMemberNotReceivedAction", UserCode);
            }

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void UpdateIssueConfirmation(string IssueCode)
        {

            try
            {


                ds = dal.UpdateIssueConfirmation(IssueCode);


                    Common[] met = new Common[]
                    {
                        new Common()
                        {
                            message="Success"
                        }
                    };
                    //return new JavaScriptSerializer().Serialize(emp);
                    Context.Response.Write(js.Serialize(new { met }));
                

            }
            catch (Exception ex)
            {
                // dsError = dal.ErrorLog(ex.Message, ex.GetType().Name, ex.TargetSite.Name, ex.StackTrace, "WebService", "InsertMemberNotReceivedAction", UserCode);
            }

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void InsertIssue(string IssueCode, string OldIssueCode,string DealerCode, string IssueDate, string IssueTypeCode, string IssueDescription, string UserCode, string IPAdd)
        {
          
            
            try
            {

                ds = dal.InsertIssue(IssueCode, OldIssueCode, DealerCode,IssueDate, IssueTypeCode, IssueDescription, "REGISTERED", "1", UserCode, IPAdd);
                //DataSet dslog = new DataSet();
                //dslog = dal.LogHit("2022-06-21","InsertIssue");

                if (ds.Tables.Count > 0)
                {

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Context.Response.Clear();
                        Context.Response.ContentType = "application/json";
                        string UserName = Convert.ToString(ds.Tables[0].Rows[0]["GCHName"]);
                        string Mailto = Convert.ToString(ds.Tables[0].Rows[0]["GCHEmail"]);
                        string EntryBy = Convert.ToString(ds.Tables[0].Rows[0]["FirmName"]);
                        string IssuType = Convert.ToString(ds.Tables[0].Rows[0]["IssueType"]);
                        string IssueCat = Convert.ToString(ds.Tables[0].Rows[0]["IssueCat"]);
                        string IssueType = Convert.ToString(ds.Tables[0].Rows[0]["IssueTypeCode"]);

                        

                        //if (Convert.ToString(ds.Tables[0].Rows[0]["ASMEmail"]) != null)
                        //{
                        //    Mailto = Mailto + "," + Convert.ToString(ds.Tables[0].Rows[0]["ASMEmail"]);
                        //}

                        if (Convert.ToString(ds.Tables[0].Rows[0]["ASMEmail"]) != null)
                            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                            {
                                Mailto = Mailto + "," + Convert.ToString(ds.Tables[0].Rows[i]["ASMEmail"] );

                            }


                        if (IssueTypeCode == "IST00006")
                        {
                            Mailto = "";
                        }


                        DataSet dsEmail = new DataSet();
                        dsEmail = dal.GetIssueConEmails(IssueTypeCode);
                        if (Convert.ToString(dsEmail.Tables[0].Rows[0]["UserEmail"]) != null)
                            for (int i = 0; i <= dsEmail.Tables[0].Rows.Count - 1; i++)
                            {
                                Mailto = Mailto + "," + Convert.ToString(dsEmail.Tables[0].Rows[i]["UserEmail"]);

                            }

                        //DataSet dsEmmail = new DataSet();
                        //dsEmmail = dal.GetMatrixUserEmails(IssueCode);
                        //if (dsEmmail.Tables[0].Rows.Count > 0)
                        //{
                        //    for (int i = 0; i < dsEmmail.Tables[0].Rows.Count; i++)
                        //    {
                        //        string Email = Convert.ToString(dsEmmail.Tables[0].Rows[i]["email"]);
                        //        if (Email != "")
                        //        {
                        //            Mailto = Mailto + "," + Email;
                        //        }

                        //    }
                        //}


                        DataColumn dc = new DataColumn("message", typeof(System.String));
                        dc.DefaultValue = "Success";
                        ds.Tables[0].Columns.Add(dc);

                        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                        rows = DataTable2JSON(ds);

                        try
                        {
                            this.Context.Response.Write(js.Serialize(new { IssueCode = rows }));
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }

                        SendMail(UserName, IssuType, Mailto, EntryBy);
                        //Thread.Sleep(7000);
                    }
                    else
                    {
                        Common[] met = new Common[]
                        {
                        new Common()
                        {
                            message="Fail"
                        }
                        };

                        Context.Response.Write(js.Serialize(new { met }));
                    }
                }
                else
                {
                    DataTable objDt1 = new DataTable();
                    DataTable dt = new DataTable();
                    string strColName = "message";
                    DataColumn colNew = new DataColumn(strColName, typeof(string));
                    colNew.DefaultValue = "Success";
                    dt.Columns.Add(colNew);
                    DataRow dr = dt.NewRow();
                    dr[0] = "Success";
                    dt.Rows.Add(dr);
                    DataSet dsmes = new DataSet();
                    dsmes.Tables.Add(dt);
                    dsmes.AcceptChanges();
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    rows = DataTable2JSON(dsmes);

                    try
                    {
                        this.Context.Response.Write(js.Serialize(new { IssueCode = rows }));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }


                //  ds = dal.InsertLog("InsertCustomerInvoice");
            }
            catch (Exception ex)
            {
                // dsError = dal.ErrorLog(ex.Message, ex.GetType().Name, ex.TargetSite.Name, ex.StackTrace, "WebService", "InsertMemberNotReceivedAction", UserCode);
            }
           
        }
        

        public static void SendMail(string UserName, string IssueType, string SendTo, string EntryBy)
        {


            EmailBll emailBll = new EmailBll();
            CDal dal = new CDal();
            EmailCommon emailCommon = new EmailCommon();
            emailBll.P_UserName = UserName;
            emailBll.P_IssueType = IssueType;
            emailBll.P_Mailto = SendTo;
            emailBll.P_EntryBy = EntryBy;
            
            emailCommon.SendMailWithFormated(emailBll, "IssueForward", "Sender");


        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetIssueType()
        {

            ds = dal.GetIssueType();

            Context.Response.Clear();
            Context.Response.ContentType = "application/json";

            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataColumn dc = new DataColumn("message", typeof(System.String));
                    dc.DefaultValue = "Success";
                    ds.Tables[0].Columns.Add(dc);
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    rows = DataTable2JSON(ds);
                    this.Context.Response.Write(js.Serialize(new { IssueType = rows }));
                }
                else
                {
                    Common[] IssueType = new Common[]
                    {
                        new Common()
                        {
                            message="Fail"
                        }
                    };

                    Context.Response.Write(js.Serialize(new { IssueType }));
                }
            }
            catch (Exception ex)
            {

            }



        }



        public void GenrateAgreementandSendMail(string ISDCode)
        {
            //DataSet dsAgree = new DataSet();
            //dsAgree = dal.GetISDForAggrement();
            //for (int i = 0; i < dsAgree.Tables[0].Rows.Count; i++)
            //{
            //    ISDCode = dsAgree.Tables[0].Rows[i]["ISDCode"].ToString();
                ds = dal.GetISDDetails(ISDCode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string Name = Convert.ToString(ds.Tables[0].Rows[0]["Name"]);
                    string PanNo = Convert.ToString(ds.Tables[0].Rows[0]["PANNO"]);
                    string Email = Convert.ToString(ds.Tables[0].Rows[0]["EmailId"]);
                    string MobNo = Convert.ToString(ds.Tables[0].Rows[0]["MobileNo"]);
                    DateTime AgrementDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["AcceptedDate"]);
                    string TemplateName = Server.MapPath("~/Template/Agreement.html");
                    //string FileName = bll.ChallenNo + "_DeliveryChallen.doc";
                    string FileName = ISDCode + "_SalesAgreement.pdf";
                    string strContent = File.ReadAllText(TemplateName);
                    string output = "";
                    if (strContent.Contains("{NAME}"))
                    {
                        strContent = strContent.Replace("{NAME}", Name);
                    }
                    if (strContent.Contains("{PANNO}"))
                    {
                        strContent = strContent.Replace("{PANNO}", PanNo);
                    }
                    if (strContent.Contains("{MOB}"))
                    {
                        strContent = strContent.Replace("{MOB}", MobNo);
                    }
                    if (strContent.Contains("{EMAIL}"))
                    {
                        strContent = strContent.Replace("{EMAIL}", Email);
                    }
                    if (strContent.Contains("{DATE}"))
                    {
                        strContent = strContent.Replace("{DATE}", AgrementDate.Day.ToString());
                    }
                    if (strContent.Contains("{FULLDATE}"))
                    {
                        strContent = strContent.Replace("{FULLDATE}", AgrementDate.ToString("dd-MMM-yyyy"));
                    }
                    if (strContent.Contains("{MONTH}"))
                    {
                        strContent = strContent.Replace("{MONTH}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(AgrementDate.Month));
                    }
                    if (strContent.Contains("{YEAR}"))
                    {
                        strContent = strContent.Replace("{YEAR}", AgrementDate.Year.ToString());
                    }
                    output = strContent;

                    string path = Server.MapPath("~/Template/" + FileName);


                    StringReader sr = new StringReader(output.ToString());

                    Document pdfDoc = new Document(PageSize.A4, 35f, 35f, 35f, 35f);
                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                        pdfDoc.Open();

                        htmlparser.Parse(sr);

                        pdfDoc.Close();

                        byte[] bytes = memoryStream.ToArray();

                        memoryStream.Close();
                        System.IO.File.WriteAllBytes(path, bytes);
                        string Subject = "Sales Consultant Agreement - " + Name + " ( " + ISDCode + " )";
                        string Body = "Hi, <br/> Please find subject document attached herewith.";


                       SendHTMLFormatedMail(Subject, Body, bytes, FileName);
                    }
                }
            //}
        }

        public void SendHTMLFormatedMail(string strSubject, string strBody, byte[] bytes, string AttachedFilePath)
        {
            string strUserName = ConfigurationManager.AppSettings["UserName"].ToString();
            string strPassword = ConfigurationManager.AppSettings["Password"].ToString();

            string strToAddress = ConfigurationManager.AppSettings["MailTo"].ToString();
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(strUserName, "OVOT PRIVATE LIMITED");
                mailMessage.Subject = strSubject;
                mailMessage.Body = strBody;
                mailMessage.IsBodyHtml = true;

                mailMessage.To.Add(new MailAddress(strToAddress));
                //For PDF
                mailMessage.Attachments.Add(new Attachment(new MemoryStream(bytes), AttachedFilePath));
                //For Word
                //mailMessage.Attachments.Add(new Attachment(AttachedFilePath));
                SmtpClient smtp = new SmtpClient();
                smtp.Host = ConfigurationManager.AppSettings["Host"];
                smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                NetworkCred.UserName = strUserName;
                NetworkCred.Password = strPassword;
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = int.Parse(ConfigurationManager.AppSettings["Port"]);
                smtp.Send(mailMessage);
            }
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetProductDetails(string ProductSRNO)
        {

            ds = dal.ValidateProduct(ProductSRNO);
            if (ds.Tables[0].Rows.Count > 0)
            {


                if (ds.Tables[0].Rows[0][0].ToString() != "0")
                {
                    DataColumn dc = new DataColumn("message", typeof(System.String));
                    dc.DefaultValue = "Serial No Already Exist in Invoice";
                    ds.Tables[0].Columns.Add(dc);
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    rows = DataTable2JSON(ds);
                    this.Context.Response.Write(js.Serialize(new { ProductDt = rows }));
                }

                else
                {
                    ds = dal.GetProductDt(ProductSRNO);

                    Context.Response.Clear();
                    Context.Response.ContentType = "application/json";

                    try
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataColumn dc = new DataColumn("message", typeof(System.String));
                            dc.DefaultValue = "Success";
                            ds.Tables[0].Columns.Add(dc);
                            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                            rows = DataTable2JSON(ds);
                            this.Context.Response.Write(js.Serialize(new { ProductDt = rows }));
                        }
                        else
                        {
                            Common[] ProductDt = new Common[]
                            {
                        new Common()
                        {
                            message="Fail"
                        }
                            };

                            Context.Response.Write(js.Serialize(new { ProductDt }));
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetAllProductManual(string Category)
        {

            ds = dal.GetAllProductManual(Category);

            Context.Response.Clear();
            Context.Response.ContentType = "application/json";

            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataColumn dc = new DataColumn("message", typeof(System.String));
                    dc.DefaultValue = "Success";
                    ds.Tables[0].Columns.Add(dc);
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    rows = DataTable2JSON(ds);
                    this.Context.Response.Write(js.Serialize(new { ManualList = rows }));
                }
                else
                {
                    Common[] ManualList = new Common[]
                    {
                        new Common()
                        {
                            message="Fail"
                        }
                    };

                    Context.Response.Write(js.Serialize(new { ManualList }));
                }
            }
            catch (Exception ex)
            {

            }

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetProductManual(string ManualCode)
        {

            ds = dal.GetProductManual(ManualCode);

            Context.Response.Clear();
            Context.Response.ContentType = "application/json";

            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataColumn dc = new DataColumn("message", typeof(System.String));
                    dc.DefaultValue = "Success";
                    ds.Tables[0].Columns.Add(dc);
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    rows = DataTable2JSON(ds);
                    this.Context.Response.Write(js.Serialize(new { Manualdt = rows }));
                }
                else
                {
                    Common[] Manualdt = new Common[]
                    {
                        new Common()
                        {
                            message="Fail"
                        }
                    };

                    Context.Response.Write(js.Serialize(new { Manualdt }));
                }
            }
            catch (Exception ex)
            {

            }

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetCategory()
        {

            ds = dal.GetProductCategoryDll();

            Context.Response.Clear();
            Context.Response.ContentType = "application/json";

            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataColumn dc = new DataColumn("message", typeof(System.String));
                    dc.DefaultValue = "Success";
                    ds.Tables[0].Columns.Add(dc);
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    rows = DataTable2JSON(ds);
                    this.Context.Response.Write(js.Serialize(new { Category = rows }));
                }
                else
                {
                    Common[] Category = new Common[]
                    {
                        new Common()
                        {
                            message="Fail"
                        }
                    };

                    Context.Response.Write(js.Serialize(new { Category }));
                }
            }
            catch (Exception ex)
            {

            }

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetSubCategory(string Category)
        {

            ds = dal.GetProductSubCategoryDll(Category);

            Context.Response.Clear();
            Context.Response.ContentType = "application/json";

            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataColumn dc = new DataColumn("message", typeof(System.String));
                    dc.DefaultValue = "Success";
                    ds.Tables[0].Columns.Add(dc);
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    rows = DataTable2JSON(ds);
                    this.Context.Response.Write(js.Serialize(new { SubCategory = rows }));
                }
                else
                {
                    Common[] SubCategory = new Common[]
                    {
                        new Common()
                        {
                            message="Fail"
                        }
                    };

                    Context.Response.Write(js.Serialize(new { SubCategory }));
                }
            }
            catch (Exception ex)
            {

            }

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetModel(string SubCategory)
        {

            ds = dal.GetModelDll(SubCategory);

            Context.Response.Clear();
            Context.Response.ContentType = "application/json";

            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataColumn dc = new DataColumn("message", typeof(System.String));
                    dc.DefaultValue = "Success";
                    ds.Tables[0].Columns.Add(dc);
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    rows = DataTable2JSON(ds);
                    this.Context.Response.Write(js.Serialize(new { Model = rows }));
                }
                else
                {
                    Common[] Model = new Common[]
                    {
                        new Common()
                        {
                            message="Fail"
                        }
                    };

                    Context.Response.Write(js.Serialize(new { Model }));
                }
            }
            catch (Exception ex)
            {

            }

        }



        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public void UploadFileAndUpdateFilePath(string fileName, byte[] bytes, string UploadCode)
        //{
        //    //string filePath = Server.MapPath(string.Format("~/Uploads/{0}", fileName));
        //    //File.WriteAllBytes(filePath, bytes);



        //    try
        //    {
        //        string ftp = ConfigurationManager.AppSettings["ftp"];
        //        string ftpFolder = "/Uploads/";
        //        byte[] fileBytes = bytes;

        //        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftp + ftpFolder + fileName);
        //        request.Method = WebRequestMethods.Ftp.UploadFile;

        //        string FTPUser = ConfigurationManager.AppSettings["ftpUser"];
        //        string FTPPass = ConfigurationManager.AppSettings["ftpPass"];
        //        request.Credentials = new NetworkCredential(FTPUser, FTPPass);
        //        request.ContentLength = fileBytes.Length;
        //        request.UsePassive = true;
        //        request.UseBinary = true;
        //        request.ServicePoint.ConnectionLimit = fileBytes.Length;
        //        request.EnableSsl = false;

        //        using (Stream requestStream = request.GetRequestStream())
        //        {
        //            requestStream.Write(fileBytes, 0, fileBytes.Length);
        //            requestStream.Close();
        //        }

        //        FtpWebResponse response = (FtpWebResponse)request.GetResponse();
        //        response.Close();
        //        string FilePath = ConfigurationManager.AppSettings["ImagePathURL"] + fileName;
        //        ds = dal.UpdateInvoiceFilePath(UploadCode, FilePath);
        //        Context.Response.Clear();
        //        Context.Response.ContentType = "application/json";

        //        Common[] FileDt = new Common[]
        //                {
        //                new Common()
        //                {
        //                    message="Success"
        //                }
        //                };

        //        Context.Response.Write(js.Serialize(new { FileDt }));
        //    }
        //    catch (Exception ex)
        //    {
        //        Common[] met = new Common[]
        //           {
        //                new Common()
        //                {
        //                    message="Fail"
        //                }
        //           };

        //        Context.Response.Write(js.Serialize(new { met }));
        //    }

        //}
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetVersionInfo()
        {
            ds = dal.GetVersionInfo();
            //string VersionInfo = Convert.ToString(ds.Tables[0].Rows[0]["VersionNo"]);
            //Common[] Version = new Common[]
            //        {
            //            new Common()
            //            {
            //                message = VersionInfo
            //            }
            //        };

            //Context.Response.Write(js.Serialize(new { Version }));
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";

            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataColumn dc = new DataColumn("message", typeof(System.String));
                    dc.DefaultValue = "Success";
                    ds.Tables[0].Columns.Add(dc);
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    rows = DataTable2JSON(ds);
                    this.Context.Response.Write(js.Serialize(new { Version = rows }));
                }
                else
                {
                    Common[] Version = new Common[]
                    {
                        new Common()
                        {
                            message="Fail"
                        }
                    };

                    Context.Response.Write(js.Serialize(new { Version }));
                }
            }
            catch (Exception ex)
            {

            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void IncentiveReport(string ISDCODE, string FromDt, string ToDt)
        {
            ds = dal.GETINCENTIVEREPORT(ISDCODE, FromDt, ToDt);

            Context.Response.Clear();
            Context.Response.ContentType = "application/json";

            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataColumn dc = new DataColumn("message", typeof(System.String));
                    dc.DefaultValue = "Success";
                    ds.Tables[0].Columns.Add(dc);
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    rows = DataTable2JSON(ds);
                    this.Context.Response.Write(js.Serialize(new { IncentiveRPT = rows }));
                }
                else
                {
                    Common[] IncentiveRPT = new Common[]
                    {
                        new Common()
                        {
                            message="Fail"
                        }
                    };

                    Context.Response.Write(js.Serialize(new { IncentiveRPT }));
                }
            }
            catch (Exception ex)
            {

            }

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetMyIssue(string DealerCode, string FromDt, string ToDt, string Status)
        {
            if(Status == "ALL")
            {
                Status = null;
            }
            ds = dal.GetMyIssue(DealerCode, FromDt, ToDt, Status);

            Context.Response.Clear();
            Context.Response.ContentType = "application/json";

            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataColumn dc = new DataColumn("message", typeof(System.String));
                    dc.DefaultValue = "Success";
                    ds.Tables[0].Columns.Add(dc);
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    rows = DataTable2JSON(ds);
                    this.Context.Response.Write(js.Serialize(new { MyIssue = rows }));
                }
                else
                {
                    Common[] MyIssue = new Common[]
                    {
                        new Common()
                        {
                            message="Fail"
                        }
                    };

                    Context.Response.Write(js.Serialize(new { MyIssue }));
                }
            }
            catch (Exception ex)
            {

            }

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetISDDetails(string ISDCODE)
        {
            ds = dal.GetISDDetails(ISDCODE);

            Context.Response.Clear();
            Context.Response.ContentType = "application/json";

            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataColumn dc = new DataColumn("message", typeof(System.String));
                    dc.DefaultValue = "Success";
                    ds.Tables[0].Columns.Add(dc);
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    rows = DataTable2JSON(ds);
                    this.Context.Response.Write(js.Serialize(new { ISDDt = rows }));
                }
                else
                {
                    Common[] ISDDt = new Common[]
                    {
                        new Common()
                        {
                            message="Fail"
                        }
                    };

                    Context.Response.Write(js.Serialize(new { ISDDt }));
                }
            }
            catch (Exception ex)
            {

            }

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetDealerWiseISDSale(string DealerCode, string ISDCode, string Model, string FromDate, string ToDate)
        {
            if (ISDCode.ToUpper() == "ALL")
            {
                ISDCode = null;
            }
            if (Model.ToUpper() == "ALL")
            {
                Model = null;
            }
            ds = dal.GetDealerWiseISDSale( DealerCode,  ISDCode,  Model,  FromDate,  ToDate);

            Context.Response.Clear();
            Context.Response.ContentType = "application/json";

            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataColumn dc = new DataColumn("message", typeof(System.String));
                    dc.DefaultValue = "Success";
                    ds.Tables[0].Columns.Add(dc);
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    rows = DataTable2JSON(ds);
                    this.Context.Response.Write(js.Serialize(new { ReportDt = rows }));
                }
                else
                {
                    Common[] ReportDt = new Common[]
                    {
                        new Common()
                        {
                            message="Fail"
                        }
                    };

                    Context.Response.Write(js.Serialize(new { ReportDt }));
                }
            }
            catch (Exception ex)
            {

            }

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetISDDealerWise(string DealerCode)
        {
            ds = dal.GetISDDealerWise(DealerCode);

            Context.Response.Clear();
            Context.Response.ContentType = "application/json";

            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataColumn dc = new DataColumn("message", typeof(System.String));
                    dc.DefaultValue = "Success";
                    ds.Tables[0].Columns.Add(dc);
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    rows = DataTable2JSON(ds);
                    this.Context.Response.Write(js.Serialize(new { ISDDt = rows }));
                }
                else
                {
                    Common[] ISDDt = new Common[]
                    {
                        new Common()
                        {
                            message="Fail"
                        }
                    };

                    Context.Response.Write(js.Serialize(new { ISDDt }));
                }
            }
            catch (Exception ex)
            {

            }

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetModelList()
        {
            ds = dal.GetModelList();

            Context.Response.Clear();
            Context.Response.ContentType = "application/json";

            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataColumn dc = new DataColumn("message", typeof(System.String));
                    dc.DefaultValue = "Success";
                    ds.Tables[0].Columns.Add(dc);
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    rows = DataTable2JSON(ds);
                    this.Context.Response.Write(js.Serialize(new { ModelDt = rows }));
                }
                else
                {
                    Common[] ModelDt = new Common[]
                    {
                        new Common()
                        {
                            message="Fail"
                        }
                    };

                    Context.Response.Write(js.Serialize(new { ModelDt }));
                }
            }
            catch (Exception ex)
            {

            }

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetUpdatAndNotifications(string ContentType)
        {

            ds = dal.GetUpdatAndNotifications(ContentType);

            Context.Response.Clear();
            Context.Response.ContentType = "application/json";

            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataColumn dc = new DataColumn("message", typeof(System.String));
                    dc.DefaultValue = "Success";
                    ds.Tables[0].Columns.Add(dc);
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    rows = DataTable2JSON(ds);
                    this.Context.Response.Write(js.Serialize(new { UpdateDt = rows }));
                }
                else
                {
                    Common[] UpdateDt = new Common[]
                    {
                        new Common()
                        {
                            message="Fail"
                        }
                    };

                    Context.Response.Write(js.Serialize(new { UpdateDt }));
                }
            }
            catch (Exception ex)
            {

            }



        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetNotificationCnt(string LastDate)
        {

            ds = dal.GetNotificationCnt(LastDate);

            Context.Response.Clear();
            Context.Response.ContentType = "application/json";

            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataColumn dc = new DataColumn("message", typeof(System.String));
                    dc.DefaultValue = "Success";
                    ds.Tables[0].Columns.Add(dc);
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    rows = DataTable2JSON(ds);
                    this.Context.Response.Write(js.Serialize(new { Cnt = rows }));
                }
                else
                {
                    Common[] Cnt = new Common[]
                    {
                        new Common()
                        {
                            message="Fail"
                        }
                    };

                    Context.Response.Write(js.Serialize(new { Cnt }));
                }
            }
            catch (Exception ex)
            {

            }



        }
        public List<Dictionary<string, object>> DataTable2JSON(DataSet ds)
        {
            DataTable dt = new DataTable();
            dt = ds.Tables[0];

            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = null;
            foreach (DataRow rs in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, rs[col]);
                }
                rows.Add(row);
            }


            try
            {
                return rows;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public class Common
        {
            public string message { get; set; }

            public Common()
            {
                //
                // TODO: Add constructor logic here
                //
            }
        }
    }
}
