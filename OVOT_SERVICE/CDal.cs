using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Net;
using System.Web;

namespace OVOT_SERVICE
{
   
    public class CDal
    {
        DataSet ds = new DataSet();
        MySqlCommand cmd = new MySqlCommand();
        MySqlDataAdapter da = new MySqlDataAdapter();

        public CDal()
        {

        }

        public DataSet GETLOGIN(string LoginMobileNo)
        {
            MySqlCommand command = new MySqlCommand();

            //DataSet DSGet = new DataSet();
            //string CommandText = "SELECT A.UserCode,A.UserId,A.UserName, A.UserType,A.PassWord,A.ConfirmPassword,A.ContactNumber, B.RoleName AS 'UserRole', A.IMEI, A.Active,A.UserLevel  FROM usermaster A LEFT JOIN rolemaster AS B ON A.UserRole = B.RoleCode WHERE A.ContactNumber = " + LoginMobileNo + " AND A.ACTIVE = 'TRUE';";

            //// string CommandText = "SELECT A.USERCODE, A.UserName, A.UserRole, A.UserType  FROM usermaster A WHERE A.ContactNumber = " + LoginMobileNo + " AND ACTIVE = 'TRUE';";
            //command.CommandType = CommandType.Text;
            //command.CommandText = CommandText;
            //command.Connection = new MySqlConnection(getConnectionString());
            //command.Connection.Open();
            //da.SelectCommand = command;

            command.Parameters.AddWithValue("P_MobileNo", LoginMobileNo);
            ds = FillDS(command, "app_get_LoginDetails");
            return ds;
        }

        public DataSet GetUserDetails(string UserId)
        {
            MySqlCommand command = new MySqlCommand();
            command.Parameters.AddWithValue("P_UserId", UserId);
            ds = FillDS(command, "app_get_UserDetails");
            return ds;
        }

        public DataSet GetVersionInfo()
        {
            MySqlCommand command = new MySqlCommand();
          
            ds = FillDS(command, "app_get_VersionInfo");
            return ds;
        }

        public DataSet ValidateProduct(string ProductSerial)
        {
            MySqlCommand command = new MySqlCommand();

            DataSet DSGet = new DataSet();

            string CommandText = " SELECT  count(*) AS CNT  FROM customerinvoice AS C WHERE C.ProductSrNo = '" + ProductSerial + "' AND ACTIVE = 'TRUE' AND ApproveStatus != 'Rejected';";
            command.CommandType = CommandType.Text;
            command.CommandText = CommandText;
            command.Connection = new MySqlConnection(getConnectionString());
            command.Connection.Open();
            da.SelectCommand = command;

            da.Fill(DSGet);
            command.Connection.Close();
            return DSGet;
        }

        public DataSet GetISDForAggrement()
        {
            MySqlCommand command = new MySqlCommand();

            DataSet DSGet = new DataSet();

            string CommandText = " SELECT ISDCode FROM isdmaster WHERE AcceptedDate != '';";
            command.CommandType = CommandType.Text;
            command.CommandText = CommandText;
            command.Connection = new MySqlConnection(getConnectionString());
            command.Connection.Open();
            da.SelectCommand = command;

            da.Fill(DSGet);
            command.Connection.Close();
            return DSGet;
        }

        public DataSet GetISDDetails(string P_ISDCode)
        {
            MySqlCommand command = new MySqlCommand();
            command.Parameters.AddWithValue("P_ISDCode", P_ISDCode);
            ds = FillDS(command, "usp_get_isdmaster");
            return ds;
        }

        public DataSet GetISDDealerWise(string P_DealerCode)
        {
            MySqlCommand command = new MySqlCommand();
            command.Parameters.AddWithValue("P_DealerCode", P_DealerCode);
            ds = FillDS(command, "app_get_ISDDll");
            return ds;
        }

