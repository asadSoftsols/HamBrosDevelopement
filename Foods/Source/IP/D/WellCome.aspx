<%@ Page Title="WelCome" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="WellCome.aspx.cs" Inherits="Foods.WellCome" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
            /* Scroller Start */
        .scrollit {
            overflow:scroll;
            height:300px;
	        width:100%;           
	        margin:0px auto;
        }
      /* Scroller End */

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <noscript>
    <div class="alert alert-block span10">
    <h4 class="alert-heading">Warning!</h4>
    <p>You need to have <a href="http://en.wikipedia.org/wiki/JavaScript" target="_blank">JavaScript</a> enabled to use this site.</p>
    </div>
    </noscript>		
			
    <ul class="breadcrumb">
        <li>
        <i class="icon-home"></i>
            <a href="WellCome.aspx">Home</a> 
        <i class="icon-angle-right"></i>
        </li>
        <li><a href="#">Dashboard</a></li>
    </ul>
    <asp:Panel ID="pnlchkusr" runat="server">
        	<div class="row-fluid">	
				<div class="box span12">
			<%--		<div class="box-header">
						<h2><i class="halflings-icon hand-top"></i><span class="break"></span>Quick links</h2>
					</div>--%>
					<div class="box-content">						
                          

						<a class="quick-button-small span1" href="frm_Emp.aspx">
							<i class="icon-group"></i>
							<p>Employees</p>
						</a>
						<a class="quick-button-small span1" href="frm_Sal.aspx">
							<i class="icon-comments-alt"></i>
							<p>Sales</p>
						</a>
						<a class="quick-button-small span1" href="frm_DSR_.aspx" >
							<i class="icon-shopping-cart"></i>
							<p>Orders</p>
						</a>
						<a class="quick-button-small span1" href="frm_Products.aspx">
							<i class="icon-barcode"></i>
							<p>Products</p>
						</a>
						<a class="quick-button-small span1" href="frm_Purchase.aspx">
							<i class="icon-envelope"></i>
							<p>Purchases</p>
						</a>
						<a class="quick-button-small span1" href="Rpt_PurchaseForm.aspx">
							<i class="icon-calendar"></i>
							<p>Reporting</p>
						</a>
						<div class="clearfix"></div>
					</div>	
				</div><!--/span-->				
			</div><!--/row-->			
    </asp:Panel>
    <div class="row-fluid">				
        <h3>WelCome <asp:Label ID="lblUserName" runat="server"></asp:Label> to <asp:LinkButton ID="lbl_compnam" runat="server"></asp:LinkButton> </h3>
    </div>	
    <div class="row-fluid sortable">		
        <asp:Panel ID="pnlsalchart" runat="server">
            <div class="box span6">
				<div class="box-header">
					<h2><i class="halflings-icon list-alt"></i><span class="break"></span>Monthly Sales</h2>
					<div class="box-icon">
						<a href="#" class="btn-setting"><i class="halflings-icon wrench"></i></a>
						<a href="#" class="btn-minimize"><i class="halflings-icon chevron-up"></i></a>
						<a href="#" class="btn-close"><i class="halflings-icon remove"></i></a>
					</div>
				</div>
				<div class="box-content">
                    <asp:Chart ID="Chart1" runat="server" Height="300px" Width="400px" >
                        <Titles>
                            <asp:Title ShadowOffset="3" Name="Items" />
                        </Titles>
                        <Legends>
                            <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="False" Name="Sales Per Day" LegendStyle="Row" />
                        </Legends>
                        <Series>
                            <asp:Series Name="Sales Per Day" />
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1" BorderWidth="0" />
                        </ChartAreas>
                    </asp:Chart>
				</div>
			</div>
        </asp:Panel>
        <asp:Panel ID="pnlchkpro" runat="server">
            <div class="box black span6 noMargin" onTablet="span12" onDesktop="span6">                
				    <div class="box-header">
					    <h2>Items Less Then 5</h2>
					    <div class="box-icon">
						    <a href="#" class="btn-minimize"><i class="halflings-icon white chevron-up"></i></a>
						    <a href="#" class="btn-close"><i class="halflings-icon white remove"></i></a>
					    </div>
				    </div>
				    <div class="box-content">                    
					    <div class="todo metro">                            
                           
                            <asp:Repeater ID="RepterDetails" runat="server">  
                                <HeaderTemplate>  
                                <ul class="todo-list">
                                </HeaderTemplate>  
                                <ItemTemplate>                                     
                                    <li class="red">
                                        <asp:Label ID="lblUser" runat="server" Font-Bold="true" Text='<%#Eval("ProductName") %>'/> 
                                        <strong><asp:Label ID="lblDate" runat="server" Font-Bold="true" Text='<%#Eval("Dstk_ItmQty") %>'/></strong>
                                    </li>                                    
                                </ItemTemplate>  
                                <FooterTemplate>  
                                </ul> 
                                </FooterTemplate>  
                            </asp:Repeater>
                        					
                    </div>	
				</div>               
			</div>
        </asp:Panel>
        <asp:Panel ID="pnlDayEnd" runat="server">
			<div class="box span6">
				<div class="box-header" data-original-title>
					<h2><i class="halflings-icon align-justify"></i><span class="break"></span>Day End</h2>
					<div class="box-icon">
					</div>
				</div>
				<div class="box-content">
                    <div class="span8">
                        <div class="control-group  span12">							
						    <div class="controls">                                                                
                                <div class="span5"></div>
                                <div class="span6">
                                    <div class="span5">
                                        <div class="item">
                                            <asp:Button ID="btnSubmit" runat="server" Text="Create BackUP!!..." CssClass="btn btn-primary" OnClick="btnSubmit_Click"  />
                                            <br />
                                            <asp:FileUpload ID="FileUpload1" runat="server" />
                                            <asp:Button ID="btn_upload" runat="server" Text="Upload" CssClass="btn btn-primary" OnClick="btn_upload_Click" />
					                    </div>
                                    </div>                          
                 		        </div>
				            </div>
                        </div>
                    </div>
                </div>
            </div>		
        </asp:Panel>
	</div><!--/row-->    
</asp:Content>
