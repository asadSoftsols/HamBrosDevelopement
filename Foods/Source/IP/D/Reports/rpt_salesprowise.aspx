﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rpt_salesprowise.aspx.cs" Inherits="Foods.rpt_salesprowise" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Product Wise Sales Reports</title>
        <style type="text/css">
        #container{
	        width:100%;
	        height:100%;
	        font-family:CordiaUPC;
            font-size:18px; 
        }
        .uppper{
	        width:45%;
	        height:auto;
	        margin:0px auto;
	        text-align:center;
        }
        .left{
	        width:45%;
	        height:auto;
	        float:left;
	        margin-left:20px;
        }
        .right{
	        width:49%;
	        height:auto;
	        float:right;
	        text-align:center;
        }
        .gv{
	        width:100%;
	        height: auto;
            text-align:center;
        }
        .clear{
        clear: both;
        }
        .shpkep{
	        width:50%;
	        height:auto;
	        text-align:center;
	        float:left;
        }
        .salemansig{
	        width:49%;
	        text-align:center;
	        height:auto;
	        float:right;
        }

        h1 {
        margin:0px;
        padding: 0px;
        }
        h2 {
        margin:0px;
        padding: 0px;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <div>
            <div id="container">
            <div class="uppper">
                <h1>Ham Brorhers</h1>
                <h2>Sales Report Product Wise</h2>
            </div>
            <div class="left">
                Product Name: <asp:Label ID="lbl_pro" runat="server" ></asp:Label><br />
            </div>
            <div class="right">
                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkBtnExportExcel_Click">Export to Excel</asp:LinkButton>       
            </div>
            <div class="clear"></div>
                <fieldset>
                    <br />
                    <br />
                    <div class="gv">
                        <asp:GridView ID="GV_ProSale" runat="server" EmptyDataText="No Record Found!" ShowHeader="true" AutoGenerateColumns="False" style="width:100%; height:auto;" DataKeyNames="MSal_id,DSal_id" >
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                                <asp:BoundField DataField="MSal_dat" HeaderText="Sales Date" SortExpression="MSal_dat" />
                                <asp:BoundField DataField="ProductName" HeaderText="Product" SortExpression="ProductName" />
                                <asp:BoundField DataField="DSal_ItmQty" HeaderText="Quantity" SortExpression="DSal_ItmQty" />
                                <asp:BoundField DataField="rat" HeaderText="Rate" SortExpression="rat" />
                                <asp:BoundField DataField="Dis" HeaderText="Discount" SortExpression="Dis" />
                                <asp:BoundField DataField="Amt" HeaderText="Amount" SortExpression="Amt" />
                                <asp:BoundField DataField="GTtl" HeaderText="Total" SortExpression="GTtl" />
                                <asp:BoundField DataField="MSal_Rmk" HeaderText="Remarks" SortExpression="MSal_Rmk" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </fieldset>
        </div>
    </div>
    </form>
</body>
</html>