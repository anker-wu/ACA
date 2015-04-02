using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Brettle.Web.NeatUpload
{
    public class FileUpload : WebControl
    {
        public long MaxLength
        {
            get
            {
                return Config.Current.MaxRequestLength / 1024;
            }
            set
            {
                Config.Current.MaxRequestLength = value * 1024;
            }
        }

        public override Unit Width
        {
            get
            {
                if (base.Width == Unit.Empty)
                {
                    return Unit.Pixel(738);
                }
                return base.Width;
            }
            set
            {
                base.Width = value;
            }
        }

        /// <summary>
        /// the client function called after upload finished
        /// </summary>
        public string CallbackFunc
        {
            get
            {
                return ViewState["CallbackFunc"] as string;
            }
            set
            {
                ViewState["CallbackFunc"] = value;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            double width = Width.Value;
            string url = Context.Request.ApplicationPath + "/FileUpload/FileUploadPage.aspx?width=" + width;

            if (!string.IsNullOrEmpty(CallbackFunc))
            {
                url += "&CallbackFunc=" + CallbackFunc;
            }
            Attributes["src"] = url;
            Attributes["width"] = string.Format("{0}px", width);
            Attributes["height"] = "210px";
            Attributes["frameborder"] = "0";
            Attributes["scrolling"] = "no";
            Attributes["name"] = this.ClientID;

            base.OnPreRender(e);
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (DesignMode)
            {
                writer.Write("<i>File Upload -- No Page Refresh</i>");
            }
            else
            {
                AddAttributesToRender(writer);
                writer.RenderBeginTag(HtmlTextWriterTag.Iframe);
                writer.RenderEndTag();
            }
        }
    }
}
