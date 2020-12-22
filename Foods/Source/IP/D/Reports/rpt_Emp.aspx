﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rpt_Emp.aspx.cs" Inherits="Foods.rpt_Emp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Employee Reports</title>
        <style type="text/css">
        body {
            font-family:CordiaUPC;
            font-size:18px;            
            text-align:center;
           }
        h4, GridView {
            text-align:center;
        }
        .gv {
            width:100%;
            height:auto;
            margin:0px auto;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
     <div>
         <%--<asp:LinkButton ID="LinkBtnExportExcel" runat="server" OnClick="LinkBtnExportExcel_Click">Export to Excel</asp:LinkButton>--%>       
 
    <h4>Daily Employee Sales Report</h4>
        <asp:GridView ID="GVBook" runat="server" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" CssClass="gv" >
            <Columns>
                <asp:BoundField DataField="booker" HeaderText="Employee Name" SortExpression="booker" />
                <asp:BoundField DataField="DSal_ItmQty" HeaderText="Quantity Sold" SortExpression="DSal_ItmQty" />
                <asp:TemplateField HeaderText="Total Amount">
                    <ItemTemplate>
                        <asp:Label ID="lbl_ttlbook" runat="server" Text='<%# Eval("Total")%>'></asp:Label> 
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CreatedAt" HeaderText="CreatedAt" SortExpression="CreatedAt" />

            </Columns>
        </asp:GridView>
         <asp:GridView ID="GVSal" runat="server" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" CssClass="gv" >
            <Columns>
                <asp:BoundField DataField="SalMan" HeaderText="Employee Name" SortExpression="SalMan" />
                <asp:BoundField DataField="DSal_ItmQty" HeaderText="Quantity Sold" SortExpression="DSal_ItmQty" />
                <asp:TemplateField HeaderText="Total Amount">
                    <ItemTemplate>
                        <asp:Label ID="lbl_ttlsalman" runat="server" Text='<%# Eval("Total")%>'></asp:Label> 
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CreatedAt" HeaderText="CreatedAt" SortExpression="CreatedAt" />
            </Columns>
        </asp:GridView>
         
        <br />
         Total Sales in this month:&nbsp;&nbsp;&nbsp;<asp:Label ID="lbl_ttl" runat="server"></asp:Label>
     </div>
    </form>
</body>
</html>
