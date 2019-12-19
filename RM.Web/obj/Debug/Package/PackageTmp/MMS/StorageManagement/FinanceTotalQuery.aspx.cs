using System;
using System.Data.SqlClient;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using RM.Busines;
using System.Text;
using RM.Common.DotNetBean;

namespace RM.Web.MMS.StorageManagement
{
    public partial class FinanceTotalQuery : System.Web.UI.Page
    {

        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {

            dt = dt_Query(ttbStartDate.Text.ToString().Trim(), ttbEndDate.Text.ToString().Trim());
           
            ReportDocument reportDoc = new ReportDocument();
            reportDoc.Load(Server.MapPath("Rpt_FinanceTotal.rpt"));
            reportDoc.SetDataSource(dt);
            string TEXT_OBJECT_NAME = @"FinanceDate";
            TextObject text;
            text = reportDoc.ReportDefinition.ReportObjects[TEXT_OBJECT_NAME] as TextObject;
            text.Text = DateTime.Now.Year.ToString() + "年" + DateTime.Now.Month.ToString() + "月";


            CrystalReportViewer1.ReportSource = reportDoc;
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            DataTable ds = dt_Query(ttbStartDate.Text.ToString().Trim(), ttbEndDate.Text.ToString().Trim());
            ReportDocument reportDoc = new ReportDocument();
            reportDoc.Load(Server.MapPath("Rpt_FinanceTotal.rpt"));
            reportDoc.SetDataSource(ds);
            string TEXT_OBJECT_NAME = @"FinanceDate";
            TextObject text;
            text = reportDoc.ReportDefinition.ReportObjects[TEXT_OBJECT_NAME] as TextObject;
            text.Text = Convert.ToDateTime(ttbStartDate.Text.ToString().Trim()).Year.ToString() + "年" + Convert.ToDateTime(ttbStartDate.Text.ToString().Trim()).Month.ToString() + "月";


            CrystalReportViewer1.ReportSource = reportDoc;
        }





        protected DataTable dt_Query(string beginyear, string beginmonth)
        {
            string userremark = RequestSession.GetSessionUser().UserRemark.ToString();
            string strSql =
               @"select  plancontent.deptName,sum(devlivery.Quantity*devlivery.Price) as Amount from MMS_PurchasePlanContent plancontent inner join 
(select * from MMS_PurchasePlanDetail where  AuditFlag IS NULL  or AuditFlag=1) detail on plancontent.PurchaseBillCode=detail.PurchaseBillCode
inner join MMS_Delivery_Detail  devlivery
on detail.PurchaseBillCode=devlivery.PurchaseBillCode and detail.ProductCode=devlivery.ProductCode and detail.price=devlivery.price
where  devlivery.OperatorDate>='{0}' and devlivery.OperatorDate<='{1}'
group by DeptName";
            if (!string.IsNullOrEmpty(userremark))
            {
                strSql =
               @"select  plancontent.deptName,sum(devlivery.Quantity*devlivery.Price) as Amount,Material_Attr01 from MMS_PurchasePlanContent plancontent inner join 
(select * from MMS_PurchasePlanDetail where  AuditFlag IS NULL  or AuditFlag=1) detail on plancontent.PurchaseBillCode=detail.PurchaseBillCode
inner join MMS_Delivery_Detail  devlivery
on detail.PurchaseBillCode=devlivery.PurchaseBillCode and detail.ProductCode=devlivery.ProductCode and detail.price=devlivery.price
inner join MMS_MaterialInfo  material
on material.Material_ID=devlivery.ProductCode
where  devlivery.OperatorDate>='{0}' and devlivery.OperatorDate<='{1}' and Material_Attr01 like '%{2}%'
group by DeptName,Material_Attr01";
                strSql = string.Format(strSql, beginyear, beginmonth, userremark);
            }
            else
            {
                strSql = string.Format(strSql, beginyear, beginmonth);

            }
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(strSql));

            return dt;
        }



    }
}