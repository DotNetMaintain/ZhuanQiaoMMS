using RM.Busines.DAL;
using RM.Busines.DAO;
using RM.Common.DotNetCode;
using RM.Common.DotNetUI;
using RM.Web.App_Code;
using RM.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using RM.ServerControl;
using RM.ServiceProvider.Model;
using RM.Busines;
using RM.Common.DotNetBean;
using System.IO;

namespace RM.Web.MMS.StorageManagement
{
    public partial class StorageInfo : PageBase
    {
        private string _key;
        private RM_System_IDAO system_idao = new RM_System_Dal();
        private MMS_StoreTransation_Dal storetransation_idao = new MMS_StoreTransation_Dal();
        private string _Invoice_ID;
        // protected PageControl PageControl1;
        protected Repeater rp_Item;
        public List<deliveryquery> ulist = new List<deliveryquery>();
        public decimal totalamount = 0;



        protected void Page_Load(object sender, EventArgs e)
        {

            // this.PageControl1.pageHandler += new EventHandler(this.pager_PageChanged);

            this._Invoice_ID = base.Request["StorageForm_ID"];

            if (!base.IsPostBack)
            {

                Init_StorageInfo();
                this.DataBindGrid();
                //  this.PageControl1.pageHandler += new EventHandler(this.pager_PageChanged);
            }

        }



        private void Init_StorageInfo()
        {
            ddlStates.Items.Add("未发料");
            ddlStates.Items.Add("已发料");
            ddlStates.Items.Add("退回");

            BindDept(ddlDeptName);

            ddlDeptName.Items.Add(new ListItem("", ""));
            //ddlDeptName.Items.Remove(new ListItem("北桥一病区", "北桥一病区"));
            //ddlDeptName.Items.Remove(new ListItem("北桥二病区", "北桥二病区"));
            ddlDeptName.SelectedValue = "";
        }


        private void BindDept(DropDownList ddl)
        {
            DataTable afterdt = new DataTable();
            StringBuilder SqlWhere = new StringBuilder();
            // SqlWhere.Append(" and CheckQuantity=0 and (AuditFlag is null or AuditFlag>'0')");
            DataTable dt = this.storetransation_idao.GetStoreDeliveryPage(SqlWhere);
            DataView dv = new DataView(dt);
            dv.Sort = "DeptName";

            afterdt = dv.ToTable(true, "DeptName");
            ddl.DataSource = afterdt; //设置下拉框的数据源
            ddl.DataTextField = "DeptName";
            ddl.DataValueField = "DeptName";
            ddl.DataBind(); //下拉框数据绑定
        }


