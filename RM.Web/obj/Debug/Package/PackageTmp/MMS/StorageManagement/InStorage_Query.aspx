<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InStorage_Query.aspx.cs" Inherits="RM.Web.MMS.StorageManagement.InStorage_Query" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>入库单查询</title> 
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/jquery.divbox.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/TreeView/treeview.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/TreeView/treeview.pack.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
  
</head>  
    
    <body>
    <form id="form2" runat="server">
       <div class="div-body">
       <table>
       <tr><td>入库单号</td>
       <td>
           <asp:TextBox ID="txtpurchasebillcode" runat="server"></asp:TextBox></td>
       
       <td>
          是否固定资产：<asp:RadioButtonList 
            ID="radlfixedset" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
            <asp:ListItem Selected="True" Value="0">否</asp:ListItem>
            <asp:ListItem Value="1">是</asp:ListItem>
        </asp:RadioButtonList>

       </td>
       <td>
           <asp:Button ID="btnsearch" runat="server" Text="查询" 
               onclick="btnsearch_Click" /></td>
       </tr>
       </table>
    <table class="example" id="dnd-example">
        <tr>
            <td>
                <table class ="width100">
                    <tr>
                        <td>
                            <cc1:GridView ID="dgvInfo" runat="server" AutoGenerateColumns="False" 
                                          DataKeyNames="ID" EmptyDataText="没有可显示的数据记录。" 
                                          onrowcommand="dgvInfo_RowCommand" 
                                          onpageindexchanging="dgvInfo_PageIndexChanging" 
                                          onrowdatabound="dgvInfo_RowDataBound" ShowFooter="True"
                                          CssClass="grid"
                                          EmptyDataTableCssClass="datagridTable" EmptyDataTitleRowCssClass="GvTitlebg" EmptyDataContentRowCssClass="datagridContentRow">
                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" 
                                                    SortExpression="ID" Visible ="false" />
                                    <asp:BoundField DataField="PurchaseBillCode" HeaderText="入库单号" 
                                                    SortExpression="PurchaseBillCode" />
                                     
                                    <asp:BoundField DataField="PurchaseMan" HeaderText="入库人" 
                                                    SortExpression="PurchaseMan" />
                                      <asp:BoundField DataField="Provider" HeaderText="供应商" 
                                                    SortExpression="Provider"  />
                                    <asp:BoundField DataField="PurchaseDate" HeaderText="入库日期" 
                                                    SortExpression="PurchaseDate" DataFormatString="{0:yyyy-MM-dd}"/>
                                                                   
                                    <asp:ButtonField CommandName="Edi" Text="详细信息" />
                                </Columns>
                            </cc1:GridView>
                            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <cc1:OurPager ID="OurPager1" runat="server" onpagechanged="OurPager1_PageChanged" />                        
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
