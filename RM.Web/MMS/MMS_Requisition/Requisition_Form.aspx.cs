﻿using RM.Busines;
using RM.Busines.DAL;
using RM.Busines.DAO;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Common.DotNetUI;
using RM.Web.App_Code;
using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Linq;
using System.Collections.Generic;
using RM.ServiceProvider;
using RM.ServiceProvider.Service;
using RM.ServiceProvider.Model;
using RM.ServiceProvider.Interface;
using RM.ServiceProvider.Dao;
using RM.ServiceProvider.Enum;
using System.Data;
using System.Text;

namespace RM.Web.MMS.MMS_Requisition
{
    public partial class Requisition_Form : PageBase
    {
        private string id
        {
            get
            {
                if (ViewState["id"] == null || ViewState["id"].ToString() == "")
                {
                    return "";
                }
                else
                {
                    return ViewState["id"].ToString();
                }
            }
            set { ViewState["id"] = value; }
        }

        private string detailId
        {
            get
            {
                if (ViewState["detailId"] == null || ViewState["detailId"].ToString() == "")
                {
                    return "";
                }
                else
                {
                    return ViewState["detailId"].ToString();
                }
            }
            set { ViewState["detailId"] = value; }
        }

        /// <summary>
        ///   session实体
        /// </summary>
        private TPurchasePlan _TPurchasePlan
        {
            get
            {
                if (Session["TPurchasePlan"] == null)
                {
                    return new TPurchasePlan { Content = new MMS_PurchasePlanContent() };
                }
                else
                {
                    return (TPurchasePlan)(Session["TPurchasePlan"]);
                }
            }
            set { Session["TPurchasePlan"] = value; }
        }

        RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDict("Unit", ddlUnit); //将计量单位下拉框绑定字典

