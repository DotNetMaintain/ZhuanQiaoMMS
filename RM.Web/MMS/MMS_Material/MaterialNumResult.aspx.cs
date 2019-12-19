using System;
using System.Data.SqlClient;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using RM.Busines;
using System.Text;
using System.Web.UI.WebControls;

namespace RM.Web.MMS.MMS_Material
{
    public partial class MaterialNumResult : System.Web.UI.Page
    {
        DataTable dt = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["myrpt"] != null)
            {
                CrystalReportViewer1.ReportSource = (ReportDocument)Session["myrpt"];
            }
            if (ttbStartDate.Text.ToString().Trim() != "")
            {

                dt = dt_Query(ttbStartDate.Text.ToString().Trim(), ttbEndDate.Text.ToString().Trim());
                ReportDocument reportDoc = new ReportDocument();
                reportDoc.Load(Server.MapPath("Rpt_MaterialNumResult.rpt"));
                reportDoc.SetDataSource(dt);
                string TEXT_OBJECT_NAME = @"FinanceMonth";
                TextObject text;
                text = reportDoc.ReportDefinition.ReportObjects[TEXT_OBJECT_NAME] as TextObject;
                text.Text = Convert.ToDateTime(ttbEndDate.Text.ToString().Trim()).Year.ToString() + "年" + Convert.ToDateTime(ttbEndDate.Text.ToString().Trim()).Month.ToString() + "月";
                CrystalReportViewer1.ReportSource = reportDoc;
                Session["myrpt"] = reportDoc;
            }
            
         
            
