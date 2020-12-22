<%@ Page Title="Sales" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="frm_Sal.aspx.cs" Inherits="Foods.frm_Sal" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
            /* Scroller Start */
        .scrollit {
            overflow:scroll;
            height:100%;
	        width:100%;           
	        margin:0px auto;
        }discount
        .calender {
            border:solid 1px Gray;
            margin:0px;
            padding:3px;
            height: 200px;
            overflow:auto;
            background-color: #FFFFFF;     
            z-index:12000 !important;
            position:absolute;
        }

        .completionList {
            border:solid 1px Gray;
            margin:0px;
            padding:3px;
            height: 120px;
            overflow:auto;
            background-color: #FFFFFF;   
            z-index:12000 !important;  
            position:absolute;
        } 

        .listItem {
            color: #191919;
        } 

        .itemHighlighted {
            background-color: #ADD6FF;               
        }


      /* Scroller End */
      /* Modal SalespUp Start */

        .modalBackground{
                background-color: #000000;
                filter: alpha(opacity=10);
                opacity: 0.7;
                z-index:1000 !important;
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
     <asp:UpdatePanel  ID="UpdatePanel1" runat="server">
        <ContentTemplate>  
           
        <ul class="breadcrumb">
        <li>
            <i class="icon-home"></i>
                <a href="WellCome.aspx">Home</a> 
            <i class="icon-angle-right"></i>
        </li>
        <li>
            <a href="#">Sales</a>
            <i class="icon-angle-right"></i>
        </li>
            <li><a href="frm_Sal.aspx">Sales</a></li>
    </ul>

    
    <!-- imageLoader - START -->

    <img id='HiddenLoadingImage' src="../../img/page-loader.gif" class="LoadingProgress" />

    <!-- imageLoader - END -->
    <div class="row-fluid">
    <div class="box  span12">
        <div class="box-header" data-original-title>
            <h2><i class="halflings-icon edit"></i><span class="break"></span> Create Sales </h2>
        </div>
        <div class="box-content">
            <asp:Panel ID="PanelShowClosed" runat="server" Visible="false" style="visibility:hidden;">
                <div class="row-fluid">	
                    <div class="box span12">
                        <div class="box-header" data-original-title>
                            <div class="centerClosed">    
                                <P>Closed!</P>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <div class="span12">
                        <div style="text-align:center">
                            <asp:Label ID="lbl_err" runat="server" ForeColor="Red"></asp:Label>
                        </div>
                     </div>
            <div class="row-fluid">	
                <div class="box span12">
                    <div class="box-content">                                 
                        <div class="span1">
                            <div class="control-group">
                                <label class="control-label" for="TBSearchSales">Sales</label>
                            </div>
                        </div>
                        <div class="span10">
                            <div class="controls">
                                <div class="input-append">
                                    <asp:TextBox runat="server" class="span12" ID="TBSearchSales" AutoPostBack="true" OnTextChanged="TBSearchSalesNum_TextChanged"   ></asp:TextBox><asp:LinkButton runat="server" ID="LinkButton1" CssClass="add-on" ><i class="icon-search"></i></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                        <div class="span12">
                            <div class="controls">
                                <div class="scrollit">
                                    <asp:GridView ID="GVScrhMSal" runat="server" class="table table-striped table-bordered" AllowPaging="True" PageSize="5" AutoGenerateColumns="False" DataKeyNames="MSal_id,CustomerID" OnPageIndexChanging="GVScrhMSal_PageIndexChanging" OnRowDeleting="GVScrhMSal_RowDeleting" OnRowCommand="GVScrhMSal_RowCommand">
                                        <Columns>
                                            <asp:BoundField DataField="MSal_sono" HeaderText="ID" SortExpression="MSal_sono" ReadOnly="True" />                                            
                                            <asp:BoundField DataField="CustomerName" HeaderText="Customer" SortExpression="CustomerName" />
                                            <asp:BoundField DataField="SalDate" HeaderText="Date" SortExpression="SalDate" />
                                            <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy" />                                            
                                            <asp:BoundField DataField="CreatDate" HeaderText="Created At" SortExpression="CreatDate" />                                            
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LBtnScrhMSal" runat="server" CommandName="Select" > Select </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LBtnDel" runat="server" Text="Delete" CommandName="Delete" > Delete </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LBtn" runat="server" Text="Invoice" CommandName="Show" > Invoice </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="span12"></div>               
            <div class="row-fluid">	
                <div class="span12">
                    <div class="span3">
                        <h1><span style="color:black;"> Sales</span></h1>                                    
                    </div>
                    <div class="span3"></div>
                    <div class="span3">
                        <div class="span12">
                            <div class="box-content">
                                <h2>Date:</h2>
                                <asp:TextBox runat="server" class="span10" ID="TBSalDat" ></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" CssClass="calender" PopupButtonID="imgPopup" runat="server" 
                                    TargetControlID="TBSalDat" Format="yyyy-MM-dd"> </asp:CalendarExtender>
                            </div>
                        </div>
                    </div>                               
                    <div class="span1"></div>
                    <div  class="span3">
                        <div class="span12">                            
                            <h2>Voucher No.</h2>
                            <div class="box-content">
                                <asp:Label runat="server" class="span12" ID="TBSalesNum" ></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="box-content">
                            <div class="span1">&nbsp;</div>
                            <div class="span2">
                                <div class="control-group">                                            
                                    <div class="controls">
                                        <asp:CheckBox ID="chk_Act" runat="server" Text="Active" />
                                    </div>
                                </div>
                            </div>
                            <div class="span2">
                                <div class="control-group">                                            
                                    <div class="controls">
                                        <asp:CheckBox ID="chk_prtd" runat="server" Text="Printed" />
                                    </div>
                                </div>
                            </div>
                            <div class="span2">
                                <div class="control-group">                                            
                                    <div class="controls">
                                        <asp:RadioButton id="ckcrdt" GroupName="CreCsh" Text="Credit" runat="server" Checked="true" />
                                    </div>
                                </div>
                            </div>
                            <div class="span2">
                                <div class="control-group">                                            
                                    <div class="controls">
                                        <asp:RadioButton id="ckcsh" GroupName="CreCsh" Text="Cash" runat="server"/>
                                    </div>
                                </div>
                            </div>
                            <div class="span3">
                                <div class="span6">
                                    <asp:Button ID="btn_ImportDSR" Text="Import DSR" CssClass="btn btn-primary" runat="server" />
                                </div>						            
						    </div>      
                            <div class="span4">
                                <div class="control-group">    
                                    Gate Pass No.                              
                                    <div class="controls">
                                        <asp:TextBox runat="server" class="span12" ID="TBGPNo" placeholder="Gate Pass No..." ></asp:TextBox>
                                    </div>
                                </div>
                            </div>  
                                
                            <div class="span6">
                                <div class="control-group">
                                    Customer
                                    <div class="controls">
                                        <asp:DropDownList id="DDL_Cust" runat="server" data-rel="chosen" CssClass="span12" AutoPostBack="true" OnSelectedIndexChanged="ddl_Cust_SelectedIndexChanged" ></asp:DropDownList>                                           
                                    </div>
                            <asp:Label ID="v_custumer" runat="server" Font-Bold="true" ForeColor="Red" Text=""></asp:Label>
                                </div>
                            </div>
                            <div class="span5">
                                <div class="control-group">
                                    Booker
                                    <div class="controls">
                                        <asp:DropDownList id="DDL_Book" runat="server" data-rel="chosen" CssClass="span12" AutoPostBack="true" OnSelectedIndexChanged="ddl_Cust_SelectedIndexChanged" ></asp:DropDownList>                                           
                                    </div>
                                </div>  
                            </div><div class="span6">
                                <div class="control-group">
                                    Sales Man
                                    <div class="controls">
                                        <asp:DropDownList id="DDL_SalMan" runat="server" data-rel="chosen" CssClass="span12" AutoPostBack="true" OnSelectedIndexChanged="ddl_Cust_SelectedIndexChanged" ></asp:DropDownList>                                           
                                    </div>
                                </div>
                            </div>                                
                            <div class="span12">
                                <div class="control-group">
                                        <label style="color:black" for="TBRmk" >Remarks</label>
                                    <div class="controls">
                                        <asp:TextBox runat="server" class="span12" ID="TBRmk" TextMode="MultiLine"></asp:TextBox>                                    
                                    </div>
                                </div>
                            </div>
                        </div>
                    <div class="scrollit">
                    <asp:Panel ID="PnlCrtSalItem" runat="server">
                        <div class="row-fluid">	
                            <div class="span12">
                                <div class="box-content span12">                            
                                    <asp:GridView ID="GVItms" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" EmptyDataText="No Record Exits!!"  class="table table-striped table-bordered" OnRowDeleting="GVItms_RowDeleting" OnRowDataBound="GVItms_RowDataBound" OnRowCommand="GVItms_RowCommand" OnSelectedIndexChanged="GVItms_SelectedIndexChanged" >
                                        <Columns>
                                            <asp:TemplateField HeaderText="PARTICULARS" Visible="false">  
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DDL_Par" runat="server" OnSelectedIndexChanged="DDL_Par_SelectedIndexChanged"  AutoPostBack="True" ></asp:DropDownList>
                                                    <asp:Label ID="lbl_Par" runat="server" Visible="false" Text='<%# Eval("PARTICULARS") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PRODUCT">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="tbprodt" placeholder="Enter Product Name.." AutoPostBack="true" runat="server" Text='<%# Eval("PRODUCTNAM") %>' OnTextChanged="tbprodt_TextChanged" ></asp:TextBox>
                                                     <asp:AutoCompleteExtender ServiceMethod="GetSearch" CompletionListCssClass="completionList"
                                                        CompletionListItemCssClass="listItem" CompletionListHighlightedItemCssClass="itemHighlighted"
                                                          MinimumPrefixLength="1" CompletionInterval="10" EnableCaching="false" 
                                                         CompletionSetCount="10" TargetControlID="tbprodt" ID="AutoCompleteExtender1"  
                                                        runat="server" FirstRowSelected="false"></asp:AutoCompleteExtender>
                                                    <asp:Label ID="lbl_Pro" runat="server" Visible="false"  Text='<%# Eval("PRODUCT") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Percentage" Visible="false">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TBDIS" runat="server" Text='<%# Eval("DIS")%>' style="width:50px; height:20px; background:none; border:none;" AutoPostBack="true" OnTextChanged="TBDIS_TextChanged"></asp:TextBox>                                                        
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="QTY Available">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItmQty" runat="server" Text='<%# Eval("QTYAVAIL")%>' />                                                        
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="RATE">
                                                <ItemTemplate>    
                                                    <asp:TextBox ID="TBrat" runat="server"  Text='<%# Eval("RATE")%>'  style="width:80px; height:20px; background:none; border:none;" ></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RFVRat" ForeColor="Red" ValidationGroup="gvItems" runat="server" ErrorMessage="Please Write Some in Rate" ControlToValidate="TBrat"></asp:RequiredFieldValidator>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="QTY">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbshw" runat="server" />                                                        
                                                    <asp:TextBox ID="TBItmQty" runat="server" Text='<%# Eval("QTY")%>' style="width:50px; height:20px; background:none; border:none;" AutoPostBack="true" OnTextChanged="TBItmQty_TextChanged" ></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RFVQty" ForeColor="Red" ValidationGroup="gvItems" runat="server" ErrorMessage="Please Write Some in Quantity" ControlToValidate="TBItmQty"></asp:RequiredFieldValidator>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="UNIT" Visible="false">
                                                <ItemTemplate>       
                                                    <asp:TextBox ID="TbItmunt"  runat="server" Text='<%# Eval("UNIT")%>' style="width:80px; height:20px; background:none; border:none;" ></asp:TextBox>                    
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="AMOUNT">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Flag"  runat="server" Text="0" Visible="false" />
                                                    <asp:TextBox ID="TBamt" runat="server"  Text='<%# Eval("AMT")%>' style="width:80px; height:20px; background:none; border:none;" ></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RFVAmt" ForeColor="Red" ValidationGroup="gvItems" runat="server" ErrorMessage="Please Write Some in Amount" ControlToValidate="TBamt"></asp:RequiredFieldValidator>
                                                </ItemTemplate>
                                            </asp:TemplateField>        
                                            <asp:TemplateField>                                                
                                                <ItemTemplate>
                                                    <asp:Label ID="Isupdat" runat="server" Text="" Visible="false"></asp:Label>
                                                    <asp:LinkButton ID="lnkbtnadd" ValidationGroup="gvItems" CommandName="Add"  OnClick="linkbtnadd_Click" runat="server"><i class="halflings-icon plus-sign" >+</i></asp:LinkButton>
                                                    <asp:HiddenField ID="HFDSal" runat="server" Value='<%# Eval("DSal_id")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>                                     
                                            <asp:CommandField ShowDeleteButton="True" DeleteText="-"  >
                                                <ControlStyle CssClass="halflings-icon minus-sign" />
                                            </asp:CommandField>

                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>     
                    </asp:Panel>
                </div>
            <div class="row-fluid">
                <div class="box span12">
                    <div class="box-content">
                        <div class="row-fluid">	
                            <div class="span12">
                                <div  class="span2">
                                    <div style="display:none;">
                                        <h5><span class="break"></span>Recovery</h5>
                                        <asp:TextBox ID="TBRecov" runat="server" AutoPostBack="true"  OnTextChanged="TBRecov_TextChanged" ></asp:TextBox>                                     
                                    </div>
                                </div>
                                <div  class="span2">
                                    <h5><span class="break"></span>Previous Balance</h5>
                                    <asp:Label ID="lbl_outstan" runat="server"  ></asp:Label>                                     
                                </div>
                                <div  class="span2">
                                    <h5><span class="break"></span>Out Standing</h5>
                                    <asp:TextBox runat="server" class="span12" ID="TBoutstan"  AutoPostBack="true" OnTextChanged="TBoutstan_TextChanged"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="VGSal" ForeColor="Red" runat="server" ErrorMessage="Recovery Can not be Null" ControlToValidate="TBoutstan"></asp:RequiredFieldValidator>  
                                </div>
                                <div  class="span2">
                                    <h5><span class="break"></span> Grand Total:</h5>
                                    <asp:TextBox runat="server" class="span12" ID="TBGTtl" Text="0.00"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RFVGTtl" ValidationGroup="VGSal"  runat="server" ErrorMessage="GST Can not be Null" ControlToValidate="TBGST" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>
                                <div  class="span2">
                                    <h5><span class="break"></span> Discount %:</h5>
                                    <asp:TextBox ID="TBDIS" runat="server" AutoPostBack="true" OnTextChanged="TBDIS_TextChanged1" ></asp:TextBox>
                                </div>
                                <div  class="span2">
                                    <h5><span class="break"></span> Discount Amount:</h5>
                                    <asp:Label ID="lbl_discamt" runat="server"  Text="0.00" ></asp:Label>
                                </div>
                            </div>                        
                            <div class="span12">
                                
                                <div  class="span2">
                                    <h5><span class="break"></span> Special Discount %:</h5>
                                    <asp:TextBox ID="TB_SDIS" runat="server" AutoPostBack="true" Text="0.00" OnTextChanged="TB_SDIS_TextChanged" ></asp:TextBox>
                                </div>
                                <div  class="span2">
                                    <h5><span class="break"></span> Special Discount Amount:</h5>
                                    <asp:Label ID="lbl_SdisAmt" runat="server" Text="0.00" ></asp:Label>
                                </div>
                                <div  class="span2">
                                    <h5><span class="break"></span>GST%</h5>
                                    <asp:TextBox runat="server" class="span12" ID="TBGST" Text="0.00" AutoPostBack="true" OnTextChanged="TBGST_TextChanged"></asp:TextBox>
                                </div>
                                <div  class="span2">
                                    <h5><span class="break"></span>Other Tax %</h5>
                                    <asp:TextBox runat="server" class="span12" AutoPostBack="true" ID="TBOthTax" Text="0.00" OnTextChanged="TBOthTax_TextChanged" ></asp:TextBox>
                                </div>
                                <div  class="span2" >
                                    <h5><span class="break"></span>Net Total</h5>
                                    <asp:TextBox runat="server" class="span12" ID="TBTtl" Text="0.00"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1"  ValidationGroup="VGSal" ForeColor="Red" runat="server" ErrorMessage="Total Amount Can not be Null" ControlToValidate="TBTtl"></asp:RequiredFieldValidator>
                                </div>
                                <div  class="span2" >
                                    <h5><span class="break"></span>Total</h5>
                                    <asp:TextBox runat="server" class="span12" ID="TB_ttl" Text="0.00"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row-fluid">	
                            <div class="span12">
                                <div class="span2" style="visibility:hidden;">
                                    <asp:CheckBox runat="server" ID="ckSch" AutoPostBack="true" Text="Scheme" TextAlign="Left" OnCheckedChanged="ckSch_CheckedChanged" /> 
                                </div>
                                <div class="span1"></div>
                                <asp:Panel ID="pnl_sch" runat="server" >    
                                    <div class="row-fluid">	
                                        <div class="box-content span12">  
                                            <div class="span12">                                            
                                                <div class="control-group">
                                                    Scheme
                                                    <div class="controls">
                                                        <asp:TextBox runat="server" class="span12" ID="TBSchm"  placeholder="Scheme..." ></asp:TextBox>
                                                    </div>
                                                </div>
                                            <%--</div>
                                            <div class="span12">--%>
                                                <div class="control-group">
                                                    Bonus
                                                    <div class="controls">
                                                        <asp:TextBox runat="server" class="span12" ID="TBBns" placeholder="Bonus..." ></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>                                                             
                                </asp:Panel>
                                <div  class="span2"></div>
                            </div>
                        </div>
                        <div class="row-fluid">	
                            <div class="span12">
                                <div class="span4">
                                    <asp:Button runat="server"  ID="btnSave" Text="Save" ValidationGroup="VGSal"  CssClass="btn btn-info"  OnClick="btnSave_Click" />   
                                    <asp:Button runat="server"  ID="btnRevert" Text="Cancel" CssClass="btn" OnClick="btnRevert_Click"  />       
                                </div>
                                <div class="span4">
                                    <asp:Label ID="lbl_saldat" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    <asp:Label ID="lbl_Cust" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    <asp:Label ID="lbl_book" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    <asp:Label ID="lbl_SalMan" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    <asp:Label ID="lbl_Gttl" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    <asp:Label ID="lbl_ttl" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    <asp:Label ID="lbl_Pros" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    <asp:Label ID="lbl_Chksalrat" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    <asp:Label ID="lbl_Itmqty" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    <asp:Label ID="lbl_ChkAmt" runat="server" Text="" ForeColor="Red"></asp:Label>
                                </div>                                                                  
                            </div>
                        </div>
                    </div>
                </div>
                <asp:HiddenField ID="HFMSal" runat="server" />
                <asp:HiddenField ID="HFSupAcc" runat="server" />                
            </div>
        </div>
            <asp:Panel ID="Panel1"  CssClass="modalPopup"  runat="server" HorizontalAlign="Center"  Style=" Width:495px; Height:600px; display: none;">
            <div class="modal" >
                <div class="modal-header">
                    <!--<button type="button" class="close" data-dismiss="modal">×</button>-->
                    <asp:Button ID="closebtn" Text="x"  CssClass="close" data-dismiss="modal" runat="server"   />
                    <h3>Import DSR</h3>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <table style="text-align:left; height:600px; width:500px;">
                                <tr>
                                    <td valign="top">
                                        <table>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="lblerr" runat="server" ForeColor="Red" ></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:HiddenField ID="HFDSRID" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                 <td>
                                                    Customer
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tb_cust" runat="server"  placeholder="-- Select Customer--" ></asp:TextBox>
                                                    <asp:TextBox ID="TB_CustNam" runat="server" Visible="false" Height="18px" EnableViewState="true" Width="142px"></asp:TextBox>                                                    
                                                    <asp:AutoCompleteExtender ServiceMethod="GetCust" CompletionListCssClass="completionList"
                                                    CompletionListItemCssClass="listItem" CompletionListHighlightedItemCssClass="itemHighlighted"
                                                    MinimumPrefixLength="1" CompletionInterval="10" EnableCaching="false" CompletionSetCount="10"
                                                    TargetControlID="tb_cust" ID="AutoCompleteExtender6"  
                                                    runat="server" FirstRowSelected="false"></asp:AutoCompleteExtender>
                                                </td>
                                                 <td>
                                                    Date
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TBDSRDat" runat="server" placeholder="Date.." Width="100" ></asp:TextBox>
                                                    <asp:CalendarExtender ID="Calendar1" CssClass="calender" PopupButtonID="imgPopup" runat="server" 
                                        TargetControlID="TBDSRDat" Format="yyyy-MM-dd"> </asp:CalendarExtender>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btn_searchdsr" runat="server"  CssClass="btn btn-primary" Text="Search" OnClick="btn_searchdsr_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:GridView ID="GV_DSR" CssClass="table table-striped table-bordered" DataKeyNames="Voucher" ShowHeaderWhenEmpty="true" ShowHeader="true" runat="server" PageSize="5" AutoGenerateColumns="false" OnRowCommand="GV_DSR_RowCommand" OnPageIndexChanging="GV_DSR_PageIndexChanging">
                                                        <Columns>
                                                            <asp:BoundField DataField="Voucher" HeaderText="Voucher" SortExpression="Voucher" />
                                                            <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" />
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkDSRSelect" CommandName="Select" runat="server" Text="Select" ></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </asp:Panel>

        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
        PopupControlID="Panel1" TargetControlID="btn_ImportDSR"
        CancelControlID="closebtn" BackgroundCssClass="modalBackground1">
        </asp:ModalPopupExtender>

             </ContentTemplate>
    </asp:UpdatePanel>
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
        <script src="Controller/Common.js"></script>

        <div class="span12" >
            <asp:Panel ID="pnlSO" runat="server">
                <div class="control-group">
                    Sale Order
                    <div class="controls">
                        <asp:DropDownList id="DDL_SO" runat="server" data-rel="chosen" CssClass="span12" ></asp:DropDownList>                                           
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div class="span12" style="visibility:hidden">
            <div class="control-group">                                            
                <div class="controls">
                    <asp:CheckBox ID="chk_SO" runat="server" Text="Sales Order" AutoPostBack="true" OnCheckedChanged="chk_SO_CheckedChanged" />
                </div>
            </div>
        </div>      
        <div class="span12">
            <div class="span6"></div>
            <div  class="span4"></div>
            <div  class="span2" style="visibility:hidden;">
                <h5><span class="break"></span>Total:</h5>
                <asp:TextBox runat="server" class="span12" ID="TBTotal" Text="0.00"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RFVTtl" ValidationGroup="VGSal" ForeColor="Red" runat="server" ErrorMessage="Total Can not be Null" ControlToValidate="TBTotal"></asp:RequiredFieldValidator>
            </div>
        </div>


<script>

    function PopupShown(sender, args) {
        sender._popupBehavior._element.style.zIndex = 99999999;
    }

</script>
                        
</asp:Content>
