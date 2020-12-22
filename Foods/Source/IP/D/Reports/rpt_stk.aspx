<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rpt_stk.aspx.cs" Inherits="Foods.rpt_stk" %>

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
<body >
    <form id="form1" runat="server">
    <div>
        <div class="logo">
            <img src="../../../../img/Lv_logo.jpg" id="imglogo" width="150" height="70" alt="" runat="server" />
        </div>
        <div class="comp">
            <div class="uppper">
                <h1>Lawai Traders</h1>
                <h2>Stock Report</h2>
                <h3>Plot # MC.41 Sector  6.D mehran town korangi Industrial Area<br /> 0333-3896390</h3>
            </div>
        </div>
        <div class="clear"></div>
        <div class="comp">
            <h2><asp:Label ID="lbl_Comp" runat="server"></asp:Label> </h2>
            <p><asp:Label ID="lbl_compadd" runat="server"></asp:Label></p>
            <p><asp:Label ID="lbl_no" runat="server"></asp:Label></p>
        </div>
        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
        <div class="clear"></div>
        <div class="main">
            <fieldset>
                <legend>Stock Report</legend>
                <asp:LinkButton ID="LinkBtnExportExcel" runat="server" OnClick="LinkBtnExportExcel_Click">Export to Excel</asp:LinkButton>       
                <table style="width:100%; float:right; margin-right:20px;">
                    <tr>
                        <%--<td>
                            <b>Sale Rate: <asp:Label ID="lbl_salrat" runat="server" Text="0.00" Visible="true" /></b>
                        </td>
                        <td>
                            <b>Purchase Rate: <asp:Label ID="lbl_purrat" runat="server" Text="0.00" Visible="true" /></b>
                        </td>--%>
                        <td>
                            <div style="width:200px; height:auto; margin-left:20px"><b>Stock Value: <asp:Label ID="lbl_ttlStkVal" runat="server" Text="0.00" /> Rs.</b></div>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="GVStk" runat="server" AutoGenerateColumns="false" class="gv" EmptyDataText="NO Record Found" ShowHeader="true" ShowHeaderWhenEmpty="true" >
                    <Columns>                    
                        <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                        <asp:BoundField DataField="productname" HeaderText="Product" SortExpression="productname" />
                        <asp:BoundField DataField="Dstk_ItmQty" HeaderText="Qty" SortExpression="Dstk_ItmQty" />
                        <asp:BoundField DataField="packets" HeaderText="Packets" SortExpression="packets" />

                        <asp:BoundField DataField="cartons" HeaderText="Cartons" SortExpression="cartons" />
                        <asp:BoundField DataField="Items" HeaderText="Items" SortExpression="Items" />
                        <%--<asp:BoundField DataField="dozen1" HeaderText="Dozens" SortExpression="dozen1" />--%>
                        <asp:BoundField DataField="Dstk_salrat" HeaderText="Sale rate" SortExpression="Dstk_salrat" />
                        <asp:BoundField DataField="Dstk_purrat" HeaderText="Purchase Rate" SortExpression="Dstk_purrat" />                           
                        <asp:BoundField DataField="Profit" HeaderText="Profit" SortExpression="Profit" />                                            
                        <%--<asp:BoundField DataField="stk_carton" HeaderText="Cartons" SortExpression="stk_carton" />--%>    
                        <asp:TemplateField HeaderText="Stock Value">
                            <ItemTemplate>
                                <asp:Label ID="lbl_stkval" runat="server" Text='<%# Eval("Stock Value")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>                                    
                    </Columns>  
                </asp:GridView>
                 
            </fieldset>
        </div>
<%--dozens =  qty/12, 230/12 = 19.16
cartons = 19/6 = 3.16--%>
    </div>
    </form>
</body>
</html>
