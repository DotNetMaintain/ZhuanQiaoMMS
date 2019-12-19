<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MatStorequerydetail.aspx.cs" Inherits="RM.Web.MMS.report.MatStorequerydetail" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="shortcut icon" href="favicon.ico">
    <link href="/Themes/Styles/bootstrap/bootstrap.min14ed.css" rel="stylesheet" />
    <link href="/Themes/Styles/bootstrap/font-awesome.min93e3.css" rel="stylesheet" />
    <link href="/Themes/Styles/bootstrap/dataTables.bootstrap.css" rel="stylesheet" />
    <link href="/Themes/Styles/bootstrap/animate.min.css" rel="stylesheet" />
    <link href="/Themes/Styles/bootstrap/style.min862f.css" rel="stylesheet" />
</head>
<body class="gray-bg">
    <div class="wrapper wrapper-content animated fadeInRight">
        <div class="row">
            <div class="col-sm-12">
                <div class="ibox float-e-margins">
                    <div class="ibox-content">
                        <div>
                            入库总数：<label><%=q.quantity %></label>
                            出库总数：<label><%=q.usequantity %></label>
                            结余：<label><%=q.qua %></label>
                        </div>
                        <table class="table table-striped table-bordered table-hover dataTables-example">
                              <thead>
                                    <tr>
                                        <th>物料名称</th>
                                        <th>入库单号</th>
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
                                        <td><%=item.material_name %></td>
                                        <td><%=item.PurchaseBillCode %></td>
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
                        
                        <div>
                            出库总数：<label><%=qs.quantity %></label>
                            结余：<label><%=qs.qua %></label>
                        </div>
                        <table class="table table-striped table-bordered table-hover dataTables-example">
                              <thead>
                                    <tr>
                                        <th>物料名称</th>
                                        <th>出库单号</th>
                                        <th>领料部门</th>
                                        <th>领料数</th>
                                        <th>单价</th>
                                        <th>总金额</th>
                                        <th>领料人员</th>
                                        <th>领料时间</th>
                                    </tr>
                                </thead>
                                <tbody><%if (dlist.Count>0) {
                                           foreach (RM.ServiceProvider.Model.deliveryquery item in dllist)
                                           { %>
                                    <tr>
                                        <td><%=item.material_name %></td>
                                        <td><%=item.PurchaseBillCode %></td>
                                        <td><%=item.DeptName %></td>
                                        <td><%=item.quantity %></td>
                                        <td><%=item.price %></td>
                                        <td><%=item.amount %></td>
                                        <td><%=item.username %></td>
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
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/jscript"></script>
<script src="/Themes/Scripts/bootstrap/bootstrap.min.js" type="text/jscript"></script>
<script src="/Themes/Scripts/bootstrap/jquery.jeditable.js" type="text/jscript"></script>
<script src="/Themes/Scripts/bootstrap/jquery.dataTables.js" type="text/jscript"></script>
<script src="/Themes/Scripts/bootstrap/dataTables.bootstrap.js" type="text/jscript"></script>
<script src="/Themes/Scripts/bootstrap/content.min.js" type="text/jscript"></script>
    <script> 
        $(document).ready(function () {
            $(".dataTables-example").dataTable(); var oTable = $("#editable").dataTable(); oTable.$("td").editable("http://www.zi-han.net/theme/example_ajax.php", { "callback": function (sValue, y) { var aPos = oTable.fnGetPosition(this); oTable.fnUpdate(sValue, aPos[0], aPos[1]) }, "submitdata": function (value, settings) { return { "row_id": this.parentNode.getAttribute("id"), "column": oTable.fnGetPosition(this)[2] } }, "width": "90%", "height": "100%" })
        }); 
   </script>
</body>
</html>
