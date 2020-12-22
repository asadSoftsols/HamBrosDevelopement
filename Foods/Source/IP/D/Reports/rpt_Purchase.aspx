<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rpt_Purchase.aspx.cs" Inherits="Foods.rpt_Purchase" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
.container{
font-family:Arial Narrow;
width:100%;
height:auto;

}
h2{ 
	text-align:center; width:100%; height:30px;

	
}
.up{
	width:100%;
	height:auto;

	
}
.dwn{
	width:100%;
	height:320px;
    margin-top:30px;

	
}
.left{	
	width:55%;
	height:auto;
	float:left;
	
}
.right{	
	width:43%;
	height:auto;
	float:right;

}
.table{
	width:100%;	
	height:auto;
    text-align:center;
}
.clear{
	clear:both;
}
.logoAdd{
	
	width:200px;
	height:auto;
	
}
.dwnleft{
	width:540px;
	height:auto;
	float:left;
	padding:0px 0px 0px 10px;
    margin-top:80px;
}
.dwnright{	
	width:430px;
	height:200px;
	float:right;	
	text-align:right;
	padding:0px 10px 0px 0px;	
     margin-top:80px;
}
        .textbox {
            width:100%;
            height:20px;
            background:none;
            border:none;
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
    <fieldset>
        
<div class="container">
	<h2>Delivery Chalan</h2>
	<div class="up">
		<div class="left">
			<div class="logoAdd">
				Tariq Glass Industries Limited
				33 Km Lahore/ Sheikhupura
				Road, Pakistan
				<br />
				<br />
				Tel: +92 056 3500635-7
			</div>
			<div class="address">
			To: <br /><asp:Label ID="lblTo" style="font-weight:bold" runat="server"></asp:Label>
			<br />
			Shipment To: <br /> <asp:Label ID="LBLShpTo" style="font-weight:bold" runat="server"></asp:Label>
		</div>
		</div>
		<div class="right">		

			<table>
				<tr>
					<td><label>DC NO:</label></td>
					<td style="border-bottom:1px solid #000">
                        <asp:TextBox ID="TBDCNo" runat="server" CssClass="textbox" />
					</td>
				</tr>
				<tr>
					<td><label>Date & Time:</label></td>
					<td style="border-bottom:1px solid #000" >
                        <asp:TextBox ID="TBDatTim" runat="server" CssClass="textbox" />
					</td>
				</tr>
				<tr>
					<td><label>Bilty NO:</label></td>
					<td style="border-bottom:1px solid #000" >
                        <asp:TextBox ID="TBBilNo" runat="server" CssClass="textbox" />
					</td>
				</tr>
				<tr>
					<td><label>Vehicle NO:</label></td>
					<td style="border-bottom:1px solid #000" >
                        <asp:TextBox ID="TBVehlNo" runat="server" CssClass="textbox" />
					</td>
				</tr>
				<tr>
					<td><label>Driver Name:</label></td>
					<td style="border-bottom:1px solid #000">
                        <asp:TextBox ID="TbDriNam" runat="server" CssClass="textbox" />
					</td>
				</tr>
				<tr>
					<td><label>Driver Mobile No:</label></td>
					<td style="border-bottom:1px solid #000" >
                        <asp:TextBox ID="TBDrMobNo" runat="server" CssClass="textbox" />
					</td>
				</tr>
				<tr>
					<td><label>Transporter:</label></td>
					<td style="border-bottom:1px solid #000" >
                        <asp:TextBox ID="TBTrans" runat="server" CssClass="textbox" />
					</td>
				</tr>
				<tr>
					<td><label>Station:</label></td>
					<td style="border-bottom:1px solid #000">
                        <asp:TextBox ID="TBSTion" runat="server" CssClass="textbox" />
					</td>
				</tr>
				<tr>
					<td><label>Delivery Order:</label></td>
					<td style="border-bottom:1px solid #000">
                        <asp:TextBox ID="TBDO" runat="server" CssClass="textbox" />
					</td>
				</tr>
				<tr>
					<td><label>Frieght:</label></td>
					<td><asp:TextBox ID="TBFright" runat="server" CssClass="textbox" /></td>
				</tr>
			</table>
				
		</div>
		<div class="clear"></div>
	</div>
	<div class="clear"></div>
	<div class="dwn">
        <asp:GridView ID="GVShowpurItms" CssClass="table" runat="server" EmptyDataText="Sorry No Record Exits" AutoGenerateColumns="false" DataKeyNames="MPurID,ProductID" >
            <Columns>
                <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" ReadOnly="True" />
                <asp:BoundField DataField="productname" HeaderText="Description" SortExpression="productname" ReadOnly="True" />
                <asp:BoundField DataField="Qty" HeaderText="Quantity in Pcs" SortExpression="Qty" ReadOnly="True" />
                <%--<asp:BoundField DataField="MPurID" HeaderText="Packing carton" SortExpression="MPurID" ReadOnly="True" />--%>
                <asp:BoundField DataField="packets" HeaderText="Quantity in Packets" SortExpression="packets" ReadOnly="True" />
                <asp:BoundField DataField="cartons" HeaderText="Cartons" SortExpression="cartons" ReadOnly="True" />
                <asp:BoundField DataField="Items" HeaderText="Loose Items " SortExpression="Items" ReadOnly="True" />
            </Columns>
        </asp:GridView>

		<div class="dwnleft">
			Frieght Charges To Pay
			<br />
			Instructions:
			<br />
			_________________________<br />
			WareHouse<br />
			Officer
		</div>
		<div class="dwnright">
		______________________________<br />
		Carrier/ Buyer Representatives
		</div>
	</div>
</div>
        </fieldset>
    </div>
    </form>
</body>
</html>
