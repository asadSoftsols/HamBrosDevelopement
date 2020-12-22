<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rpt_sale.aspx.cs" Inherits="Foods.rpt_sale" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sales Reports</title>
    
        <style type="text/css">
        body{
	
		    background:#fff;
		    margin:0;
		    padding:20px;
		    font-family:Arial;
		    font-size:18px;
	    }
	    #Container{
		    width:100%;
		    height:800px auto;
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
        text-align:center; width:100%; height:30px;
        }

        h2 { 
        }
        h3{ 
	        text-align:center; width:100%; height:30px;
	        text-decoration:underline;
        }
        #container{
	        width:100%;
	        height:100%;
        }
        .up{
	        width:100%;
    	    height:30px;

    }
        .up_left{

	        width:33%;
	        height:30px;
	        float:left;
	        text-align:left;
        }
        .up_middl{

	        width:33%;
	        height:30px;
	        float:left;
	        text-align:center;
        }
        .up_right{

	        width:33%;
	        height:30px;	        
	        float:left;
	        text-align:right;
        }
        .gv{
	    width:100%;
	    height:auto;
        text-align:center;
    }
        #container{
	        width:100%;
	        height:100%;
	        font-family:CordiaUPC;
            font-size:18px; 
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
	        height:auto;
        }
        .grid {
            width:100%;
	        height:auto;
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
                <h2>Sales Report Year Wise</h2>
            </div>
            <div class="left">
                Years: <asp:Label ID="lbl_yr" runat="server" ></asp:Label><br />
            </div>
            <div class="right">
                <asp:LinkButton ID="LinkBtnExportExcel" runat="server" OnClick="LinkBtnExportExcel_Click">Export to Excel</asp:LinkButton>       
            </div>
            <div class="clear"></div>
                <fieldset>
                    <br />
                    <br />
                    <div class="gv">
                        <asp:GridView ID="GV_SAL" runat="server" EmptyDataText="No Record Found!" ShowHeader="true" AutoGenerateColumns="False" style="width:100%; height:auto;"  >
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                                <asp:BoundField DataField="ProductName" HeaderText="Product" SortExpression="ProductName" />
                                <asp:TemplateField HeaderText="Quantity">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Qty" runat="server" Text='<%# Eval("DSal_ItmQty")%>' ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount" >
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Amt" runat="server" Text='<%# Eval("Amt")%>' ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="gv">
                            Total Quantity: <asp:Label ID="ttl_qty" runat="server" Text="--"></asp:Label>
                            Total: <asp:Label ID="lbl_ttl" runat="server" Text="--"></asp:Label>
                    </div>
                </fieldset>
        </div>

    </div>
    </form>
</body>
</html>
