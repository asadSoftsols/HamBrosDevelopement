<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dsrinvoice.aspx.cs" 
    Inherits="Foods.dsrinvoice" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>DSR Invoice</title>
    <style type="text/css">
        body {
            font-family:CordiaUPC;
            font-size:18px;            
            
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
        .gv{
	        width:100%;
	        height: auto;
            text-align:center;
        }
        h1 {
        margin:0px;
        padding: 0px;
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
        <div class="logo">
            <img src="~/img/Lv_logo.jpg" id="imglogo" width="150" height="70" alt="" runat="server" />
        </div>
        <div class="comp">
            <div class="uppper">
                <h1><asp:Label ID="lbl_comNam" runat="server"></asp:Label></h1>
                <h3><asp:Label ID="lbl_comAdd" runat="server"></asp:Label><br /> <asp:Label ID="lbl_comPhnum" runat="server"></asp:Label></h3>
                <h2>CASH MEMO</h2>
            </div>
        </div>
        <div class="clear"></div>
        <div class="clear"></div>
        <div class="main">
            <fieldset>
                <legend>Invoice</legend>
            <div class="left">
	            <b></b><br />
                <br />
	            <br />
                Customer:<u><asp:Label ID="lbl_intro" runat="server" ></asp:Label></u>
                <br />
                Address:<u><asp:Label ID="lbladd" runat="server" ></asp:Label></u>
                <br />
                Phone:<u><asp:Label ID="lblph" runat="server" ></asp:Label></u>

	            <%--T001-566/SWEET MADINA CONFECTIONAY SHOP
	            SHOP # 29 MALIR TANKI / 03435454345--%>	
            </div>
                    <div class="right">
                Bill No : <asp:Label ID="lblbilno" runat="server"></asp:Label><br />
                Order / Delv Date : <asp:Label ID="lblsaldat" runat="server"></asp:Label><br />
                Booker : <asp:Label ID="lblbooker" runat="server"></asp:Label> / Salesman: <asp:Label  ID="lbl_salman" runat="server"></asp:Label><br />
            </div>

                <table style="width:100%; float:right;">
                    <tr>
                        <td colspan="4">
                                        <div class="gv">
                
                <asp:GridView ID="GVSal" runat="server" AutoGenerateColumns="False" CssClass="grid" DataKeyNames="dsrid" >
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" />
                        <asp:BoundField DataField="Products" HeaderText="Product Code / Description" ReadOnly="True" SortExpression="Products" />
                        
                        <asp:BoundField DataField="finlqry" HeaderText="Pieces" SortExpression="finlqry" />
                        <asp:BoundField DataField="salrturn" HeaderText="Pieces Return" SortExpression="salrturn" />
                        <%--<asp:BoundField DataField="Box" HeaderText="Box" ReadOnly="True" SortExpression="Box" />
                        <asp:BoundField DataField="CTNSize" HeaderText="CTNSize" ReadOnly="True" SortExpression="CTNSize" />--%>
                        <asp:BoundField DataField="Total Price" HeaderText="Trade Price" SortExpression="Total Price" />
                        <%--<asp:BoundField DataField="Dis" HeaderText="Dis" ReadOnly="True" SortExpression="Dis" />--%>
                        <asp:TemplateField HeaderText="Gross Price">
                            <ItemTemplate>
                                <asp:Label ID="lbl_GTtl" runat="server"  Text='<%# Eval("GTtl")%>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Net Amount" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lbl_afterdis" runat="server"  Text='<%# Eval("NetAmount")%>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div></td>
                    </tr>
                    <tr>
                        <td><div style="width:180px; height:auto; margin-left:20px">&nbsp;</div></td>
                        <td><div style="width:180px; height:auto; margin-left:20px">&nbsp;</div></td>
                        <td><div style="width:180px; height:auto; margin-left:20px"><b></b></div></td>
                        <td><div style="width:180px; height:auto; margin-left:20px"><b><asp:Label ID="lblgross" runat="server" Text="Gross Amount: "></asp:Label></b><asp:Label ID="lbl_grosamt" runat="server" ></asp:Label> Only/- </div></td>
                    </tr>
                    <tr>
                        <td><div style="width:180px; height:auto; margin-left:20px">&nbsp;</div></td>
                        <td><div style="width:180px; height:auto; margin-left:20px">&nbsp;</div></td>
                        <td><div style="width:180px; height:auto; margin-left:20px"><b><asp:Label Visible="false" ID="lbl_discount" runat="server" Text="0.00" /></b></div></td>
                        <td><div style="width:180px; height:auto; margin-left:20px"><b><asp:Label ID="Label1" runat="server" Text="Discount: "></asp:Label> <asp:Label ID="lbl_disc" runat="server"></asp:Label> %</b></div></td>
                    </tr>
                    <tr>
                        <td><div style="width:180px; height:auto; margin-left:20px">&nbsp;</div></td>
                        <td><div style="width:180px; height:auto; margin-left:20px">&nbsp;</div></td>
                        <td><div style="width:180px; height:auto; margin-left:20px">&nbsp;</div></td>
                        <td><div style="width:180px; height:auto; margin-left:20px"><b><asp:Label ID="Label4" runat="server" Text="Discount Amount: "></asp:Label> <asp:Label ID="lbl_discamt" runat="server"></asp:Label> Only/-</b></div></td>
                    </tr>
                </table>    
                <br />
                <br />
                <br />
                     <div id="container">
            <div class="clear"></div>
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
            </div>
            <div class="right">
                <br />
                <span style="font-size:16px; font-weight:bold; border:2px solid #000; width:160px; height:80px;">
                    Net Amount After Discount:  <asp:Label ID="lbl_net" runat="server" ></asp:Label> Rs.            
                </span>
                <br />
                <br />
	            Current Net Payable:  <asp:Label ID="lb_currnetpay" runat="server" ></asp:Label> Rs.
                <br /> 
	            Previous Outstanding: <asp:Label ID="lb_preout" runat="server" ></asp:Label>
            </div>		
        </div>
 
                 
            </fieldset>
        </div>
    </div>
    </form>
</body>
</html>
