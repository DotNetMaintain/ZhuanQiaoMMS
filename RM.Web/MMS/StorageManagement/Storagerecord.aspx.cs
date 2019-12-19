using RM.Busines;
using RM.Common.DotNetBean;
using RM.ServiceProvider.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RM.Web.MMS.StorageManagement
{
    public partial class Storagerecord : System.Web.UI.Page
    {
        public List<Inventorydetail> dlist = new List<Inventorydetail>();
        public QuantityInfo q = new QuantityInfo();
        public QuantityInfo qs = new QuantityInfo();
        public List<deliveryquery> dllist = new List<deliveryquery>();
        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DateTime datetime = (DateTime)DataFactory.SqlDataBase().GetObjectValue(new StringBuilder("select getdate()"));

                string startdate = ttbStartDate.Text.ToString().Trim() == "" ? datetime.AddDays(-7).ToString() : ttbStartDate.Text.ToString().Trim();
                string enddate = ttbEndDate.Text.ToString().Trim() == "" ? datetime.ToString() : ttbEndDate.Text.ToString().Trim();
                dlist = getInventorydetail(startdate, enddate);
                dllist = getpurchaseplandetail(startdate, enddate);

            }
        }
        protected List<Inventorydetail> getInventorydetail(string startdate, string enddate)
        {

            //            string strSql = @"select content.operatedate,content.PurchaseBillCode,detail.quantity*detail.price as amount,userinfo.user_name,content.invoicecode,detail.quantity,detail.price,detail.lot,detail.productcode,material.material_name,Client.ValueName from mms_purchasedetail detail
            // INNER JOIN MMS_PurchaseContent content on detail.purchasebillcode=content.purchasebillcode
            //INNER JOIN Base_UserInfo userinfo on userinfo.User_Code=content.checkman
            //INNER JOIN (SELECT Material_Name,material_id from MMS_MaterialInfo where Material_Attr01='{0}') material on material.material_id=detail.productcode
            //inner JOIN (select * from Base_DictionaryInfo where (TypeName = '客户分类')) Client on content.Provider=Client.DictionaryInfo_ID where content.operatedate>='{1}' and content.operatedate<='{2}'";
            //            strSql += @" GROUP BY content.operatedate,content.PurchaseBillCode,userinfo.user_name,content.invoicecode,detail.quantity,detail.price,detail.lot,detail.productcode,material.material_name,Client.ValueName ORDER BY content.operatedate desc";

            string userremark = RequestSession.GetSessionUser().UserRemark.ToString();
            string strSql = @"select * from (select mmscontent.PurchaseBillCode,mmscontent.invoicecode,vm.midtype,substring(material.material_type,0,charindex('-',material.material_type)-1) Material_Type,detail.ProductCode,material.Material_Attr01,material.Material_Name,material.Material_Specification,userinfo.User_Name
                    ,material.Material_Unit,Client.ValueName as Material_Supplier,detail.Lot,detail.Price,detail.Quantity,(detail.Price*detail.Quantity) amount,
                    mmscontent.PurchaseDate,mmscontent.Provider
                    from dbo.MMS_PurchaseContent as mmscontent
                     inner join dbo.MMS_PurchaseDetail as detail on mmscontent.PurchaseBillCode=detail.PurchaseBillCode
                     inner JOIN dbo.MMS_MaterialInfo material ON detail.ProductCode=material.Material_ID
INNER JOIN Base_UserInfo userinfo on userinfo.User_Account=mmscontent.checkman
                     inner JOIN (select * from Base_DictionaryInfo where (TypeName = '客户分类')) Client on mmscontent.Provider=Client.DictionaryInfo_ID
					 inner join view_mattype vm on vm.submat=substring(material.material_type,0,charindex('-',material.material_type)-1) 
					 or  vm.toptype=substring(material.material_type,0,charindex('-',material.material_type)-1) or  vm.midtype=substring(material.material_type,0,charindex('-',material.material_type)-1) 
					 ) content where 1=1";

            if (!string.IsNullOrEmpty(startdate))
            {
                strSql += @"and PurchaseDate>='" + startdate + "'";
            }
            if (!string.IsNullOrEmpty(enddate))
            {
                strSql += @"and PurchaseDate<='" + enddate + "'";
            }
            strSql += @"and provider<>'254'";
            if (!string.IsNullOrEmpty(userremark))
            {
                strSql += @"and Material_Attr01 like '" + userremark + "'";
            }
            strSql += @"GROUP BY PurchaseBillCode,invoicecode,midtype,material_type,productcode,material_attr01,material_name,material_specification,user_name,material_unit,material_supplier,lot,price,quantity,amount,purchasedate,provider";

            //strSql = string.Format(strSql, userremark,startdate,enddate);
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(strSql));
            List<Inventorydetail> ulist = new List<Inventorydetail>();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Inventorydetail e = new Inventorydetail();
                    e.invoicecode = dt.Rows[i]["invoicecode"].ToString();
                    e.PurchaseBillCode = dt.Rows[i]["PurchaseBillCode"].ToString();
                    e.lot = dt.Rows[i]["lot"].ToString();
                    e.material_name = dt.Rows[i]["material_name"].ToString();
                    e.price = Convert.ToDouble(dt.Rows[i]["price"]);
                    e.amount = Convert.ToDouble(dt.Rows[i]["amount"]);
                    e.quantity = Convert.ToInt32(dt.Rows[i]["quantity"]);
                    e.productcode = Convert.ToInt32(dt.Rows[i]["productcode"]);
                    e.operatedate = dt.Rows[i]["PurchaseDate"].ToString();
                    e.ValueName = dt.Rows[i]["Material_Supplier"].ToString();
                    e.user_name = dt.Rows[i]["user_name"].ToString();
                    e.Material_Type = dt.Rows[i]["Material_Type"].ToString();
                    e.midtype = dt.Rows[i]["midtype"].ToString();
                    ulist.Add(e);
                }
            }
            dlist = ulist;
            //string startcode = Convert.ToDateTime(startdate).ToString("yyyyMMddhhmmssfff");
            //string endcode = Convert.ToDateTime(enddate).ToString("yyyyMMddhhmmssfff");
            //string sql = "select sum(quantity) as quantity,sum(usequantity) as usequantity,sum(quantity)-sum(usequantity) as qua from mms_purchasedetail where PurchaseBillCode>=startcode and PurchaseBillCode<=endcode";
            //dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
            //q.quantity = Convert.ToInt32(dt.Rows[0]["quantity"]);
            //q.usequantity = Convert.ToInt32(dt.Rows[0]["usequantity"]);
            //q.qua = Convert.ToInt32(dt.Rows[0]["qua"]);
            return ulist;
        }
        protected List<deliveryquery> getpurchaseplandetail(string startdate, string enddate)
        {
            string userremark = RequestSession.GetSessionUser().UserRemark.ToString();
            //            string strSql = @"select userinfo.User_Name,material.Material_Name,detail.Price,detail.AuditFlag,detail.CheckQuantity,detail.ProductCode,detail.Quantity,detail.Price*detail.Quantity as amount,content.DeptName,content.OperateDate,content.Operator,content.PurchaseBillCode from MMS_PurchasePlanDetail detail INNER JOIN MMS_PurchasePlanContent content on detail.PurchaseBillCode=content.PurchaseBillCode
            //INNER JOIN (SELECT material_id,Material_Name,Material_Specification,Material_Unit from MMS_MaterialInfo where Material_Attr01='{0}') material on material.material_id=detail.ProductCode
            //INNER JOIN Base_UserInfo userinfo on userinfo.User_ID=content.Operator where content.OperateDate>='{1}' and content.OperateDate<='{2}' and CheckQuantity>0 and (detail.AuditFlag is null or detail.AuditFlag>'0')  ORDER BY OperateDate desc";
            string strSql = @"select PurchaseBillCode,productcode,price,quantity,isnull(price*quantity,0) amount,OperatorDate,material_id,material_type,material_name,midtype,DeptName from (select delivery.*,content.DeptName from (
select purchasebillcode,productcode,sum(quantity) quantity,price from MMS_PurchasePlanDetail 
where CheckQuantity>0 and (AuditFlag is null or AuditFlag>'0')
group by purchasebillcode,productcode,price)  PlanDetail
INNER JOIN MMS_PurchasePlanContent content on PlanDetail.purchasebillcode=content.purchasebillcode
inner join (
select purchasebillcode,productcode,sum(quantity) quantity,price,OperatorDate from MMS_Delivery_Detail 
group by purchasebillcode,productcode,price,OperatorDate
) delivery on PlanDetail.price=delivery.price and plandetail.productcode=delivery.productcode
and plandetail.purchasebillcode=delivery.purchasebillcode
where delivery.OperatorDate>='{0}' and delivery.OperatorDate<='{1}')req inner join (select material_id,material_name,material_type from MMS_MaterialInfo where Material_Attr01= '{2}') material on material.material_id=req.ProductCode 
left join view_mattype vm on vm.submat=substring(material.material_type,0,charindex('-',material.material_type)-1) or  vm.toptype=substring(material.material_type,0,charindex('-',material.material_type)-1) or  vm.midtype=substring(material.material_type,0,charindex('-',material.material_type)-1) 
group by productcode,price,purchasebillcode,quantity,operatordate,material_id,material_type,material_name,midtype,DeptName";

            strSql = string.Format(strSql, startdate, enddate, userremark);
            dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(strSql));
            List<deliveryquery> ulist = new List<deliveryquery>();

            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    deliveryquery e = new deliveryquery();
                    e.PurchaseBillCode = dt.Rows[i]["PurchaseBillCode"].ToString();
                    e.material_name = dt.Rows[i]["material_name"].ToString();
                    e.price = Convert.ToDouble(dt.Rows[i]["price"]);
                    e.amount = Convert.ToDouble(dt.Rows[i]["amount"]);
                    e.quantity = Convert.ToInt32(dt.Rows[i]["quantity"]);
                    e.productcode = Convert.ToInt32(dt.Rows[i]["material_id"]);
                    e.DeptName = dt.Rows[i]["DeptName"].ToString();
                    //e.username = dt.Rows[i]["User_Name"].ToString();
                    e.OperatorDate = dt.Rows[i]["operatordate"].ToString();
                    e.midtype = dt.Rows[i]["midtype"].ToString() == null ? "" : dt.Rows[i]["midtype"].ToString();
                    e.material_type = dt.Rows[i]["Material_Type"].ToString();
                    ulist.Add(e);
                }
            }
            dllist = ulist;
            //string sql = "select sum(CheckQuantity) as Quantity,ProductCode from MMS_PurchasePlanDetail where CheckQuantity>0 and (AuditFlag is null or AuditFlag>'0') and productcode='{0}' GROUP BY ProductCode";
            //sql = string.Format(sql, productcode);
            //dt = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
            //qs.quantity = Convert.ToInt32(dt.Rows[0]["quantity"]);
            return ulist;
        }

        protected void lbtSearch_Click(object sender, EventArgs e)
        {
            DateTime datetime = (DateTime)DataFactory.SqlDataBase().GetObjectValue(new StringBuilder("select getdate()"));

            string startdate = ttbStartDate.Text.ToString().Trim() == "" ? datetime.AddDays(-7).ToString() : ttbStartDate.Text.ToString().Trim();
            string enddate = ttbEndDate.Text.ToString().Trim() == "" ? datetime.ToString() : ttbEndDate.Text.ToString().Trim();
            dlist = getInventorydetail(startdate, enddate);
            dllist = getpurchaseplandetail(startdate, enddate);

        }

        protected void btnexcelpurchase_Click(object sender, EventArgs e)
        {
            DateTime datetime = (DateTime)DataFactory.SqlDataBase().GetObjectValue(new StringBuilder("select getdate()"));
            string startdate = ttbStartDate.Text.ToString().Trim() == "" ? datetime.AddDays(-7).ToString() : ttbStartDate.Text.ToString().Trim();
            string enddate = ttbEndDate.Text.ToString().Trim() == "" ? datetime.ToString() : ttbEndDate.Text.ToString().Trim();
            dlist = getInventorydetail(startdate, enddate);
            string tableHeader = "<td style=\"border:0.5px solid black;\">入库单号</td><td style=\"border:0.5px solid black;\">物料大类</td><td style=\"border:0.5px solid black;\">物料类型</td><td style=\"border:0.5px solid black;\">物料名称</td><td style=\"border:0.5px solid black;\">供应商</td><td style=\"border:0.5px solid black;\">发票号</td><td style=\"border:0.5px solid black;\">批号</td><td style=\"border:0.5px solid black;\">入库数</td><td style=\"border:0.5px solid black;\">单价</td><td style=\"border:0.5px solid black;\">总金额</td><td style=\"border:0.5px solid black;\">入库人员</td><td style=\"border:0.5px solid black;\">入库时间</td>";
            string tableContent = "";
            foreach (var item in dlist)
            {
                tableContent += "<tr><td style=\"border:0.5px solid black;\">" + item.PurchaseBillCode + "</td><td style=\"border:0.5px solid black;\">" + item.midtype + "</td><td style=\"border:0.5px solid black;\">" + item.Material_Type + "</td><td style=\"border:0.5px solid black;\">" + item.material_name + "</td><td style=\"border:0.5px solid black;\">" + item.ValueName + "</td><td style=\"border:0.5px solid black;\">" + item.invoicecode + "</td><td style=\"border:0.5px solid black;\">" + item.lot + "</td><td style=\"border:0.5px solid black;\">" + item.quantity + "</td><td style=\"border:0.5px solid black;\">" + item.price + "</td><td style=\"border:0.5px solid black;\">" + item.amount + "</td><td style=\"border:0.5px solid black;\">" + item.user_name + "</td><td style=\"border:0.5px solid black;\">" + item.operatedate + "</td></tr>";

            }
            ExportToExcel(tableHeader, tableContent, "实时库存报表");

        }

        protected void btnexcelpurchaseplan_Click(object sender, EventArgs e)
        {
            DateTime datetime = (DateTime)DataFactory.SqlDataBase().GetObjectValue(new StringBuilder("select getdate()"));
            string startdate = ttbStartDate.Text.ToString().Trim() == "" ? datetime.AddDays(-7).ToString() : ttbStartDate.Text.ToString().Trim();
            string enddate = ttbEndDate.Text.ToString().Trim() == "" ? datetime.ToString() : ttbEndDate.Text.ToString().Trim();
            dllist = getpurchaseplandetail(startdate, enddate);
            string tableHeader = "<td style=\"border:0.5px solid black;\">领料单号</td><td style=\"border:0.5px solid black;\">物料大类</td><td style=\"border:0.5px solid black;\">物料类型</td><td style=\"border:0.5px solid black;\">物料名称</td><td style=\"border:0.5px solid black;\">领料部门</td><td style=\"border:0.5px solid black;\">领料数</td><td style=\"border:0.5px solid black;\">单价</td><td style=\"border:0.5px solid black;\">总金额</td><td style=\"border:0.5px solid black;\">领料时间</td>";
            string tableContent = "";
            foreach (var item in dllist)
            {
                tableContent += "<tr><td style=\"border:0.5px solid black;\">" + item.PurchaseBillCode + "</td><td style=\"border:0.5px solid black;\">" + item.midtype + "</td><td style=\"border:0.5px solid black;\">" + item.material_type + "</td><td style=\"border:0.5px solid black;\">" + item.material_name + "</td><td style=\"border:0.5px solid black;\">" + item.DeptName + "</td><td style=\"border:0.5px solid black;\">" + item.quantity + "</td><td style=\"border:0.5px solid black;\">" + item.price + "</td><td style=\"border:0.5px solid black;\">" + item.amount + "</td><td style=\"border:0.5px solid black;\">" + item.OperatorDate + "</td></tr>";

            }
            ExportToExcel(tableHeader, tableContent, "实时库存报表");
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
    }
}