                if (Request.QueryString["ID"] == null) //入库单录入页面
                {
                    _TPurchasePlan = null;
                    ClearTextBox(); //清除入库单相关服务器控件的内容
                }
                else //入库单修改页面
                {
                    id = Request.QueryString["ID"];
                    //调用业务规则层-入库单服务类方法获得要修改的实体
                    _TPurchasePlan = PurchasePlanService.Instance.GetPurchasePlan(Convert.ToInt32(id));
                    ModelToTextBox(_TPurchasePlan); //将入库单实体赋值给对应的服务器控件
                }
                LoadData(); //加载GridView数据
                EntryDetailInputPage(false); //切换到入库单页面
                if (Request.QueryString["Audit"] != null || Request.QueryString["Query"] != null)
                {
                    SetReadOnly(); //如果是审核或查询页面调用的，设置所有输入控件只读
                }
                // lblOperator.Text = LoginManager.GetUserName(Context.User.Identity.Name);
            }
        }


        /// <summary>
        ///   控件设置只读方法
        /// </summary>
        private void SetReadOnly()
        {

            txtPurchasePlanBillCode.ReadOnly = true;
            txtDeptName.ReadOnly = true;
            txtPurchasePlanDate.ReadOnly = true;
            imgPurchasePlanDate.Visible = false;
           // btnSave.Visible = false;
            dgvInfo.Columns[dgvInfo.Columns.Count - 1].Visible = false;
            //dgvInfo.Columns[dgvInfo.Columns.Count - 2].Visible = false;
        }




        #region Content相关

        /// <summary>
        ///   字典绑定通用方法
        /// </summary>
        /// <param name="typeCode"> </param>
        /// <param name="ddl"> </param>
        private void BindDict(string typeCode, DropDownList ddl)
        {
            //根据字典类型取字典项
            Dictionary<string, string> dictList = DictionaryInfoService.Instance.GetListByDictType(typeCode);
            ddl.DataSource = dictList; //设置下拉框的数据源
            ddl.DataTextField = "Value";
            ddl.DataValueField = "Key";
            ddl.DataBind(); //下拉框数据绑定
        }





        private DataTable findDeptManage()
        {

            string sql = @"select traff.User_ID,traff.User_Code,traff.User_Account,traff.User_Name,traff.Organization_Name,manageuserinfo.User_Account as Organization_Manager,userinfo.User_Account as Organization_Address from 
                    (select userinfo.User_ID,userinfo.User_Code,userinfo.User_Account,userinfo.User_Name,org.Organization_Name,
                    org.Organization_Manager,org.Organization_Address
                    from dbo.Base_UserInfo userinfo inner join Base_StaffOrganize staff 
                    on  userinfo.user_id=staff.user_id
                    inner join dbo.Base_Organization org on staff.Organization_ID=org.Organization_ID) traff
                    inner join dbo.Base_UserInfo manageuserinfo on traff.Organization_Manager=manageuserinfo.User_Name
                    inner join dbo.Base_UserInfo userinfo on traff.Organization_Address=userinfo.User_Name
                    where traff.User_Account='{0}'";
            sql = string.Format(sql, RequestSession.GetSessionUser().UserAccount.ToString());
            DataTable dt_DeptInfo = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));

            return dt_DeptInfo;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {


            if (Page.IsValid)
            {
               
                TPurchasePlan info = _TPurchasePlan;
                if (info.Detail.Count == 0)
                {
                    Response.Write("<Script>window.alert('请先录入明细信息!')</Script>");
                    return;
                }


                MMS_PurchasePlanContent pstore = PurchasePlanService.Instance.GetPurchasePlanCode(txtPurchasePlanBillCode.Text.ToString());
                if (pstore != null)
                {
                    Response.Write("<Script>window.alert('该领料单已经保存！')</Script>");
                    return;
                }
               

               
                            TextBoxToModel(info);

                            info.Content.Operator = RequestSession.GetSessionUser().UserId.ToString();
                           
                            info.Content.PayMode = "1";
                           
                            info.Content.DeptName = txtDeptName.Text.ToString().Trim();
                info.Content.OperateDate = (DateTime?)DataFactory.SqlDataBase().GetObjectValue(new StringBuilder("select getdate()"));
                if (string.IsNullOrEmpty(id)) //插入
                            {
                                info.OprType = OperateType.otInsert;
                            }
                            else //修改
                            {
                                info.OprType = OperateType.otUpdate;
                            }

                            //再循环一次查找入库单是否有库存，解决并发领料的问题
                            //if (info.Detail.Count>0)
                            //{
                            //    int detailqua = 0;
                            //   foreach(TPurchasePlanDetail d in info.Detail)
                            //   {
                            //       detailqua=info.Detail.FindAll(p => p.DetDetail.ProductCode == d.DetDetail.ProductCode && p.DetDetail.Price == d.DetDetail.Price).Sum(s => s.DetDetail.Quantity);
                            //       int total = storenum(d.DetDetail.ProductCode.ToString(), Convert.ToDecimal(d.DetDetail.Price));
                            //       if (total < detailqua)
                            //       {
                            //          // info.Detail.Remove(d);
                            //           Response.Write("<Script>window.alert('物料编码为" + d.DetDetail.ProductCode + "的物料，领用数量已超过库存数或此时他人已在您先一步领取！')</Script>");
                            //           return;
                            //       }

                            //   }
                            //}
                            int tempId = PurchasePlanService.Instance.SavePurchasePlan(info);
                           
                            if (tempId > 0)
                            {
                                _TPurchasePlan = PurchasePlanService.Instance.GetPurchasePlan(tempId);
                                id = tempId.ToString();
                                ModelToTextBox(_TPurchasePlan);
                                LoadData();
                            }
                            Response.Write("<Script>window.alert('保存成功!')</Script>");
                            Response.Redirect("/MMS/MMS_Requisition/Requisition_Form.aspx");


            
                


            }
        }

        private void TextBoxToModel(TPurchasePlan info)
        {
            info.Content.AuditFlag = false;
            //info.Content.InvoiceCode = txtInvoiceCode.Text;
            //info.Content.Provider = hidProvider.Value;
            info.Content.PurchaseBillCode = txtPurchasePlanBillCode.Text;
            if (!string.IsNullOrEmpty(txtPurchasePlanDate.Text))
            {
                info.Content.PurchaseDate = Convert.ToDateTime(txtPurchasePlanDate.Text);
            }
            //info.Content.CheckMan = hidPurchaseMan.Value;

        }

        private void ClearTextBox()
        {
            txtPurchasePlanBillCode.Text = Convert.ToDateTime(DataFactory.SqlDataBase().GetObjectValue(new StringBuilder("select getdate()"))).ToString("yyyyMMddhhmmssfff"); //入库单号
            txtPurchasePlanDate.Text = Convert.ToDateTime(DataFactory.SqlDataBase().GetObjectValue(new StringBuilder("select getdate()"))).ToString("yyyy-MM-dd HH:mm:ss"); //入库日期
            txtDeptName.Text = "";
            lblOperator.Text = RequestSession.GetSessionUser().UserName.ToString(); ; //领料人员
            //hidProvider.Value = "";
            //hidProviderName.Value = "";

            // txTPurchasePlanManName.Text = ""; //经办人
            //hidPurchaseMan.Value = "";
            //hidPurchaseManName.Value = "";

        }

        private void ModelToTextBox(TPurchasePlan info)
        {

            if (!string.IsNullOrEmpty(info.Content.Provider)) //供应商
            {
                //hidProvider.Value = info.Content.Provider;
                Base_ClientInfo tempClient = ClientInfoService.Instance.GetClientInfoByCode(info.Content.Provider);
                if (tempClient != null)
                {
                    //txtProviderName.Text = tempClient.ShortName;
                    //hidProviderName.Value = tempClient.ShortName;
                }
            }
            txtPurchasePlanBillCode.Text = info.Content.PurchaseBillCode; //入库单号
            txtPurchasePlanDate.Text = info.Content.PurchaseDate.ToString(); //入库日期
            lblOperator.Text = user_idao.GetUserInfo(info.Content.Operator).Rows[0]["User_Name"].ToString();
            //if (!string.IsNullOrEmpty(info.Content.CheckMan)) //经办人
            //{
            //    //hidPurchaseMan.Value = info.Content.CheckMan;
            //    //EmployeeInfo tempEmployee = EmployeeInfoService.Instance.GetEmployeeInfoByCode(info.Content.CheckMan);
            //    //if (tempEmployee != null)
            //    //{
            //    //    txTPurchasePlanManName.Text = tempEmployee.Name;
            //    //    hidPurchaseManName.Value = tempEmployee.Name;
            //    //}
            //}

        }

        /// <summary>
        ///   跳转到入库单管理页面
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["Audit"] != null) //如果是审核页面调用
            {
                Response.Redirect(@"~/MMS/MMS_Audit/Audit_List.aspx");
            }
            else if (Request.QueryString["Query"] != null) //如果是查询页面调用
            {
                Response.Redirect(@"~/MMS/MMS_Requisition/Requisition_Form.aspx");
            }
            else
            {
                Response.Redirect(@"~/MMS/MMS_Requisition/Requisition_Form.aspx"); //返回采购计划管理页面
            }
        }



        #endregion

        #region Detail列表

        private void LoadData()
        {
            TPurchasePlan infoList = _TPurchasePlan;
            //调用业务层返回货品信息列表
            List<MMS_MaterialInfo> productList = MaterialInfoService.Instance.GetAllInfo();
            //调用业务层返回字典信息列表
            List<Base_DictionaryInfo> dictList = DictionaryInfoService.Instance.GetAllInfo();
            //将入库单货品与货品信息、字典保关联
            var query = from info in infoList.Detail
                        where info.OprType != OperateType.otDelete
                        join product in productList
                            on info.DetDetail.ProductCode equals product.Material_ID.ToString()
                        select new
                        {
                            Id = info.DetDetail.ID,
                            info.DetDetail.PurchaseBillCode,
                            product.Material_ID,
                            product.Material_Name,
                            product.Material_Specification,
                            product.Material_Supplier,
                            Unit = product.Material_Unit,
                            info.DetDetail.Quantity,
                            info.DetDetail.Price,
                            Amount = info.DetDetail.Quantity * info.DetDetail.Price,
                            info.DetDetail.Memo
                        };
            dgvInfo.DataKeyNames = new[] { "Id" }; //设置GridView数据主键
            dgvInfo.DataSource = query.ToList(); //设置GridView数据源
            dgvInfo.DataBind();
            //计算数量及金额汇总信息
            lblTotalQuantity.Text = infoList.Detail.Sum(itm => itm.DetDetail.Quantity).ToString("#");
            // lblTotalAmount.Text = infoList.Detail.Sum(itm => itm.DetDetail.Quantity*itm.DetDetail.Price).ToString("#.##");
        }

        /// <summary>
        ///   GridView行命令事件，点击GridView行按钮时触发的事件
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        protected void dgvInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.CommandName)) //判断命令名是否为空
            {
                if (e.CommandName == "Edi") //如果触发的是详细信息按钮事件
                {
                    int index = Convert.ToInt32(e.CommandArgument); //取GridView行索引
                    GridView grid = (GridView)e.CommandSource; //取当前操作的GridView
                    int tempId = Convert.ToInt32(grid.DataKeys[index].Value);
                    detailId = tempId.ToString();
                    TPurchasePlanDetail detail = _TPurchasePlan.Detail.FirstOrDefault(itm => itm.DetDetail.ID == tempId);
                    ModelToDetailTextBox(detail); //将实体赋值给对应的服务器控件 
                    EntryDetailInputPage(true); //切换到货品录入页面


                }
                else if (e.CommandName == "Del")
                {
                    int index = Convert.ToInt32(e.CommandArgument); //取GridView行索引
                    GridView grid = (GridView)e.CommandSource; //取当前操作的GridView
                    int id = Convert.ToInt32(grid.DataKeys[index].Value); //取GridView主键值

                    TPurchasePlan temp = _TPurchasePlan;
                    TPurchasePlanDetail tempDetail = temp.Detail.First(itm => itm.DetDetail.ID == id);
                    if (tempDetail.OprType == OperateType.otInsert) //如果是新插入的直接将其删除
                    {
                        temp.Detail.Remove(tempDetail);
                    }
                    else //如是不是新插入的置删除标志
                    {
                        tempDetail.OprType = OperateType.otDelete;
                    }
                    _TPurchasePlan = temp;
                    LoadData();
                }
                else if (e.CommandName == "Page")
                {
                    LoadData();
                }
            }
        }

        /// <summary>
        ///   添加按钮单击事件
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            detailId = ""; //编辑时detailId为编辑行id
            //ClearDetailTextBox();
            //EntryDetailInputPage(true); //切换到货品录入页面
        }

        /// <summary>
        ///   GridView页改变事件
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        protected void dgvInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            (sender as GridView).PageIndex = e.NewPageIndex; //指定GridView新页索引
            (sender as GridView).DataBind(); //GridView数据源绑定
        }

        /// <summary>
        ///   GridView行绑定事件
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        protected void dgvInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow) //如果是数据行
            {
                GridView grid = sender as GridView; //取当前操作的GridView
                //为GridView数据行的删除按钮添加删除确认对话框
                ((LinkButton)(e.Row.Cells[grid.Columns.Count - 1].Controls[0])).Attributes.Add("onclick",
                                                                                                "return confirm('确认删除?');");
            }
        }

        #endregion

        #region Detail录入

        protected void btnOK_Click(object sender, EventArgs e)
        {
                TPurchasePlan info = _TPurchasePlan;
                if (string.IsNullOrEmpty(detailId)) //插入操作
                {
                    //创建入库货品实例
                    TPurchasePlanDetail tinfoDetail = new TPurchasePlanDetail();
                    tinfoDetail.DetDetail = new MMS_PurchasePlanDetail();
                    if (info.Detail.Count > 0) //新插入的以-1开始,以后渐减
                    {
                        //设置新录入入库货品的主键ID，以-1开始,以后渐减
                        int minId = info.Detail.Min(itm => itm.DetDetail.ID);
                        if (minId < 0)
                            tinfoDetail.DetDetail.ID = minId - 1;
                        else
                            tinfoDetail.DetDetail.ID = -1;
                    }
                    else //该入库单没有货品信息
                    {
                        tinfoDetail.DetDetail.ID = -1;
                    }
                    DetailTextBoxToModel(tinfoDetail); //将入库货品赋值给实体
                    tinfoDetail.OprType = OperateType.otInsert;
                    info.Detail.Add(tinfoDetail); //将操作实体添加到入库货品集合中
                    _TPurchasePlan = info;

                    ClearDetailTextBox(); //清除入库货品服务器控件内容
                    LoadData(); //加载Gridview数据
                }
                else //编辑操作
                {
                    //根据入库货品ID取实体
                    TPurchasePlanDetail tinfoDetail = info.Detail.First(itm => itm.DetDetail.ID == Convert.ToInt32(detailId));
                    DetailTextBoxToModel(tinfoDetail); //将服务器控件赋给实体
                    if (tinfoDetail.OprType != OperateType.otInsert) //如果是新插入的仍保留插入状态
                    {
                        tinfoDetail.OprType = OperateType.otUpdate;
                    }
                    _TPurchasePlan = info;
                    LoadData(); //加载GridView数据
                    EntryDetailInputPage(false); //切换到入库单录入页面
                }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            detailId = "";

            EntryDetailInputPage(false); //切换到入库单录入页面
        }

        private void ClearDetailTextBox()
        {
            txtProductCode.Text = ""; //货品代码
            txtShortName.Text = ""; //货品简称
            txtSpecs.Text = ""; //规格型号
            ddlUnit.SelectedIndex = 0; //计量单位
            txtQuantity.Text = ""; //数量
            txtUseQuantity.Text = "";  //可用数量
            txtVendor.Text = "";     //生产厂商
            txtPrice.Text = "";      //价格

            hidProductCode.Value = ""; //货品代码
            hidProductName.Value = ""; //货品名称
            // hidPrice.value = "";
        }

        private void ModelToDetailTextBox(TPurchasePlanDetail tinfo)
        {
            //调用业务层方法取货品信息实体
            MMS_MaterialInfo Material = MaterialInfoService.Instance.GetProductInfoByCode(tinfo.DetDetail.ProductCode);
            // MMS_ProductInfo product = ProductInfoService.Instance.GetProductInfoByCode(tinfo.DetDetail.ProductCode);
            MMS_PurchasePlanDetail detail = tinfo.DetDetail;
            txtProductCode.Text = detail.ProductCode; //货品代码
            txtShortName.Text = Material.Material_Name; //货品简称
            txtSpecs.Text = Material.Material_Specification; //规格
            ddlUnit.Text = Material.Material_Unit; //计量单位
            txtVendor.Text = Material.Material_Supplier;  //供应商
            txtQuantity.Text = detail.Quantity.ToString(); //数量
            txtComm.Text = detail.Memo.ToString();

            // txtPrice.Text = hidPrice.Value.ToString();
        }

        private void DetailTextBoxToModel(TPurchasePlanDetail tinfo)
        {
            MMS_PurchasePlanDetail detail = tinfo.DetDetail;
            detail.PurchaseBillCode = txtPurchasePlanBillCode.Text; //入库单号
            detail.ProductCode = txtProductCode.Text; //货品代码
            detail.Memo = txtComm.Text;   //手写备注
            if (!string.IsNullOrEmpty(txtQuantity.Text)) //数量
            {
                detail.Quantity = Convert.ToInt32(txtQuantity.Text);
            }
            detail.Price = Convert.ToDouble(txtPrice.Text.ToString().Trim());

        }


        private int storenum(string productcode ,decimal price)
        {
            int totalnum=0;
            string sqlwhere = string.Empty;
            string str = @"select mms.Material_ID,mms.material_name,mms.Material_CommonlyName,Material_Specification,Material_Unit,Material_Supplier,Material_Type,pur.* from dbo.MMS_MaterialInfo mms inner join (
                        select pur.productcode,(isnull(pur.qua,0)-isnull(pur.usequa,0)) as qua,pur.price from (select productcode,sum(quantity) as  qua,sum(usequantity) as usequa,price from MMS_PurchaseDetail group by productcode,price) pur
                        left join (select * from (select productcode,sum(quantity) as qua,price from (
select detail.* from MMS_PurchasePlanContent plancontent 
inner join MMS_PurchasePlanDetail  detail
on plancontent.PurchaseBillCode=detail.PurchaseBillCode
where plancontent.paymode in ('1','2') and detail.AuditFlag is NULL  or detail.AuditFlag=1) de
group by productcode,price) purplan)purplan
                        on pur.ProductCode=purplan.ProductCode and pur.price=purplan.price) pur
                        on mms.material_id=pur.productcode
                        where (qua>0 or qua is null)";


            if (!string.IsNullOrEmpty(productcode.Trim()))
            {
                sqlwhere = " and material_id ='" + productcode.Trim() + "'";
                str = str + sqlwhere;
            }

            sqlwhere = " and Price =" + price ;
                str = str + sqlwhere;
            
           DataTable dt_Material = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(str));
           if (dt_Material.Rows.Count < 1)
           {
               return totalnum;
           }
           totalnum = Convert.ToInt32(dt_Material.Rows[0]["qua"].ToString());
           return totalnum;
            
        }

        /// <summary>
        ///   列表页及录入页切换
        /// </summary>
        /// <param name="isEntry"> 为真切换至录入页,为假切换至列表页 </param>
        private void EntryDetailInputPage(bool isEntry)
        {
            //pnlContent.Visible = !isEntry; //采购计划单面板
            // pnlDetail.Visible = isEntry; //采购货品面板
        }

        /// <summary>
        ///   选择货品
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        protected void btnSelectProduct_Click(object sender, EventArgs e)
        {
            txtProductCode.Text = hidProductCode.Value.Trim();
            txtShortName.Text = hidProductName.Value.Trim();
            txtPrice.Text = hidPrice.Value.Trim();
            txtUseQuantity.Text = hidUseQuantity.Value.Trim();
            //调用业务层的方法取货品信息实体
            MMS_MaterialInfo Material = MaterialInfoService.Instance.GetProductInfoByCode(txtProductCode.Text.Trim());
            //    MMS_ProductInfo prod = ProductInfoService.Instance.GetProductInfoByCode(txtProductCode.Text);
            if (Material != null)
            {
                txtSpecs.Text = Material.Material_Specification; //规格
                ddlUnit.SelectedItem.Text = Material.Material_Unit; //计量单位
                txtVendor.Text = Material.Material_Supplier;  //厂商

                //单价
                //txtPrice.Text =
                //    PurchasePlanService.Instance.GetLasTPurchasePlanPrice(hidProvider.Value, txtProductCode.Text).ToString();
            }
        }

        #endregion

        protected void btnSelectDept_Click(object sender, EventArgs e)
        {
            txtDeptName.Text = hidDeptName.Value;
        }


    }
}


