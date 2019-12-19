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

namespace RM.Web.MMS.report
{
    public partial class MatStore : System.Web.UI.Page
    {
        public List<Inventory> inlist;
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
            inlist = dt_Query(ttbmaterialname.Text.ToString().Trim());
        }


        protected List<Inventory> dt_Query(string materialname)
        {
            string userremark = RequestSession.GetSessionUser().UserRemark.ToString();
            string strSql = @"select ma.materialtype_name material_type,ma.Material_Unit,ma.Material_Specification,ma.material_name material_name,mms_detail.qua qua,mms_detail.price price,ma.material_attr01 material_attr01,mms_detail.productcode material_id from (
select materialtype.materialtype_name,material.Material_Unit,material.Material_Specification,material.material_name,material.material_id productcode,material.material_attr01 from MMS_MaterialInfo material 
INNER JOIN MMS_MaterialType materialtype on substring(material.material_type,0,charindex('-',material.material_type)-1)=materialtype.materialtype_name) ma
 INNER JOIN ( select detail.price,purchasedetail.qua,detail.productcode from mms_purchasedetail detail INNER JOIN (select max(id) as id,sum(quantity)-sum(usequantity) as qua,productcode from mms_purchasedetail GROUP BY productcode) purchasedetail
 on detail.id=purchasedetail.id GROUP BY detail.productcode,detail.price,purchasedetail.qua) mms_detail on ma.productcode=mms_detail.productcode";
            if (!string.IsNullOrEmpty(userremark))
            {
                strSql += " where material_attr01={0}";
                strSql = string.Format(strSql, userremark);
            }
            else
            {
                strSql = string.Format(strSql);
            }
            if (materialname != null)
            {
                strSql += " and ma.material_name like '%{0}%'";
                strSql = string.Format(strSql, materialname);
            }

            dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(strSql));


            List<Inventory> ulist = new List<Inventory>();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Inventory e = new Inventory();
                    e.material_type = dt.Rows[i]["material_type"].ToString();
                    e.material_name = dt.Rows[i]["material_name"].ToString();
                    e.Material_Specification = dt.Rows[i]["Material_Specification"].ToString();
                    e.Material_Unit = dt.Rows[i]["Material_Unit"].ToString();
                    e.price = Convert.ToDouble(dt.Rows[i]["price"]);
                    e.qua = Convert.ToInt32(dt.Rows[i]["qua"]);
                    e.material_id = Convert.ToInt32(dt.Rows[i]["material_id"]);
                    ulist.Add(e);
                }
            }
            return ulist;
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
            string tableHeader = "<td style=\"border:0.5px solid black;\">物料类型</td><td style=\"border:0.5px solid black;\">物料名称</td><td style=\"border:0.5px solid black;\">物料规格</td><td style=\"border:0.5px solid black;\">单位</td><td style=\"border:0.5px solid black;\">库存数</td><td style=\"border:0.5px solid black;\">价格</td>";
            string tableContent = "";
            foreach (var item in inlist)
            {
                tableContent += "<tr><td style=\"border:0.5px solid black;\">" + item.material_type + "</td><td style=\"border:0.5px solid black;\">" + item.material_name + "</td><td style=\"border:0.5px solid black;\">" + item.Material_Specification + "</td><td style=\"border:0.5px solid black;\">" + item.Material_Unit + "</td><td style=\"border:0.5px solid black;\">" + item.qua + "</td><td style=\"border:0.5px solid black;\">" + item.price + "</td></tr>";

            }
            ExportToExcel(tableHeader, tableContent, "实时库存报表");
        }

        protected void chkzero_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkzero.Checked)
            {
                inlist = dt_Queryzero(ttbmaterialname.Text.ToString().Trim());
            }
            else
            {
                inlist = dt_Query(ttbmaterialname.Text.ToString().Trim());
            }

        }
        protected List<Inventory> dt_Queryzero(string materialname)
        {
            string userremark = RequestSession.GetSessionUser().UserRemark.ToString();
            string strSql = @"select ma.materialtype_name material_type,ma.Material_Unit,ma.Material_Specification,ma.material_name material_name,mms_detail.qua qua,mms_detail.price price,ma.material_attr01 material_attr01,mms_detail.productcode material_id from (
select materialtype.materialtype_name,material.Material_Unit,material.Material_Specification,material.material_name,material.material_id productcode,material.material_attr01 from MMS_MaterialInfo material 
INNER JOIN MMS_MaterialType materialtype on substring(material.material_type,0,charindex('-',material.material_type)-1)=materialtype.materialtype_name) ma
 INNER JOIN ( select detail.price,purchasedetail.qua,detail.productcode from mms_purchasedetail detail INNER JOIN (select max(id) as id,sum(quantity)-sum(usequantity) as qua,productcode from mms_purchasedetail GROUP BY productcode) purchasedetail
 on detail.id=purchasedetail.id GROUP BY detail.productcode,detail.price,purchasedetail.qua) mms_detail on ma.productcode=mms_detail.productcode where qua=0 ";
            if (!string.IsNullOrEmpty(userremark))
            {
                strSql += " and material_attr01={0}";
                strSql = string.Format(strSql, userremark);
            }
            else
            {
                strSql = string.Format(strSql);
            }
            if (materialname!=null)
            {
                strSql += " and ma.material_name like '%{0}%'";
                strSql = string.Format(strSql, materialname);
            }
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(strSql));


            List<Inventory> ulist = new List<Inventory>();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Inventory e = new Inventory();
                    e.material_type = dt.Rows[i]["material_type"].ToString();
                    e.material_name = dt.Rows[i]["material_name"].ToString();
                    e.Material_Specification = dt.Rows[i]["Material_Specification"].ToString();
                    e.Material_Unit = dt.Rows[i]["Material_Unit"].ToString();
                    e.price = Convert.ToDouble(dt.Rows[i]["price"]);
                    e.qua = Convert.ToInt32(dt.Rows[i]["qua"]);
                    e.material_id = Convert.ToInt32(dt.Rows[i]["material_id"]);
                    ulist.Add(e);
                }
            }
            return ulist;
        }

        protected void lbtSearch_Click(object sender, EventArgs e)
        {
            if (this.chkzero.Checked)
            {
                inlist = dt_Queryzero(ttbmaterialname.Text.ToString().Trim());
            }
            else
            {
                inlist = dt_Query(ttbmaterialname.Text.ToString().Trim());
            }
        }
    }
}