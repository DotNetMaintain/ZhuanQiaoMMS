using System;
using System.Data.SqlClient;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using RM.Busines;
using System.Text;
using System.Web.UI.WebControls;
using RM.Common.DotNetBean;

namespace RM.Web.MMS.MMS_Material
{
    public partial class MaterialFinanceQuery : System.Web.UI.Page
    {
        DataTable dt = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {



            if (IsPostBack)
            {
                dt = dt_Query(ttbStartDate.Text.ToString().Trim(), ttbEndDate.Text.ToString().Trim());
                ReportDocument reportDoc = new ReportDocument();
                reportDoc.Load(Server.MapPath("Rpt_MaterialFinance.rpt"));
                reportDoc.SetDataSource(dt);
                string TEXT_OBJECT_NAME = @"FinanceMonth";
                TextObject text;
                text = reportDoc.ReportDefinition.ReportObjects[TEXT_OBJECT_NAME] as TextObject;
                text.Text = ttbStartDate.Text.ToString() + "----" + ttbEndDate.Text.ToString();
                string TEXT_OBJECT_USERNAME = @"txtname";
                string username = RequestSession.GetSessionUser().UserName.ToString();
                TextObject textname;
                textname = reportDoc.ReportDefinition.ReportObjects[TEXT_OBJECT_USERNAME] as TextObject;
                textname.Text = username;
                CrystalReportViewer1.ReportSource = reportDoc;
            }

        }


        //private void init_Page()
        //{
        //    ddlMaterialType.Items.Insert(0, new ListItem("", "-1"));
        //    ddlMaterialType.Items.Insert(1, new ListItem("运保物资", "0"));
        //    ddlMaterialType.Items.Insert(1, new ListItem("医疗耗材", "1"));




        //}


        protected void Page_Init(object sender, EventArgs e)
        {
            if (ttbEndDate.Text.ToString().Trim() != "" && ttbStartDate.Text.ToString().Trim() != "")
            {
                dt = dt_Query(ttbStartDate.Text.ToString().Trim(), ttbEndDate.Text.ToString().Trim());
            }

            ReportDocument reportDoc = new ReportDocument();
            reportDoc.Load(Server.MapPath("Rpt_MaterialFinance.rpt"));
            reportDoc.SetDataSource(dt);
            string TEXT_OBJECT_NAME = @"FinanceMonth";
            TextObject text;
            text = reportDoc.ReportDefinition.ReportObjects[TEXT_OBJECT_NAME] as TextObject;
            text.Text = ttbStartDate.Text.ToString() + "----" + ttbEndDate.Text.ToString();
            string TEXT_OBJECT_USERNAME = @"txtname";
            string username = RequestSession.GetSessionUser().UserName.ToString();
            TextObject textname;
            textname = reportDoc.ReportDefinition.ReportObjects[TEXT_OBJECT_USERNAME] as TextObject;
            textname.Text = username;
            CrystalReportViewer1.ReportSource = reportDoc;
        }


        protected void btnQuery_Click(object sender, EventArgs e)
        {

            dt = dt_Query(ttbStartDate.Text.ToString().Trim(), ttbEndDate.Text.ToString().Trim());
            ReportDocument reportDoc = new ReportDocument();
            reportDoc.Load(Server.MapPath("Rpt_MaterialFinance.rpt"));
            reportDoc.SetDataSource(dt);
            string TEXT_OBJECT_NAME = @"FinanceMonth";
            TextObject text;
            text = reportDoc.ReportDefinition.ReportObjects[TEXT_OBJECT_NAME] as TextObject;
            text.Text = ttbStartDate.Text.ToString() + "----" + ttbEndDate.Text.ToString();
            string TEXT_OBJECT_USERNAME = @"txtname";
            string username = RequestSession.GetSessionUser().UserName.ToString();
            TextObject textname;
            textname = reportDoc.ReportDefinition.ReportObjects[TEXT_OBJECT_USERNAME] as TextObject;
            textname.Text = username;
            CrystalReportViewer1.ReportSource = reportDoc;
        }

