using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OVOT_SERVICE.Bll
{
    public class EmailBll
    {
        public string P_UserName { get; set; }

        public string P_IssueType { get; set; }

        public string P_EntryBy { get; set; }
        
        public string P_MailBody { get; set; }
        public string P_Mailsubject { get; set; }

        public string P_Mailto { get; set; }
        
        public string UserName { get; set; }
     
    }
}