using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OVOT_SERVICE
{
    public partial class testupload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

       
        protected void btupload_Click(object sender, EventArgs e)
        {
            //string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);

            ////Get the content type of the File.
            //string contentType = FileUpload1.PostedFile.ContentType;

            ////Read the file data into Byte Array.
            //BinaryReader br = new BinaryReader(FileUpload1.PostedFile.InputStream);
            //byte[] bytes = br.ReadBytes((int)FileUpload1.PostedFile.InputStream.Length);

            ////Call the Web Service and pass the File data for upload.
            //Index idex = new Index();
            //idex.UploadFileAndUpdateFilePath(fileName, bytes, "UPL0000009");
           
        }
    }
}