<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rpt_BillSumm.aspx.cs"
     Inherits="Foods.rpt_BillSumm" %>

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
			        <img src="../../../../img/logo.png" alt="logo" class="logo" />
		        </div>
		        <div class="header_right">
                    <b></b>
		        </div>
	        </div>
	        <div class="clear"></div>
	        <div class="content">
		        <div class="heading">
			        <h1>Cash Memo SalesMan - Copy</h1>
		        </div>
		        <div class="left">
			        <h4></h4>
                    <div class="custadd">
                        <br />
                        Sales Man:<u><asp:Label ID="lbl_intro" runat="server" ></asp:Label></u>
    			        </div>
		        </div>
		        <div class="right">
		        </div>	
		        <div class="clear"></div>	
		        <br/>
		        <div id="Main">
                
		            <br />
                    <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound">
                        <HeaderTemplate>
                
            </HeaderTemplate>
            <ItemTemplate>
                <br />
                <br />
            <table class="tblcolor">
                <tr>
                    <td>
                        Bill #:
                    </td>
                    <td>
                        <asp:Label ID="lblmsalno" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.MSal_sono")%>'></asp:Label>
                        <%#DataBinder.Eval(Container,"DataItem.msaldat")%>
                    </td>
                    <td>
                        Booker: 
                    </td>
                    <td>
                        <%#DataBinder.Eval(Container,"DataItem.Booker")%>
                    </td>
                    <td>
                        Messers:
                    </td>
                    <td>
                        <%#DataBinder.Eval(Container,"DataItem.CustomerName")%>
                    </td>
                </tr>
                </table>
                <br />
                <asp:GridView ID="GVCashMemo" runat="server" ShowFooter="true" ShowHeader="true" ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found" AutoGenerateColumns="False" CssClass="grid" DataKeyNames="MSal_sono" OnRowDataBound="GVCashMemo_RowDataBound" >
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" />
                        <asp:BoundField DataField="Descriptions" HeaderText="Descriptions of Goods" ReadOnly="True" SortExpression="Descriptions" />
                        <asp:TemplateField HeaderText="Qty">
                            <ItemTemplate>
                                <asp:Label ID="lblqty" runat="server" Text='<%# Eval("Qty")%>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="SchQty" HeaderText="Sch Qty" ReadOnly="True" SortExpression="SchQty" />
                        <asp:BoundField DataField="RTGQty" HeaderText="RTG Qty" ReadOnly="True" SortExpression="RTGQty" />
                        <asp:BoundField DataField="DamageQty" HeaderText="Damage Qty" SortExpression="DamageQty" />
                        <asp:BoundField DataField="MouseCut" HeaderText="Mouse Cut" SortExpression="MouseCut" />
                        <asp:BoundField DataField="ExpiredCut" HeaderText="Expired Cut" SortExpression="ExpiredCut" />
                        <asp:TemplateField HeaderText="Unit Price">
                            <ItemTemplate>
                                <asp:Label ID="lblsalcst" runat="server" Text='<%# Eval("DSal_salcst")%>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="DSal_salcst" Visible="false" HeaderText="Unit Price" SortExpression="DSal_salcst" />
                        <asp:TemplateField HeaderText="Amount">
                            <ItemTemplate>
                                <asp:Label ID="lblttl" runat="server" Text='<%# Eval("DSal_ttl")%>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Dis" HeaderText="Sales Discount" SortExpression="Dis" Visible="false" />
                        <asp:TemplateField HeaderText="Sales Discount">
                            <ItemTemplate>
                                <asp:Label ID="lbldiscamt" runat="server" Text='<%# Eval("discamt")%>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="discamt" Visible="false" HeaderText="Sales Discount" SortExpression="discamt" />
                        <asp:BoundField DataField="TradeOffer" HeaderText="Trade Offer" SortExpression="TradeOffer" />
                        <asp:BoundField DataField="ExtraDiscount" HeaderText="Extra Discount" SortExpression="ExtraDiscount" />
                        <asp:BoundField DataField="ValueIncSalestax" HeaderText="Value Inc Sales tax" SortExpression="ValueIncSalestax" />                        
                    </Columns>
                </asp:GridView>
                <table class="tblcolor" style="width:100%;">
                    <tr>
                        <td style="width:125px;">
                            Total
                        </td>
                        <td style="width:50px;">
                            <asp:Label ID="lblttlqty" runat="server"></asp:Label>
                        </td>
                        <td style="width:330px;">
                            
                        </td>
                        <td style="width:60px;">
                            <asp:Label ID="lblamount" runat="server"></asp:Label> 
                        </td>
                        <td>
                            <asp:Label ID="lblsaldisc" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
            <FooterTemplate>
            </FooterTemplate>
                    </asp:Repeater>
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
            ____________________ <br /><br />
                    Verified By

		    <div class="clear"></div>	
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
