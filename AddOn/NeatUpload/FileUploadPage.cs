using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;

namespace Brettle.Web.NeatUpload
{
    public class FileUploadPage : System.Web.UI.Page
    {
        private const int DEFAULT_WIDTH = 738;
        private const int INFLATE_WIDTH = 75;
        private const int PROGRESSBAR_DEFAULT_WIDTH = 733;

        protected InputFile inputFile;
        protected ProgressBar progressBar;
        protected HiddenField hfCallbackFunc;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string width = Request["width"];
                if (!string.IsNullOrEmpty(width) && width != DEFAULT_WIDTH.ToString())
                { 
                    int w = Convert.ToInt32(width);
                    progressBar.Width = Unit.Pixel(w * PROGRESSBAR_DEFAULT_WIDTH / DEFAULT_WIDTH);
                    inputFile.Width = Unit.Pixel((int)progressBar.Width.Value - INFLATE_WIDTH);
                }
            }
            hfCallbackFunc.Value = "";
        }
        protected void submitButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid && inputFile.HasFile)
            {
                string fileName = Path.Combine(Request.PhysicalApplicationPath + "UploadData", inputFile.FileName);
                inputFile.MoveTo(fileName, MoveToOptions.Overwrite);
                hfCallbackFunc.Value = string.Format("Callback('{0}','{1}');", fileName.Replace("\\","\\\\"), Request["CallbackFunc"]);
            }
        }

        [WebMethod]
        public static string Save(string fileName, string description)
        {
            return "{\"FileName\":\"" + fileName + "\",\"Description\":\"" + description + "\"}";
        }
    }
}
