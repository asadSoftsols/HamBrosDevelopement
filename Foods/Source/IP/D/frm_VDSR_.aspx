﻿<%@ Page Title="Verify Daily Sales Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frm_VDSR_.aspx.cs" Inherits="Foods.frm_VDSR_" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <style type="text/css">
        /* Scroller Start */

        .scrollit {
            overflow:scroll;
            height:100%;
	        width:100%;           
	        margin:0px auto;
        }

      /* Scroller End */

      /* Modal SalespUp Start */

        .modalBackground{
                background-color: #000000;
                filter: alpha(opacity=10);
                opacity: 0.7;
        }
         .modalBackgroundSupplier {
                 background-color: #000000;
                filter: alpha(opacity=10);
                opacity: 0.7;
         }
        .modalBackground1{
                width: 500px;
                height: 500px;
                background-color: #000000;
                filter: alpha(opacity=10);
                opacity: 0.6;
        }
        .modalSalespup{
                border: 3px solid #000000;
                background-color: #FFFFFF;
                margin-top: 0px;
                color: #000000;
                margin-right: -3px;
                margin-bottom: 0px;
        }

        .modalSalespup1{
                width:202px;
                height:140px;
                border: 3px solid #000000;
                background-color: #FFFFFF;
                color: #FF0000;
                margin-right: -3px;
                text-align: center;
                margin-left: -20px;
                margin-top: -80px;
        }
        .closebtn {
                float:right;
        }

        /* Modal SalespUp End */

         .completionList {

            border:solid 1px Gray;
            margin:0px;
            padding:3px;
            height: 120px;
            overflow:auto;  
            background-color: #FFFFFF;     
        } 

        .listItem {

            color: #191919;
        } 

        .itemHighlighted {

            background-color: #ADD6FF;       
        }

    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    
    <ul class="breadcrumb">
        <li><i class="icon-home"></i><a href="WellCome.aspx">Home</a><i class="icon-angle-right"></i></li>
        <li><a href="#">Sales</a><i class="icon-angle-right"></i></li>
        <li><a href="frm_VDSR_.aspx">Daily Sales Report</a></li>
    </ul>
    <!-- imageLoader - START -->

    <img id='HiddenLoadingImage' src="../../img/page-loader.gif" class="LoadingProgress" />

    <!-- imageLoader - END -->

    <div class="row-fluid">
        <div class="box  span12">
            <div class="box-header" data-original-title>
                <h3><i class="halflings-icon edit"></i><span class="break"></span> Daily Sales Report </h3>
            </div>
            <div class="box-content">
                <div class="span12">
                    <div style="text-align:center">
                        <asp:Label ID="lbl_err" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </div>
                <div class="row-fluid">	
                    <div class="span12">
                        <div class="span5">
                            <h1><span style="color:black;">Daily Sales Report</span></h1>                                    
                        </div>
                        <div class="span12"></div>
                        <div class="span5">
                            <div class="control-group">    
                                DSR
                                <div class="controls">
                                    <asp:TextBox runat="server" class="span10 datepicker" ID="TB_DSR" AutoPostBack="true" placeholder="Enter Client Name..." OnTextChanged="TB_DSR_TextChanged" ></asp:TextBox>                                   
                                     <asp:AutoCompleteExtender ServiceMethod="GetCust" CompletionListCssClass="completionList"
                                                        CompletionListItemCssClass="listItem" CompletionListHighlightedItemCssClass="itemHighlighted" MinimumPrefixLength="1" CompletionInterval="10" EnableCaching="false" CompletionSetCount="10" TargetControlID="TB_DSR" ID="AutoCompleteExtender1"  
                                                        runat="server" FirstRowSelected="false"></asp:AutoCompleteExtender>
                                </div>                                
                            </div>
                        </div>
                        <div class="span5">
                            <div class="control-group">    
                                Date
                                <div class="controls">
                                    <asp:TextBox runat="server" class="span10 datepicker" ID="TBdsrdat" placeholder="Date..." ></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RFDat" runat="server" ForeColor="Red"
                                        ControlToValidate="TBdsrdat" ValidationGroup="VGPro"
                                        ErrorMessage="Please Enter the Date!"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="CVdat" Type="Date" Operator="DataTypeCheck"
                                        ControlToValidate="TBdsrdat" runat="server" ValidationGroup="VGPro"
                                        ForeColor="Red" ErrorMessage="Please Write Correct Date!"></asp:CompareValidator>
                                </div>                                
                            </div>
                        </div>
                        
                       <div class="row-fluid sortable">		
	                    <div class="box span12">
				        <div class="box-header" data-original-title>
					        <h2><i class="halflings-icon align-justify"></i><span class="break"></span>DSR List</h2>
					        <div class="box-icon">
						        <a href="#" class="btn-minimize"><i class="halflings-icon chevron-up"></i></a>
						        <a href="#" class="btn-close"><i class="halflings-icon remove"></i></a>
					        </div>
			            </div>
				        <div class="box-content">
                            <div class="scrollit">
                                <div class="div_print">
                                    <asp:GridView ID="GVDSR" runat="server" PageSize="7" AllowPaging="True" AutoGenerateColumns="False" ShowHeader="true" ShowHeaderWhenEmpty="true" CssClass="table table-striped table-bordered" DataKeyNames="dsrid,CustomerID" OnRowCommand="GVDSR_RowCommand" OnRowDeleting="GVDSR_RowDeleting" OnRowDataBound="GVDSR_RowDataBound" OnPageIndexChanging="GVDSR_PageIndexChanging" >
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderText="ID"  SortExpression="ID" />
                                            <asp:BoundField DataField="CustomerName" HeaderText="Customer"  SortExpression="CustomerName" />
                                            <asp:BoundField DataField="dsrdat" HeaderText="Date" SortExpression="dsrdat" />                                                                                    
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hflock" runat="server" Value='<%# Eval("Isdon")%>' />
                                                    <asp:LinkButton ID="lnkSelect" CommandName="Select" runat="server" Text="Select" ></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkinvoice" CommandName="invoice" runat="server"  > Invoice </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" CommandName="Delete"  runat="server" OnClientClick="" Text="Delete" > </asp:LinkButton>
                                                    <asp:HiddenField ID="HFdsrID" runat="server" Value='<%# Eval("dsrid")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                </asp:GridView>
                                </div>
                            </div>
                        </div>
                </div>
            </div>
            <asp:Panel ID="pnl_areacustsalman" runat="server">
                <div class="span12">
                    <div class="span3">
                        <div class="control-group">    
                            Area
                            <div class="controls">
                                <asp:DropDownList ID="DDL_area" runat="server" CssClass="span8" data-rel="chosen" AutoPostBack="true" OnSelectedIndexChanged="DDL_area_SelectedIndexChanged" ></asp:DropDownList>
                                <asp:RequiredFieldValidator id="RequiredFieldValidator1" Text="Please Select Area.." ForeColor="Red" Font-Size="Smaller"
                                        ValidationGroup="VGPro" InitialValue="0" ControlToValidate="DDL_area" Runat="server" />  
                            </div>
                        </div>
                    </div>
                    <div class="span4">
                        <div class="control-group">    
                            Customers
                            <div class="controls">
                                <asp:DropDownList ID="DDL_Cust" runat="server" CssClass="span8" data-rel="chosen" AutoPostBack="true" OnSelectedIndexChanged="DDL_Cust_SelectedIndexChanged" ></asp:DropDownList>
                                <asp:RequiredFieldValidator id="reqCust" Text="Please Select Customer.." ForeColor="Red" Font-Size="Smaller"
                                        ValidationGroup="VGPro" InitialValue="0" ControlToValidate="DDL_Cust" Runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="span4">
                        Sales Man
                        <div class="controls">
                            <asp:DropDownList ID="DDL_Salman" runat="server" CssClass="span8" data-rel="chosen" ></asp:DropDownList>
                            <asp:RequiredFieldValidator id="RequiredFieldValidator2" Text="Please Select Sales Man.."
                                ForeColor="Red" Font-Size="Smaller"
                                ValidationGroup="VGPro" InitialValue="0" ControlToValidate="DDL_Salman" Runat="server" /> 
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <div class="span12"></div>
                        <div class="scrollit">
                            <div class="row-fluid">	
                            <div class="span12">
                                <div class="box-content span12">                            
                                    <asp:GridView ID="GVPro1" runat="server"  ShowFooter="true" AutoGenerateColumns="False"  class="table table-striped table-bordered"  OnRowDataBound="GVPro1_RowDataBound" OnRowDeleting="GVPro1_RowDeleting" >
                                        <Columns>                 
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="linkbtndel" OnClick="linkbtndel_Click"  runat="server" >Delete</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>                   
                                            <asp:TemplateField HeaderText="ITEM NAME">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TextBox1" placeholder="Enter Product Name..." runat="server" AutoPostBack="true" OnTextChanged="TextBox1_TextChanged" ></asp:TextBox>
                                                    <asp:AutoCompleteExtender ServiceMethod="GetSearch" CompletionListCssClass="completionList"
                                                        CompletionListItemCssClass="listItem" CompletionListHighlightedItemCssClass="itemHighlighted" MinimumPrefixLength="1" CompletionInterval="10" EnableCaching="false" CompletionSetCount="10" TargetControlID="TextBox1" ID="AutoCompleteExtender1"  
                                                        runat="server" FirstRowSelected="false"></asp:AutoCompleteExtender>
                                                    <asp:Label ID="lblPurItm" runat="server" Visible="false" Text='<%# Eval("ITEMNAME")%>' />             
                                                </ItemTemplate>                                        
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="UNITS">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_unt" runat="server" Visible="false" Text='<%# Eval("UNITS")%>' />
                                                    <asp:DropDownList ID="DDL_Unt" runat="server" data-rel="chosen" ></asp:DropDownList>
                                                    <asp:RequiredFieldValidator InitialValue="0" ID="untid" Display="Dynamic" 
                                                 ValidationGroup="Pro" runat="server" ControlToValidate="DDL_Unt" 
                                                 ErrorMessage="Please Select Units" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="QUANTITY">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TBQty" runat="server" Enabled="false"  Text='<%# Eval("QTY")%>' AutoPostBack="true" placeholder="Quantity..." style="width:80px; height:20px; background:none; border:none;" OnTextChanged="TBQty_TextChanged" ></asp:TextBox>
                                                    <asp:Label ID="lbl_chkqty" runat="server" ForeColor="Red"></asp:Label>
                                                </ItemTemplate>                                      
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SALE RATE">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TBSalRat" runat="server"  Text='<%# Eval("SALERATE")%>' placeholder="Sale Rate..." style="width:80px; height:20px; background:none; border:none;" ></asp:TextBox>
                                                </ItemTemplate>                                      
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SALE RETURN" >
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lbSalRtrn" runat="server"  Text='<%# Eval("SALERETURN")%>' AutoPostBack="true" placeholder="Sale Return..." OnTextChanged="lbSalRtrn_TextChanged"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Final Qty" >
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtfinlqty" runat="server"  Text='<%# Eval("FINALQTY")%>'  placeholder="Final Quantity..."></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="RECOVERY" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="LBRecy" runat="server"  Text='<%# Eval("RECOVERY")%>' ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="OUTSTANDING" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbOutstan" runat="server"  Text='<%# Eval("OUTSTANDING")%>' ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="FURTHER OUTSTANDING" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblfurOutstan" runat="server"  Text='<%# Eval("FUROUTSTANDING")%>' ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="AMOUNT">
                                                <ItemTemplate>
                                                     <asp:Label ID="lbl_Flag"  runat="server" Text="0" Visible="false" />
                                                    <asp:TextBox ID="TBAmt" runat="server"  Text='<%# Eval("AMOUNT")%>' placeholder="Total Amount..." style="width:80px; height:20px; background:none; border:none;" ></asp:TextBox>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblttl" runat="server" Text='<%# Eval("Total")%>' Visible="false"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="REMARKS">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TBRmk" runat="server"  Text='<%# Eval("REMARKS")%>' placeholder="Remarks..." style="width:80px; height:20px; background:none; border:none;" ></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label ID="HFDSR" runat="server" Text='<%# Eval("ddsr")%>' Visible="false" ></asp:Label>
                                                    <asp:LinkButton ID="linkbtnadd" OnClick="linkbtnadd_Click"  runat="server" ValidationGroup="Pro" >+</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:CommandField ShowDeleteButton="True" DeleteText="-"  >
                                                    <ControlStyle CssClass="halflings-icon minus-sign" />
                                                </asp:CommandField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:Label ID="v_item" runat="server" Font-Bold="true" ForeColor="Red" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                        </div>
                        <div class="span12">&nbsp;</div>
                        <div class="span12">&nbsp;</div>
                        
                        <div class="span2">
                            <asp:Label  ID="Label4" runat="server" Text="Outstanding:"></asp:Label>
                            <asp:TextBox ID="lblOutstan" runat="server" placeholder="Outstanding..." style="width:80px; height:20px; background:none; border:none;" OnTextChanged="lblOutstan_TextChanged" AutoPostBack="true" ></asp:TextBox>
                        </div>
                        <div class="span2">
                            <asp:Label  ID="Label5" runat="server" Text="New Sales Credit:"></asp:Label>
                            <asp:TextBox ID="tbfurOutstan" runat="server"  placeholder="Further Outrstanding..." Text="0" style="width:80px; height:20px; background:none; border:none;" OnTextChanged="tbfurOutstan_TextChanged" AutoPostBack="true" ></asp:TextBox>                            
                        </div>
                        <div class="span2">
                            <asp:Label  ID="Label2" runat="server" Text="Prev Sales Recovery:"></asp:Label>
                            <asp:TextBox ID="TBRecy" runat="server" OnTextChanged="TBRecy_TextChanged"  AutoPostBack="true" placeholder="Recovery..." style="width:80px; height:20px; background:none; border:none;"  ></asp:TextBox>
                        </div>                
                        <div class="span2">
                            <asp:Label  ID="Label7" runat="server" Text="Discount:"></asp:Label>
                            <asp:Label  ID="lbl_disc" runat="server" style="width:80px; height:20px;" ></asp:Label>
                        </div>       
                        <div class="span2">
                             <asp:Label  ID="Label1" runat="server" Text="Total After Discount:"></asp:Label>&nbsp;&nbsp;&nbsp;<asp:TextBox ID="tbtotal" runat="server" Text="0.00"></asp:TextBox>
                        </div>
                        
                        <%--<div class="span2">
                            <asp:Label  Visible="false" ID="Label6" runat="server" Text="Sales Return:"></asp:Label>
                            <asp:Label  ID="Label7" runat="server" Text="Discount:"></asp:Label>
                            <asp:Label  ID="lbl_disc" runat="server" style="width:80px; height:20px;" ></asp:Label>
                            <asp:TextBox ID="TBSalRtrn" Visible="false" runat="server" placeholder="Sale Return..." style="width:80px; height:20px; background:none; border:none;" AutoPostBack="true"  OnTextChanged="TBSalRtrn_TextChanged" ></asp:TextBox>
                        </div>
                        <div class="span2">
                            <asp:Label  ID="Label4" runat="server" Text="Outstanding:"></asp:Label>
                            <asp:TextBox ID="lblOutstan" runat="server" placeholder="Outstanding..." style="width:80px; height:20px; background:none; border:none;" OnTextChanged="lblOutstan_TextChanged" AutoPostBack="true" ></asp:TextBox>
                        </div>
                        <div class="span2">
                            <asp:Label  ID="Label5" runat="server" Text="Further Outstanding:"></asp:Label>
                            <asp:TextBox ID="tbfurOutstan" runat="server"  placeholder="Further Outrstanding..." Text="0" style="width:80px; height:20px; background:none; border:none;" OnTextChanged="tbfurOutstan_TextChanged" AutoPostBack="true" ></asp:TextBox>                            
                        </div>
                        <div class="span2">
                            <asp:Label  ID="Label2" runat="server" Text="Recovery:"></asp:Label>
                            <asp:TextBox ID="TBRecy" runat="server"   AutoPostBack="true" placeholder="Recovery..." style="width:80px; height:20px; background:none; border:none;" OnTextChanged="TBRecy_TextChanged" ></asp:TextBox>
                        </div>
                        <div class="span2">
                             <asp:Label  ID="Label1" runat="server" Text="Total After Discount:"></asp:Label>&nbsp;&nbsp;&nbsp;<asp:TextBox ID="tbtotal" runat="server" Text="0.00"></asp:TextBox>
                        </div>--%>
                        <div class="span12">
                            <div class="span5">
                                <asp:Button runat="server"  ID="btnSave" Text="Save"  CssClass="btn btn-info" OnClick="btnSave_Click" />                      
                                <asp:Button runat="server"  ID="btnUpd" Text="Update"  CssClass="btn btn-danger" OnClick="btnUpd_Click" />                            
                                <asp:Button runat="server"  ID="btnCancl" Text="Cancel" CssClass="btn" OnClick="btnCancl_Click" />
                            </div>
                            <div class="span2">
                                <asp:Label ID="lbl_ChkArea" ForeColor="Red" Font-Size="Small" runat="server" Text=""></asp:Label>
                                <asp:Label ID="lbl_Chkcust" ForeColor="Red" Font-Size="Small" runat="server" Text=""></asp:Label>
                                <asp:Label ID="lbl_ChkSalman" ForeColor="Red" Font-Size="Small" runat="server" Text=""></asp:Label>
                                <asp:Label ID="lbl_Chktotl" ForeColor="Red" Font-Size="Small" runat="server" Text=""></asp:Label>
                                <asp:Label ID="lbl_ChkProNam" ForeColor="Red" Font-Size="Small" runat="server" Text=""></asp:Label>
                                <asp:Label ID="lbl_Chkunt" ForeColor="Red" Font-Size="Small" runat="server" Text=""></asp:Label>
                                <asp:Label ID="lbl_Chkqty" ForeColor="Red" Font-Size="Small" runat="server" Text=""></asp:Label>
                                <asp:Label ID="lbl_Chksalrat" ForeColor="Red" Font-Size="Small" runat="server" Text=""></asp:Label>
                                <asp:Label ID="lbl_ChkAmt" ForeColor="Red" Font-Size="Small" runat="server" Text=""></asp:Label>

                            </div>

                        </div>  
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="HFProcd" runat="server" />
    <asp:HiddenField ID="HFHead" runat="server" />
    <asp:HiddenField ID="HFSubHead" runat="server" />
    <asp:HiddenField ID="SubHeadCat" runat="server" />
    <asp:HiddenField ID="SubHeadCatFou" runat="server" />
    <asp:HiddenField ID="SubHeadCatFiv" runat="server" />
    <asp:HiddenField ID="HFSubHeadCatFivID" runat="server" />
    <asp:HiddenField ID="HFdsrID" runat="server" Value="0" />


        <div id="MyModalDelete" class="modal fade" style="display:none;">
		<div class="modal-dialog">
		<div class="modal-content">
			<div class="modal-header">
				<h4 class="modal-title">Error!</h4>
			</div>
			<!-- dialog body -->
			<div class="modal-body">
				<asp:Label ID="lblmodaldelete" runat="server"></asp:Label>
                
			</div>
			<!-- dialog buttons -->
            <div class="modal-footer">
                <asp:LinkButton ID="linkmodaldelete" runat="server" CssClass="btn btn-success" Text="Yes" OnClick="linkmodaldelete_Click"></asp:LinkButton>
                <asp:LinkButton ID="linkbtncanceldel" runat="server" CssClass="btn btn-success" Text="No"></asp:LinkButton>
                <asp:HiddenField ID="HFCustId" runat="server" />
			</div>
		</div>
		</div>
	</div>

    <div id="ModalAlert" class="modal fade" style="display:none;">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Error!</h4>
                </div>
                <!-- dialog body -->
                <div class="modal-body">
                    <asp:Label ID="lblalert" runat="server"></asp:Label>                
                </div>
                <!-- dialog buttons -->
                <div class="modal-footer">
                    <asp:LinkButton ID="btnalertOk" runat="server" CssClass="btn btn-success" Text="OK" ></asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
      </ContentTemplate>
    </asp:UpdatePanel>
  
    <script type="text/javascript">
        
         var c = function () {

             
             $("#ModalAlert").on("show", function () {    // wire up the OK button to dismiss the modal when shown

                 $("#ModalAlert a.btn").on("click", function (e) {
                     console.log("button pressed");   // just as an example...
                     $("#ModalAlert").modal('hide');     // dismiss the dialog
                 });

             });

             $("#ModalAlert").on("hide", function () {    // remove the event listeners when the dialog is dismissed
                 $("#ModalAlert a.btn").off("click");
             });

             $("#ModalAlert").on("hidden", function () {  // remove the actual elements from the DOM when fully hidden
                 $("#ModalAlert").remove();
             });

             $("#ModalAlert").modal({                    // wire up the actual modal functionality and show the dialog
                 "backdrop": "static",
                 "keyboard": true,
                 "show": true                     // ensure the modal is shown immediately
             });
         }

    </script>

    <div style="display:none">
        <div class="span2">
                            <asp:Label  Visible="false" ID="Label6" runat="server" Text="Sales Return:"></asp:Label>                            
                            <asp:TextBox ID="TBSalRtrn" Visible="false" runat="server" placeholder="Sale Return..." style="width:80px; height:20px; background:none; border:none;" AutoPostBack="true"  OnTextChanged="TBSalRtrn_TextChanged" ></asp:TextBox>
                        </div>
    </div>
</asp:Content>
