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
using RM.Web.UserControl;
using RM.Common.DotNetBean;
using RM.Busines;
using RM.ServiceProvider.Model;
using System.IO;

namespace RM.Web.MMS.MMS_Material
{
	public class Material_List : PageBase
	{
		protected HtmlForm form1;
		protected LoadButton LoadButton1;
		public StringBuilder str_tableTree = new StringBuilder();
		private MMS_MaterialInfo_IDAO  MaterialInfo_idao = new MMS_MaterialInfo_Dal();


        protected HtmlSelect Searchwhere;
        protected HtmlInputText txt_Search;
        protected LinkButton lbtSearch;
        protected Repeater rp_Item;
        protected PageControl PageControl1;

        private string _Material_ID;
        protected void Page_Load(object sender, EventArgs e)
        {
            this._Material_ID = base.Request["Material_ID"];

            this.PageControl1.pageHandler += new EventHandler(this.pager_PageChanged);
            
            if (!base.IsPostBack)
            {
                //this.PageControl1.pageHandler += new EventHandler(this.pager_PageChanged);
            }
        }
        protected void pager_PageChanged(object sender, EventArgs e)
        {
            this.DataBindGrid();
        }
        private void DataBindGrid()
        {
            int count = 0;
            StringBuilder SqlWhere = new StringBuilder();

            string userremark = RequestSession.GetSessionUser().UserRemark.ToString();
            // SqlWhere.Append(" AND DeleteMark=1 ");
            IList<SqlParam> IList_param = new List<SqlParam>();
            if (!string.IsNullOrEmpty(this.txt_Search.Value))
            {
                SqlWhere.Append(" and U." + this.Searchwhere.Value + " like @obj ");
                IList_param.Add(new SqlParam("@obj", '%' + this.txt_Search.Value.Trim() + '%'));
            }
            if (!string.IsNullOrEmpty(this._Material_ID))
            {
                SqlWhere.Append("  AND Material_ID IN(" + this._Material_ID + ")");
            }
            if (!string.IsNullOrEmpty(userremark))
            {
                SqlWhere.Append(" and U.material_attr01 like @material_attr01 ");
                IList_param.Add(new SqlParam("@material_attr01", '%' + userremark + '%'));
            }
            DataTable dt = this.MaterialInfo_idao.GetMaterialInfoPage(SqlWhere, IList_param, this.PageControl1.PageIndex, this.PageControl1.PageSize, ref count);
            ControlBindHelper.BindRepeaterList(dt, this.rp_Item);
            this.PageControl1.RecordCount = Convert.ToInt32(count);
        }


