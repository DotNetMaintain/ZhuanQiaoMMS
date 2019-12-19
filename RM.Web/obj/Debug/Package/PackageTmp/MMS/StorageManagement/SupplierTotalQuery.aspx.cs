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
    public partial class SupplierTotalQuery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet ds = ds_Query(ttbStartDate.Text.ToString().Trim(), ttbEndDate.Text.ToString().Trim());
            ReportDocument reportDoc = new ReportDocument();
            reportDoc.Load(Server.MapPath("Rpt_SupplierTotalQuery.rpt"));
            reportDoc.SetDataSource(ds.Tables[0]);
            string TEXT_OBJECT_NAME = @"BeginDate";
            TextObject text;
            text = reportDoc.ReportDefinition.ReportObjects[TEXT_OBJECT_NAME] as TextObject;
            text.Text = ttbStartDate.Text.ToString().Trim();

            string TEXT_OBJECT_NAME2 = @"EndDate";
            TextObject text2;
            text2 = reportDoc.ReportDefinition.ReportObjects[TEXT_OBJECT_NAME2] as TextObject;
            text2.Text = ttbEndDate.Text.ToString().Trim();



            CrystalReportViewer1.ReportSource = reportDoc;
            
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            DataSet ds=ds_Query(ttbStartDate .Text .ToString().Trim(),ttbEndDate.Text .ToString ().Trim());
            ReportDocument reportDoc = new ReportDocument();
            reportDoc.Load(Server.MapPath("Rpt_SupplierTotalQuery.rpt"));
            reportDoc.SetDataSource(ds.Tables[0]);
            string TEXT_OBJECT_NAME = @"BeginDate";
            TextObject text;
            text = reportDoc.ReportDefinition.ReportObjects[TEXT_OBJECT_NAME] as TextObject;
            text.Text = ttbStartDate.Text.ToString().Trim();

            string TEXT_OBJECT_NAME2 = @"EndDate";
            TextObject text2;
            text2 = reportDoc.ReportDefinition.ReportObjects[TEXT_OBJECT_NAME2] as TextObject;
            text2.Text = ttbEndDate.Text.ToString().Trim();



            CrystalReportViewer1.ReportSource = reportDoc;
        }


        protected DataSet ds_Query(string begindate,string enddate)
        {
            string userremark = RequestSession.GetSessionUser().UserRemark.ToString();
            string str = @"select PurCon.PurchaseDate,client.ValueName as Material_Supplier,purcon.InvoiceCode as InvoiceCode,(detail.Quantity* detail.price) as Amount,MMS_MaterialInfo.Material_Attr01 from MMS_PurchaseContent PurCon INNER JOIN  MMS_PurchaseDetail detail
                             on PurCon.PurchaseBillCode=detail.PurchaseBillCode
                            inner join dbo.MMS_MaterialInfo on detail.ProductCode=MMS_MaterialInfo.Material_ID
                            LEFT JOIN (select * from Base_DictionaryInfo where (TypeName = '客户分类')) Client on PurCon.Provider=Client.DictionaryInfo_ID
                             where PurchaseDate>='{0}' and PurchaseDate<='{1}'";
            if (!string.IsNullOrEmpty(userremark))
            {
                str = str + @"and Material_Attr01 like '%{2}%'";
                str = string.Format(str, begindate, enddate, userremark);
            }
            else
            {
                str = string.Format(str, begindate, enddate);

            }
            DataSet ds = DataFactory.SqlDataBase().GetDataSetBySQL(new StringBuilder(str));
            return ds;
        
        }



    }
}