        public DataSet GetDealerWiseISDSale(string DealerCode, string ISDCode, String Model, string FromDate, string ToDate)
        {
            MySqlCommand command = new MySqlCommand();
            command.Parameters.AddWithValue("P_DealerCode", DealerCode);
            command.Parameters.AddWithValue("P_ISDCode", ISDCode);
            command.Parameters.AddWithValue("P_Model", Model);
            command.Parameters.AddWithValue("P_DateFrom", FromDate);
            command.Parameters.AddWithValue("P_DateTo", ToDate);
            ds = FillDS(command, "app_rpt__ISDWiseModelSale");
            return ds;
        }

        public DataSet GetModelList()
        {
            MySqlCommand command = new MySqlCommand();
         
            ds = FillDS(command, "app_get_ModelDll");
            return ds;
        }

        public DataSet GetUpdatAndNotifications(string P_ContentType)
        {
            MySqlCommand command = new MySqlCommand();
            command.Parameters.AddWithValue("P_ContentType", P_ContentType);
            ds = FillDS(command, "app_get_UpdateNotification");
            return ds;
        }

        public DataSet GetNotificationCnt(string P_Date)
        {
            MySqlCommand command = new MySqlCommand();
            command.Parameters.AddWithValue("P_Date", P_Date);
            ds = FillDS(command, "app_get_Notificationcnt");
            return ds;
        }

        public DataSet GetIssueType()
        {
            MySqlCommand command = new MySqlCommand();
            
            ds = FillDS(command, "usp_get_IssueTypeDll");
            return ds;
        }
        public DataSet GetProductDetails(string P_ProductCode)
        {
            MySqlCommand command = new MySqlCommand();
            command.Parameters.AddWithValue("P_ProductCode", P_ProductCode);
            ds = FillDS(command, "usp_get_ProductMaster");
            return ds;
        }

        public DataSet GetAllProductManual(string P_Model)
        {
            MySqlCommand command = new MySqlCommand();
            command.Parameters.AddWithValue("P_Model", P_Model);
            ds = FillDS(command, "app_list_ProductManual");
            return ds;
        }

        public DataSet GetProductManual(string P_ManualCode)
        {
            MySqlCommand command = new MySqlCommand();
            command.Parameters.AddWithValue("P_ManualCode", P_ManualCode);
            ds = FillDS(command, "app_get_ProductManual");
            return ds;
        }

        public DataSet GetProductCategoryDll()
        {
            MySqlCommand command = new MySqlCommand();

            ds = FillDS(command, "usp_get_ProductCategoryDll");
            return ds;
        }

        public DataSet GetProductSubCategoryDll(string P_Category)
        {
            MySqlCommand command = new MySqlCommand();
            command.Parameters.AddWithValue("P_Category", P_Category);
            ds = FillDS(command, "usp_get_ProductSubCategoryDll");
            return ds;
        }

        public DataSet GetModelDll(string P_SubCategory)
        {
            MySqlCommand command = new MySqlCommand();
            command.Parameters.AddWithValue("P_SubCategory", P_SubCategory);
            ds = FillDS(command, "usp_get_ModelDll");
            return ds;
        }

        public DataSet GetMyIssue(string DealerCode, string FromDt, string ToDt, string Status)
        {
            MySqlCommand command = new MySqlCommand();
            command.Parameters.AddWithValue("P_DateFrom", FromDt);
            command.Parameters.AddWithValue("P_DateTo", ToDt);
          
            command.Parameters.AddWithValue("P_UserCode", DealerCode);
            command.Parameters.AddWithValue("P_Status", Status);
            ds = FillDS(command, "app_list_IssueMaster");
            return ds;
        }
        public DataSet GETINCENTIVEREPORT(string ISDCODE, string fromdt, string Todt)
        {
            MySqlCommand command = new MySqlCommand();

            DataSet DSGet = new DataSet();

            string CommandText = " SELECT C.Customer, C.MobileNo, C.InvoiceNo, P.ProductCat, P.SubCat, C.Model, C.ProductSrNo, C.IncentiveAmt, C.ApproveStatus, E.UserName as 'ApproveBy', D.Reason, DATE_FORMAT(C.CreatedDate, '%Y-%m-%d') as CreatedDate,DATE_FORMAT(C.ApproveDate, '%Y-%m-%d') as ApproveDate, C.Remarks FROM CUSTOMERINVOICE as C" +
                " left join PRODUCTMASTER as P on C.PRODUCTCODE = P.PRODUCTCODE" +
                " LEFT JOIN rejectionreason AS D ON C.RejectionReason = D.ReasonCode " +
                " LEFT JOIN usermaster AS E ON C.ApproveBy = E.UserCode " +
                " WHERE  C.ISDCODE = '" + ISDCODE + "' AND CAST( C.CreatedDate AS DATE) >= '" + fromdt + "' AND CAST( C.CreatedDate AS DATE )<= '" + Todt + "' AND C.ACTIVE = 'TRUE';";
            command.CommandType = CommandType.Text;
            command.CommandText = CommandText;
            command.Connection = new MySqlConnection(getConnectionString());
            command.Connection.Open();
            da.SelectCommand = command;

            da.Fill(DSGet);
            command.Connection.Close();
            return DSGet;
        }