        protected DataTable dt_Query(string starttime, string endtime)
        {
            //string strSql =
            //   @"select material_type,LastMonthBalancePrice,InStoragePrice,ReceivePrice,LastMonthBalancePrice+InStoragePrice-ReceivePrice as CurrentMonthBalancePrice from (select substring(material_type,0,charindex('-',material_type)) as material_type,sum(lastamount) LastMonthBalancePrice ,sum(storeamount) InStoragePrice,sum(reqamount) ReceivePrice from (
            //select mms.*,isnull(laststore.amount,0) as lastamount,isnull(store.amount,0)as storeamount,isnull(req.amount,0) as reqamount from  (select * from MMS_MaterialInfo where deletemark='1') mms left join 
            //(select productcode,isnull(sum(amount),0) as amount,isnull(sum(quantity),0) as quantity from MMS_Store where year(createdatetime)='{2}' and month(createdatetime)+1='{3}'
            //group by productcode) laststore on mms.material_id=laststore.productcode
            //left join 
            //(select productcode,isnull(sum(quantity),0) as quantity,isnull(sum(amount),0) as amount from dbo.StorageQuery where PurchaseDate>='{0}' and PurchaseDate<='{1}'
            //group by productcode) store  on mms.material_id=store.productcode
            //left join 
            // (select productcode,isnull(sum(quantity),0) as quantity,isnull(sum(amount),0) as amount from dbo.RequisitionQuery  where PurchaseDate>='{0}' and PurchaseDate<='{1}'
            //group by productcode) req on mms.material_id=req.productcode ) storetotal
            //group by material_type) store 
            //inner join (select mat.materialtype_name matname,parentmat.materialtype_name parentmatname from dbo.MMS_MaterialType mat
            //inner join  dbo.MMS_MaterialType parentmat on mat.parentid=parentmat.materialtype_id) matt
            //on store.material_type=matt.matname
            //order by matt.parentmatname";
            //string strSql = @"select toptype,midtype,sum(LastMonthBalancePrice) LastMonthBalancePrice,sum(InStoragePrice) InStoragePrice,
            //sum(ReceivePrice) ReceivePrice,sum(CurrentMonthBalancePrice) CurrentMonthBalancePrice
            // from 
            //(select mattype.toptype,mattype.midtype,isnull(store.LastMonthBalancePrice,0) LastMonthBalancePrice,
            //isnull(store.InStoragePrice,0) InStoragePrice,isnull(store.ReceivePrice,0) ReceivePrice,
            //isnull(store.CurrentMonthBalancePrice,0)CurrentMonthBalancePrice from 
            //(select * from dbo.view_mattype
            //where toptype is not null and toptype<>'其他不统计类') mattype
            //left join 
            //(select * from  (select material_type,LastMonthBalancePrice,InStoragePrice,ReceivePrice,LastMonthBalancePrice+InStoragePrice-ReceivePrice as CurrentMonthBalancePrice from (select substring(material_type,0,charindex('-',material_type)) as material_type,sum(lastamount) LastMonthBalancePrice ,sum(storeamount) InStoragePrice,sum(reqamount) ReceivePrice from (
            //select mms.*,isnull(laststore.amount,0) as lastamount,isnull(store.amount,0)as storeamount,isnull(req.amount,0) as reqamount from  (select * from MMS_MaterialInfo where deletemark='1') mms left join 
            //(select productcode,isnull(sum(amount),0) as amount,isnull(sum(quantity),0) as quantity from MMS_Store where year(createdatetime)='{2}' and month(createdatetime)='{3}'
            //group by productcode) laststore on mms.material_id=laststore.productcode
            //full join 
            //(select productcode,isnull(sum(quantity),0) as quantity,isnull(sum(amount),0) as amount from dbo.StorageQuery where PurchaseDate>='{0}' and PurchaseDate<='{1}'
            //group by productcode) store  on mms.material_id=store.productcode
            //full join 
            // (select productcode,isnull(sum(quantity),0) as quantity,isnull(sum(amount),0) as amount from dbo.RequisitionQuery  where PurchaseDate>='{0}' and PurchaseDate<='{1}'
            //group by productcode) req on mms.material_id=req.productcode ) storetotal
            //group by material_type) store 
            //inner join (select mat.materialtype_name matname,parentmat.materialtype_name parentmatname from dbo.MMS_MaterialType mat
            //inner join  dbo.MMS_MaterialType parentmat on mat.parentid=parentmat.materialtype_id) matt
            //on store.material_type=matt.matname) store) store
            //on mattype.submat=store.material_type) matstore
            //group by toptype,midtype
            //order by toptype";
            string strSql = @"select toptype,midtype,ROUND(sum(LastMonthBalancePrice),2) LastMonthBalancePrice,ROUND(sum(InStoragePrice),2) InStoragePrice,ROUND(sum(ReceivePrice),2) ReceivePrice,(ROUND(sum(LastMonthBalancePrice),2)+ROUND(sum(InStoragePrice),2)-ROUND(sum(ReceivePrice),2)) CurrentMonthBalancePrice from (
select toptype,midtype,lastquantity,lastquantity*price LastMonthBalancePrice,storequantity,(price*storequantity) InStoragePrice,
reqquantity,(price*reqquantity)ReceivePrice,lastquantity+storequantity-reqquantity currentquntity,
price*(lastquantity+storequantity-reqquantity) CurrentMonthBalancePrice
 from (
select substring(mms.material_type,0,charindex('-',mms.material_type)) Material_Type,productcode,mms.Material_Name,price,sum(lastquantity) lastquantity,sum(price*lastquantity) LastMonthBalancePrice,sum(storequantity) storequantity, sum(price*storequantity) InStoragePrice,sum(reqquantity) reqquantity,sum(price*reqquantity) ReceivePrice  from (
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
group by material_type,Material_Name,productcode,price) matstore
inner join dbo.view_mattype mattype on matstore.material_type=mattype.submat or matstore.material_type=mattype.midtype or matstore.material_type=mattype.toptype
where toptype <>'其他不统计类' and material_type!='医疗用品类' and material_type!='印刷品' and material_type!='清洁/日用类' and material_type!='办公用品类' and material_type!='日常用品' and material_type!='其他材料') mstore
group by toptype,midtype
order by toptype,midtype";

            //            string strSql = @"select toptype,midtype,ROUND(sum(LastMonthBalancePrice),2) LastMonthBalancePrice,ROUND(sum(InStoragePrice),2) InStoragePrice,ROUND(sum(ReceivePrice),2) ReceivePrice,(ROUND(sum(LastMonthBalancePrice),2)+ROUND(sum(InStoragePrice),2)-ROUND(sum(ReceivePrice),2)) CurrentMonthBalancePrice,Material_Attr01 from (
            // sum(reqquantity) reqquantity,sum(ReceivePrice) ReceivePrice,sum(currentquntity) currentquntity,sum(CurrentMonthBalancePrice) CurrentMonthBalancePrice from (
            //select toptype,midtype,lastquantity,lastquantity*price LastMonthBalancePrice,storequantity,(price*storequantity) InStoragePrice,
            //reqquantity,(price*reqquantity)ReceivePrice,lastquantity+storequantity-reqquantity currentquntity,
            //price*(lastquantity+storequantity-reqquantity) CurrentMonthBalancePrice
            // from (
            //select substring(mms.material_type,0,charindex('-',mms.material_type)) Material_Type,productcode,mms.Material_Name,price,sum(lastquantity) lastquantity,sum(price*lastquantity) LastMonthBalancePrice,sum(storequantity) storequantity, sum(price*storequantity) InStoragePrice,sum(reqquantity) reqquantity,sum(price*reqquantity) ReceivePrice  from (
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
            //end reqquantity
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
            //end Reqquantity
            //from (select productcode,price,isnull(sum(price*quantity),0) as amount,isnull(sum(quantity),0) as quantity from MMS_Store where year(createdatetime)='{2}' and month(createdatetime)='{3}'
            //group by productcode,price) laststore
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
            //on re.ProductCode=req.ProductCode and re.Price=req.Price) store
            //inner join (select * from MMS_MaterialInfo ) mms
            //on mms.Material_ID=store.productcode
            //group by material_type,Material_Name,productcode,price) matstore
            //inner join dbo.view_mattype mattype on matstore.material_type=mattype.submat
            //where toptype <>'其他不统计类'  and material_type!='医疗用品类') mstore
            //group by toptype,midtype
            //order by toptype,midtype";
            string userremark = RequestSession.GetSessionUser().UserRemark.ToString();
            string strSqlbytype = @"select toptype,midtype,ROUND(sum(LastMonthBalancePrice),2) LastMonthBalancePrice,ROUND(sum(InStoragePrice),2) InStoragePrice,ROUND(sum(ReceivePrice),2) ReceivePrice,(ROUND(sum(LastMonthBalancePrice),2)+ROUND(sum(InStoragePrice),2)-ROUND(sum(ReceivePrice),2)) CurrentMonthBalancePrice,Material_Attr01 from (
select toptype,midtype,lastquantity,lastquantity*price LastMonthBalancePrice,(price*storequantity) InStoragePrice,(price*reqquantity)ReceivePrice,price*(lastquantity+storequantity-reqquantity) CurrentMonthBalancePrice,Material_Attr01,Material_ID
 from (
select substring(mms.material_type,0,charindex('-',mms.material_type)) Material_Type,productcode,mms.Material_Name,price,sum(lastquantity) lastquantity,sum(price*lastquantity) LastMonthBalancePrice,sum(storequantity) storequantity, sum(price*storequantity) InStoragePrice,sum(reqquantity) reqquantity,sum(price*reqquantity) ReceivePrice,mms.Material_Attr01 Material_Attr01,mms.material_id material_id  from (
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
from (select productcode,price,isnull(sum(price*quantity),0) as amount,isnull(sum(quantity),0) as quantity from   MMS_Store   where year(createdatetime)='{2}' and month(createdatetime)='{3}'
group by productcode,price) laststore
full join
(select productcode,price,isnull(sum(quantity),0) as quantity,isnull(sum(amount),0) as amount from   StorageQuery   where PurchaseDate>='{0}' and PurchaseDate<='{1}'
group by productcode,price) store on laststore.ProductCode=store.ProductCode and laststore.Price=store.Price) re
full join 
 (
select productcode,price,isnull(sum(quantity),0) quantity,isnull(sum(amount),0) amount  from (
select PurchaseBillCode,productcode,price,isnull(qua,0) quantity,isnull(price*qua,0) amount  from 
(select PurchaseBillCode,productcode,price,sum(quantity) qua from 
(select delivery.* from (
select purchasebillcode,productcode,sum(quantity) quantity,price from   MMS_PurchasePlanDetail   
where AuditFlag IS NULL or AuditFlag=1
group by purchasebillcode,productcode,price)  PlanDetail
inner join (
select purchasebillcode,productcode,sum(quantity) quantity,price,OperatorDate from   MMS_Delivery_Detail   
group by purchasebillcode,productcode,price,OperatorDate
) delivery on PlanDetail.price=delivery.price and plandetail.productcode=delivery.productcode
and plandetail.purchasebillcode=delivery.purchasebillcode
where delivery.OperatorDate>='{0}' and delivery.OperatorDate<='{1}')req
group by PurchaseBillCode,productcode,price) req)req
group by ProductCode,price) req
on re.ProductCode=req.ProductCode and re.Price=req.Price) store
inner join   MMS_MaterialInfo   mms
on mms.Material_ID=store.productcode
group by material_type,Material_Name,productcode,price,Material_Attr01,material_id) matstore
inner join   view_mattype   mattype on matstore.material_type=mattype.submat or matstore.material_type=mattype.midtype  or matstore.material_type=mattype.toptype
where toptype <>'其他不统计类'  and  Material_Attr01 like '{4}'  and material_type!='印刷品' and material_type!='清洁/日用类' and material_type!='办公用品类' and material_type!='日常用品' and material_type!='其他材料'  GROUP BY toptype,midtype,lastquantity,LastMonthBalancePrice,InStoragePrice,ReceivePrice,Material_Attr01,price,storequantity,reqquantity,Material_id) mstore
group by toptype,midtype,Material_Attr01
order by toptype,midtype,Material_Attr01";
            if (!string.IsNullOrEmpty(userremark))
            {
                if (Convert.ToDateTime(endtime) >= Convert.ToDateTime("2019-08-26"))
                {
                    strSqlbytype = @"select toptype,midtype,ROUND(sum(LastMonthBalancePrice),2) LastMonthBalancePrice,ROUND(sum(InStoragePrice),2) InStoragePrice,ROUND(sum(ReceivePrice),2) ReceivePrice,(ROUND(sum(LastMonthBalancePrice),2)+ROUND(sum(InStoragePrice),2)-ROUND(sum(ReceivePrice),2)) CurrentMonthBalancePrice,Material_Attr01 from (
select toptype,midtype,lastquantity,lastquantity*price LastMonthBalancePrice,(price*storequantity) InStoragePrice,(price*reqquantity)ReceivePrice,price*(lastquantity+storequantity-reqquantity) CurrentMonthBalancePrice,Material_Attr01,Material_ID
 from (
select substring(mms.material_type,0,charindex('-',mms.material_type)) Material_Type,productcode,mms.Material_Name,price,sum(lastquantity) lastquantity,sum(price*lastquantity) LastMonthBalancePrice,sum(storequantity) storequantity, sum(price*storequantity) InStoragePrice,sum(reqquantity) reqquantity,sum(price*reqquantity) ReceivePrice,mms.Material_Attr01 Material_Attr01,mms.material_id material_id  from (
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
from (select productcode,price,isnull(sum(price*quantity),0) as amount,isnull(sum(quantity),0) as quantity from   MMS_Store   where year(createdatetime)='{2}' and month(createdatetime)='{3}'
group by productcode,price) laststore
full join
(select productcode,price,isnull(sum(quantity),0) as quantity,isnull(sum(amount),0) as amount from   StorageQuery   where PurchaseDate>='{0}' and PurchaseDate<='{1}'
group by productcode,price) store on laststore.ProductCode=store.ProductCode and laststore.Price=store.Price) re
full join 
 (
select productcode,price,isnull(sum(quantity),0) quantity,isnull(sum(amount),0) amount  from (
select PurchaseBillCode,productcode,price,isnull(qua,0) quantity,isnull(price*qua,0) amount  from 
(select PurchaseBillCode,productcode,price,sum(quantity) qua from 
(select delivery.* from (
select purchasebillcode,productcode,sum(quantity) quantity,price from   MMS_PurchasePlanDetail   
where AuditFlag IS NULL or AuditFlag=1
group by purchasebillcode,productcode,price)  PlanDetail
inner join (
select purchasebillcode,productcode,sum(quantity) quantity,price,OperatorDate from   MMS_Delivery_Detail   
group by purchasebillcode,productcode,price,OperatorDate
) delivery on PlanDetail.price=delivery.price and plandetail.productcode=delivery.productcode
and plandetail.purchasebillcode=delivery.purchasebillcode
where delivery.OperatorDate>='{0}' and delivery.OperatorDate<='{1}')req
group by PurchaseBillCode,productcode,price) req)req
group by ProductCode,price) req
on re.ProductCode=req.ProductCode and re.Price=req.Price) store
inner join   MMS_MaterialInfo   mms
on mms.Material_ID=store.productcode
group by material_type,Material_Name,productcode,price,Material_Attr01,material_id) matstore
inner join   view_mattype   mattype on matstore.material_type=mattype.submat or matstore.material_type=mattype.midtype  or matstore.material_type=mattype.toptype
where toptype <>'其他不统计类'  and  Material_Attr01 like '{4}'  GROUP BY toptype,midtype,lastquantity,LastMonthBalancePrice,InStoragePrice,ReceivePrice,Material_Attr01,price,storequantity,reqquantity,Material_id) mstore
group by toptype,midtype,Material_Attr01
order by toptype,midtype,Material_Attr01";
                    strSql = string.Format(strSqlbytype, starttime, endtime, Convert.ToDateTime(starttime).Year, Convert.ToDateTime(starttime).Month, userremark);
                }
                else if (Convert.ToDateTime(endtime) >= Convert.ToDateTime("2019-07-26"))
                {
                    strSql = string.Format(strSqlbytype, starttime, endtime, Convert.ToDateTime(starttime).Year, Convert.ToDateTime(starttime).Month, userremark);
                }
                else if (Convert.ToDateTime(endtime) >= Convert.ToDateTime("2019-06-26"))//6月的查询
                {
                    strSqlbytype = @"select toptype,midtype,ROUND(sum(LastMonthBalancePrice),2) LastMonthBalancePrice,ROUND(sum(InStoragePrice),2) InStoragePrice,ROUND(sum(ReceivePrice),2) ReceivePrice,(ROUND(sum(LastMonthBalancePrice),2)+ROUND(sum(InStoragePrice),2)-ROUND(sum(ReceivePrice),2)) CurrentMonthBalancePrice,Material_Attr01 from (
select toptype,midtype,lastquantity,lastquantity*price LastMonthBalancePrice,(price*storequantity) InStoragePrice,(price*reqquantity)ReceivePrice,price*(lastquantity+storequantity-reqquantity) CurrentMonthBalancePrice,Material_Attr01,Material_ID
 from (
select substring(mms.material_type,0,charindex('-',mms.material_type)) Material_Type,productcode,mms.Material_Name,price,sum(lastquantity) lastquantity,sum(price*lastquantity) LastMonthBalancePrice,sum(storequantity) storequantity, sum(price*storequantity) InStoragePrice,sum(reqquantity) reqquantity,sum(price*reqquantity) ReceivePrice,mms.Material_Attr01 Material_Attr01,mms.material_id material_id  from (
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
from (select productcode,price,isnull(sum(price*quantity),0) as amount,isnull(sum(quantity),0) as quantity from   MMS_Store   where year(createdatetime)='{2}' and month(createdatetime)='{3}'
group by productcode,price) laststore
full join
(select productcode,price,isnull(sum(quantity),0) as quantity,isnull(sum(amount),0) as amount from   StorageQuery   where PurchaseDate>='{0}' and PurchaseDate<='{1}'
group by productcode,price) store on laststore.ProductCode=store.ProductCode and laststore.Price=store.Price) re
full join 
 (
select productcode,price,isnull(sum(quantity),0) quantity,isnull(sum(amount),0) amount  from (
select PurchaseBillCode,productcode,price,isnull(qua,0) quantity,isnull(price*qua,0) amount  from 
(select PurchaseBillCode,productcode,price,sum(quantity) qua from 
(select delivery.* from (
select purchasebillcode,productcode,sum(quantity) quantity,price from   MMS_PurchasePlanDetail   
where AuditFlag IS NULL or AuditFlag=1
group by purchasebillcode,productcode,price)  PlanDetail
inner join (
select purchasebillcode,productcode,sum(quantity) quantity,price,OperatorDate from   MMS_Delivery_Detail   
group by purchasebillcode,productcode,price,OperatorDate
) delivery on PlanDetail.price=delivery.price and plandetail.productcode=delivery.productcode
and plandetail.purchasebillcode=delivery.purchasebillcode
where delivery.OperatorDate>='{0}' and delivery.OperatorDate<='{1}')req
group by PurchaseBillCode,productcode,price) req)req
group by ProductCode,price) req
on re.ProductCode=req.ProductCode and re.Price=req.Price) store
inner join   MMS_MaterialInfo   mms
on mms.Material_ID=store.productcode
group by material_type,Material_Name,productcode,price,Material_Attr01,material_id) matstore
inner join   view_mattype   mattype on matstore.material_type=mattype.submat or matstore.material_type=mattype.midtype  or matstore.material_type=mattype.toptype
where toptype <>'其他不统计类'  and  Material_Attr01 like '{4}'  and material_type!='医疗用品类' and material_type!='印刷品' and material_type!='清洁/日用类' and material_type!='办公用品类' and material_type!='日常用品' and material_type!='其他材料'  GROUP BY toptype,midtype,lastquantity,LastMonthBalancePrice,InStoragePrice,ReceivePrice,Material_Attr01,price,storequantity,reqquantity,Material_id) mstore
group by toptype,midtype,Material_Attr01
order by toptype,midtype,Material_Attr01";//or matstore.material_type=mattype.toptype or matstore.material_type=mattype.midtype
                    strSql = string.Format(strSqlbytype, starttime, endtime, Convert.ToDateTime(starttime).Year, Convert.ToDateTime(starttime).Month, userremark);
                }
                else
                {
                    strSql = string.Format(strSql, starttime, endtime, Convert.ToDateTime(starttime).Year, Convert.ToDateTime(starttime).Month);
                }
            }
            else
            {
                strSql = string.Format(strSql, starttime, endtime, Convert.ToDateTime(starttime).Year, Convert.ToDateTime(starttime).Month);
            }
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(strSql));

            return dt;
        }
    }
}