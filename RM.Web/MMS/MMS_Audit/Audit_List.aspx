<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Audit_List.aspx.cs" Inherits="RM.Web.MMS.MMS_Audit.Audit_List" %>
<%@ Register Src="../../UserControl/PageControl.ascx" TagName="PageControl" TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/LoadButton.ascx" TagName="LoadButton" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>物资领用审核</title>

 
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/TreeView/treeview.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/TreeView/treeview.pack.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        //初始化
        $(function () {
            treeAttrCss();
            $("#UserInfolist").height(270);
            $("#UserInfolistright").height(273);
            $("#UserInfolist").hide();
            //SubmitCheckForRC();
        })
        //双击添加员工
        function addUserInfo(userName, userID, Organization_Name) {
            var IsExist = true;
            $("#table1 tbody tr").each(function (i) {
                if ($(this).find('td:eq(0)').html() == userName) {
                    IsExist = false;
                    return false;
                }
            })
            if (IsExist == true) {
                $("#table1 tbody").append("<tr ondblclick='$(this).remove()'><td>" + userName + "</td><td>" + Organization_Name + "</td><td  style='display:none'>" + userID + "</td></tr>");
                publicobjcss();
            } else {
                showWarningMsg("【" + userName + "】员工已经存在");
            }
        }
        //面板切换
        function TabPanel(id) {
            if (id == 1) {
                $(".frm").show();
                $("#UserInfolist").hide();
            } else if (id == 2) {
                $(".frm").hide();
                $("#UserInfolist").show();
                //固定表头
                FixedTableHeader("#table1", $(window).height() - 127);
            }
        }
        //获取表单值
        function CheckValid() {
            if (!CheckDataValid('#form1')) {
                return false;
            }
            var item_value = "";
            $("#table1 tbody  tr").each(function (i) {
                item_value += $(this).find('td:eq(2)').html() + ",";
            })
            $("#User_ID_Hidden").val(item_value);
            if (!confirm('确认要保存此操作吗？')) {
                return false;
            }
        }


    </script>
    <style type="text/css">
        .style1
        {
            width: 642px;
        }
    </style>
</head>
<body>
    <form id="form2" runat="server">
    <div class="btnbartitle">
        <div>
            物资领用审核列表
        </div>
    </div>
    <div class="btnbarcontetn">
       
        <div style="text-align: right">
            <uc2:LoadButton ID="LoadButton1" runat="server" />
        </div>

        

    </div>
    <div class="div-body ">
    <table style="border-style: groove; border-width: thin" class ="width100">
        <tr>
            <td>
                <table >
                    <tr>
                        <td class="style1">
                            <cc1:GridView ID="dgvInfo" runat="server" AutoGenerateColumns="False" 
                                          DataKeyNames="ID" EmptyDataText="没有可显示的数据记录。" 
                                          onrowcommand="dgvInfo_RowCommand" AllowPaging="True" 
                                          CssClass="grid"
                                          onpageindexchanging="dgvInfo_PageIndexChanging" 
                                          EmptyDataTableCssClass="datagridTable" EmptyDataTitleRowCssClass="GvTitlebg" EmptyDataContentRowCssClass="datagridContentRow">
                                <PagerSettings Mode="NextPreviousFirstLast" FirstPageText="首页" 
                                               LastPageText="末页" NextPageText="下一页" PreviousPageText="上一页" />
                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" 
                                                    SortExpression="ID" Visible ="false" />
                                    <asp:TemplateField HeaderText="审核领料单">
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="chkAuditFlag" runat="server" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkAuditFlag" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="PurchaseBillCode" HeaderText="领料单号" 
                                                    SortExpression="PurchaseBillCode" />
                                     <asp:BoundField DataField="DeptName" HeaderText="领料部门" 
                                                    SortExpression="DeptName" />
                                    <asp:BoundField DataField="PurchaseMan" HeaderText="领料人工号" 
                                                    SortExpression="PurchaseMan" />
                                      
                                       <asp:BoundField DataField="PurchaseDate" HeaderText="领用日期" 
                                                    SortExpression="PurchaseDate" />

                                    <asp:ButtonField CommandName="Edi" Text="详细信息" />
                                </Columns>
                            </cc1:GridView>
                            
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class ="style3">
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnAudit" runat="server" Text="审核" onclick="btnAudit_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

     

    </div>
    </form>
</body>
</html>









