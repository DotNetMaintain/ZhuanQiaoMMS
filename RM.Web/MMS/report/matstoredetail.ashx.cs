using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RM.Busines;
using System.Text;
using RM.ServiceProvider.Model;
using System.Data;
using System.Web.Script.Serialization;

namespace RM.Web.MMS.report
{
    /// <summary>
    /// matstoredetail 的摘要说明
    /// </summary>
    public class matstoredetail : IHttpHandler
    {

        DataTable dt = new DataTable();
        public List<Inventorydetail> dlist = new List<Inventorydetail>();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string productcode = context.Request["productcode"];
            dlist= getInventorydetail(Convert.ToInt32(productcode));
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsson;
            jsson = jss.Serialize(dlist);
            context.Response.Write(jsson);
        }

        protected List<Inventorydetail> getInventorydetail(int productcode)
        {
            string strSql = @"select content.operatedate,userinfo.user_name,content.invoicecode,detail.quantity,detail.price,detail.lot,detail.productcode,material.material_name,Client.ValueName from mms_purchasedetail detail
 INNER JOIN MMS_PurchaseContent content on detail.purchasebillcode=content.purchasebillcode
INNER JOIN Base_UserInfo userinfo on userinfo.User_Code=content.checkman
INNER JOIN MMS_MaterialInfo material on material.material_id=detail.productcode
inner JOIN (select * from Base_DictionaryInfo where (TypeName = '客户分类')) Client on content.Provider=Client.DictionaryInfo_ID where productcode={0}";

            strSql = string.Format(strSql, productcode);
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(strSql));


            List<Inventorydetail> ulist = new List<Inventorydetail>();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Inventorydetail e = new Inventorydetail();
                    e.invoicecode = dt.Rows[i]["invoicecode"].ToString();
                    e.lot = dt.Rows[i]["lot"].ToString();
                    e.material_name = dt.Rows[i]["material_name"].ToString();
                    e.price = Convert.ToInt32(dt.Rows[i]["price"]);
                    e.quantity = Convert.ToInt32(dt.Rows[i]["quantity"]);
                    e.productcode = Convert.ToInt32(dt.Rows[i]["productcode"]);
                    e.operatedate = dt.Rows[i]["operatedate"].ToString();
                    e.ValueName = dt.Rows[i]["ValueName"].ToString();
                    e.user_name = dt.Rows[i]["user_name"].ToString();
                    ulist.Add(e);
                }
            }
            dlist = ulist;
            return ulist;
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