<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rpt_salinv1.aspx.cs" Inherits="Foods.rpt_salinv1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sales Invoice</title>

    <style type="text/css">
	    .container
        {
	        width:100%;
	        height:auto;
	        font-family: Arial;
        }
        .grid {
            width:100%;
	        height:auto;
            text-align:center;
            margin-top:10px;
        }
  
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="container">
	    <div style="width:100%; height:auto;text-align:center;">
		    <h1>Sales Invoice(Distribution S)</h1>
	    </div>
	    <div style="width:100%; height:auto; border-bottom:3px solid #000;">
		    <div style="width:200px; float:left;">Date: <asp:Label ID="lbl_dat" runat="server" Text=""></asp:Label></div>
            <div style="width:200px; float:right;">Print Time: <asp:Label ID="lbl_tim" runat="server" Text=""></asp:Label></div>
		    <div style="clear:both"></div>
	    </div>
	    <div style="width:100%; border-bottom:3px solid #000; margin:0px 30px 0px 0px; height:200px;">
		    <table style="width:100%; height:200px;">
			    <tr>
				    <td>INV NO.</td>
				    <td>
                        <asp:Label ID="lblbilno" runat="server" Text=""></asp:Label>
				    </td>
				    <td>O.Booker</td>
				    <td>
                        <asp:Label ID="lblbooker" runat="server" Text=""></asp:Label>
				    </td>
				    <td>Document Type:</td>
				    <td> INV </td>
			    </tr>
                 <tr>
				    <td>M/s:</td>
				    <td><asp:Label ID="lbl_intro" runat="server" Text=""></asp:Label></td>
				    <td></td>
				    <td></td>
				    <td></td>
				    <td></td>
			    </tr>
			    <tr>
				    <td>Date:</td>
				    <td> <asp:Label ID="lblsaldat" runat="server" Text=""></asp:Label></td>
				    <td>Segment</td>
				    <td><asp:Label ID="lbl_seg" runat="server" Text=""></asp:Label></td>
				    <td>Driver Name #.</td>
				    <td><asp:Label ID="lbl_drivnam" runat="server" Text=""></asp:Label></td>
			    </tr>
			    <tr>
				    <td>Address:</td>
				    <td><asp:Label ID="lbladd" runat="server" Text=""></asp:Label></td>
				    <td>Remarks:</td>
				    <td><asp:Label ID="lbl_rmks" runat="server" Text=""></asp:Label></td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
		    	</tr>
	    	</table>
    	</div><%--Aa22394780_--%>
            <asp:GridView ID="GVSal" runat="server" AutoGenerateColumns="false" CssClass="grid">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="Sr #" ReadOnly="True" SortExpression="ID" />
                    <asp:BoundField DataField="Products" HeaderText="Item Description" ReadOnly="True" SortExpression="Products" />
                    <%--<asp:BoundField DataField="Pcs" HeaderText="Quantity" SortExpression="Pcs" />--%>
                    <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <asp:Label ID="lbl_qty" runat="server"  Text='<%# Eval("Pcs")%>' ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="rat" HeaderText="Rate" ReadOnly="True" SortExpression="rat" /> 
                    <asp:BoundField DataField="Dis" HeaderText="Dis %" ReadOnly="True" SortExpression="Dis" /> 
                    <%--<asp:BoundField DataField="NetAmt" HeaderText="Amount" ReadOnly="True" SortExpression="NetAmt" />--%> 
                     <asp:TemplateField HeaderText="Amount">
                        <ItemTemplate>
                            <asp:Label ID="lbl_amt" runat="server"  Text='<%# Eval("NetAmt")%>' ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="DisAmt" HeaderText="Dis Amount" ReadOnly="True" SortExpression="DisAmt" />                     
                    <asp:BoundField DataField="sdis" HeaderText="S.Dis %" ReadOnly="True" SortExpression="sdis" />                     
                    <asp:BoundField DataField="SpecialDisamt" HeaderText="S.Dis Amount" ReadOnly="True" SortExpression="SpecialDisamt" />  
                    <asp:TemplateField HeaderText="Net Amount">
                        <ItemTemplate>
                            <asp:Label ID="lbl_ttl" runat="server"  Text='<%# Eval("NetTotal")%>' ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
                <table style="width:100%" border="0">
                    <tr>
                        <td style="width:12%" >&nbsp;</td>
                        <td>&nbsp;</td>
			    	    <td>&nbsp;</td>
                        <td>&nbsp;</td>
			    	    <td>&nbsp;</td>
			    	    <td>&nbsp;</td>
			    	    <td>&nbsp;</td>
			    	    <td>&nbsp;</td>
			    	    <td>&nbsp;</td>
			    	    <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width:12%" >&nbsp;</td>
                        
                        <td><b>Total Quantity:</b></td>
			    	    <td>
                            <asp:Label ID="lbl_ttlqty" runat="server" Text="0" Font-Bold="true" Font-Size="Large"></asp:Label>
			    	    </td>
                        <td>&nbsp;</td>
                        <td style="width:10%">&nbsp;</td>
                         <td>&nbsp;</td>
			    	    <td>&nbsp;</td>
                        <td style="width:2%;"></td>
                        <td><b>Total Gross Amount:</b></td>
			    	    <td>
                            <asp:Label ID="lbl_ttlgross" runat="server" Text="0" Font-Bold="true" Font-Size="Large"></asp:Label>
			    	    </td>
                    </tr>
                     <tr>
                        <td style="width:12%" >&nbsp;</td>                        
                        <td>&nbsp;</td>
			    	    <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td style="width:10%">&nbsp;</td>                        
                        <td>&nbsp;</td>
			    	    <td>&nbsp;</td>
                        <td style="width:2%;"></td>                          
                        <td><b>Discount:</b></td>
			    	    <td>
                            <asp:Label ID="lbl_dis" runat="server" Text="0" Font-Bold="true" Font-Size="Large"></asp:Label>
			    	    </td>                      
                    </tr>
                     <tr>
                        <td style="width:12%" >&nbsp;</td>                        
                        <td>&nbsp;</td>
			    	    <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td style="width:10%">&nbsp;</td>                        
                        <td>&nbsp;</td>
			    	    <td>&nbsp;</td>
                        <td style="width:2%;"></td>                          
                        <td><b>Discount Amount:</b></td>
			    	    <td>
                            <asp:Label ID="lbl_disamt" runat="server" Text="0" Font-Bold="true" Font-Size="Large"></asp:Label>
			    	    </td>                      
                    </tr>
                    <tr>
                        <td style="width:12%" >&nbsp;</td>                        
                        <td>&nbsp;</td>
			    	    <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td style="width:10%">&nbsp;</td>                        
                        <td>&nbsp;</td>
			    	    <td>&nbsp;</td>
                        <td style="width:2%;"></td>                          
                        <td><b>Total Amount After Discount:</b></td>
			    	    <td>
                            <asp:Label ID="lbl_totl" runat="server" Text="0" Font-Bold="true" Font-Size="Large"></asp:Label>
			    	    </td>                      
                    </tr>
                    <tr>
                        <td style="width:12%" >&nbsp;</td>                        
                        <td>&nbsp;</td>
			    	    <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td style="width:10%">&nbsp;</td>                        
                        <td>&nbsp;</td>
			    	    <td>&nbsp;</td>
                        <td style="width:2%;"></td>                          
                        <td><b>Previous Out Standing:</b></td>
			    	    <td>
                            <asp:Label ID="lbl_outstan" runat="server" Text="0" Font-Bold="true" Font-Size="Large"></asp:Label>
			    	    </td>                      
                    </tr>
                </table>
        </div>
	</div>
    </form>
</body>
</html>
