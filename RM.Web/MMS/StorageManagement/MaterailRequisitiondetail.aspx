<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MaterailRequisitiondetail.aspx.cs" Inherits="RM.Web.MMS.StorageManagement.MaterailRequisitiondetail" %>

<!DOCTYPE html>

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
    <input type="button"  onclick="printer()" value="打印"/>
    <div class="wrapper wrapper-content animated fadeInRight">
        <div class="row">
            <div class="col-sm-12">
                <div class="ibox float-e-margins">
                        出库金额：<input id="count" style="border:none;background-color:#f3f3f4;" readonly="true"/>
                    <div class="ibox-content" id="tablediv">
                        <table style="font-size:15px; border:1px black solid;width:45%">
                            <tr style="border:1px black solid">
                                <td style="border:1px black solid">领料单号</td>
                                <td style="border:1px black solid"><%=inventorydetail.PurchaseBillCode %></td>
                                <td style="border:1px black solid">领料日期</td>
                                <td style="border:1px black solid"><%=inventorydetail.operatedate %></td>
                            </tr>
                            <tr style="border:1px black solid">
                                <td style="border:1px black solid">领料部门</td>
                                <td style="border:1px black solid"><%=inventorydetail.ValueName %></td>
                                <td style="border:1px black solid">领料人员</td>
                                <td style="border:1px black solid"><%=inventorydetail.user_name %></td>
                            </tr>
                        </table>
                        <table class="table table-striped table-bordered table-hover dataTables-example" id="tab" style="margin-top:20px;">
                              <thead>
                                    <tr>
                                        <th>物料名称</th>
                                        <th>物料规格</th>
                                        <th>单位</th>
                                        <th>领料数量</th>
                                        <th>单价</th>
                                        <th>总金额</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <%if (ulist.Count>0) {
                                           foreach (RM.ServiceProvider.Model.Inventorydetail item in ulist)
                                           { %>
                                    <tr>
                                        <td><%=item.material_name %></td>
                                        <td><%=item.Material_Specification %></td>
                                        <td><%=item.Material_Unit %></td>
                                        <td><%=item.quantity %></td>
                                        <td><%=item.price %></td>
                                        <td><%=item.amount %></td>
                                    </tr>
                    <% 
                            }
                        } %>
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
    <script src="/Themes/Scripts/LodopFuncs.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <object id="LODOP" classid="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" width="0" height="0">
  <embed id="LODOP_EM1" TYPE="application/x-print-lodop" width=0 height=0 PLUGINSPAGE="install_lodop32.exe"></object>
    <script> 
        $(function () {
            var count = document.getElementById("tab").rows.length;
            var title =0;
            for (var i = 1; i < count; i++) {
                title = title + parseFloat($("table").eq(1).find("tr").eq(i).find("td").eq(5).text());
            }
            $("#count").val(title);
        });
        //$(document).ready(function () { $(".dataTables-example").dataTable(); var oTable = $("#editable").dataTable(); oTable.$("td").editable("http://www.zi-han.net/theme/example_ajax.php", { "callback": function (sValue, y) { var aPos = oTable.fnGetPosition(this); oTable.fnUpdate(sValue, aPos[0], aPos[1]) }, "submitdata": function (value, settings) { return { "row_id": this.parentNode.getAttribute("id"), "column": oTable.fnGetPosition(this)[2] } }, "width": "90%", "height": "100%" }) }); 
        var LODOP;
        function printer() {
            prn1_preview();

        }



        function prn1_preview() {
            CreateOneFormPage();
            LODOP.PREVIEW();
        };

        function CreateOneFormPage() { 
            var count = document.getElementById("tab").rows.length;
            var title =0;
            for (var i = 1; i < count; i++) {
                title = title + parseFloat($("table").eq(1).find("tr").eq(i).find("td").eq(5).text());
            }
		LODOP=getLodop(document.getElementById('LODOP'),document.getElementById('LODOP_EM1'));  
            LODOP.PRINT_INIT("打印插件功能演示_Lodop功能_表单一");

            var strBodyStyle = "<style>table{ border-collapse:collapse; width:100%; font-size:15px; border:3px blue solid}td{ border:2px solid #cccccc;}</style>";
            var strFormHtml = strBodyStyle + "<body>" + document.getElementById("tablediv").innerHTML + "</body>";
            LODOP.ADD_PRINT_TEXT(50, 250, 980, 580, "上海闵行颛桥社区卫生服务中心 出库单");
            LODOP.SET_PRINT_TEXT_STYLE(1, "黑体", 20, 1, 0, 0, 1);
            LODOP.ADD_PRINT_HTM(88, 20, 1050, 650, strFormHtml);
            LODOP.ADD_PRINT_TEXT(50, 25, 980, 580, "出库金额:"+title);
        };
        </script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/TreeView/treeview.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/TreeView/treeview.pack.js" type="text/javascript"></script>
</body>
</html>
