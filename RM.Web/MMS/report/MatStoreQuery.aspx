<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MatStoreQuery.aspx.cs" Inherits="RM.Web.MMS.report.MatStore" %>


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
<%--<OBJECT  ID="LODOP" CLASSID="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" WIDTH=0 HEIGHT=0> </OBJECT>--%> 
</head>
<body class="gray-bg">
    <form  runat="server">
                                          <asp:Button ID="btnexcel" runat="server" Text="导出excel" onclick="btnexcel_Click" />
            显示零库存：<asp:CheckBox ID="chkzero" runat="server" OnCheckedChanged="chkzero_CheckedChanged" AutoPostBack="true"/>
                物料名称：
             <asp:TextBox ID="ttbmaterialname" runat="server" class="txt" Style="width: 115px;"></asp:TextBox>
                <asp:LinkButton ID="lbtSearch" runat="server" class="button green"
                    OnClick="lbtSearch_Click"><span class="icon-botton"
            style="background: url('/Themes/images/Search.png') no-repeat scroll 0px 4px;">
                        <asp:TextBox ID="txtorder" runat="server" class="txt" Style="display:none"></asp:TextBox>
        </span>查 询</asp:LinkButton>
    <div class="wrapper wrapper-content animated fadeInRight">
        <div class="row">
            <div class="col-sm-12">
                <div class="ibox float-e-margins">
                    <div class="ibox-content">

                        <table class="table table-striped table-bordered table-hover dataTables-example" id="tablediv">
                                <thead>
                                    <tr>
                                        <th>物料类型</th>
                                        <th>物料名称</th>
                                        <th>物料规格</th>
                                        <th>单位</th>
                                        <th>库存数</th>
                                        <th>价格</th>
                                    </tr>
                                </thead>
                                <tbody><% foreach (RM.ServiceProvider.Model.Inventory item in inlist)
                       { %>
                                    <tr class="gradeX">
                                        <td><%=item.material_type %></td>
                                        <td><span onclick="store(<%=item.material_id %>,<%=item.qua %>)"><%=item.material_name %></span></td>
                                        <td><%=item.Material_Specification %></td>
                                        <td><%=item.Material_Unit %></td>
                                        <td><%=item.qua %></td>
                                        <td><%=item.price %></td>
                                    </tr>
                    <%  } %>
                                </tbody>
                        </table>

                    </div>
                </div>
            </div>
        </div>
    </div></form>
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/jscript"></script>
<script src="/Themes/Scripts/bootstrap/bootstrap.min.js" type="text/jscript"></script>
<script src="/Themes/Scripts/bootstrap/jquery.jeditable.js" type="text/jscript"></script>
    <script src="/Themes/Scripts/bootstrap/jquery.dataTables.js" type="text/jscript"></script>
<script src="/Themes/Scripts/bootstrap/dataTables.bootstrap.js" type="text/jscript"></script>
<script src="/Themes/Scripts/bootstrap/content.min.js" type="text/jscript"></script>
<script>
     $(document).ready(function(){$(".dataTables-example").dataTable();var oTable=$("#editable").dataTable();oTable.$("td").editable("http://www.zi-han.net/theme/example_ajax.php",{"callback":function(sValue,y){var aPos=oTable.fnGetPosition(this);oTable.fnUpdate(sValue,aPos[0],aPos[1])},"submitdata":function(value,settings){return{"row_id":this.parentNode.getAttribute("id"),"column":oTable.fnGetPosition(this)[2]}},"width":"90%","height":"100%"})});
    function store(id,qua) {
         var url = "/mms/report/MatStorequerydetail.aspx?ID=" + id+"&qua="+qua;
                top.openDialog(url, 'MatStorequerydetail', '入库详细信息管理', 1200, 500, 50, 50);
    }
    
    </script>
</body>
</html>
