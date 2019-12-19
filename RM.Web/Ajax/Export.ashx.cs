using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace RM.Web.Ajax
{
    /// <summary>
    /// Export 的摘要说明
    /// </summary>
    public class Export : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Buffer = true;
            context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1.0);
            context.Response.AddHeader("pragma", "no-cache");
            context.Response.AddHeader("cache-control", "");
            context.Response.CacheControl = "no-cache";
            string tableHeader = context.Request["tableHeader"];
            string fileName = DateTime.Now.ToString();
            //string tabData = htmlTable;
            if (tableHeader != null)
            {
                StringWriter sw = new System.IO.StringWriter();
                sw.WriteLine("<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" /></head><body>");
                //sw.WriteLine("<table style=\"border:0.5px solid black;\">");
                //sw.WriteLine("<tr style=\"background-color: #e4ecf7; text-align: center; font-weight: bold\">");
                sw.WriteLine(tableHeader);
                //sw.WriteLine("</tr>");
                //sw.WriteLine(tableContent);
                //sw.WriteLine("</table>");
                sw.WriteLine("</body>");
                sw.WriteLine("</html>");
                sw.Close();
                context.Response.Clear();
                context.Response.Buffer = true;
                context.Response.Charset = "UTF-8";
                context.Response.Charset = "GB2312";
                //this.EnableViewState = false;
                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".xls");
                context.Response.ContentType = "application/ms-excel";
                context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                context.Response.Write(sw);
                context.Response.End();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}