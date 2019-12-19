<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StorageQuery.aspx.cs" Inherits="RM.Web.MMS.StorageManagement.StorageQuery" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>入库单明细查询</title>
    <link rel="shortcut icon" href="favicon.ico">
    <link href="../../Themes/Styles/bootstrap/bootstrap.min14ed.css" rel="stylesheet" />
    <link href="../../Themes/Styles/bootstrap/font-awesome.min93e3.css" rel="stylesheet" />
    <link href="../../Themes/Styles/bootstrap/dataTables.bootstrap.css" rel="stylesheet" />
    <link href="../../Themes/Styles/bootstrap/animate.min.css" rel="stylesheet" />
    <link href="../../Themes/Styles/bootstrap/style.min862f.css" rel="stylesheet" />
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
</head>
<body style="overflow: auto">
    <form id="form1" runat="server">
        <div class="btnbartitle">
            <div>
                入库单明细查询
            </div>
        </div>
        <%--<asp:Button ID="btnexcel" runat="server" Text="导出excel" onclick="btnexcel_Click" />--%>
        <div class="btnbarcontetn">
            <div style="float: left;">
                查询月份:
            <asp:TextBox ID="ttbStartDate" runat="server" class="txt" Style="width: 115px;"
                onfocus="WdatePicker({dateFmt: 'yyyy-MM' })"></asp:TextBox>

                入库单号：
             <asp:TextBox ID="ttbInStorageID" runat="server" class="txt" Style="width: 115px;"></asp:TextBox>
                <asp:LinkButton ID="lbtSearch" runat="server" class="button green"
                    OnClick="lbtSearch_Click"><span class="icon-botton"
            style="background: url('/Themes/images/Search.png') no-repeat scroll 0px 4px;">
                        <asp:TextBox ID="txtorder" runat="server" class="txt" Style="display:none"></asp:TextBox>
        </span>查 询</asp:LinkButton>
            </div>
        </div>
        <div class="div-body">

            <div class="wrapper wrapper-content animated fadeInRight">
                <div class="row">
                    <div class="col-sm-12">
                        <div class="ibox float-e-margins">
                            <div class="ibox-content">

                                <table class="table table-striped table-bordered table-hover dataTables-example" id="tablediv">
                                    <thead>
                                        <tr>
                                            <th><asp:LinkButton ID="orderbcode" runat="server" OnClick="orderbcode_Click">入库单号</asp:LinkButton></th>
                                            <th><asp:LinkButton ID="orderbygys" runat="server" OnClick="orderbygys_Click">供应商</asp:LinkButton></th>
                                            <th>入库日期</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <% 
                                            if (ulist.Count > 0) { 
                                            foreach (RM.ServiceProvider.Model.storedetail item in ulist)
                                            { %>
                                        <tr class="gradeX">
                                            <td onclick="store(<%=item.id %>)"><%=item.PurchaseBillCode %></td>
                                            <td><%=item.Material_Supplier %></td>
                                            <td><%=item.PurchaseDate %></td>
                                        </tr>
                                        <%  }} %>
                                    </tbody>
                                </table>

                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>

        <div>

            <cc1:OurPager ID="OurPager1" runat="server" OnPageChanged="OurPager1_PageChanged" />

        </div>

    </form>


    <script src="../../Themes/Scripts/jquery-1.8.2.min.js" type="text/jscript"></script>
    <script src="../../Themes/Scripts/bootstrap/bootstrap.min.js" type="text/jscript"></script>
    <script src="../../Themes/Scripts/bootstrap/jquery.jeditable.js" type="text/jscript"></script>
    <script src="../../Themes/Scripts/bootstrap/jquery.dataTables.js" type="text/jscript"></script>
    <script src="../../Themes/Scripts/bootstrap/dataTables.bootstrap.js" type="text/jscript"></script>
    <script src="../../Themes/Scripts/bootstrap/content.min.js" type="text/jscript"></script>
    <script>   
        function store(id) {
            var str = id + " ";
            var url = "/mms/StorageManagement/MatStorequerydetails.aspx?ID=" + str;
            top.openDialog(url, 'MatStorequerydetails', '入库详细信息管理', 1200, 700, 50, 50);
        }
     //$(document).ready(function(){$(".dataTables-example").dataTable();var oTable=$("#editable").dataTable();oTable.$("td").editable("http://www.zi-han.net/theme/example_ajax.php",{"callback":function(sValue,y){var aPos=oTable.fnGetPosition(this);oTable.fnUpdate(sValue,aPos[0],aPos[1])},"submitdata":function(value,settings){return{"row_id":this.parentNode.getAttribute("id"),"column":oTable.fnGetPosition(this)[2]}},"width":"90%","height":"100%"})});
    </script>
</body>
</html>
