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
    public partial class MaterialNumQuery : System.Web.UI.Page
    {
        DataTable dt = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ttbStartDate.Text.ToString().Trim() != "")
            {

                dt = dt_Query(ttbStartDate.Text.ToString().Trim(), ttbEndDate.Text.ToString().Trim(), ddlMaterialType.SelectedItem.Text.ToString());
                ReportDocument reportDoc = new ReportDocument();
                reportDoc.Load(Server.MapPath("Rpt_MaterialNumQuery.rpt"));
                reportDoc.SetDataSource(dt);
                string TEXT_OBJECT_NAME = @"FinanceMonth";
                TextObject text;
                text = reportDoc.ReportDefinition.ReportObjects[TEXT_OBJECT_NAME] as TextObject;
                text.Text = Convert.ToDateTime(ttbEndDate.Text.ToString().Trim()).Year.ToString() + "年" + Convert.ToDateTime(ttbEndDate.Text.ToString().Trim()).Month.ToString() + "月";
                CrystalReportViewer1.ReportSource = reportDoc;
                Session["myrpt"] = reportDoc;
            }
            
            if (!IsPostBack)
            { init_Page(); }
            if (Session["myrpt"]!=null) {
                CrystalReportViewer1.ReportSource = (ReportDocument)Session["myrpt"];
            }
           
        }


        private void init_Page()
        {
            ddlMaterialType.Items.Insert(0, new ListItem("", "-1"));
            ddlMaterialType.Items.Insert(1, new ListItem("办公日用品", "0"));
            ddlMaterialType.Items.Insert(2, new ListItem("检验耗材", "1"));
            ddlMaterialType.Items.Insert(3, new ListItem("日杂用品", "2"));
            ddlMaterialType.Items.Insert(4, new ListItem("医用耗材", "3"));
            ddlMaterialType.Items.Insert(5, new ListItem("印刷品", "4"));




        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            dt = dt_Query(ttbStartDate.Text.ToString().Trim(), ttbEndDate.Text.ToString().Trim(),ddlMaterialType.SelectedValue.ToString());
            ReportDocument reportDoc = new ReportDocument();
            reportDoc.Load(Server.MapPath("Rpt_MaterialNumQuery.rpt"));
            reportDoc.SetDataSource(dt);
            string TEXT_OBJECT_NAME = @"FinanceMonth";
            TextObject text;
            text = reportDoc.ReportDefinition.ReportObjects[TEXT_OBJECT_NAME] as TextObject;
            text.Text = Convert.ToDateTime(ttbEndDate.Text.ToString().Trim()).AddDays(-1).Year.ToString() + "年" + Convert.ToDateTime(ttbEndDate.Text.ToString().Trim()).AddDays(-1).Month.ToString() + "月";

           
            CrystalReportViewer1.ReportSource = reportDoc;
        }

        protected DataTable dt_Query(string starttime, string endtime,string material_type)
        {
//             string strSql =
//                @"select mat.material_name Material_Type,laststorage.* from (select distinct productcode,
//case when storageprice=0 then 
//  case when  deliveryprice=0 then storeprice else  deliveryprice end
//else storageprice  end price,
//storagequantity InStoragePrice ,deliveryquantity ReceivePrice,storequantity LastMonthBalancePrice,
//storagequantity-deliveryquantity+storequantity  CurrentMonthBalancePrice from (
//select mms.productcode,isnull(storage.price,0) storageprice,isnull(storage.quantity,0) storagequantity,isnull(delivery.price,0) deliveryprice,isnull(delivery.quantity,0) deliveryquantity,isnull(store.price,0) storeprice,isnull(store.quantity,0) storequantity from dbo.StorageQuery mms
//full join (select productcode,price,sum(quantity) as quantity  from storagequery where purchasedate>='{0}' and purchasedate<='{1}'
//group by productcode,price) storage on mms.productcode=storage.productcode and mms.price=storage.price
//full join (select productcode,price,sum(quantity) as quantity  from DeliveryQuery where OperatorDate>='{0}' and OperatorDate<='{1}'
//group by productcode,price) delivery 
//on mms.productcode=delivery.productcode and mms.price=delivery.price
//full join (select productcode,price,sum(quantity) as quantity  from MMS_Store store where createdatetime>=dateadd(month,-1,'{0}') and createdatetime<=dateadd(month,-1,'{1}')
//group by productcode,price) store
//on mms.productcode=store.productcode and mms.price=store.price) laststorage) laststorage  inner join dbo.MMS_MaterialInfo mat 
//on laststorage.productcode=mat.Material_ID where mat.Material_Attr01='{2}' ";

            string strSql = @"select * from (select matparent.materialtype_name material_type,Material_ID,Material_Name,price,lastquantity,price*lastquantity lastamount,storequantity,price*storequantity storeamount,reqquantity,price*reqquantity reqamount,lastquantity+storequantity-reqquantity quantity,price*(lastquantity+storequantity-reqquantity) amount from (
select substring(material_type,0,charindex('-',material_type)) as material_type,Material_ID,Material_Name,price,sum(lastquantity) lastquantity,sum(price*lastquantity) LastMonthBalancePrice,sum(storequantity) storequantity, sum(price*storequantity) InStoragePrice,sum(reqquantity) reqquantity,sum(price*reqquantity) ReceivePrice 
from MMS_MaterialInfo mms inner join 
(
select case when laststore.ProductCode is not null 
then 
laststore.ProductCode
else 
   case when store.ProductCode is not null then store.ProductCode
   else
     req.ProductCode
   end 
end productcode,
case when laststore.price is not null 
then 
laststore.price
else 
   case when store.price is not null then store.price
   else
     req.price
   end 
end price,
case when laststore.quantity is not null then  laststore.quantity
else
0
end lastquantity,
case when store.quantity is not null then  store.quantity
else
0
end storequantity,
case when Req.quantity is not null then  Req.quantity
else
0
end Reqquantity
  from (
select productcode,price,isnull(sum(amount),0) as amount,isnull(sum(quantity),0) as quantity from MMS_Store where year(createdatetime)='{2}' and month(createdatetime)='{3}'
group by productcode,price) laststore
full join
(select productcode,price,isnull(sum(quantity),0) as quantity,isnull(sum(amount),0) as amount from dbo.StorageQuery where PurchaseDate>='{0}' and PurchaseDate<='{1}'
group by productcode,price) store on laststore.ProductCode=store.ProductCode and laststore.Price=store.Price
full join 
 (select productcode,price,isnull(sum(quantity),0) as quantity,isnull(sum(amount),0) as amount from 
(  select req.purchasebillcode,req.productcode,req.price,isnull(qub,0) as quantity,isnull((req.price*qub),0) amount from (select PurchaseBillCode,productcode,price,sum(quantity) qua from RequisitionQuery 
group by productcode,price,purchasebillcode)Req
inner join (
select purchasebillcode,productcode,price,sum(quantity) qub from 
(select * from MMS_Delivery_Detail where OperatorDate>='{0}' and OperatorDate<='{1}')  devlivery
group by productcode,price,purchasebillcode) Deli on
req.productcode=deli.productcode and req.price=deli.price and req.purchasebillcode=deli.purchasebillcode   ) req 
group by ProductCode,price) req
on laststore.ProductCode=req.ProductCode and laststore.Price=req.Price) store
on mms.Material_ID=store.productcode
group by material_type,Material_ID,Material_Name,price) store
inner join  dbo.MMS_MaterialType mattype on store.material_type=mattype.MaterialType_Name
inner join ( select materialtype_id,materialtype_name from dbo.MMS_MaterialType where ParentId='0') matparent
on mattype.ParentId=matparent.MaterialType_ID) s where 1=1 
";
            if (!string.IsNullOrEmpty(material_type))
            {
                strSql = strSql + " and material_type='{4}'";
                strSql = string.Format(strSql, starttime, endtime, Convert.ToDateTime(starttime).AddDays(-1).Year, Convert.ToDateTime(starttime).AddDays(-1).Month, material_type);
            }
            else
            {
                strSql = string.Format(strSql, starttime, endtime, Convert.ToDateTime(starttime).AddDays(-1).Year, Convert.ToDateTime(starttime).AddDays(-1).Month);
            }
            
            
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(strSql));

            return dt;
        }

        protected void lbtExce_Click(object sender, EventArgs e)
        {
//            string strSql = @"insert into dbo.MMS_Store(productcode,quantity,price,amount,createdatetime)
//select laststorage.productcode,laststorage.CurrentMonthBalancePrice Quantity,laststorage.price,(laststorage.price*laststorage.CurrentMonthBalancePrice) as amount,'{1}' CreateDateTime
// from (select distinct productcode,
//case when storageprice=0 then 
//  case when  deliveryprice=0 then storeprice else  deliveryprice end
//else storageprice  end price,storagequantity InStoragePrice ,deliveryquantity ReceivePrice,storequantity LastMonthBalancePrice,
//storagequantity-deliveryquantity+storequantity  CurrentMonthBalancePrice from (
//select mms.productcode,isnull(storage.price,0) storageprice,isnull(storage.quantity,0) storagequantity,isnull(delivery.price,0) deliveryprice,isnull(delivery.quantity,0) deliveryquantity,isnull(store.price,0) storeprice,isnull(store.quantity,0) storequantity from dbo.StorageQuery mms
//full join (select productcode,price,sum(quantity) as quantity  from storagequery where purchasedate>='{0}' and purchasedate<='{1}'
//group by productcode,price) storage on mms.productcode=storage.productcode and mms.price=storage.price
//full join (select productcode,price,sum(quantity) as quantity  from DeliveryQuery where OperatorDate>='{0}' and OperatorDate<='{1}'
//group by productcode,price) delivery 
//on mms.productcode=delivery.productcode and mms.price=delivery.price
//full join (select productcode,price,sum(quantity) as quantity  from MMS_Store store where createdatetime>=dateadd(month,-1,'{0}') and createdatetime<=dateadd(month,-1,'{1}')
//group by productcode,price) store
//on mms.productcode=store.productcode and mms.price=store.price) laststorage) laststorage 
// inner join dbo.MMS_MaterialInfo mat 
//on laststorage.productcode=mat.Material_ID where mat.Material_Attr01='{2}' ";

            string strSql = @"insert into dbo.MMS_Store(productcode,quantity,price,amount,createdatetime)
select productcode,sum(quantity) quantity,price,sum(amount) amount,max(createdatetime) createdatetime  from (select productcode,quantity,price,quantity*price amount,'{1}' createdatetime  from (
select productcode,lastquantity+storequantity-reqquantity quantity,case when lastprice=0 then 
  case when  storeprice=0 then reqprice else  storeprice end
else lastprice  end price from (
select case when laststore.productcode is null then 
   case when store.productcode is null then req.productcode else store.productcode  end
else laststore.productcode end productcode,isnull(laststore.quantity,0) as lastquantity,isnull(laststore.price,0) as lastprice,isnull(laststore.amount,0) as lastamount,
isnull(store.quantity,0)as storequantity,isnull(store.price,0) as storeprice,isnull(store.amount,0)as storeamount,
isnull(req.quantity,0) as reqquantity,isnull(req.price,0) as reqprice,isnull(req.amount,0) as reqamount
 from 
(select productcode,isnull(sum(amount),0) as amount,isnull(sum(quantity),0) as quantity,price from MMS_Store where createdatetime>dateadd(month,-1,'{0}') and createdatetime<='{0}'
group by productcode,price) laststore 
full join 
(select distinct productcode,isnull(sum(quantity),0) as quantity,isnull(sum(amount),0) as amount,price  from dbo.StorageQuery where PurchaseDate>='{0}' and PurchaseDate<='{1}'
group by productcode,price) store  on laststore.productcode=store.productcode and  laststore.price=store.price 
full join 
 (select distinct productcode,isnull(sum(quantity),0) as quantity,isnull(sum(amount),0) as amount,price  from 
(SELECT distinct RequisitionQuery.* FROM dbo.RequisitionQuery  
INNER JOIN MMS_Delivery_Detail  devlivery on RequisitionQuery.PurchaseBillCode =devlivery .PurchaseBillCode and RequisitionQuery.ProductCode=devlivery.ProductCode
and RequisitionQuery.Price=devlivery.Price 
where OperatorDate>='{0}' and OperatorDate<='{1}')RequisitionQuery
group by productcode,price) req on laststore.productcode=req.productcode and laststore.price=req.price ) amount) amount) amount 
group by productcode,price";

            strSql = string.Format(strSql, ttbStartDate.Text.ToString().Trim(), ttbEndDate.Text.ToString().Trim(), ddlMaterialType.SelectedValue.ToString());
            int re = DataFactory.SqlDataBase().ExecuteBySql(new StringBuilder(strSql));

            Response.Write("<script language='javascript'>alert('结转成功！');</script>");
        }
    }
}