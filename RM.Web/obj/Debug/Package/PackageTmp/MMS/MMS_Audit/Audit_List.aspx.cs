using RM.Busines;
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


namespace RM.Web.MMS.MMS_Audit
{
    public partial class Audit_List : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadData();
        }
    }

    /// <summary>
    ///   GridView数据导入方法
    /// </summary>
    private void LoadData()
    {
        List<MMS_PurchasePlanContent> infoList = new List<MMS_PurchasePlanContent>();
        //调用业务层方法取采购计划单列表
        infoList = PurchasePlanService.Instance.GetAllInfo();
               
        //取所有的字典信息
        List<Base_DictionaryInfo> dictList = DictionaryInfoService.Instance.GetAllInfo();

        var query = from info in infoList
                    where (info.AuditFlag == null || info.AuditFlag == false) && info.PurchaseMan==RequestSession.GetSessionUser().UserAccount.ToString().Trim()
                    select new
                        {
                            id = info.ID,
                            info.PurchaseBillCode,
                            info.PurchaseMan,
                            info.PurchaseDate,
                            info.InvoiceCode,
                            info.DeptName ,
                            info.AuditFlag

                        };
        dgvInfo.DataKeyNames = new[] {"ID"}; //设置GridView数据主键
        dgvInfo.DataSource = query.ToList(); //设置GridView数据源
        dgvInfo.DataBind();
    }

    /// <summary>
    ///   GridView行命令事件
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
                GridView grid = (GridView) e.CommandSource; //取当前操作的GridView
                int id = Convert.ToInt32(grid.DataKeys[index].Value); //取GridView主键值
                Response.Redirect(@"~/MMS/MMS_Requisition/Requisition_Form.aspx?Audit=True&&ID=" + id.ToString());
                LoadData();
            }
            else if (e.CommandName == "Page")
            {
                LoadData();
            }
        }
    }

    /// <summary>
    ///   采购计划审核确认
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void btnAudit_Click(object sender, EventArgs e)
    {
        int cnt = 0;
        foreach (GridViewRow row in dgvInfo.Rows) //遍历GridView所有的行
        {
            bool chk = ((CheckBox) row.Cells[1].FindControl("chkAuditFlag")).Checked;
            if (chk) //判断该行是行被选中
            {
                cnt++;
                //取GridView行主键值
                string tempId = dgvInfo.DataKeys[row.RowIndex].Value.ToString();
                string billCode = row.Cells[2].Text; //取GridView行单据号内容
                if (!string.IsNullOrEmpty(tempId))
                {
                    try
                    {
                        //调用业务层采购计划审核方法
                        PurchasePlanService.Instance.AuditPurchasePlan(Convert.ToInt32(tempId), true,
                                                                       Context.User.Identity.Name);
                        ((CheckBox) row.Cells[1].FindControl("chkAuditFlag")).Checked = false;
                    }
                    catch
                    {
                        throw new Exception("单据号" + billCode + "审核失败");
                    }
                }
            }
        }
        if (cnt == 0)
        {
            Response.Write("<Script>window.alert('请选择要转订单的采购计划单!')</Script>");
        }
        else
        {
            LoadData();
            Response.Write("<Script>window.alert('审核成功!')</Script>");
        }
    }

    /// <summary>
    ///   GridView页索引改变事件
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void dgvInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        (sender as GridView).PageIndex = e.NewPageIndex; //指定GridView新页索引
        (sender as GridView).DataBind(); //GridView数据源绑定
    }
    }
}





