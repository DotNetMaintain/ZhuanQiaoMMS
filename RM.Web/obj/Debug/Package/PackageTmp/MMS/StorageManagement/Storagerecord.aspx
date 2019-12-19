<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Storagerecord.aspx.cs" Inherits="RM.Web.MMS.StorageManagement.Storagerecord" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="shortcut icon" href="favicon.ico">
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="/Themes/Styles/bootstrap/bootstrap.min14ed.css" rel="stylesheet" />
    <link href="/Themes/Styles/bootstrap/font-awesome.min93e3.css" rel="stylesheet" />
    <link href="/Themes/Styles/bootstrap/dataTables.bootstrap.css" rel="stylesheet" />
    <link href="/Themes/Styles/bootstrap/animate.min.css" rel="stylesheet" />
    <link href="/Themes/Styles/bootstrap/style.min862f.css" rel="stylesheet" />
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body class="gray-bg"  style="overflow: scroll">
    <form runat="server" id="form1">
        
<%--    <div class="btnbarcontetn">--%>
        <div id="formContent">
                开始日期:
            <asp:TextBox ID="ttbStartDate" runat="server" class="txt" Style="width: 115px;"
                onfocus="WdatePicker({dateFmt: 'yyyy-MM-dd 00:00:00' })"></asp:TextBox>
                结束日期:
            <asp:TextBox ID="ttbEndDate" runat="server" class="txt" Style="width: 115px;"
                onfocus="WdatePicker({dateFmt: 'yyyy-MM-dd 00:00:00' })"></asp:TextBox>
                <asp:LinkButton ID="lbtSearch" runat="server" class="button green"
                    OnClick="lbtSearch_Click"><span class="icon-botton"
            style="background: url('/Themes/images/Search.png') no-repeat scroll 0px 4px;">
        </span>查 询</asp:LinkButton>
            </div>
        <%--</div>--%>
    <div class="wrapper wrapper-content animated fadeInRight" style="overflow: scroll">
        <div class="row">
            <div class="col-sm-12">
                <div class="ibox float-e-margins">
                    <div class="ibox-content">
                        <h3>入库记录</h3><asp:Button ID="btnexcelpurchase" runat="server" Text="导出excel" onclick="btnexcelpurchase_Click" />
                        <table class="table table-striped table-bordered table-hover dataTables-example">
                              <thead>
                                    <tr>
                                        <th>入库单号</th>
                                        <th>物料大类</th>
                                        <th>物料类型</th>
                                        <th>物料名称</th>
                                        <th>供应商</th>
                                        <th>发票号</th>
                                        <th>批号</th>
                                        <th>入库数</th>
                                        <th>单价</th>
                                        <th>总金额</th>
                                        <th>入库人员</th>
                                        <th>入库时间</th>
                                    </tr>
                                </thead>
                                <tbody><%if (dlist.Count>0) {
                                           foreach (RM.ServiceProvider.Model.Inventorydetail item in dlist)
                                           { %>
                                    <tr>
                                        <td><%=item.PurchaseBillCode %></td>
                                        <td><%=item.midtype %></td>
                                        <td><%=item.Material_Type %></td>
                                        <td><%=item.material_name %></td>
                                        <td><%=item.ValueName %></td>
                                        <td><%=item.invoicecode %></td>
                                        <td><%=item.lot %></td>
                                        <td><%=item.quantity %></td>
                                        <td><%=item.price %></td>
                                        <td><%=item.amount %></td>
                                        <td><%=item.user_name %></td>
                                        <td><%=item.operatedate %></td>
                                    </tr>
                    <% 
                                           } } %>
                                </tbody>
                        </table>
                        <h3>出库记录</h3><asp:Button ID="btnexcelpurchaseplan" runat="server" Text="导出excel" onclick="btnexcelpurchaseplan_Click" />
                        <table class="table table-striped table-bordered table-hover dataTables-example">
                              <thead>
                                    <tr>
                                        <th>领料单号</th>
                                        <th>物料大类</th>
                                        <th>物料类型</th>
                                        <th>物料名称</th>
                                    <%--    <th>领料部门</th>--%>
                                        <th>领料数</th>
                                        <th>单价</th>
                                        <th>总金额</th>
                                        <%--<th>领料人员</th>--%>
                                        <th>发料时间</th>
                                    </tr>
                                </thead>
                                <tbody><%if (dlist.Count>0) {
                                           foreach (RM.ServiceProvider.Model.deliveryquery item in dllist)
                                           { %>
                                    <tr>
                                        <td><%=item.PurchaseBillCode %></td>
                                        <td><%=item.material_name %></td>
                                        <td><%=item.midtype %></td>
                                        <td><%=item.material_type %></td>
                                        <%--<td><%=item.DeptName %></td>--%>
                                        <td><%=item.quantity %></td>
                                        <td><%=item.price %></td>
                                        <td><%=item.amount %></td>
                                        <%--<td><%=item.username %></td>--%>
                                        <td><%=item.OperatorDate %></td>
                                    </tr>
                    <% 
                                           } } %>
                                </tbody>
                        </table>

                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/jscript"></script>
<script src="/Themes/Scripts/bootstrap/bootstrap.min.js" type="text/jscript"></script>
<script src="/Themes/Scripts/bootstrap/jquery.jeditable.js" type="text/jscript"></script>
<script src="/Themes/Scripts/bootstrap/jquery.dataTables.js" type="text/jscript"></script>
<script src="/Themes/Scripts/bootstrap/dataTables.bootstrap.js" type="text/jscript"></script>
<script src="/Themes/Scripts/bootstrap/content.min.js" type="text/jscript"></script>
    <script> 
        $(document).ready(function () { $(".dataTables-example").dataTable(); var oTable = $("#editable").dataTable(); oTable.$("td").editable("http://www.zi-han.net/theme/example_ajax.php", { "callback": function (sValue, y) { var aPos = oTable.fnGetPosition(this); oTable.fnUpdate(sValue, aPos[0], aPos[1]) }, "submitdata": function (value, settings) { return { "row_id": this.parentNode.getAttribute("id"), "column": oTable.fnGetPosition(this)[2] } }, "width": "90%", "height": "100%" }) }); 
   </script>
</body>
</html>
