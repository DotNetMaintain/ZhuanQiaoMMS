using RM.Busines;
using RM.Common.DotNetBean;
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
    public partial class MaterailRequisitiondetail : System.Web.UI.Page
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
            string userremark = RequestSession.GetSessionUser().UserRemark.ToString();
            string strSql = @"select material.Material_Specification,material.Material_Unit,userinfo.User_Name,material.Material_Name,detail.AuditFlag,detail.CheckQuantity,detail.ProductCode,detail.Price,detail.Quantity,detail.Price*detail.Quantity as amount,content.DeptName,content.ID,content.OperateDate,content.Operator,content.PurchaseBillCode from MMS_PurchasePlanDetail detail INNER JOIN MMS_PurchasePlanContent content on detail.PurchaseBillCode=content.PurchaseBillCode
INNER JOIN (SELECT material_id,Material_Name,Material_Specification,Material_Unit from MMS_MaterialInfo where Material_Attr01='{0}') material on material.material_id=detail.ProductCode
INNER JOIN Base_UserInfo userinfo on userinfo.User_ID=content.Operator where  content.id='{1}' ORDER BY content.OperateDate DESC";

            strSql = string.Format(strSql, userremark, PurchaseBillCode);
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(strSql));


            List<Inventorydetail> ulist = new List<Inventorydetail>();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Inventorydetail e = new Inventorydetail();
                    e.PurchaseBillCode = dt.Rows[i]["PurchaseBillCode"].ToString();
                    e.material_name = dt.Rows[i]["material_name"].ToString();
                    e.price = Convert.ToDouble(dt.Rows[i]["price"]);
                    e.amount = Convert.ToDouble(dt.Rows[i]["amount"]);
                    e.quantity = Convert.ToInt32(dt.Rows[i]["quantity"]);
                    e.productcode = Convert.ToInt32(dt.Rows[i]["productcode"]);
                    e.operatedate = dt.Rows[i]["OperateDate"].ToString();
                    e.ValueName = dt.Rows[i]["DeptName"].ToString();
                    e.user_name = dt.Rows[i]["user_name"].ToString();
                    e.Material_Specification = dt.Rows[i]["Material_Specification"].ToString();
                    e.Material_Unit = dt.Rows[i]["Material_Unit"].ToString();
                    ulist.Add(e);
                }
            }
            return ulist;
        }
    }
}