           //
        }


      

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dt = dt_Query(ttbStartDate.Text.ToString().Trim(), ttbEndDate.Text.ToString().Trim());
            ReportDocument reportDoc = new ReportDocument();
            reportDoc.Load(Server.MapPath("Rpt_MaterialNumResult.rpt"));
            reportDoc.SetDataSource(dt);
            string TEXT_OBJECT_NAME = @"FinanceMonth";
            TextObject text;
            text = reportDoc.ReportDefinition.ReportObjects[TEXT_OBJECT_NAME] as TextObject;
            text.Text = Convert.ToDateTime(ttbEndDate.Text.ToString().Trim()).Year.ToString() + "年" + Convert.ToDateTime(ttbEndDate.Text.ToString().Trim()).Month.ToString() + "月";

           
            CrystalReportViewer1.ReportSource = reportDoc;
        }

        protected DataTable dt_Query(string starttime, string endtime)
        {


            string strSql = @"select * from (select toptype,midtype,productcode,Material_Name,price,sum(lastquantity) lastqua,sum(lastquantity*price) lastamount,sum(storequantity) inqua,sum(storequantity*price) inamount,sum(reqquantity) reqqua,sum(reqquantity*price) reqamount,sum(lastquantity+storequantity-reqquantity) curqua,sum(price*(lastquantity+storequantity-reqquantity)) curamount from (
select case when re.ProductCode is not null 
then 
re.ProductCode
else 
   req.ProductCode 
end productcode,
case when re.price is not null 
then 
re.price
else 
   req.price 
end price,
case when lastquantity is not null then  lastquantity
else 0 end lastquantity,
case when Reqquantity is not null then  Reqquantity
else 0
end storequantity,
case when Req.quantity is not null then  Req.quantity
else 0
end reqquantity
 from (
select case when laststore.ProductCode is not null 
then 
laststore.ProductCode
else 
   store.ProductCode 
end productcode,
case when laststore.price is not null 
then 
laststore.price
else 
   store.price 
end price,
case when laststore.quantity is not null then  laststore.quantity
else 0 end lastquantity,
case when store.quantity is not null then  store.quantity
else 0
end Reqquantity
from (select productcode,price,isnull(sum(price*quantity),0) as amount,isnull(sum(quantity),0) as quantity from MMS_Store where year(createdatetime)='{2}' and month(createdatetime)='{3}'
group by productcode,price) laststore
full join
(select productcode,price,isnull(sum(quantity),0) as quantity,isnull(sum(amount),0) as amount from dbo.StorageQuery where PurchaseDate>='{0}' and PurchaseDate<='{1}'
group by productcode,price) store on laststore.ProductCode=store.ProductCode and laststore.Price=store.Price) re
full join 
 (
select productcode,price,isnull(sum(quantity),0) quantity,isnull(sum(amount),0) amount  from (
select PurchaseBillCode,productcode,price,isnull(qua,0) quantity,isnull(price*qua,0) amount  from 
(select PurchaseBillCode,productcode,price,sum(quantity) qua from 
(select delivery.* from (
select purchasebillcode,productcode,sum(quantity) quantity,price from MMS_PurchasePlanDetail 
where AuditFlag IS NULL or AuditFlag=1
group by purchasebillcode,productcode,price)  PlanDetail
inner join (
select purchasebillcode,productcode,sum(quantity) quantity,price,OperatorDate from MMS_Delivery_Detail 
group by purchasebillcode,productcode,price,OperatorDate
) delivery on PlanDetail.price=delivery.price and plandetail.productcode=delivery.productcode
and plandetail.purchasebillcode=delivery.purchasebillcode
where delivery.OperatorDate>='{0}' and delivery.OperatorDate<='{1}')req
group by PurchaseBillCode,productcode,price) req)req
group by ProductCode,price) req
on re.ProductCode=req.ProductCode and re.Price=req.Price) store
inner join MMS_MaterialInfo mms
on mms.Material_ID=store.productcode
inner join dbo.view_mattype mattype on substring(mms.material_type,0,charindex('-',mms.material_type))=mattype.submat
group by toptype,midtype,productcode,Material_Name,price) mstore
where toptype <>'其他不统计类'
 ";
           
                strSql = string.Format(strSql, starttime, endtime, Convert.ToDateTime(starttime).AddDays(-1).Year, Convert.ToDateTime(starttime).AddDays(-1).Month);
        

            dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(strSql));

            return dt;

           
        }

        protected void lbtExce_Click(object sender, EventArgs e)
        {

            string strSql = @"insert into mms_store(ProductCode,price,Quantity,Amount,CreateDateTime)
            select productcode,price,lastquantity+storequantity-reqquantity quantity,price*(lastquantity+storequantity-reqquantity) Amount,'{1}' CreateDateTime from (
            select case when re.ProductCode is not null 
            then 
            re.ProductCode
            else 
               req.ProductCode 
            end productcode,
            case when re.price is not null 
            then 
            re.price
            else 
               req.price 
            end price,
            case when lastquantity is not null then  lastquantity
            else 0 end lastquantity,
            case when Reqquantity is not null then  Reqquantity
            else 0
            end storequantity,
            case when Req.quantity is not null then  Req.quantity
            else 0
            end reqquantity
             from (
            select case when laststore.ProductCode is not null 
            then 
            laststore.ProductCode
            else 
               store.ProductCode 
            end productcode,
            case when laststore.price is not null 
            then 
            laststore.price
            else 
               store.price 
            end price,
            case when laststore.quantity is not null then  laststore.quantity
            else 0 end lastquantity,
            case when store.quantity is not null then  store.quantity
            else 0
            end Reqquantity
            from (select productcode,price,isnull(sum(price*quantity),0) as amount,isnull(sum(quantity),0) as quantity from MMS_Store where year(createdatetime)='{2}' and month(createdatetime)='{3}'
            group by productcode,price) laststore
            full join
            (select productcode,price,isnull(sum(quantity),0) as quantity,isnull(sum(amount),0) as amount from dbo.StorageQuery where PurchaseDate>='{0}' and PurchaseDate<='{1}'
            group by productcode,price) store on laststore.ProductCode=store.ProductCode and laststore.Price=store.Price) re
            full join 
             (
            select productcode,price,isnull(sum(quantity),0) quantity,isnull(sum(amount),0) amount  from (
            select PurchaseBillCode,productcode,price,isnull(qua,0) quantity,isnull(price*qua,0) amount  from 
            (select PurchaseBillCode,productcode,price,sum(quantity) qua from 
            (select delivery.* from (
            select purchasebillcode,productcode,sum(quantity) quantity,price from MMS_PurchasePlanDetail 
            where AuditFlag IS NULL  or AuditFlag=1
            group by purchasebillcode,productcode,price)  PlanDetail
            inner join (
            select purchasebillcode,productcode,sum(quantity) quantity,price,OperatorDate from MMS_Delivery_Detail 
            group by purchasebillcode,productcode,price,OperatorDate
            ) delivery on PlanDetail.price=delivery.price and plandetail.productcode=delivery.productcode
            and plandetail.purchasebillcode=delivery.purchasebillcode
            where delivery.OperatorDate>='{0}' and delivery.OperatorDate<='{1}')req
            group by PurchaseBillCode,productcode,price) req)req
            group by ProductCode,price) req
            on re.ProductCode=req.ProductCode and re.Price=req.Price) store";

            //            string strSql = @"insert into mms_store(ProductCode,price,Quantity,Amount,CreateDateTime)
            //select productcode,price,lastquantity+storequantity-reqquantity quantity,price*(lastquantity+storequantity-reqquantity) Amount,'{1}' CreateDateTime,Material_Attr01 from (
            //select case when re.ProductCode is not null 
            //then 
            //re.ProductCode
            //else 
            //   req.ProductCode 
            //end productcode,
            //case when re.price is not null 
            //then 
            //re.price
            //else 
            //   req.price 
            //end price,
            //case when lastquantity is not null then  lastquantity
            //else 0 end lastquantity,
            //case when Reqquantity is not null then  Reqquantity
            //else 0
            //end storequantity,
            //case when Req.quantity is not null then  Req.quantity
            //else 0
            //end reqquantity,
            //case when re.Material_Attr01 is not null then  re.Material_Attr01
            //else re.Material_Attr01
            //end Material_Attr01
            // from (
            //select case when laststore.ProductCode is not null 
            //then 
            //laststore.ProductCode
            //else 
            //   store.ProductCode 
            //end productcode,
            //case when laststore.price is not null 
            //then 
            //laststore.price
            //else 
            //   store.price 
            //end price,
            //case when laststore.quantity is not null then  laststore.quantity
            //else 0 end lastquantity,
            //case when store.quantity is not null then  store.quantity
            //else 0
            //end Reqquantity,
            //case when laststore.Material_Attr01 is not null then  laststore.Material_Attr01
            //else laststore.Material_Attr01
            //end Material_Attr01
            //from (select productcode,price,isnull(sum(price*quantity),0) as amount,isnull(sum(quantity),0) as quantity,material.Material_Attr01 as Material_Attr01 from MMS_Store mmsstore inner join MMS_MaterialInfo material on mmsstore.ProductCode=material.Material_ID where year(createdatetime)='{2}' and month(createdatetime)='{3}'
            //group by productcode,price,Material_Attr01) laststore
            //full join
            //(select productcode,price,isnull(sum(quantity),0) as quantity,isnull(sum(amount),0) as amount from dbo.StorageQuery where PurchaseDate>='{0}' and PurchaseDate<='{1}'
            //group by productcode,price) store on laststore.ProductCode=store.ProductCode and laststore.Price=store.Price) re
            //full join 
            // (
            //select productcode,price,isnull(sum(quantity),0) quantity,isnull(sum(amount),0) amount  from (
            //select PurchaseBillCode,productcode,price,isnull(qua,0) quantity,isnull(price*qua,0) amount  from 
            //(select PurchaseBillCode,productcode,price,sum(quantity) qua from 
            //(select delivery.* from (
            //select purchasebillcode,productcode,sum(quantity) quantity,price from MMS_PurchasePlanDetail 
            //where AuditFlag IS NULL
            //group by purchasebillcode,productcode,price)  PlanDetail
            //inner join (
            //select purchasebillcode,productcode,sum(quantity) quantity,price,OperatorDate from MMS_Delivery_Detail 
            //group by purchasebillcode,productcode,price,OperatorDate
            //) delivery on PlanDetail.price=delivery.price and plandetail.productcode=delivery.productcode
            //and plandetail.purchasebillcode=delivery.purchasebillcode

            //where delivery.OperatorDate>='{0}' and delivery.OperatorDate<='{1}')req
            //group by PurchaseBillCode,productcode,price) req)req
            //group by ProductCode,price) req
            //on re.ProductCode=req.ProductCode and re.Price=req.Price) store"; 
            strSql = string.Format(strSql, ttbStartDate.Text.ToString().Trim(), ttbEndDate.Text.ToString().Trim(), Convert.ToDateTime(ttbStartDate.Text.ToString().Trim()).AddDays(-1).Year, Convert.ToDateTime(ttbStartDate.Text.ToString().Trim()).AddDays(-1).Month);
            int re = DataFactory.SqlDataBase().ExecuteBySql(new StringBuilder(strSql));

            Response.Write("<script language='javascript'>alert('结转成功！');</script>");
        }

        protected void delete_Click(object sender, EventArgs e)
        {
            string startdate = ttbStartDate.Text.ToString().Trim();
            string enddate = ttbEndDate.Text.ToString().Trim();
            string strSql = "delete MMS_Store where CreateDateTime='"+ enddate+"'";
            int re = DataFactory.SqlDataBase().ExecuteBySql(new StringBuilder(strSql));
            Response.Write("<script language='javascript'>alert('删除成功！');</script>");
        }
    }
}