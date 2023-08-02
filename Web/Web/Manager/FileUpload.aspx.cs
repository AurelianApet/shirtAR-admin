using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.IO;

using retroplus.Common;

namespace Web.Manager
{
    public partial class FileUpload : PageBase
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            string strIconPath = "";
            string type = Request.Params["type"];
            string extension = System.IO.Path.GetExtension(Request.Files["upfile"].FileName);

            Boolean isCorrect = false;

            if (type == "1")
            {//jpg, png파일 
                if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png")
                {
                    isCorrect = true;
                }
            }
            else if (type == "2")
            {//pdf파일
                if (extension.ToLower() == ".pdf")
                {
                    isCorrect = true;
                }
            }
            else if (type == "3")
            {//zip파일
                if (extension.ToLower() == ".zip")
                {
                    isCorrect = true;
                }
            }
    
            if(isCorrect)
                strIconPath = uploadFile(Request.Files["upfile"], "/Data/temp", Guid.NewGuid().ToString().Replace("-", "") + extension);
            
            Response.Write(Server.UrlEncode(strIconPath));
        }
    }
}