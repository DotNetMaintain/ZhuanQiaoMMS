using RM.Busines;
using RM.ServiceProvider.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RM.Web.MMS.StorageManagement
{
    public partial class MatStorequerydetails : System.Web.UI.Page
    {
        public List<Inventorydetail> ulist = new List<Inventorydetail>();
        public Inventorydetail inventorydetail = new Inventorydetail();
        DataTable dt = new DataTable();
        private int id
        {
            get
            {
                if (ViewState["id"] == null || ViewState["id"].ToString() == "")
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(ViewState["id"]);
                }
            }
            set { ViewState["id"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            id = Convert.ToInt32(Request.QueryString["ID"]);
            ulist = getInventorydetail(id);
            inventorydetail = ulist[0];
        }
        protected List<Inventorydetail> getInventorydetail(int PurchaseBillCode)
        {
            string strSql = @"select content.operatedate,content.id,content.PurchaseBillCode,detail.quantity*detail.price as amount,userinfo.user_name,content.invoicecode,detail.quantity,detail.price,detail.lot,detail.productcode,material.Material_Unit,material.Material_Specification,material.material_name,Client.ValueName,detail.validdate from mms_purchasedetail detail
 INNER JOIN MMS_PurchaseContent content on detail.purchasebillcode=content.purchasebillcode
INNER JOIN Base_UserInfo userinfo on userinfo.User_Code=content.checkman
INNER JOIN MMS_MaterialInfo material on material.material_id=detail.productcode
inner JOIN (select * from Base_DictionaryInfo where (TypeName = '客户分类')) Client on content.Provider=Client.DictionaryInfo_ID where content.id='{0}' GROUP BY content.operatedate,content.id,content.PurchaseBillCode,userinfo.user_name,content.invoicecode,detail.quantity,detail.price,detail.lot,detail.productcode,material.Material_Unit,material.Material_Specification,material.material_name,Client.ValueName,detail.validdate ORDER BY content.operatedate";

            strSql = string.Format(strSql, PurchaseBillCode);
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(strSql));


            List<Inventorydetail> ulist = new List<Inventorydetail>();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Inventorydetail e = new Inventorydetail();
                    e.invoicecode = dt.Rows[i]["invoicecode"].ToString();
                    e.PurchaseBillCode = dt.Rows[i]["PurchaseBillCode"].ToString();
                    e.lot = dt.Rows[i]["lot"].ToString();
                    e.material_name = dt.Rows[i]["material_name"].ToString();
                    e.price = Convert.ToDouble(dt.Rows[i]["price"]);
                    e.amount = Convert.ToDouble(dt.Rows[i]["amount"]);
                    e.quantity = Convert.ToInt32(dt.Rows[i]["quantity"]);
                    e.productcode = Convert.ToInt32(dt.Rows[i]["productcode"]);
                    e.operatedate = dt.Rows[i]["operatedate"].ToString();
                    e.ValueName = dt.Rows[i]["ValueName"].ToString();
                    e.user_name = dt.Rows[i]["user_name"].ToString();
                    e.Material_Specification = dt.Rows[i]["Material_Specification"].ToString();
                    e.Material_Unit = dt.Rows[i]["Material_Unit"].ToString();
                    e.validdate = dt.Rows[i]["validdate"].ToString();
                    ulist.Add(e);
                }
            }
            return ulist;
        }
    }
}