using System;
using System.Data.SqlClient;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using RM.Busines;
using System.Text;

namespace RM.Web.MMS.StorageManagement
{
    public partial class DeliveryDeptMaterialTotal : System.Web.UI.Page
    {
        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {

            dt = dt_Query(ttbStartDate.Text.ToString().Trim(), ttbEndDate.Text.ToString().Trim());
            ReportDocument reportDoc = new ReportDocument();
            reportDoc.Load(Server.MapPath("Rpt_DeliveryDeptMaterialTotal.rpt"));
            reportDoc.SetDataSource(dt);
            string TEXT_OBJECT_NAME = @"FinanceDate";
            TextObject text;
            text = reportDoc.ReportDefinition.ReportObjects[TEXT_OBJECT_NAME] as TextObject;
            text.Text = DateTime.Now.Year.ToString() + "年" + DateTime.Now.Month.ToString() + "月";


            CrystalReportViewer1.ReportSource = reportDoc;
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            DataTable ds = dt_Query(ttbStartDate.Text.ToString().Trim(),ttbEndDate.Text.ToString().Trim());
            ReportDocument reportDoc = new ReportDocument();
            reportDoc.Load(Server.MapPath("Rpt_DeliveryDeptMaterialTotal.rpt"));
            reportDoc.SetDataSource(ds);
            string TEXT_OBJECT_NAME = @"FinanceDate";
            TextObject text;
            text = reportDoc.ReportDefinition.ReportObjects[TEXT_OBJECT_NAME] as TextObject;
            text.Text = Convert.ToDateTime(ttbStartDate.Text.ToString().Trim()).Year.ToString() + "年" + Convert.ToDateTime(ttbStartDate.Text.ToString().Trim()).Month.ToString() + "月";


            CrystalReportViewer1.ReportSource = reportDoc;
        }





        protected DataTable dt_Query(string beginyear, string beginmonth)
        {
            string strSql =
               @"select  plancontent.deptName,substring(mat.Material_Type,1,LEN(mat.Material_Type)-5) as material_type,material_name,sum(devlivery.Quantity*devlivery.Price) amount from MMS_PurchasePlanContent plancontent inner join 
(select * from MMS_PurchasePlanDetail where  AuditFlag IS NULL  or AuditFlag=1) detail on plancontent.PurchaseBillCode=detail.PurchaseBillCode
inner join MMS_Delivery_Detail  devlivery
on detail.PurchaseBillCode=devlivery.PurchaseBillCode and detail.ProductCode=devlivery.ProductCode
inner join MMS_MaterialInfo mat on detail.productcode=mat.material_id
where  devlivery.OperatorDate>='{0}' and devlivery.OperatorDate<='{1}'
group by DeptName,substring(Material_Type,1,LEN(Material_Type)-5),material_name";
            strSql = string.Format(strSql, beginyear, beginmonth);
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(strSql));

            return dt;
        }

    }
}