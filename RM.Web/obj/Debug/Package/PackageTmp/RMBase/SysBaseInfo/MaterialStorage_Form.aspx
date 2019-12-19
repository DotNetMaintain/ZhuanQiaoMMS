<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MaterialStorage_Form.aspx.cs" Inherits="RM.Web.RMBase.SysBaseInfo.MaterialStorage_Form" %>
<%@ Register Src="../../UserControl/LoadButton.ascx" TagName="LoadButton" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>领用物资表单</title>
   <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
     <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type ="text/javascript" >

        function linkval() {
         
            //var Material_ID = document.getElementById("Material_ID").value;
            //  window.location.assign("http://localhost:56506/MMS/MMS_Requisition/Requisition_Form.aspx");

            window.parent.opener=null;
            window.close();
            window.opener = null;
            
        }

    </script>

</head>
<body>
    <form id="form2" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" class="frm">
        <tr>
            <th>
                物资编号:
            </th>
            <td>
                <input id="Material_ID" maxlength="7" runat="server" type="text" class="txt"
                    datacol="yes" err="物资编号" checkexpession="NotNull" style="width: 200px" disabled="disabled" />
            </td>
            <th>
                 物资名称:
            </th>
            <td>
                <input id="material_name" runat="server" type="text" class="txt" datacol="yes"
                    err="物资名称" checkexpession="NotNull" style="width: 200px" disabled="disabled" />
            </td>
        </tr>
        <tr>
            <th>
                物资规格:
            </th>
            <td>
                <input id="Material_Specification" runat="server" type="text" class="txt" datacol="yes"
                    style="width: 200px" disabled="disabled" />
            </td>
            <th>
                单位:
            </th>
            <td>
                <input id="Material_Unit" runat="server" type="text" class="txt"
                    style="width: 200px" disabled="disabled" />
            </td>
        </tr>
        <tr>
            <th>
                生产厂商:
            </th>
            <td>
                <input id="Material_Supplier" runat="server" type="text" class="txt" style="width: 200px"  disabled="disabled"/>
            </td>
            <th>
                可用数量:
            </th>
            <td>
                <input id="qua" runat="server" type="text" class="txt" style="width: 200px" disabled="disabled" />
            </td>
        </tr>
        <tr>
            <th>
                领用数量:
            </th>
            <td>
                <input id="usequa" runat="server" type="text" class="txt" style="width: 200px"  err="领用数量" checkexpession="NotNull"/>
            </td>
            <th>
                单价:
            </th>
            <td>
                <input id="price" maxlength="6" runat="server" type="text" class="txt"  style="width: 200px"   disabled="disabled" />
            </td>
        </tr>
        <tr>
            <th>
                备注:
            </th>
            <td colspan="2">
                <input id="Remark" runat="server" type="text" class="txt" style="width: 200px" />
            </td>
           
        </tr>
       
    </table>
    <div class="frmbottom">
        <asp:LinkButton class="l-btn" href="javascript:void(0)" onclick="linkval();"><span class="l-btn-left">
            <img src="/Themes/Images/disk.png" alt="" />保 存</span></asp:LinkButton>
        <a class="l-btn" href="javascript:void(0)" onclick="OpenClose();"><span class="l-btn-left">
            <img src="/Themes/Images/cancel.png" alt="" />关 闭</span></a>
    </div>
      
    </form>
</body>
</html>

