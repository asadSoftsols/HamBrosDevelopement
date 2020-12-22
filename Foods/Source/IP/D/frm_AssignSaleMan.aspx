<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="frm_AssignSaleMan.aspx.cs" Inherits="Foods.frm_AssignSaleMan" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">

         /* Modal Pop UP Start */

        .modalBackground{
                background-color: #000000;
                filter: alpha(opacity=10);
                opacity: 0.7;
        }
        /* Modal Pop UP End */
        .centerClosed {
            text-align:center;
        }
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
                    <a href="#">Set UP</a>
                    <i class="icon-angle-right"></i>
                </li>
                <li><a href="frm_AssignSaleMan.aspx">Assign Sales Man</a></li>
            </ul>
            <!-- imageLoader - START -->

            <img id='HiddenLoadingImage' src="../../../img/page-loader.gif" class="LoadingProgress" />

            <!-- imageLoader - END -->

             <asp:Panel ID="Panel_mssg" runat="server"  >
                <div class="row-fluid">	
                    <div class="box span12">
                        <div class="box-header" data-original-title>
                            <div class="centerClosed">    
                                <asp:Label ID="lbl_msg" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <div class="row-fluid">
                <div class="box  span12">
                     <div class="box-header" data-original-title>
                        <h2><i class="halflings-icon edit"></i><span class="break"></span> Assign Sales Man </h2>
                     </div>
                     <div class="box-content">
                         <div class="span6">
                             <div class="span12">&nbsp;</div>
                             <div class="span12">
                                <div class="control-group">
                                    <label class="control-label" for="DDL_Booker">Booker</label>
                                    <asp:DropDownList ID="DDL_Booker" runat="server" ValidationGroup="AssignSalman" AutoPostBack="True" OnSelectedIndexChanged="DDL_Booker_SelectedIndexChanged" ></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RFV_Booker" runat="server" InitialValue="0" ForeColor="Red" ControlToValidate="DDL_Booker" ValidationGroup="AssignSalman" ErrorMessage="Please Select Booker..."></asp:RequiredFieldValidator>
                                </div>
                            </div>
                             <div class="span12">
                                <div class="control-group">
                                    <label class="control-label" for="DDL_SalesMan">Sales Man</label>
                                    <asp:DropDownList ID="DDL_SalesMan" runat="server"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RFV_SalesMan" runat="server" InitialValue="0" ForeColor="Red" ControlToValidate="DDL_SalesMan" ValidationGroup="AssignSalman" ErrorMessage="Please Select Sales Man..."></asp:RequiredFieldValidator>

                                </div>
                            </div>
                             <div class="span12">
                                <div class="control-group">
                                    <label class="control-label" for="DDL_Assign">Assign</label>
                                    <asp:DropDownList ID="DDL_Assign" runat="server">
                                        <asp:ListItem Value="">-- Select--</asp:ListItem>
                                        <asp:ListItem Value="1">Assign</asp:ListItem>
                                        <asp:ListItem Value="0">Not Assign</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RFV_Assign" runat="server" InitialValue="" ForeColor="Red" ControlToValidate="DDL_Assign" ValidationGroup="AssignSalman" ErrorMessage="Please Select Assignment..."></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                         <div class="span6">
                             <div class="span12">&nbsp;</div>
                             <div class="span12">
                                 <asp:GridView ID="GV_Booker" ShowHeader="true" EmptyDataText="No Record" style="text-align:center;" ShowHeaderWhenEmpty="true" runat="server" class="table table-striped table-bordered" AutoGenerateColumns="False">
                                     <Columns>
                                         <asp:BoundField DataField="bookerid" HeaderText="Booker" SortExpression="bookerid" />
                                         <asp:BoundField DataField="salmanid" HeaderText="Sale Man" SortExpression="salmanid" />
                                         <asp:BoundField DataField="isAssign" HeaderText="Assign" SortExpression="isAssign" />
                                     </Columns>
                                 </asp:GridView>
                             </div>
                         </div>
                         <br />
                         <div class="span12">
                            <asp:Button ID="btn_SavSalMan" runat="server" CssClass="btn btn-info" ValidationGroup="AssignSalman"  Text="Assign Sales Man" OnClick="btn_SavSalMan_Click" />
                         </div>
                     </div>    
                </div>
            </div>
            <div style="width:100%; height:500px"></div> 
            <asp:HiddenField ID="HFBook" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <script src="../Controller/Common.js"></script>

    <script type="text/javascript">

        function DisplayLoadingImage() {

            if ($(".Search").val() == '') {
                alert('a');
                document.getElementById("HiddenLoadingImage").style.display = "none";

            } else {
                alert('b');
                document.getElementById("HiddenLoadingImage").style.display = "block";
                $("body").css({ opacity: 0.9 });
            }
        };

    </script>                            
</asp:Content>
