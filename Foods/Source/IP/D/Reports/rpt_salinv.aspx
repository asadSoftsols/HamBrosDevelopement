<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rpt_salinv.aspx.cs" Inherits="Foods.rpt_salinv" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sales Invoice</title>
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

    </style>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div id="Div1">
	        <div class="header">
		        <div class="header_left">
			        <img src="../../../../img/Lv_logo.jpg" alt="logo" class="logo" />
		        </div>
		        <div class="header_right">
                    <b></b>
		        </div>
	        </div>
	        <div class="clear"></div>
	        <div class="content">
		        <div class="heading">
			        <h1>Cash Memo</h1>
		        </div>
		        <div class="left">
			        <h4></h4>
                    <div class="custadd">
                        Customer:<u><asp:Label ID="lbl_intro" runat="server" ></asp:Label></u>
                        <br />
                        Address:<u><asp:Label ID="lbladd" runat="server" ></asp:Label></u>
                        <br />
                        Phone:<u><asp:Label ID="lblph" runat="server" ></asp:Label></u>
    			    </div>
		        </div>
		        <div class="right">

                    Bill No : <asp:Label ID="lblbilno" runat="server"></asp:Label><br />
                    Order / Delv Date : <asp:Label ID="lblsaldat" runat="server"></asp:Label><br />
                    Booker / SalesMan : <asp:Label ID="lblbooker" runat="server"></asp:Label><br />

		        </div>	
		        <div class="clear"></div>	
		        <br/>
		        <div id="Main">
 		            <br />
                <asp:GridView ID="GVSal" runat="server" AutoGenerateColumns="False" CssClass="grid" DataKeyNames="MSal_id" >
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" />
                        <asp:BoundField DataField="Products" HeaderText="Product Code / Description" ReadOnly="True" SortExpression="Products" />
                        <asp:BoundField DataField="Pcs" HeaderText="Dozens" SortExpression="Pcs" />
                        <asp:BoundField DataField="rat" HeaderText="Rate" ReadOnly="True" SortExpression="rat" />
                        <%--<asp:BoundField DataField="Box" HeaderText="Box" ReadOnly="True" SortExpression="Box" />
                        <asp:BoundField DataField="CTNSize" HeaderText="CTNSize" ReadOnly="True" SortExpression="CTNSize" />--%>
                        <%--<asp:BoundField DataField="GTtl" HeaderText="Gross Price" SortExpression="GTtl" />--%>
<%--                        <asp:BoundField DataField="Total Price" HeaderText="Trade Price" SortExpression="Total Price" />--%>
                        <%--<asp:BoundField DataField="Dis" HeaderText="Dis" ReadOnly="True" SortExpression="Dis" />--%>
                        <asp:TemplateField HeaderText="Net Amount">
                            <ItemTemplate>
                                <asp:Label ID="lbl_ttl" runat="server"  Text='<%# Eval("Total")%>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
     		        </div>	
                <div class="left">
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <div class="shpkep">
                    ____________________ <br />
                    ShopKeeper Signature
                    </div>
                    <div class="salemansig">
                    ____________________ <br />
                    Amount Recieved by<br />
                    SalesMan Signature
                    </div>
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />                    
                    </div>
                </div>
                <div class="right">
                    <br />
		            <br />
                    Gross Amount: <asp:Label ID="lblgrssamt" runat="server" ></asp:Label>
                    <br />
                    Discount Percentage: <asp:Label ID="lbldisper" runat="server" >%</asp:Label>
                    <br />
                    Discount Amount:  <asp:Label ID="lbldiscamt" runat="server" ></asp:Label>
	                <br />
                    <br />
                    Previous Outstanding: <asp:Label ID="lb_preout" runat="server" ></asp:Label>
	                <br />
                    GST: <asp:Label ID="lbl_gst" runat="server" Text="0" ></asp:Label>
	                <br />
                    Other Tax: <asp:Label ID="lbl_othtax" runat="server" Text="0" ></asp:Label>
                    <br />
                    <br />
                    <span style="font-size:16px; font-weight:bold; border:2px solid #000; width:160px; height:80px;">
                        Current Net Payable:  <asp:Label ID="lb_currnetpay" runat="server" ></asp:Label>
                    </span>
	                <br />
	                <br />
	                <br />
	                <br />
	                <br />
                </div>
                    <br />
		            <br />
                    <br />
                    <br />
                    <br />
	                <br />
                    <br />
                    <br />
	                <br />
	                <br />
	                <br />
	                <br />
	                <br />
	                <br />		
            <div id="Div2">
		        <div class="end"></div>
	        </div>
        </div>  
	</div>
    </form>
</body>
</html>
