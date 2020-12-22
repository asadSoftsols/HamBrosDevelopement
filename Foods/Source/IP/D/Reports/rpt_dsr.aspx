<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rpt_dsr.aspx.cs"
     Inherits="Foods.rpt_dsr" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body{
	
		    background:#fff;
		    margin:0;
		    padding:20px;
		    font-family:Arial;
		    font-size:12px;
	    }
	    #Container{
		    width:100%;
		    height:800px auto;
		    border:1px solid #000;
	    }
	    .header{
		    width:100%;
		    height:auto;
	    }
	    .header_left
	    {
		    width:50%;
		    height:auto;
		    float:left;
	    }
	    .header_right
	    {
		    width:30%;
            padding:80px 0px 0px 40px;
		    height:auto;
		    float:right;
		    text-decoration:underline;
	    }
	    .logo
	    {
		    width:300px;
		    height:180px;
	    }
	    .small{
	
		    width:100%;
		    height: 30px;
		    font-size:12px;
		    background:#bfbfbf;
	    }
	    .content
	    {
		    width:94%;
		    height:auto;
		    padding:30px;
	    }
	    .heading
	    {
		    width:100%;
		    height:auto;
		    text-decoration:underline;
		    text-align:center;
		    font-style:italic;
	    }
	    .left{
		    width:49%;
		    height:auto;
		    float:left;
	    }
	    .low_left{
		    width:50%;
		    height:auto;
		    float:left;
	    }
	    .custadd
	    {
		    width:300px;
		    height:80px;		 
	    }
	    .right{
		    width:49%;
		    height:auto;
		    float:right;
	    }
	    .low_right{
		    width:29%;
		    height:auto;
		    float:right;
		    padding:180px 0px 40px 130px;
	    }
	    .clear{
		    clear:both;
	    }
        table {
            width: 100%;
            height: auto;
            font-size: 12px;
        }
	    tr td {
		    font-size:12px;			 
	    }
	    #Main
	    {
		    width:100%;
		    height:auto;
	    }
	    #Footer
	    {
		    width:100%;
		    font-size:14px;
		    height:auto;
		    text-align:center;
	    }
	    .end
	    {
		    width:100%;
		    height:30px;
		    background:green;
	    }
	    p{
		    padding:0px;
		    margin:0px;
	    }

         h2 {
             margin:0px;
             padding:0px;
         }
        h4, GridView {
            text-align:center;
        }
        .gv {
            width:100%;
            height:auto;
            margin:0px auto;
            text-align:center;
        }
  
        /* Scroller Start */
        .scrollit {
        overflow:scroll;
        height:100%;
        width:100%;           
        margin:0px auto;
        }
        /* Scroller End onload="window.print();" */
         .logo {
             width:30%;
             height:auto;
             float:left;
             text-align:center;
         }
         .comp {
             width:65%;
             height:auto;
             float:right;
             text-align:center;
         }
         .main {
             width:100%;
             height:auto;
         }
         .clear {
             clear:both;
         }
         p {
             margin:0px;
             padding:0px;
         }
        .left{
	        width:20%;
	        height:auto;
	        float:left;
	        margin-left:20px;
        }
        .right{
	        width:30%;
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
        <div class="logo">
            <img src="~/img/Lv_logo.jpg" id="imglogo" width="150" height="70" alt="" runat="server" />
        </div>
        <div class="comp">
            <div class="uppper">
                <h1>Lawai Traders</h1>
                <h2>Daily Sales Report</h2>
                <h3>Plot # MC.41 Sector  6.D mehran town korangi Industrial Area<br /> 0333-3896390</h3>
            </div>
        </div>
        <div class="clear"></div>
        <div class="left">
            <b>Booker Name:</b> <asp:Label ID="lbl_booker" runat="server" ></asp:Label> / <b>Sales Man:</b> <asp:Label ID="lbl_Salman" runat="server" ></asp:Label><br />
        </div>
        <div class="left">
           <b>Date:</b> <asp:Label ID="lbl_dat" runat="server" ></asp:Label><br />
        </div>
        <div class="left">
            <asp:LinkButton ID="LinkBtnExportExcel" Visible="false" runat="server" OnClick="LinkBtnExportExcel_Click">Export to Excel</asp:LinkButton>       
        </div>
        <div class="left">
            <asp:Label ID="lbl_area" runat="server" ></asp:Label><br />
        </div>
        <div class="clear"></div>
        <div class="main">
            <fieldset>
                <legend>DSR Report</legend>
         
                <table style="width:100%; float:right;">
                    <tr>
                        <td colspan="4">
                            <asp:GridView ID="GVdsr" runat="server" AutoGenerateColumns="false" class="gv"  EmptyDataText="NO Record Found" ShowHeader="true" ShowHeaderWhenEmpty="true" >
                                <Columns>                    
                                    <asp:BoundField DataField="ProductName" HeaderText="Item Name" SortExpression="ProductName" />
                                    <asp:BoundField DataField="QTY" HeaderText="Items" SortExpression="QTY" />
                                    <%--<asp:BoundField DataField="cartons" HeaderText="Cartons" SortExpression="cartons" />--%>                                    
                                    <asp:BoundField DataField="FQty" HeaderText="Sales Qty" SortExpression="FQty" />
                                    <asp:BoundField DataField="Boxes" HeaderText="Packets" SortExpression="Boxes" />
                                    <asp:BoundField DataField="items" HeaderText="Loose Items" SortExpression="items" />
                                    <asp:BoundField DataField="Return" HeaderText="Return" SortExpression="Return" />  
                                    <%--<asp:BoundField DataField="updateby" HeaderText="Return by" SortExpression="updateby" />                         
                                    <asp:BoundField DataField="salrat" HeaderText="Sales" SortExpression="salrat" />--%>                                            
                                    <asp:BoundField DataField="salrat" HeaderText="Rate" SortExpression="salrat" />  
                                    <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />  
                                </Columns>  
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td><div style="width:100px; height:auto; margin-left:20px">&nbsp;</div></td>
                        <td><div style="width:100px; height:auto; margin-left:20px">&nbsp;</div></td>
                        <td><div style="width:80px; height:auto; margin-left:20px"><b>Discount: <asp:Label ID="lbl_discount" runat="server" Text="0.00" /> %</b><br /><b>Sales: <asp:Label ID="lbl_grsssal" runat="server" Text="0.00" /> Rs.</b></div></td>
                        <td><div style="width:100px; height:auto; margin-left:20px"></div></td>
                    </tr>
                </table>    
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                 
                <table border="0" style="width:100%; float:right;" >
                    <tr>
                        <td>
                            <span style="font-weight:bold; font-size:20px; text-align:center;"> Credit Details </span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="GVCred" runat="server" ShowFooter="true" AutoGenerateColumns="false" class="gv"  EmptyDataText="NO Record Found" ShowHeader="true" ShowHeaderWhenEmpty="true">
                                <Columns>                    
                                    <asp:BoundField DataField="ID" HeaderText="SNO" SortExpression="ID" />
                                    <asp:BoundField DataField="Party Name" HeaderText="Party Name" SortExpression="Party Name" />
                                    <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
                                    <asp:BoundField DataField="saleper" HeaderText="Discount" SortExpression="saleper" />
                                    <asp:BoundField DataField="Chq" HeaderText="Chq #" SortExpression="Chq" />
                                    <asp:BoundField DataField="Bill" HeaderText="Bill #" SortExpression="Bill" />
                                    <asp:BoundField DataField="furout" HeaderText="Net Value" SortExpression="furout" />
                                </Columns>  
                            </asp:GridView>
                        </td>
                    </tr>
                </table>            
                <table border="0"  style="width:100%; float:right;" >
                    <tr>
                        <td>
                            <span style="font-weight:bold; font-size:20px; text-align:center;"> Recovery Details </span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="GV_Recov" runat="server" ShowFooter="true" AutoGenerateColumns="false" class="gv"  EmptyDataText="NO Record Found" ShowHeader="true" ShowHeaderWhenEmpty="true">
                                <Columns>                    
                                    <asp:BoundField DataField="ID" HeaderText="SNO" SortExpression="ID" />
                                    <asp:BoundField DataField="Party Name" HeaderText="Party Name" SortExpression="Party Name" />
                                    <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
                                    <asp:BoundField DataField="saleper" HeaderText="Discount" SortExpression="saleper" />
                                    <asp:BoundField DataField="Chq" HeaderText="Chq #" SortExpression="Chq" />
                                    <asp:BoundField DataField="Bill" HeaderText="Bill #" SortExpression="Bill" />
                                    <asp:BoundField DataField="NetValue" HeaderText="Net Value" SortExpression="NetValue" />
                                </Columns>  
                            </asp:GridView>
                        </td>
                    </tr>
                </table>

                <table border="0" style="width:100%; height:100%;">
                    <tr>
                        <td>
                            <b>Gross Sales</b>
                        </td>
                        <td>
                            <asp:Label ID="lbl_grosssal" runat="server"></asp:Label> Rs.
                        </td>
                        <td>
                            <b>Net Sales after Discount:</b>
                        </td>
                        <td>
                            <asp:Label ID="lbl_netsal" runat="server"></asp:Label> Rs.
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <b>Recovery</b>
                        </td>
                        <td>
                            <asp:Label ID="lbl_recovery" runat="server"></asp:Label> Rs.
                        </td>
                        <td>
                            <b>Net Sales after Recovery</b>
                        </td>
                        <td>
                            <asp:Label ID="lbl_netsalafter" runat="server"></asp:Label> Rs.
                        </td>
                        <td>
                            <b>Credit</b>
                        </td>
                        <td>
                            <asp:Label ID="lbl_cre" runat="server"></asp:Label> Rs.
                        </td>
                    </tr>
                    <tr>
                        <td style="display:none;">
                            <b>Petty Cash</b>
                        </td>
                        <td style="display:none;">
                            <asp:Label ID="lbl_ptycash" runat="server"></asp:Label> Rs.
                        </td>
                        <td>
                            <b>Sale return</b>
                        </td>
                        <td>
                            <asp:Label ID="lbl_salreturn" runat="server"></asp:Label> 
                        </td>
                        <td>
                            <b>Net Cash</b>
                        </td>
                        <td>
                            <asp:Label ID="lbl_netcash" runat="server"></asp:Label> Rs.
                        </td>
                        <td>
                            <asp:Label Visible="false" ID="lbl_dics" runat="server"></asp:Label>
                        </td>

                    </tr>
                </table>            
            </fieldset>
        </div>
    </div>
    <div id="Footer">
		<div class="end"></div>
	</div>
    </form>
</body>
</html>