        public DataSet CheckDuplicate(string TblName, string ColumnName, string Value)
        {
            //MySqlCommand command = new MySqlCommand();
            //command.Parameters.AddWithValue("P_RFIDNO", P_RFIDNO);
            //ds = FillDS(command, "app_get_CurrentLocation");
            //return ds;
            MySqlCommand command = new MySqlCommand();

            DataSet DSGet = new DataSet();

            string CommandText = "SELECT count(*) FROM  " + TblName + " Where ApproveStatus != 'Rejected' AND " + ColumnName + " = '" + Value + "';";
            command.CommandType = CommandType.Text;
            command.CommandText = CommandText;
            command.Connection = new MySqlConnection(getConnectionString());
            command.Connection.Open();
            da.SelectCommand = command;

            da.Fill(DSGet);
            command.Connection.Close();
            return DSGet;
        }


        public DataSet UpdateInvoiceFilePath(string UploadCode, string FilePath)
        {
            MySqlCommand command = new MySqlCommand();

            DataSet DSGet = new DataSet();

            string CommandText = " UPDATE customerinvoice SET InvoiceFilePath = '" + FilePath + "' WHERE UploadCode = '" + UploadCode + "'";
            command.CommandType = CommandType.Text;
            command.CommandText = CommandText;
            command.Connection = new MySqlConnection(getConnectionString());
            command.Connection.Open();
            da.SelectCommand = command;

            da.Fill(DSGet);
            command.Connection.Close();
            return DSGet;
        }

        public DataSet InsertLog(string WebServiceName)
        {
            MySqlCommand command = new MySqlCommand();

            DataSet DSGet = new DataSet();

            string CommandText = " insert into log (WEbService,ExecutionTime) VALUES ('" + WebServiceName + "', '"+ DateTime.Now+"');";
            command.CommandType = CommandType.Text;
            command.CommandText = CommandText;
            command.Connection = new MySqlConnection(getConnectionString());
            command.Connection.Open();
            da.SelectCommand = command;

            da.Fill(DSGet);
            command.Connection.Close();
            return DSGet;
        }

        public DataSet GetProductDt(string ProductSRNO)
        {
            MySqlCommand command = new MySqlCommand();

            DataSet DSGet = new DataSet();

            string CommandText = " SELECT ProductCode,ProductCat,SubCat, SerialNo,Model,DealerCode, (case when RIGHT(SerialNo,2) = 'OD' then '0' else IncentiveAmt END) as  IncentiveAmt,DistributerName,DistributerMobile FROM PRODUCTMASTER AS P WHERE P.SERIALNO = '" + ProductSRNO + "' AND ACTIVE = 'TRUE';";

            // string CommandText = " SELECT P.PRODUCTCODE, P.PRODUCTCAT, P.SUBCAT, P.MODEL, P.SERIALNO, P.INCENTIVEAMT FROM PRODUCTMASTER P WHERE P.SERIALNO = '" + ProductSRNO + "' AND ACTIVE = 'TRUE';";
            command.CommandType = CommandType.Text;
            command.CommandText = CommandText;
            command.Connection = new MySqlConnection(getConnectionString());
            command.Connection.Open();
            da.SelectCommand = command;

            da.Fill(DSGet);
            command.Connection.Close();
            return DSGet;
        }


