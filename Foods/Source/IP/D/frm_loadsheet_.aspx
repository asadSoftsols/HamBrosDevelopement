<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frm_loadsheet_.aspx.cs" Inherits="Foods.frm_loadsheet_" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Load Sheet</title>
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
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <script>
            function printDiv(divName) {
                var printContents = document.getElementById(divName).innerHTML;
                var originalContents = document.body.innerHTML;
                document.body.innerHTML = printContents;
                window.print();
                document.body.innerHTML = originalContents;
            }
	</script>

      
        <button onclick="printDiv('printMe')">Print</button>
        <div  id="printMe">
            <div id="container">

	<h2> <asp:Label ID="lblcom" runat="server"></asp:Label> </h2>
	<h3> Load Sheet </h3>
	<div class="up">	
		<div class="up_left">
			Sales Man: ______________________________
		</div>	
		<div class="up_middl">
			Area: __________________________________
		</div>	
		<div class="up_right">
			Date: __________________________________
		</div>	
	</div>
    <asp:GridView ID="GVLoadSheet" runat="server" ShowHeader="true" ShowHeaderWhenEmpty="true" AutoGenerateColumns="true"  CssClass="gv">
        <%--<Columns>
            <asp:BoundField DataField="dsrdat" HeaderText="ID" ReadOnly="True" SortExpression="dsrdat" />
            <asp:BoundField DataField="ProductName" HeaderText="Product" SortExpression="ProductName" />
            <asp:BoundField DataField="Qty" HeaderText="Quantity" SortExpression="Qty" />
            <asp:BoundField DataField="cartons" HeaderText="Cartons" SortExpression="cartons" />
            <asp:BoundField DataField="Items" HeaderText="Items" SortExpression="items" />
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:CheckBox ID="chkok" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
                
        </Columns>--%>
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:CheckBox ID="chkok" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
	<div class="clear"></div>		
    </div>
        </div>
    </form>
</body>
</html>
