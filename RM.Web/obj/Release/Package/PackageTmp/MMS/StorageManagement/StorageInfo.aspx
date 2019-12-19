<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StorageInfo.aspx.cs" Inherits="RM.Web.MMS.StorageManagement.StorageInfo" %>
<%@ Register Src="../../UserControl/LoadButton.ascx" TagName="LoadButton" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>入库表单</title>

 
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/TreeView/treeview.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/TreeView/treeview.pack.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            divresize(63);
            FixedTableHeader("#dnd-example", $(window).height() - 91);
            GetClickTableValue();
        })




        var data = [];

        function GetClickTableValue() {  //jquery获取复选框值
            $('table tr').not('#td').click(function () {

                var chk_value = [];

                $('input[name="checkbox"]:checked').each(function () {
                    chk_value.push($(this).val());
                });
                data = chk_value;
            });

        }




        //新增
        function allotButton() {

            var JsonStr = JSON.stringify(data);
            deliveryConfig('/Ajax/Delivery_Button.ashx', data)


        }


        //退库还原按钮
        function restore() {

            var JsonStr = JSON.stringify(data);
            restoreConfig('/Ajax/Restore_Button.ashx', data)


        }


        //编辑
        function edit() {
            var key = StorageForm_ID;
            if (IsEditdata(key)) {
                var url = "/MMS/StorageManagement/StorageForm.aspx?key=" + key;
                top.openDialog(url, 'StorageForm', '进货单 - 编辑', 700, 335, 50, 50);
            }
        }
        //删除
        function Delete() {
            var key = StorageForm_ID;
            if (IsDelData(key)) {
                var delparm = 'action=Virtualdelete&module=物资管理&tableName=MMS_MaterialInfo&pkName=StorageForm_ID&pkVal=' + key;
                delConfig('/Ajax/Common_Ajax.ashx', delparm)
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
            LODOP.PRINT_INIT("打印插件功能演示_Lodop功能_表单一");
            var strBodyStyle = "<style>table{ border-collapse:collapse; width:100%; font-size:15px; border:3px blue solid}td{ border:2px solid #cccccc;}</style>";
           // var strFormHtml = strBodyStyle + "<body>" + document.getElementById("Div1").innerHTML + "</body>";
            var strFormHtml = strBodyStyle + "<body><div class='div-body'>";
            var datalist="";

            LODOP.ADD_PRINT_TEXT(20, 380, 650, 681, "领料单");
            LODOP.SET_PRINT_TEXT_STYLE(1, "黑体", 20, 1, 0, 0, 1);
            strFormHtml = strBodyStyle + "<table class='grid'>";
           // LODOP.ADD_PRINT_HTM(8, 28, 1050, 600, strFormHtml);
           strFormHtml=strFormHtml+" <thead> <tr><td style='width:10%; text-align: center;'>领料单号</td><td style='width:10%; text-align: center;'>领料部门</td><td style='width:15%; text-align: center;'>物资类型</td><td style='width:25%; text-align: center;'>物资名称</td><td style='width:10%; text-align: center;'>物资规格</td><td style='width:4%; text-align: center;'>单位</td><td style='width:5%; text-align: center;'>数量</td><td style='width:5%; text-align: center;'>金额</td><td style='width:6%; text-align: center;'>合计</td><td style='width:10%; text-align: center;'>领料日期</td></tr></thead>"
            var rows = document.getElementById("table1").rows;
            var a = document.getElementsByName("checkbox");
            trFormHtml = strFormHtml + "<tbody>";
                        for(var j=0;j<a.length;j++) {
                            datalist = datalist+"<tr>";
                            if (a[j].checked) {
                                var row = a[j].parentElement.parentElement.rowIndex;
                                datalist =datalist+ "<td>" + rows[row].cells[1].innerHTML + "</td>";
                                datalist =datalist+ "<td>" +  rows[row].cells[2].innerHTML + "</td>";
                                datalist =datalist+ "<td>" + rows[row].cells[3].innerHTML + "</td>";
                                datalist = datalist + "<td>" + rows[row].cells[4].innerHTML + "</td>";
                                datalist = datalist + "<td>" + rows[row].cells[5].innerHTML + "</td>";
                                datalist = datalist + "<td>" + rows[row].cells[6].innerHTML + "</td>";
                                datalist = datalist + "<td>" + rows[row].cells[7].innerHTML + "</td>";
                                datalist = datalist + "<td>" + rows[row].cells[8].innerHTML + "</td>";
                                datalist = datalist + "<td>" + rows[row].cells[9].innerHTML + "</td>";
                                datalist = datalist + "<td>" + rows[row].cells[10].innerHTML + "</td>";
//                                LODOP.ADD_PRINT_TEXT(iCurLine, 15, 100, 20, document.getElementById("PurchaseBillCode" + data[j]).value);
//                                LODOP.ADD_PRINT_TEXT(iCurLine, 149, 100, 20, document.getElementById("deptname" + data[j]).value);
                                //                                iCurLine = iCurLine + 25;
                                datalist = datalist + "</tr>";
                            }
                            
                        }


                        strFormHtml = strFormHtml + datalist;
                        strFormHtml = strFormHtml + "</table></div></body>";
                        LODOP.ADD_PRINT_TABLE(50, 0, "85%", 470, strFormHtml);
                        

        };	


    </script>


    <script type="text/javascript"  src="../../Themes/Scripts/CheckActivX.js"></script>

<OBJECT  ID="LODOP" CLASSID="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" WIDTH=0 HEIGHT=0> </OBJECT> 

<script type ="text/javascript" >    CheckLodop();</script>

</head>
<body>
    <form id="form1" runat="server">
    <div class="btnbartitle">
        <div>
            物资入库管理
        </div>
    </div>
     <div class="btnbarcontetn">

    
        <div style="text-align: right">
            <uc2:LoadButton ID="LoadButton1" runat="server" />
        </div>

    </div>

   <div id="formContent">
                        领料单号：<asp:TextBox ID="txtInvoiceID" runat="server"></asp:TextBox>
                         领料部门：<asp:DropDownList ID="ddlDeptName" runat="server" 
                            onselectedindexchanged="ddlDeptName_SelectedIndexChanged"></asp:DropDownList>
                         发料状态：<asp:DropDownList ID="ddlStates" runat="server"></asp:DropDownList>
                       
       <asp:Button ID="btnSearch" runat="server" Text="查询" onclick="btnSearch_Click" />

      

</div>


    <div id="Content" class="div-body">
    <div id="Div1" class="div-body">
      <table id="table1" class="grid" >
            <thead>
                <tr>
                    <td style="width: 3%; text-align: left;">
                        <label id="checkAllOff" onclick="CheckAllLine()" title="全选">
                            &nbsp;</label>
                    </td>
                    <td style="width:10%; text-align: center;">
                        领料单号
                    </td>
                   
                      <td style="width:10%; text-align: center;">
                        领料部门
                    </td>
                    <td style="width:15%; text-align: center;">
                        物资类型
                    </td>
                    <td style="width:25%; text-align: center;">
                        物资名称
                    </td>
                    <td style="width:10%; text-align: center;">
                        物资规格
                    </td>

                     <td style="width:4%; text-align: center;">
                        单位
                    </td>
                    
                    <td style="width:5%; text-align: center;">
                        数量
                    </td>
                      <td style="width:5%; text-align: center;">
                        金额
                    </td>
                      <td style="width:6%; text-align: center;">
                        合计
                    </td>
                    <td style="width:10%; text-align: center;">
                        领料日期
                    </td>
                   
                </tr>
            </thead>
           

          

            <tbody>
                <asp:Repeater ID="rp_Item" runat="server" OnItemDataBound="rp_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td style="width:3%; text-align: left;">
                                <input type="checkbox" id="<%#Eval("ID")%>" value="<%#Eval("ID")%>" name="checkbox" />
                            </td>
                            <td style="width:10%; text-align: center;">
                                <%#Eval("PurchaseBillCode")%>
                            </td>
                           
                            <td style="width:10%; text-align: center;">
                                <%#Eval("DeptName")%>
                            </td>
                            <td style="width:15%; text-align: center;">
                                <%#Eval("Material_Type").ToString().Substring(0, Eval("Material_Type").ToString().LastIndexOf('-')-1)%>
                            </td>
                            <td style="width:25%; text-align: center;">
                               <%#Eval("Material_Name")%>
                            </td>
                            <td style="width:10%; text-align: center;">
                               <%#Eval("Material_Specification")%>
                            </td>
                            <td style="width:4%; text-align: center;">
                                <%#Eval("Material_Unit")%>
                            </td>
                           
                             <td style="width:5%; text-align: center;">
                                <%#Eval("Quantity")%>
                            </td>
                            <td style="width:5%; text-align: center;">
                                <%#Eval("price")%>
                            </td>
                             <td style="width:6%; text-align: center;">
                                <%#Eval("amount")%>
                            </td>
                             <td style="width:10%; text-align: center;">
                               <%#Convert.ToDateTime(Eval("PurchaseDate")).ToShortDateString()%>
                            </td>
                             
                        </tr>
                    </ItemTemplate>
            
                    <FooterTemplate>
                        <% if (rp_Item != null)
                           {
                               if (rp_Item.Items.Count == 0)
                               {
                                   Response.Write("<tr><td colspan='10' style='color:red;text-align:center'>没有找到您要的相关数据！</td></tr>");
                               }
                           }
                         
                           %>
                    </FooterTemplate>
                </asp:Repeater>
            </tbody>


            <tfoot>
  <tr>

    <td width="100%" colspan="7"><b>领料人：&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</b> </td> 
    <td colspan="2" align="center"><b>金额总计：</b></td><td><b><%=totalamount%></b></td>
    <td>&nbsp;</td>
  </tr>
  
</tfoot>



        </table>
    
    <div>
   
     <cc1:OurPager ID="OurPager1" runat="server" onpagechanged="OurPager1_PageChanged" />                        
  
    </div>
   
    </form>
</body>
</html>
