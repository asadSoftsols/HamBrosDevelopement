<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GV_DSR.aspx.cs" Inherits="menu.GV_DSR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>DSR</title>
    <style type="text/css">
        body{
            font-size:12px;
            font-family:arial;
            text-align:center;
        }
        h2 { 
	        text-align:center; width:100%; height:30px;
        }
        h3{ 
	        text-align:center; width:100%; height:30px;
	        text-decoration:underline;
        }
        #container{
	        width:100%;
	        height:100%;
            margin:0px auto;
        }
        .up{
	        width:100%;
    	    height:30px;
            text-align:center;
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
            <div  id="container">
        <h2> <asp:Label ID="lblcom" runat="server"></asp:Label> </h2>
    	<h3> Daily Sales Report </h3>
        <asp:Repeater ID="Repeater1" runat="server"  onitemdatabound="Repeater1_ItemDataBound"  >
            <ItemTemplate>
                <table border="0">
                    <tr>
                        <td>
                            <div class="up">
                                Customer Name : <asp:Label runat="server" ID="Label2" ><%#Eval("CustomerName")%></asp:Label><br />
                            </div>
                        </td>
                        <td>
                            <asp:HiddenField runat="server" Visible="false" ID="HFCustNam" Value='<%#Eval("Username")%>' ></asp:HiddenField><br />
                            <asp:HiddenField runat="server" Visible="false" ID="HFCustID" Value='<%#Eval("CustomerID")%>' ></asp:HiddenField><br />                            
                        </td>
                    </tr>                    
                    <tr>
                        <td colspan="2">
                             <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" class="gv" ShowFooter="true" OnRowDataBound="GridView1_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="Productname" HeaderText="Product Name" SortExpression="Productname" />                                       
                                    <asp:BoundField DataField="cartons" HeaderText="Cartons" SortExpression="cartons" />                                        
                                    <asp:BoundField DataField="Items" HeaderText="Items" SortExpression="Items" /> 
                                    <asp:BoundField DataField="outstan" HeaderText="Out Standing" SortExpression="outstan" /> 
                                    <asp:BoundField DataField="recvry" HeaderText="Recovery" SortExpression="recvry" />
                                    <asp:BoundField DataField="dsrrmk" HeaderText="Remarks" SortExpression="dsrrmk" />
                                    <asp:TemplateField HeaderText="Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_amt" runat="server" Text='<%# Eval("Amt")%>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lbl_ttlamt" runat="server" ></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            </td>
                        </tr>
                </table>
            </ItemTemplate>
        </asp:Repeater>
        
        <%--<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:D %>"
            SelectCommand="SELECT * FROM [Users]"></asp:SqlDataSource>--%>
               
        </div>
        </div>
    </form>
</body>
</html>
