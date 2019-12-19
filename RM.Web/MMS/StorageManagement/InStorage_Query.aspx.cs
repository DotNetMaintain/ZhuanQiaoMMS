﻿using RM.Busines;
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
using RM.ServerControl;

namespace RM.Web.MMS.StorageManagement
{
    public partial class InStorage_Query : PageBase
{
    private string SqlWhere
    {
        get
        {
            if (ViewState["SqlWhere"] == null || ViewState["SqlWhere"].ToString() == "")
            {
                return "";
            }
            else
            {
                return ViewState["SqlWhere"].ToString();
            }
        }
        set { ViewState["SqlWhere"] = value; }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SqlWhere = "a.checkman='" + RequestSession.GetSessionUser().UserAccount + "'";
            List<TPurchaseQuery> sumList = QueryService.Instance.PurchaseQuery(SqlWhere, -1, OurPager1.PageSize);
            OurPager1.RecordCount = Convert.ToInt32(sumList[0].ID);
           // LoadData(SqlWhere, OurPager1.CurrentPageIndex);
        }
    }


    private void LoadData(string sqlWhere, int newPageIndex)
    {
        // sqlWhere = sqlWhere +" a.checkman='" + RequestSession.GetSessionUser().UserAccount + "'";
        List<TPurchaseQuery> lst = QueryService.Instance.PurchaseQuery(sqlWhere, newPageIndex, OurPager1.PageSize);
        dgvInfo.DataKeyNames = new[] { "ID" }; //设置GridView数据主键
        dgvInfo.DataSource = lst;
        dgvInfo.DataBind();
    }

    protected void dgvInfo_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.CommandName)) //判断命令名是否为空
        {
            if (e.CommandName == "Edi") //如果触发的是详细信息按钮事件
            {
                int index = Convert.ToInt32(e.CommandArgument); //取GridView行索引
                System.Web.UI.WebControls.GridView grid = (System.Web.UI.WebControls.GridView)e.CommandSource; //取当前操作的GridView
                int id = Convert.ToInt32(grid.DataKeys[index].Value); //取GridView主键值
                string provider = Convert.ToString(grid.Rows[index].Cells[3].Text); //取GridView主键值
                if (provider == "254")
                {
                    Response.Redirect(@"~/MMS/StorageManagement/StorageListInfoNoprice.aspx?Query=True&&ID=" + id.ToString());
                }
                else
                {
                    Response.Redirect(@"~/MMS/StorageManagement/StorageListInfo.aspx?Query=True&&ID=" + id.ToString());
                }



                LoadData(SqlWhere, OurPager1.CurrentPageIndex);
            }
        }
    }

    protected void dgvInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        (sender as RM.ServerControl.GridView).PageIndex = e.NewPageIndex; //指定GridView新页索引
        (sender as RM.ServerControl.GridView).DataBind(); //GridView数据源绑定
    }

    protected void dgvInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            List<TPurchaseQuery> sumList = QueryService.Instance.PurchaseQuery(SqlWhere, -1, OurPager1.PageSize);
            if (sumList.Count > 0)
            {
                RM.ServerControl.GridView grid = sender as RM.ServerControl.GridView; //取当前操作的GridView
                TPurchaseQuery sum = sumList[0];
            }
        }
    }

    protected void btnQueryCondition_Click(object sender, EventArgs e)
    {
        // SqlWhere = hidQueryCondition.Value;
        if (!string.IsNullOrEmpty(SqlWhere))
        {
            List<TPurchaseQuery> sumList = QueryService.Instance.PurchaseQuery(SqlWhere, -1, OurPager1.PageSize);
            OurPager1.RecordCount = Convert.ToInt32(sumList[0].ID);
            LoadData(SqlWhere, OurPager1.CurrentPageIndex);
        }
    }

    protected void OurPager1_PageChanged(object sender, PageArgs e)
    {
        LoadData(SqlWhere, e.NewPageIndex);
    }

    protected void btnsearch_Click(object sender, EventArgs e)
    {

        SqlWhere = "a.checkman='" + RequestSession.GetSessionUser().UserAccount + "'";

        if (!string.IsNullOrEmpty(txtpurchasebillcode.Text.Trim()))
        {
            SqlWhere = SqlWhere + "and purchasebillcode='" + txtpurchasebillcode.Text.Trim() + "'";
        }

        if (radlfixedset.SelectedValue == "1")
        {
            SqlWhere = SqlWhere + " and provider='254'";
        }
        if (radlfixedset.SelectedValue == "0")
        {
            SqlWhere = SqlWhere + " and provider<>'254'";
        }


        List<TPurchaseQuery> sumList = QueryService.Instance.PurchaseQuery(SqlWhere, -1, OurPager1.PageSize);
        OurPager1.RecordCount = Convert.ToInt32(sumList[0].ID);
        LoadData(SqlWhere, OurPager1.CurrentPageIndex);
    }


    
}
}



