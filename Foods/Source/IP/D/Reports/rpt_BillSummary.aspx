<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rpt_BillSummary.aspx.cs" Inherits="Foods.rpt_BillSummary" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bill Summary</title>
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
			        <h1>Bill Summary</h1>
		        </div>
		        <div class="left">
			        <h4></h4>
                    <div class="custadd">
                        <br />
                        <b>Sales Man:</b><u>  <asp:Label ID="lbl_intro" runat="server" ></asp:Label></u>
                        <br />
                        <b>Date:</b><asp:Label ID="lbldat" runat="server"></asp:Label>
    			        </div>
		        </div>
		        <div class="right">
		        </div>	
		        <div class="clear"></div>	
		        <br/>
		        <div id="Main">
 		            <br />
                    <asp:GridView ID="GVCashMemo" runat="server" style="text-align:center;" ShowFooter="true" ShowHeader="true" ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found" AutoGenerateColumns="False" CssClass="grid" DataKeyNames="MSal_sono"  >
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" />
                        <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" ReadOnly="True" SortExpression="CustomerName" />
                        <asp:BoundField DataField="Area" HeaderText="Area" ReadOnly="True" SortExpression="Area" />
                        <asp:BoundField DataField="GTtl" HeaderText="Gross Amount" ReadOnly="True" SortExpression="GTtl" />
                        <asp:BoundField DataField="DiscAmt" HeaderText="Disc Amount" SortExpression="DiscAmt" />
                         <asp:TemplateField HeaderText="Net Amount" >
                            <ItemTemplate>
                                <asp:Label ID="lbl_Amt" runat="server" Text='<%# Eval("Amt")%>' ></asp:Label>
                            </ItemTemplate>
                         </asp:TemplateField> 
                    </Columns>
                </asp:GridView>
                    <br />
                    <center><b>Total Amount: <asp:Label ID="lbl_ttl" runat="server"></asp:Label></b></center> 
 		        </div>	
                <div class="left">
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
                    </div>
                </div>
                <div class="right">                    
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
            <div id="Footer">
		        <div class="end"></div>
	        </div>
        </div>  
    </div>

    </form>
</body>
</html>
