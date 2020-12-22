<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rpt_NetProfit.aspx.cs" Inherits="Foods.rpt_NetProfit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style type="text/css">
	    body{
	
		    background:#fff;
		    margin:0;
		    padding:20px;
		    font-family:Arial;
		    font-size:12px;
            text-align:center;
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
	    table{
		    width:100%;
		    height:auto;
		    font-size:12px;
		    border:1px solid #000;
	    }
	    tr td {
		    font-size:12px;	
		    border:1px solid #000;
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
			           <h1>Mubashir Traders</h1>
		            </div>
		            <div class="header_right">
                        <b></b>
		            </div>
	            </div>
	            <div class="clear"></div>
	            <div class="content">
		            <div class="heading">
			            <h1>Net Profit Sheet</h1>
		            </div>
		            <div class="left">
			            <asp:Panel ID="pnl_monyr" runat="server">
                         <div class="left">
			                <table style="float:left; width:50%; height:auto;">
				                <tr>
                                    <td>Month : <asp:Label ID="lbl_mon" runat="server"></asp:Label></td>                                  
                                    <td>Year : <asp:Label ID="lbl_yr" runat="server"></asp:Label></td>
                                </tr>
			                </table>
		                </div>
                    </asp:Panel>
		            </div>
		            <div class="right">
			            <table style="float:right; width:50%; height:auto;">
				            <tr>
                                <td>Current Date : <asp:Label ID="lbl_dat" runat="server"></asp:Label></td>
                            </tr>
			            </table>
		            </div>
                    <asp:Panel ID="pnl_dat" runat="server">
                         <div class="left">
			                <table style="float:left; width:50%; height:auto;">
				                <tr>
                                    <td>From Date : <asp:Label ID="lbl_netproffdat" runat="server"></asp:Label></td>
                                    
                                    <td>To Date : <asp:Label ID="lbl_netproftdat" runat="server"></asp:Label></td>
                                </tr>
			                </table>
		                </div>
                    </asp:Panel>
                    
		            <div class="clear"></div>	
		            <br/>
		            <div id="Main">
                        <div style="float:left; width:400px;">
                           
                            <asp:GridView ID="GVExpence" runat="server" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Acc No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_accno" runat="server"  Text='<%# Eval("accno")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_accnam" runat="server"  Text='<%# Eval("acctitle")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Payment Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_typofpay" runat="server"  Text='<%# Eval("typeofpay")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bill No">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_billno" runat="server"  Text='<%# Eval("billno")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount Paid">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_amtpad" runat="server"  Text='<%# Eval("Amountpaid")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div style="clear:both"></div>
                        <br />
                        <div style="float:left; width:100%;">

                        <asp:GridView ID="GVProf" runat="server" AutoGenerateColumns="False" CssClass="grid">
                            <Columns>
                                <asp:TemplateField HeaderText="Purchase">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_prrat" runat="server"  Text='<%# Eval("PurRat")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sale">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_salrat" runat="server" Text='<%# Eval("SalAmount")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Profit">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_prof" runat="server" Text='<%# Eval("Profit")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Expence">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_exp" runat="server" Text='<%# Eval("Expence")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField> 
                                        
                                <asp:TemplateField HeaderText="Net Profit/Loss">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_netprof" runat="server" Text='<%# Eval("NetProf")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <table style="width:200px; height:auto; border:0px; float:right; margin-right:30px;" border="0">
                            <tr>
                                <td>Total Profit/Loss:  <asp:Label ID="lbl_proff" runat="server" ></asp:Label></td>
                            </tr>
                        </table>
                        </div>
		            </div>	
                    <div class="left">
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                        ____________________ <br />
                        Verified By
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
