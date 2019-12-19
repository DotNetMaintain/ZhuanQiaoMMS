using System;
using System.Data.SqlClient;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using RM.Busines;
using System.Text;
using RM.Common.DotNetBean;
using System.Collections.Generic;
using RM.ServiceProvider.Model;

namespace RM.Web.MMS.report
{
    public partial class MatStorequerydetail : System.Web.UI.Page
    {
        public List<Inventorydetail> dlist = new List<Inventorydetail>();
        public QuantityInfo q = new QuantityInfo();
        public QuantityInfo qs = new QuantityInfo();
        public List<deliveryquery> dllist = new List<deliveryquery>();
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
        private int qua
        {
            get
            {
                if (ViewState["qua"] == null || ViewState["qua"].ToString() == "")
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(ViewState["qua"]);
                }
            }
            set { ViewState["qua"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            id = Convert.ToInt32(Request.QueryString["ID"]);
            qua = Convert.ToInt32(Request.QueryString["qua"]);
            getInventorydetail(id);
            getpurchaseplandetail(id, qua);
        }
        protected List<Inventorydetail> getInventorydetail(int productcode)
        {
            string strSql = @"select content.operatedate,content.PurchaseBillCode,detail.quantity*detail.price as amount,userinfo.user_name,content.invoicecode,detail.quantity,detail.price,detail.lot,detail.productcode,material.material_name,Client.ValueName from mms_purchasedetail detail
 INNER JOIN MMS_PurchaseContent content on detail.purchasebillcode=content.purchasebillcode
INNER JOIN Base_UserInfo userinfo on userinfo.User_Code=content.checkman
INNER JOIN MMS_MaterialInfo material on material.material_id=detail.productcode
inner JOIN (select * from Base_DictionaryInfo where (TypeName = '客户分类')) Client on content.Provider=Client.DictionaryInfo_ID where productcode={0} GROUP BY content.operatedate,content.PurchaseBillCode,userinfo.user_name,content.invoicecode,detail.quantity,detail.price,detail.lot,detail.productcode,material.material_name,Client.ValueName ORDER BY content.operatedate desc";

            strSql = string.Format(strSql, productcode);
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
                    ulist.Add(e);
                }
            }
            dlist = ulist;
            string sql = "select sum(quantity) as quantity,sum(usequantity) as usequantity,sum(quantity)-sum(usequantity) as qua from mms_purchasedetail where productcode='{0}'";
            sql = string.Format(sql, productcode);
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
            if (dt.Rows.Count > 0)
            {
                q.quantity = Convert.ToInt32(dt.Rows[0]["quantity"]);
                q.usequantity = Convert.ToInt32(dt.Rows[0]["usequantity"]);
                q.qua = Convert.ToInt32(dt.Rows[0]["qua"]);
            }
            else
            {
                q.quantity = 0;
                q.usequantity = 0;
                q.qua = 0;
            }
            return ulist;
        }
        protected List<deliveryquery> getpurchaseplandetail(int productcode, int qua)
        {
            string strSql = @"select userinfo.User_Name,material.Material_Name,detail.Price,detail.AuditFlag,detail.CheckQuantity,detail.ProductCode,detail.Quantity,detail.Price*detail.Quantity as amount,content.DeptName,content.OperateDate,content.Operator,content.PurchaseBillCode from MMS_PurchasePlanDetail detail INNER JOIN MMS_PurchasePlanContent content on detail.PurchaseBillCode=content.PurchaseBillCode
INNER JOIN (SELECT material_id,Material_Name,Material_Specification,Material_Unit from MMS_MaterialInfo) material on material.material_id=detail.ProductCode
INNER JOIN Base_UserInfo userinfo on userinfo.User_ID=content.Operator where ProductCode='{0}' and CheckQuantity>0 and (detail.AuditFlag is null or detail.AuditFlag>'0')  ORDER BY OperateDate desc";

            strSql = string.Format(strSql, productcode);
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(strSql));
            List<deliveryquery> ulist = new List<deliveryquery>();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    deliveryquery e = new deliveryquery();
                    e.PurchaseBillCode = dt.Rows[i]["PurchaseBillCode"].ToString();
                    e.material_name = dt.Rows[i]["material_name"].ToString();
                    e.price = Convert.ToDouble(dt.Rows[i]["price"]);
                    e.amount = Convert.ToDouble(dt.Rows[i]["amount"]);
                    e.quantity = Convert.ToInt32(dt.Rows[i]["quantity"]);
                    e.productcode = Convert.ToInt32(dt.Rows[i]["productcode"]);
                    e.DeptName = dt.Rows[i]["DeptName"].ToString();
                    e.username = dt.Rows[i]["User_Name"].ToString();
                    e.OperatorDate = dt.Rows[i]["OperateDate"].ToString();
                    ulist.Add(e);
                }
            }
            dllist = ulist;
            string sql = "select sum(Quantity) as Quantity,ProductCode from MMS_PurchasePlanDetail where CheckQuantity>0 and (AuditFlag is null or AuditFlag>'0') and productcode='{0}' GROUP BY ProductCode";
            sql = string.Format(sql, productcode);
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
            if (dt.Rows.Count > 0)
            {
                qs.quantity = Convert.ToInt32(dt.Rows[0]["quantity"]);
                qs.qua = qua;
            }
            else
            {
                qs.quantity = 0;
                qs.qua = 0;
            }
            return ulist;
        }
    }
}