        public DataSet ValidateInvoiceNo(string InvoiceNo)
        {
            MySqlCommand command = new MySqlCommand();

            DataSet DSGet = new DataSet();

            string CommandText = " SELECT  count(*) AS CNT  FROM customerinvoice AS C WHERE C.InvoiceNo = '" + InvoiceNo + "' AND ACTIVE = 'TRUE';";
            command.CommandType = CommandType.Text;
            command.CommandText = CommandText;
            command.Connection = new MySqlConnection(getConnectionString());
            command.Connection.Open();
            da.SelectCommand = command;

            da.Fill(DSGet);
            command.Connection.Close();
            return DSGet;
        }

        public DataSet UpdateIssueConfirmation(string IssueCode)
        {
            MySqlCommand command = new MySqlCommand();

            DataSet DSGet = new DataSet();

            string CommandText = " UPDATE issuemaster SET IssueStatus = 'CLOSED' WHERE IssueCode = '" + IssueCode + "';";
            command.CommandType = CommandType.Text;
            command.CommandText = CommandText;
            command.Connection = new MySqlConnection(getConnectionString());
            command.Connection.Open();
            da.SelectCommand = command;

            da.Fill(DSGet);
            command.Connection.Close();
            return DSGet;
        }


        public static string getConnectionString()
        {
            // EncryptDecrypt EnDe = new EncryptDecrypt();
            string Connection = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            return Connection;
        }

        public DataSet InsertCustomerInvoice(string InvoiceNo, string ProductCode, string ProductSrNo, string Model, string Dealer,  string Customer, string MobileNo, string ISDCode, string IncentiveAmt,string FileName, string UserCode, string IPAdd)
        {
            MySqlCommand command = new MySqlCommand();
            command.Parameters.AddWithValue("P_UploadCode", "0");
            command.Parameters.AddWithValue("P_InvoiceNo", InvoiceNo);
            command.Parameters.AddWithValue("P_ProductCode", ProductCode);
            command.Parameters.AddWithValue("P_ProductSrNo", ProductSrNo);
            command.Parameters.AddWithValue("P_Customer", Customer);
            command.Parameters.AddWithValue("P_MobileNo", MobileNo);
            command.Parameters.AddWithValue("P_ISDCode", ISDCode);
            command.Parameters.AddWithValue("P_FilePath", FileName);
            command.Parameters.AddWithValue("P_Model", Model);
            command.Parameters.AddWithValue("P_Dealer", Dealer);
            command.Parameters.AddWithValue("P_Remarks", "");
            command.Parameters.AddWithValue("P_IncentiveAmt", IncentiveAmt);
            command.Parameters.AddWithValue("P_Active", "TRUE");
            command.Parameters.AddWithValue("P_UserCode", UserCode);
            command.Parameters.AddWithValue("P_IPAdd", IPAdd);
            ds = FillDS(command, "usp_iu_customerinvoiceApp");
            return ds;
        }

        public DataSet UpdateISDDetails(string P_ISDCode, string P_EmailId, string P_AdharNo, string P_PANNO, string P_BankName, string P_BankACNo, string P_UPINo, string P_IFSCCOde, string UserCode, string IPAdd)
        {
            MySqlCommand command = new MySqlCommand();
            command.Parameters.AddWithValue("P_ISDCode", P_ISDCode);
            command.Parameters.AddWithValue("P_EmailId", P_EmailId);
            command.Parameters.AddWithValue("P_AdharNo", P_AdharNo);
            command.Parameters.AddWithValue("P_PANNO", P_PANNO);
            command.Parameters.AddWithValue("P_BankName", P_BankName);
            command.Parameters.AddWithValue("P_BankACNo", P_BankACNo);
            command.Parameters.AddWithValue("P_UPINo", P_UPINo);
            command.Parameters.AddWithValue("P_IFSCCOde", P_IFSCCOde);
            
            command.Parameters.AddWithValue("P_UserCode", UserCode);
            command.Parameters.AddWithValue("P_IPAdd", IPAdd);
            ds = FillDS(command, "App_update_ISDmaster");
            return ds;
        }

