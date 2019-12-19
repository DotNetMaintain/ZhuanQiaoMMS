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
using System.IO;
using System.Linq;
using RM.Common.DotNetCode;

namespace RM.Web.MMS.StorageManagement
{
    public partial class StorageQuery : System.Web.UI.Page
    {
        DataTable dt = new DataTable();
        public List<storedetail> ulist = new List<storedetail>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ulist = bind("");
            }
            else
            {
                ulist = bind(txtorder.Text.ToString().Trim());
            }
        }
        public List<storedetail> bind(string str)
        {
            DataTable dt = dt_Query(ttbStartDate.Text.ToString().Trim(), ttbInStorageID.Text.ToString().Trim(),str);
            List<storedetail> ulist = new List<storedetail>();
            if (dt.Rows.Count>0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    storedetail s = new storedetail();
                    s.PurchaseDate = dt.Rows[i]["PurchaseDate"].ToString();
                    s.PurchaseBillCode = dt.Rows[i]["PurchaseBillCode"].ToString();
                    s.Material_Supplier = dt.Rows[i]["valuename"].ToString();
                    s.id = Convert.ToInt32(dt.Rows[i]["id"]);
                    ulist.Add(s);
                }
            }
            return ulist;
        }

        protected void lbtSearch_Click(object sender, EventArgs e)
        {
            ulist = bind(txtorder.Text.ToString().Trim());


        }


        protected DataTable dt_Query(string startdate, string instorage,string str)
        {
            int count = 0;
            string userremark = RequestSession.GetSessionUser().UserRemark.ToString();
            string strSql = @"select content.id,content.purchasebillcode,content.purchasedate,Client.valuename,d.Material_Attr01 from MMS_PurchaseContent content
 INNER JOIN (select * from Base_DictionaryInfo where (TypeName = '客户分类')) Client on content.Provider=Client.DictionaryInfo_ID
INNER JOIN (select detail.purchasebillcode,info.Material_Attr01 from mms_purchasedetail detail INNER JOIN MMS_MaterialInfo info on detail.productcode=info.material_id GROUP BY detail.purchasebillcode,info.Material_Attr01) d on d.purchasebillcode=content.purchasebillcode where 1=1";

   string sqlwhere = string.Empty;
            if (!string.IsNullOrEmpty(startdate))
            {
                startdate = startdate + "-26";
                DateTime date2 = Convert.ToDateTime(startdate).AddMonths(-1);
                sqlwhere = sqlwhere + @"and PurchaseDate>='" + date2 + "'and PurchaseDate<'" + startdate + "'";
            }
            else
            {
                startdate = Convert.ToDateTime(DataFactory.SqlDataBase().GetObjectValue(new StringBuilder("select getdate()"))).ToString("yyyy-MM") + "-26";
                DateTime date2 = Convert.ToDateTime(startdate).AddMonths(-1);
                sqlwhere = sqlwhere + @"and PurchaseDate>='" + date2 + "'and PurchaseDate<'" + startdate + "'";

            }
            if (!string.IsNullOrEmpty(instorage))
            {
                sqlwhere = sqlwhere + @"and content.PurchaseBillCode like '%" + instorage + "%'";
            }
            if (!string.IsNullOrEmpty(userremark))
            {
                sqlwhere = sqlwhere + @"and Material_Attr01 like '%" + userremark + "%'";
            }
            IList<SqlParam> IList_param = new List<SqlParam>();
            strSql += sqlwhere;
            if (str == "gys")
            {
                dt = DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray<SqlParam>(), "valuename", "asc", this.OurPager1.CurrentPageIndex, this.OurPager1.PageSize, ref count);
            }
            else {
                dt = DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray<SqlParam>(), "PurchaseBillCode", "Desc", this.OurPager1.CurrentPageIndex, this.OurPager1.PageSize, ref count);
            }
            this.OurPager1.RecordCount = Convert.ToInt32(count);
            return dt;
        }

        public void ExportToExcel(string tableHeader, string tableContent, string sheetName)
        {
            string fileName = DateTime.Now.ToString();
            //string tabData = htmlTable;
            if (tableContent != null)
            {
                StringWriter sw = new System.IO.StringWriter();
                sw.WriteLine("<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" /></head><body>");
                sw.WriteLine("<table style=\"border:0.5px solid black;\">");
                sw.WriteLine("<tr style=\"background-color: #e4ecf7; text-align: center; font-weight: bold\">");
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
            string tableHeader = "<td style=\"border:0.5px solid black;\">入库单号</td><td style=\"border:0.5px solid black;\">供应商</td><td style=\"border:0.5px solid black;\">入库日期</td>";
            string tableContent = "";
            if (ulist.Count>0)
            {
                foreach (var item in ulist)
                {
                    tableContent += "<tr><td style=\"border:0.5px solid black;\">" + item.PurchaseBillCode + "</td><td style=\"border:0.5px solid black;\">" + item.Material_Supplier + "</td><td style=\"border:0.5px solid black;\">" + item.PurchaseDate + "</td></tr>";

                }

            }
            ExportToExcel(tableHeader, tableContent, "实时库存报表");
        }

        protected void OurPager1_PageChanged(object sender, ServerControl.PageArgs e)
        {
            ulist = bind(txtorder.Text.ToString().Trim());

        }

        protected void orderbygys_Click(object sender, EventArgs e)
        {
            txtorder.Text = "gys";
            ulist = bind(txtorder.Text.ToString().Trim());
        }

        protected void orderbcode_Click(object sender, EventArgs e)
        {
            txtorder.Text = "";
            ulist = bind(txtorder.Text.ToString().Trim());
        }
    }
}