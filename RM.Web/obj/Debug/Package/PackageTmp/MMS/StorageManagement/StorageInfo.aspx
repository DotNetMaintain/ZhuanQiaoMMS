<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StorageInfo.aspx.cs" Inherits="RM.Web.MMS.StorageManagement.StorageInfo" %>

<%@ Register Src="/UserControl/LoadButton.ascx" TagName="LoadButton" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>发货表单</title>


    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/TreeView/treeview.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/TreeView/treeview.pack.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/LodopFuncs.js" type="text/javascript"></script>
    <style>
        
    .mytable {
        border: 1px solid #e7e7e7;
        text-align: center;
        color:black;
    }

        .mytable td {
        border: 1px solid #e7e7e7;
            text-align: center;
            height: 35px;
            font-size: 13px;
        }

        thead td {
        background-color: #F5F5F5;
        }
        #formContent {
        font-size:14px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            divresize(63);
            FixedTableHeader("#dnd-example", $(window).height() - 91);
            GetClickTableValue();
            GetClickTableValues();
        })

        var LODOP; //声明为全局变量

        var CheckAllLinestatus = 0;
        function CheckAllLine() {
            if (CheckAllLinestatus == 0) {
                CheckAllLinestatus = 1;
                $(".check :checkbox").attr("checked", true);
            } else {
                CheckAllLinestatus = 0;
                $(".check :checkbox").attr("checked", false);
            }
        }
        

        var data = [];
        var datas = [];

        function GetClickTableValue() {  //jquery获取复选框值
            $('table tr').not('#td').click(function () {

                var chk_value = [];

                $('input[name="checkbox"]:checked').each(function () {
                    chk_value.push($(this).val());
                });
                data = chk_value;
            });

        }
        var b;

        function GetClickTableValues() {  //jquery获取复选框值
            $('table tr').not('#td').click(function () {
                b = 0;
                var chk_value = [];

                $('input[name="checkbox"]:checked').each(function () {
                    chk_value.push($(this).val());
                    chk_value.push($(this).siblings().val());
                    chk_value.push($(this).parent().find('input').eq(2).val());
                    if ($(this).siblings().val() > $(this).parent().find('input').eq(2).val()) {
                        b = 1;
                    }
                });
                datas = chk_value;
            });
        }



        //新增
        function allotButton() {
            if ($("#ddlStates").val() != "已发料") {
                //if (b == 1) {
                //    alert("发货数量不能大于库存");
                //} else
                    if (data.length == 0) {
                    alert("请选择要发货的数据");
                } else {
                    deliveryConfig('/Ajax/Delivery_Button.ashx', data)
                }
            }
        }


        //退库还原按钮
        function restore() {
            if ($("#ddlStates").val() != "退回") {
                if (data.length == 0) {
                    alert("请选择要发货的数据");
                } else {
                    restoreConfig('/Ajax/Restore_Button.ashx', data)
                }
            }
        }

        //编辑
        function edit() {
            if ($("#ddlStates").val() == "已发料") {
                alert("已发料状态不能修改数量");
            } else if (datas.length > 3) {
                alert("每次只能编辑一条数据");
            } else {
                var url = "/MMS/StorageManagement/UpdateStorage.aspx?data=" + datas;
                top.openDialog(url, 'UpdateStorage', '领料单 - 编辑', 280, 300, 50, 50);
            }
        }


        function printer() {
            prn1_preview();

        }



        function prn1_preview() {
            CreateOneFormPage();
            LODOP.PREVIEW();
        };

        function CreateOneFormPage() {
            var a = document.getElementsByName("checkbox");
            if (data.length < 1) {
                alert("请选择领料信息");
                return;
            }

            LODOP = getLodop(document.getElementById('LODOP'), document.getElementById('LODOP_EM1'));
            LODOP = getLodop();
            LODOP.PRINT_INIT("打印插件功能演示_Lodop功能_表单一");
            var strBodyStyle = "<style>table{ border-collapse:collapse; width:100%; font-size:15px; border:3px blue solid}td{ border:2px solid #cccccc;}</style>";
            var strFormHtml = strBodyStyle + "<body><div class='div-body'>";
            var datalist = "";

            LODOP.ADD_PRINT_TEXT(20, 380, 650, 681, "领料单");
            LODOP.SET_PRINT_TEXT_STYLE(1, "黑体", 20, 1, 0, 0, 1);
            strFormHtml = strBodyStyle + "<table class='grid'>";
            strFormHtml = strFormHtml + " <thead> <tr><td style='width:10%; text-align: center;'>领料单号</td><td style='width:10%; text-align: center;'>领料部门</td><td style='width:15%; text-align: center;'>物资类型</td><td style='width:20%; text-align: center;'>物资名称</td><td style='width:10%; text-align: center;'>物资规格</td><td style='width:4%; text-align: center;'>单位</td><td style='width:5%; text-align: center;'>数量</td><td style='width:5%; text-align: center;'>金额</td><td style='width:6%; text-align: center;'>合计</td><td style='width:6%; text-align: center;'>库存</td><td style='width:5%; text-align: center;'>领料日期</td></tr></thead>"
            var rows = document.getElementById("tablediv").rows;
            trFormHtml = strFormHtml + "<tbody>";
            var total = 0;
            for (var j = 0; j < a.length; j++) {
                datalist = datalist + "<tr>";
                if (a[j].checked) {
                    var row = a[j].parentElement.parentElement.rowIndex;
                    datalist = datalist + "<td>" + rows[row].cells[1].innerHTML + "</td>";
                    datalist = datalist + "<td>" + rows[row].cells[2].innerHTML + "</td>";
                    datalist = datalist + "<td>" + rows[row].cells[3].innerHTML + "</td>";
                    datalist = datalist + "<td>" + rows[row].cells[4].innerHTML + "</td>";
                    datalist = datalist + "<td>" + rows[row].cells[5].innerHTML + "</td>";
                    datalist = datalist + "<td>" + rows[row].cells[6].innerHTML + "</td>";
                    datalist = datalist + "<td>" + rows[row].cells[7].innerHTML + "</td>";
                    datalist = datalist + "<td>" + rows[row].cells[8].innerHTML + "</td>";
                    datalist = datalist + "<td>" + rows[row].cells[9].innerHTML + "</td>";
                    datalist = datalist + "<td>" + rows[row].cells[10].innerHTML + "</td>";
                    datalist = datalist + "<td>" + rows[row].cells[11].innerHTML + "</td>";
                    datalist = datalist + "</tr>";
                    total += parseInt(rows[row].cells[9].innerHTML);
                }

            }


            strFormHtml = strFormHtml + datalist;
            strFormHtml = strFormHtml + "</table></div></body>";
            LODOP.ADD_PRINT_TEXT(30, 25, 980, 580, "出库金额:"+total);
            LODOP.ADD_PRINT_TABLE(50, 0, "85%", 470, strFormHtml);


        };


    </script>


    <script type="text/javascript" src="/Themes/Scripts/CheckActivX.js"></script>

    <object id="LODOP" classid="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" width="0" height="0">
        <embed id="LODOP_EM1" type="application/x-print-lodop" width="0" height="0" pluginspage="install_lodop32.exe"></object>

    <%--<script type="text/javascript">    CheckLodop();</script>--%>