        protected void ddlDeptName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        protected void pager_PageChanged(object sender, EventArgs e)
        {
            this.DataBindGrid();
        }
        private void DataBindGrid(string store="")
        {
            string sqlcondition = string.Empty;
            string statues = string.Empty;

            int count = 0;
            //发货的状态
            switch (ddlStates.Text.ToString().Trim())
            {

                case "已发料":
                    statues = "已发料";
                    break;
                case "未发料":
                    statues = @" and CheckQuantity=0 and (AuditFlag is null or AuditFlag>'0')";
                    break;
                case "退回":
                    statues = "退回";
                    break;

            };

            



            StringBuilder SqlWhere = new StringBuilder();
            IList<SqlParam> IList_param = new List<SqlParam>();
            if (ddlDeptName.SelectedValue.ToString().Trim() != "")
            {
                string sql = @" and DeptName like '%{0}%'";
                sql = string.Format(sql, ddlDeptName.SelectedValue.ToString().Trim());
                sqlcondition = sqlcondition + sql;
            }
            else if (txtInvoiceID.Text.ToString().Trim() != "")
            {
                string sql_voice = @" and PurchaseBillCode like '%{0}%'";
                sql_voice = string.Format(sql_voice, txtInvoiceID.Text.ToString().Trim());
                sqlcondition = sqlcondition + sql_voice;
            }
            else if (txtPurchasePlanDate.Text.ToString().Trim() != "")
            {
                string date = txtPurchasePlanDate.Text.ToString().Substring(0, txtPurchasePlanDate.Text.ToString().IndexOf(' '));
                string sql = @" and purchasedate >= '{0}' and purchasedate < '{1}'";
                sql = string.Format(sql, date,Convert.ToDateTime(date).AddDays(1));
                sqlcondition = sqlcondition + sql;
            }
            DataTable dt = new DataTable();

            if (statues == "已发料")
            {
                statues = @" and CheckQuantity>0 and (AuditFlag is null or AuditFlag>'0')";
                sqlcondition = sqlcondition + statues;
                SqlWhere.Append(sqlcondition);
                dt = this.storetransation_idao.GetStoreDeliveryPage(SqlWhere, IList_param, this.OurPager1.CurrentPageIndex, this.OurPager1.PageSize, ref count);
                ulist = new List<deliveryquery>();
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        deliveryquery e = new deliveryquery();
                        e.DeptName = dt.Rows[i]["DeptName"].ToString();
                        e.PurchaseBillCode = dt.Rows[i]["PurchaseBillCode"].ToString();
                        e.PurchaseDate = dt.Rows[i]["PurchaseDate"].ToString();
                        e.material_name = dt.Rows[i]["material_name"].ToString();
                        e.auditflag = dt.Rows[i]["auditflag"].ToString();
                        e.material_type = dt.Rows[i]["material_type"].ToString();
                        e.Material_Specification = dt.Rows[i]["Material_Specification"].ToString();
                        e.Material_Unit = dt.Rows[i]["Material_Unit"].ToString();
                        e.quantity = Convert.ToInt32(dt.Rows[i]["quantity"]);
                        e.checkquantity = Convert.ToInt32(dt.Rows[i]["checkquantity"]);
                        e.price = Convert.ToDouble(dt.Rows[i]["price"]);
                        e.id = Convert.ToInt32(dt.Rows[i]["id"]);
                        e.qua = Convert.ToInt32(dt.Rows[i]["qua"]);
                        e.amount = Convert.ToDouble(dt.Rows[i]["amount"]);
                        ulist.Add(e);
                    }

                }
            } else if (statues == "退回")
            {
                statues = @" and AuditFlag='0'";
                sqlcondition = sqlcondition + statues;
                SqlWhere.Append(sqlcondition);
                dt = this.storetransation_idao.GetStoreDeliverysPage(SqlWhere, IList_param, this.OurPager1.CurrentPageIndex, this.OurPager1.PageSize, ref count);
                ulist = new List<deliveryquery>();

                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        deliveryquery e = new deliveryquery();
                        e.DeptName = dt.Rows[i]["DeptName"].ToString();
                        e.PurchaseBillCode = dt.Rows[i]["PurchaseBillCode"].ToString();
                        e.PurchaseDate = dt.Rows[i]["PurchaseDate"].ToString();
                        e.material_name = dt.Rows[i]["material_name"].ToString();
                        e.auditflag = dt.Rows[i]["auditflag"].ToString();
                        e.material_type = dt.Rows[i]["material_type"].ToString();
                        e.Material_Specification = dt.Rows[i]["Material_Specification"].ToString();
                        e.Material_Unit = dt.Rows[i]["Material_Unit"].ToString();
                        e.quantity = Convert.ToInt32(dt.Rows[i]["quantity"]);
                        e.checkquantity = Convert.ToInt32(dt.Rows[i]["checkquantity"]);
                        e.id = Convert.ToInt32(dt.Rows[i]["id"]);
                        e.price = Convert.ToDouble(dt.Rows[i]["price"]);
                        e.qua = Convert.ToInt32(dt.Rows[i]["qua"]);
                        e.amount = Convert.ToDouble(dt.Rows[i]["amount"]);
                        e.PurchaseDate = dt.Rows[i]["PurchaseDate"].ToString();
                        ulist.Add(e);
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(store))
                {
                    string sql = " and quantity<=qua";
                    sqlcondition = sqlcondition + sql;
                }
                sqlcondition = sqlcondition + statues;
                SqlWhere.Append(sqlcondition);
                dt = this.storetransation_idao.GetStoreDeliveryPages(SqlWhere, IList_param, this.OurPager1.CurrentPageIndex, this.OurPager1.PageSize, ref count);
                ulist = new List<deliveryquery>();

                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        deliveryquery e = new deliveryquery();
                        e.DeptName = dt.Rows[i]["DeptName"].ToString();
                        e.PurchaseBillCode = dt.Rows[i]["PurchaseBillCode"].ToString();
                        e.PurchaseDate = dt.Rows[i]["PurchaseDate"].ToString();
                        e.material_name = dt.Rows[i]["material_name"].ToString();
                        e.auditflag = dt.Rows[i]["auditflag"].ToString();
                        e.material_type = dt.Rows[i]["material_type"].ToString();
                        e.Material_Specification = dt.Rows[i]["Material_Specification"].ToString();
                        e.Material_Unit = dt.Rows[i]["Material_Unit"].ToString();
                        e.quantity = Convert.ToInt32(dt.Rows[i]["quantity"]);
                        e.checkquantity = Convert.ToInt32(dt.Rows[i]["checkquantity"]);
                        e.id = Convert.ToInt32(dt.Rows[i]["id"]);
                        e.price = Convert.ToDouble(dt.Rows[i]["price"]);
                        e.qua = Convert.ToInt32(dt.Rows[i]["qua"]);
                        e.amount = Convert.ToDouble(dt.Rows[i]["amount"]);
                        e.PurchaseDate = dt.Rows[i]["PurchaseDate"].ToString();
                        ulist.Add(e);
                    }
                }
            }
            this.OurPager1.RecordCount = Convert.ToInt32(count);
        }



        protected void rp_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                totalamount = totalamount + Convert.ToDecimal(drv["amount"]);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DataBindGrid();
        }


        protected void PageControl1_PageChanged(object sender, PageArgs e)
        {
            DataBindGrid();
        }

        protected void OurPager1_PageChanged(object sender, PageArgs e)
        {
            DataBindGrid();
        }
        public void ExportToExcel(string tableHeader)
        {
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
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "UTF-8";
                Response.Charset = "GB2312";
                this.EnableViewState = false;
                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                Response.Write(sw);
                Response.End();
            }
        }

        protected void chkstore_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkstore.Checked)
            {
                DataBindGrid("1");
            }
            else
            {
                DataBindGrid("");
            }
        }
    }
}