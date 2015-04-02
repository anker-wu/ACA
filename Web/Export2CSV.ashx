<%@ WebHandler Language="C#" Class="Export2CSV" %>

using System;
using System.Web;
using System.Configuration;
using Accela.ACA.Common.Util;

public class Export2CSV : IHttpHandler,System.Web.SessionState.IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        string name = string.Format("{0}.csv", DateTime.Now.ToString("yyyyMMdd")); 
        if (context.Session["ExportFileName"] != null)
        {
            string[] v = context.Session["ExportFileName"].ToString().Split('.');
            if (v.Length == 2)
            {
                name = v[1] + name;
            }  
        } 
        context.Response.Clear();
        context.Response.Charset = "UTF-8";
        context.Response.Buffer = true;
        context.Response.ContentType = "text/csv";
        context.Response.AppendHeader("Content-Disposition",
                                                  "attachment;filename="+HttpUtility.UrlEncode(name, System.Text.Encoding.UTF8).Replace("+", ""));
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        context.Response.Write(ReadContent(context));
        context.Response.End();
    }

    private string ReadContent(HttpContext context)  
    {
        string content = "an error has occured when exporting the result"; 
        if (context.Session["ExportFileName"] != null)
        {
            string fn = ConfigurationManager.AppSettings["TempDirectory"] + string.Format("\\{0}.csv", context.Session["ExportFileName"]);
            System.IO.StreamReader sr = new System.IO.StreamReader(fn);
            content = sr.ReadToEnd();
            content = System.Web.HttpUtility.HtmlDecode(content);
            sr.Close();
            System.IO.File.Delete(fn);
            context.Session["ExportFileName"] = null;
        }
        return content;
    } 
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