        public DataSet UpdateISDAggreement(string P_ISDCode, string P_AggreementAccepted,  string UserCode, string IPAdd)
        {
            MySqlCommand command = new MySqlCommand();
            command.Parameters.AddWithValue("P_ISDCode", P_ISDCode);
            command.Parameters.AddWithValue("P_AggreementAccepted", P_AggreementAccepted);
           
            command.Parameters.AddWithValue("P_UserCode", UserCode);
            command.Parameters.AddWithValue("P_IPAdd", IPAdd);
            ds = FillDS(command, "App_update_ISDAggreement");
            return ds;
        }

        public DataSet GetMailMaster(string P_MailType)
        {
            MySqlCommand command = new MySqlCommand();
            command.Parameters.AddWithValue("P_MailType", P_MailType);

            ds = FillDS(command, "usp_get_MailMaster");
            return ds;
        }

        public DataSet GetMatrixUser(string IssueType)
        {
            MySqlCommand command = new MySqlCommand();
            command.Parameters.AddWithValue("P_IssueType", IssueType);
            command.Parameters.AddWithValue("P_Level", "1");

            ds = FillDS(command, "usp_get_MatrixLevelUser");
            return ds;
        }

        public DataSet GetMatrixUserEmails(string IssueCode) // Get User Emails 
        {
            MySqlCommand command = new MySqlCommand();
            command.Parameters.AddWithValue("P_IssueType", IssueCode);
            command.Parameters.AddWithValue("P_Level", "1");

            ds = FillDS(command, "usp_get_MatrixLevelUserEmails");
            return ds;
        }

        public DataSet GetIssueConEmails(string IssueTypeCode) // Get User Emails For Issue Con
        {
            MySqlCommand command = new MySqlCommand();
            command.Parameters.AddWithValue("P_IssueTypeCode", IssueTypeCode);
            

            ds = FillDS(command, "usp_get_IssueCon_Email");
            return ds;
        }

        public DataSet InsertIssue(string IssueCode, string OldIssueCode,string DealerCode, string IssueDate, string IssueTypeCode, string IssueDescription, string IssueStatus, string CurrentUserLevel,string UserCode, string IPAdd)
        {
            MySqlCommand command = new MySqlCommand();
            command.Parameters.AddWithValue("P_IssueCode", "0");
            command.Parameters.AddWithValue("P_OldIssueCode", OldIssueCode);
            command.Parameters.AddWithValue("P_DealerCode", DealerCode);
            command.Parameters.AddWithValue("P_IssueDate", IssueDate);
            command.Parameters.AddWithValue("P_IssueTypeCode", IssueTypeCode);
            command.Parameters.AddWithValue("P_IssueDescription", IssueDescription);
            command.Parameters.AddWithValue("P_IssueStatus", IssueStatus);
            command.Parameters.AddWithValue("P_CurrentUserLevel", CurrentUserLevel);
            command.Parameters.AddWithValue("P_UserCode", UserCode);
            command.Parameters.AddWithValue("P_IPAdd", IPAdd);
            ds = FillDS(command, "usp_iu_IssueMaster");
            return ds;
        }

        public DataSet LogHit(string P_Date, string P_WebServiceName)
        {
            MySqlCommand command = new MySqlCommand();
            command.Parameters.AddWithValue("logdate", P_Date);
            command.Parameters.AddWithValue("webserviceName", P_WebServiceName);
            ds = FillDS(command, "usp_log_service");
            return ds;
        }

        public DataSet FillDS(MySqlCommand command, string strTableName)
        {
            DataSet DSGet = new DataSet();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = strTableName;
            command.Connection = new MySqlConnection(getConnectionString());
            command.Connection.Open();
            da.SelectCommand = command;

            da.Fill(DSGet, strTableName);
            command.Connection.Close();
            return DSGet;
        }
    }
}