        protected void rp_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

              
                Label lblDeleteMark = e.Item.FindControl("lblDeleteMark") as Label;
                if (lblDeleteMark != null)
                {
                    
                    string textDeleteMark = lblDeleteMark.Text;
                    textDeleteMark = textDeleteMark.Replace("1", "<span style='color:Blue'>启用</span>");
                    textDeleteMark = textDeleteMark.Replace("2", "<span style='color:red'>停用</span>");
                    lblDeleteMark.Text = textDeleteMark;
                }
            }
        }
        protected void lbtSearch_Click(object sender, EventArgs e)
        {
          
            this.DataBindGrid();
            this.PageControl1.PageChecking();

        }
        public void ExportToExcel(string tableHeader, string tableContent, string sheetName)
        {
            string fileName = DateTime.Now.ToString();
            //string tabData = htmlTable;
            if (tableContent != null)
            {
                StringWriter sw = new System.IO.StringWriter();
                sw.WriteLine("<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" /></head><body>");
                sw.WriteLine("<table style=\"border:1px solid black;\">");
                sw.WriteLine("<tr style=\"text-align: center; font-weight: bold\">");
                sw.WriteLine(tableHeader);
                sw.WriteLine("</tr>");
                sw.WriteLine(tableContent);
                sw.WriteLine("</table>");
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

        protected void btnexcel_Click(object sender, EventArgs e)
        {

            StringBuilder SqlWhere = new StringBuilder();

            string userremark = RequestSession.GetSessionUser().UserRemark.ToString();
            if (!string.IsNullOrEmpty(this.txt_Search.Value))
            {
                SqlWhere.Append(" and U." + this.Searchwhere.Value + " like @obj ");
            }
            if (!string.IsNullOrEmpty(this._Material_ID))
            {
                SqlWhere.Append("  AND Material_ID IN(" + this._Material_ID + ")");
            }
            if (!string.IsNullOrEmpty(userremark))
            {
                SqlWhere.Append(" and U.material_attr01 like " + userremark);
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select u.material_id,u.material_type,u.material_name,u.Material_CommonlyName,u.Material_Specification,u.Material_Unit,u.Material_Supplier,u.Material_SafetyStock,u.price,u.qua,u.Material_Comm from (SELECT * from MMS_MaterialInfo info INNER JOIN(select detail.qua, detail.productcode, mms_detail.price from(select max(id) as id, sum(quantity) - sum(usequantity) as qua, productcode from mms_purchasedetail GROUP BY productcode) detail INNER JOIN mms_purchasedetail mms_detail on mms_detail.id = detail.id ) details on info.material_id = details.productcode) u where 1 =1");
            strSql.Append(SqlWhere);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(strSql);

            List<materialInfo> ulist = new List<materialInfo>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                materialInfo ma = new materialInfo();
                ma.material_type = dt.Rows[i]["material_type"].ToString();
                ma.Material_Comm = dt.Rows[i]["Material_Comm"].ToString();
                ma.Material_CommonlyName = dt.Rows[i]["Material_CommonlyName"].ToString();
                ma.Material_Supplier = dt.Rows[i]["Material_Supplier"].ToString();
                ma.material_name = dt.Rows[i]["material_name"].ToString();
                ma.Material_Specification = dt.Rows[i]["Material_Specification"].ToString();
                ma.Material_Unit = dt.Rows[i]["Material_Unit"].ToString();
                ma.price = Convert.ToDouble(dt.Rows[i]["price"]);
                ma.qua = Convert.ToInt32(dt.Rows[i]["qua"]);
                ma.Material_SafetyStock = Convert.ToInt32(dt.Rows[i]["Material_SafetyStock"]);
                ma.material_id = Convert.ToInt32(dt.Rows[i]["material_id"]);
                ulist.Add(ma);
            }
            string tableHeader = "<td style=\"border:1px solid black;\">物资编码</td><td style=\"border:1px solid black;\">物资类型</td><td style=\"border:1px solid black;\">物资名称</td><td style=\"border:1px solid black;\">物资简称</td><td style=\"border:1px solid black;\">物资规格</td><td style=\"border:1px solid black;\">物资单位</td><td style=\"border:1px solid black;\">生产厂家</td><td style=\"border:1px solid black;\">安全库存</td><td style=\"border:1px solid black;\">上一次入库单价</td><td style=\"border:1px solid black;\">总库存</td><td style=\"border:1px solid black;\">备注</td>";
            string tableContent = "";
            if (ulist.Count > 0)
            {
                foreach (var item in ulist)
                {
                    tableContent += "<tr style=\"text-align: center; font-weight: bold\"><td style=\"border:1px solid black;\">" + item.material_id + "</td><td style=\"border:1px solid black;\">" + item.material_type + "</td><td style=\"border:1px solid black;\">" + item.material_name + "</td><td style=\"border:1px solid black;\">" + item.Material_CommonlyName + "</td><td style=\"border:1px solid black;\">" + item.Material_Specification + "</td><td style=\"border:1px solid black;\">" + item.Material_Unit + "</td><td style=\"border:1px solid black;\">" + item.Material_Supplier + "</td><td style=\"border:1px solid black;\">" + item.Material_SafetyStock + "</td><td style=\"border:1px solid black;\">" + item.price + "</td><td style=\"border:1px solid black;\">" + item.qua + "</td><td style=\"border:1px solid black;\">" + item.Material_Comm + "</td></tr>";

                }

            }
            ExportToExcel(tableHeader, tableContent, "实时库存报表");
        }
    }
}