</head>
<body>
    <form id="form1" runat="server">
        <div class="btnbartitle">
            <div>
                物资发货管理
            </div>
        </div>
        <div class="btnbarcontetn">


            <div style="text-align: right">
                <uc2:LoadButton ID="LoadButton1" runat="server" />
            </div>

        </div>

        <div id="formContent">
            领料单号：<asp:TextBox ID="txtInvoiceID" runat="server" Width="200px" Height="20px"></asp:TextBox>
            领料部门：<asp:DropDownList ID="ddlDeptName" runat="server"
                OnSelectedIndexChanged="ddlDeptName_SelectedIndexChanged" Width="200px" Height="25px">
            </asp:DropDownList>
            发料状态：<asp:DropDownList ID="ddlStates" runat="server" Width="100px" Height="25px"></asp:DropDownList>
            领用日期：
        
                    <asp:TextBox ID="txtPurchasePlanDate" runat="server" class="txt"
                        onfocus="WdatePicker({dateFmt: 'yyyy-MM-dd 00:00:00' })"  Width="200px" Height="20px"></asp:TextBox>
            <%--<asp:ImageButton ID="imgPurchasePlanDate" runat="server"  ImageUrl="~/Themes/Images/calendar.gif"/>--%>
            显示库存充足数据：<asp:CheckBox ID="chkstore" runat="server" OnCheckedChanged="chkstore_CheckedChanged" AutoPostBack="true"/>
            <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click"  Width="80px" Height="25px"/>



        </div>


        <div id="Content" class="div-body">
            <div id="Div1" class="div-body">
                                    <table class="mytable" id="tablediv" cellspacing="0" cellpadding="0" style="width:100%">
                                        <thead>
                                            <tr>
                                                <td>
                                                        <input type="checkbox" onclick="CheckAllLine()" /></td>
                                                <td>领料单号</td>
                                                <td>领料部门</td>
                                                <td>物资类型</td>
                                                <td>物资名称</td>
                                                <td>物资规格</td>
                                                <td>单位</td>
                                                <td>数量</td>
                                                <td>金额</td>
                                                <td>合计</td>
                                                <td>库存</td>
                                                <td>领料日期</td>
                                            </tr>
                                        </thead>
                                        <tbody id="body">
                                            <% if (ulist.Count > 0)
                                                {
                                                    foreach (RM.ServiceProvider.Model.deliveryquery item in ulist)
                                                    {
                                                        if (item.checkquantity == 0 && item.quantity > item.qua)
                                                        { %>
                                            <tr class="gradeX" style="color: red">
                                                <td class="check">
                                                    <input type="checkbox" id="<%=item.id%>" value="<%=item.id%>" name="checkbox" />
                                                    <input type="text" id="<%=item.quantity %>" value="<%=item.quantity %>" name="Quantity" style="display: none;" />
                                                    <input type="text" id="<%=item.qua %>" value="<%=item.qua %>" name="qua" style="display: none;" /></td>
                                                <td><%=item.PurchaseBillCode %></td>
                                                <td><%=item.DeptName %></td>
                                                <td><%=item.material_type %></td>
                                                <td><%=item.material_name %></td>
                                                <td><%=item.Material_Specification %></td>
                                                <td><%=item.Material_Unit %></td>
                                                <td><%=item.quantity %></td>
                                                <td><%=item.price %></td>
                                                <td><%=item.amount %></td>
                                                <td><%=item.qua %></td>
                                                <td><%=item.PurchaseDate %></td>
                                            </tr>
                                            <%  }
                                                else
                                                { %>
                                            <tr class="gradeX">
                                                <td class="check">
                                                    <input type="checkbox" id="<%=item.id%>" value="<%=item.id%>" name="checkbox" />
                                                    <input type="text" id="<%=item.quantity %>" value="<%=item.quantity %>" name="Quantity" style="display: none;" />
                                                    <input type="text" id="<%=item.qua %>" value="<%=item.qua %>" name="qua" style="display: none;" /></td>
                                                <td><%=item.PurchaseBillCode %></td>
                                                <td><%=item.DeptName %></td>
                                                <td><%=item.material_type %></td>
                                                <td><%=item.material_name %></td>
                                                <td><%=item.Material_Specification %></td>
                                                <td><%=item.Material_Unit %></td>
                                                <td><%=item.quantity %></td>
                                                <td><%=item.price %></td>
                                                <td><%=item.amount %></td>
                                                <td><%=item.qua %></td>
                                                <td><%=item.PurchaseDate %></td>
                                            </tr>
                                            <%  }
                                                    }
                                                } %>
                                        </tbody>
                                    </table>
                <div>

                    <cc1:OurPager ID="OurPager1" runat="server" OnPageChanged="OurPager1_PageChanged" />

                </div>
                </div>
            </div>
    </form>
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/jscript"></script>
</body>
</html>
