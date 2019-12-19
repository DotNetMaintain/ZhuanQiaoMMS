using RM.Busines.DAO;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Common.DotNetEncrypt;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace RM.Busines.DAL
{
    public class MMS_StoreTransation_Dal : MMS_StoreTransation_IDAO
    {


         #region method

         public DataTable Load_StoreTransationList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM MMS_StoreTransation ORDER BY Material_id ASC");
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
        }


        public DataTable GetStoreTransationPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * from MMS_StoreTransation U where 1=1");
            strSql.Append(SqlWhere);
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray<SqlParam>(), "CreateDate", "Desc", pageIndex, pageSize, ref count);
        }



        public DataTable GetStoreDeliveryPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            string userremark = RequestSession.GetSessionUser().UserRemark.ToString();
            strSql.Append(@"select * from (
                        select plancon.id,plancon.purchasebillcode,plancon.deptname,plancon.purchasedate,plancon.checkquantity,plancon.productcode,plancon.quantity,plancon.price,plancon.amount,plancon.auditflag,material.Material_Type,Material_Name,Material_Specification,Material_Unit,Material_Supplier,material.Material_Attr01,(isnull(plancon.qua,0)-isnull(plancon.usequa,0)) as qua,d.OperatorDate from (
                        select detail.id,plancontent.PurchaseBillCode,plancontent.DeptName,plancontent.PurchaseDate,detail.CheckQuantity,detail.ProductCode,detail.Quantity,detail.price,(detail.quantity*detail.price) amount,detail.AuditFlag,detail.qua,detail.usequa from (select * from MMS_PurchasePlanContent ) plancontent inner join 
                        (select sum(PurchaseDetail.quantity) as  qua,sum(PurchaseDetail.usequantity) as usequa,PlanDetail.purchasebillcode,PlanDetail.quantity,PlanDetail.price,PlanDetail.checkquantity,PlanDetail.auditflag,PlanDetail.id,PurchaseDetail.productcode from MMS_PurchasePlanDetail PlanDetail  inner join 
                        MMS_PurchaseDetail PurchaseDetail on PurchaseDetail.productcode=PlanDetail.productcode and PurchaseDetail.price=PlanDetail.price group by PlanDetail.purchasebillcode,PlanDetail.quantity,PlanDetail.price,PlanDetail.checkquantity,PlanDetail.auditflag,PlanDetail.id,PurchaseDetail.productcode) detail on plancontent.PurchaseBillCode=detail.PurchaseBillCode) plancon
                        INNER JOIN (select OperatorDate,de.ProductCode,de.PurchaseBillCode,de.Price from  MMS_Delivery_Detail de INNER JOIN (select MAX(Delivery_id) as id,PurchaseBillCode,ProductCode from MMS_Delivery_Detail GROUP BY PurchaseBillCode,ProductCode) det on de.Delivery_id=det.id ) d ON
d.PurchaseBillCode=plancon.PurchaseBillCode and d.ProductCode=plancon.productcode
left join MMS_MaterialInfo material on plancon.ProductCode=material.Material_ID ) planconen where 1 =1");
            strSql.Append(SqlWhere);
            if (!string.IsNullOrEmpty(userremark))
            {
                strSql.Append(@"  and Material_Attr01='" + userremark + "'");
            }
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray<SqlParam>(), "OperatorDate", "Desc", pageIndex, pageSize, ref count);
        }

        public DataTable GetStoreDeliveryPages(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            string userremark = RequestSession.GetSessionUser().UserRemark.ToString();
            strSql.Append(@"select * from (
                        select plancon.id,plancon.purchasebillcode,plancon.deptname,plancon.purchasedate,plancon.checkquantity,plancon.productcode,plancon.quantity,plancon.price,plancon.amount,plancon.auditflag,material.Material_Type,Material_Name,Material_Specification,Material_Unit,Material_Supplier,material.Material_Attr01,(isnull(plancon.qua,0)-isnull(plancon.usequa,0)) as qua from (
                        select detail.id,plancontent.PurchaseBillCode,plancontent.DeptName,plancontent.PurchaseDate,detail.CheckQuantity,detail.ProductCode,detail.Quantity,detail.price,(detail.quantity*detail.price) amount,detail.AuditFlag,detail.qua,detail.usequa from (select * from MMS_PurchasePlanContent ) plancontent inner join 
                        (select sum(PurchaseDetail.quantity) as  qua,sum(PurchaseDetail.usequantity) as usequa,PlanDetail.purchasebillcode,PlanDetail.quantity,PlanDetail.price,PlanDetail.checkquantity,PlanDetail.auditflag,PlanDetail.id,PurchaseDetail.productcode from MMS_PurchasePlanDetail PlanDetail  inner join 
                        MMS_PurchaseDetail PurchaseDetail on PurchaseDetail.productcode=PlanDetail.productcode and PurchaseDetail.price=PlanDetail.price group by PlanDetail.purchasebillcode,PlanDetail.quantity,PlanDetail.price,PlanDetail.checkquantity,PlanDetail.auditflag,PlanDetail.id,PurchaseDetail.productcode) detail on plancontent.PurchaseBillCode=detail.PurchaseBillCode) plancon
                        left join MMS_MaterialInfo material on plancon.ProductCode=material.Material_ID) planconen where 1 =1");
            strSql.Append(SqlWhere);
            if (!string.IsNullOrEmpty(userremark))
            {
                strSql.Append(@"  and Material_Attr01='" + userremark + "'");
            }
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray<SqlParam>(), "PurchaseDate", "Desc", pageIndex, pageSize, ref count);
        }
        public DataTable GetStoreDeliverysPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            string userremark = RequestSession.GetSessionUser().UserRemark.ToString();
            strSql.Append(@"select * from (
                        select plancon.OperatorDate,plancon.id,plancon.purchasebillcode,plancon.deptname,plancon.purchasedate,plancon.checkquantity,plancon.productcode,plancon.quantity,plancon.price,plancon.amount,plancon.auditflag,material.Material_Type,Material_Name,Material_Specification,Material_Unit,Material_Supplier,material.Material_Attr01,(isnull(plancon.qua,0)-isnull(plancon.usequa,0)) as qua from (
                        select detail.id,plancontent.PurchaseBillCode,plancontent.DeptName,plancontent.PurchaseDate,detail.CheckQuantity,detail.ProductCode,detail.Quantity,detail.price,(detail.quantity*detail.price) amount,detail.AuditFlag,detail.qua,detail.usequa,detail.OperatorDate from (select * from MMS_PurchasePlanContent ) plancontent inner join 
                        (select sum(PurchaseDetail.quantity) as  qua,sum(PurchaseDetail.usequantity) as usequa,PlanDetail.purchasebillcode,PlanDetail.quantity,PlanDetail.OperatorDate,PlanDetail.price,PlanDetail.checkquantity,PlanDetail.auditflag,PlanDetail.id,PurchaseDetail.productcode from MMS_PurchasePlanDetail PlanDetail  inner join 
                        MMS_PurchaseDetail PurchaseDetail on PurchaseDetail.productcode=PlanDetail.productcode and PurchaseDetail.price=PlanDetail.price group by PlanDetail.OperatorDate,PlanDetail.purchasebillcode,PlanDetail.quantity,PlanDetail.price,PlanDetail.checkquantity,PlanDetail.auditflag,PlanDetail.id,PurchaseDetail.productcode) detail on plancontent.PurchaseBillCode=detail.PurchaseBillCode) plancon
                        left join MMS_MaterialInfo material on plancon.ProductCode=material.Material_ID) planconen where 1 =1");
            strSql.Append(SqlWhere);
            if (!string.IsNullOrEmpty(userremark))
            {
                strSql.Append(@"  and Material_Attr01='" + userremark + "'");
            }
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray<SqlParam>(), "OperatorDate", "Desc", pageIndex, pageSize, ref count);
        }

        public DataTable GetStoreDeliveryPage(StringBuilder SqlWhere)
        {
            DataTable dt = new DataTable();
            string userremark = RequestSession.GetSessionUser().UserRemark.ToString();
            StringBuilder strSql = new StringBuilder();
            ////strSql.Append(@"select * from (
            //            select plancon.*,material.Material_Type,material.Material_Attr01,Material_Name,Material_Specification,Material_Unit,Material_Supplier from (
            //            select detail.id,plancontent.PurchaseBillCode,plancontent.DeptName,CONVERT(varchar(10),plancontent.PurchaseDate,120) as PurchaseDate,detail.CheckQuantity,detail.ProductCode,detail.Quantity,detail.Memo,detail.AuditFlag from (select * from MMS_PurchasePlanContent where AuditFlag='True') plancontent inner join 
            //            MMS_PurchasePlanDetail detail on plancontent.PurchaseBillCode=detail.PurchaseBillCode
            //            where  detail.quantity-detail.checkquantity>0) plancon 
            //            left join dbo.MMS_MaterialInfo material on plancon.ProductCode=material.Material_Code) planconen where 1 =1");
            strSql.Append(@"select * from (select content.PurchaseBillCode,content.PurchaseDate,userinfo.User_Name,content.DeptName,code.Material_Attr01 from MMS_PurchasePlanContent content 
INNER JOIN (SELECT detail.PurchaseBillCode,info.Material_Attr01 from MMS_PurchasePlanDetail detail INNER JOIN MMS_MaterialInfo info on detail.ProductCode=info.Material_ID) code
on code.PurchaseBillCode=content.PurchaseBillCode 
INNER JOIN Base_UserInfo userinfo on userinfo.User_ID=content.Operator
group by content.PurchaseBillCode,content.PurchaseDate,userinfo.User_Name,content.DeptName,code.Material_Attr01) planconen where 1=1");
            if (!string.IsNullOrEmpty(userremark)) {
                strSql.Append(@"  and Material_Attr01='" + userremark + "'");
            }
            DataSet ds = DataFactory.SqlDataBase().GetDataSetBySQL(strSql);
            dt = ds.Tables[0];
            //strSql.Append(SqlWhere);
            return dt;
        }


        public DataTable GetStoreTransation(StringBuilder SqlWhere, IList<SqlParam> IList_param)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * from MMS_StoreTransation where 1 =1");
            strSql.Append(SqlWhere);
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, IList_param.ToArray<SqlParam>());
        }




        public bool Add_MaterialAllotMember(string[] pkVal, string Store_id)
        {
            bool result;
            try
            {
                StringBuilder[] sqls = new StringBuilder[pkVal.Length + 1];
                object[] objs = new object[pkVal.Length + 1];
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("Delete From MMS_StoreTransation Where Store_id =@Store_id");
                SqlParam[] parm = new SqlParam[]
				{
					new SqlParam("@Store_id", Store_id)
				};
                sqls[0] = sbDelete;
                objs[0] = parm;
                int index = 1;
                for (int i = 0; i < pkVal.Length; i++)
                {
                    string item = pkVal[i];
                    if (item.Length > 0)
                    {
                        StringBuilder sbadd = new StringBuilder();
                        sbadd.Append("Insert into MMS_StoreTransation(");
                        sbadd.Append("UserRole_ID,User_ID,Roles_ID,CreateUserId,CreateUserName");
                        sbadd.Append(")Values(");
                        sbadd.Append("@UserRole_ID,@User_ID,@Material_ID,@CreateUserId,@CreateUserName)");
                        SqlParam[] parmAdd = new SqlParam[]
						{
							new SqlParam("@UserRole_ID", CommonHelper.GetGuid),
							new SqlParam("@User_ID", item),
							new SqlParam("@Store_id", Store_id),
							new SqlParam("@CreateUserId", RequestSession.GetSessionUser().UserId),
							new SqlParam("@CreateUserName", RequestSession.GetSessionUser().UserName)
						};
                        sqls[index] = sbadd;
                        objs[index] = parmAdd;
                        index++;
                    }
                }
                result = (DataFactory.SqlDataBase().BatchExecuteBySql(sqls, objs) >= 0);
            }
            catch
            {
                result = false;
            }
            return result;
        }

        #endregion Method





    }
}
