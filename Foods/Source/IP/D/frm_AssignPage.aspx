<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="frm_AssignPage.aspx.cs" Inherits="Foods.frm_AssignPage" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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
                    <a href="#">SetUP</a>
                    <i class="icon-angle-right"></i>
                </li>
                <li><a href="frm_AssignPage.aspx">Assign Pages</a></li>
            </ul>
            <!-- imageLoader - START -->

            <img id='HiddenLoadingImage' src="../../img/page-loader.gif" class="LoadingProgress" />

            <!-- imageLoader - END -->

            <div class="row-fluid">
                <div class="span12">&nbsp;</div>
                <div class="span12">
                    <div class="control-group">
                        <label class="control-label" for="DDL_User">User</label>
                        <asp:DropDownList ID="DDL_User" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDL_User_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="row-fluid">
                <div class="box  span12">
                     <div class="box-header" data-original-title>
                        <h2><i class="halflings-icon edit"></i><span class="break"></span> Assign Modules </h2>
                     </div>
                     <div class="box-content">
                         <div class="span12">&nbsp;</div>
                         <div class="span12">
                            <div class="control-group">
                                <label class="control-label" for="DDL_Mod">Modules</label>
                                <asp:DropDownList ID="DDL_Mod" runat="server"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="span12">
                            <div class="control-group">
                                <label class="control-label" for="DDL_Mod">Assign</label>
                                <asp:DropDownList ID="DDL_Assign" runat="server">
                                    <asp:ListItem>-- Select--</asp:ListItem>
                                    <asp:ListItem Value="1">Assign</asp:ListItem>
                                    <asp:ListItem Value="0">Not Assign</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="span12">
                            <asp:Button ID="btn_Savmod" runat="server" CssClass="btn btn-info"  Text="Save Modules" OnClick="btn_Savmod_Click" />
                        </div>
                         <br />
                        <div class="span12">
                            <asp:GridView ID="GVMod"  class="table table-striped table-bordered" runat="server" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:BoundField DataField="MenuId" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="MenuId" />
                                    <asp:BoundField DataField="Title" HeaderText="Module Name" SortExpression="Title" />
                                </Columns>
                            </asp:GridView>
                        </div>
                     </div>    
                </div>
            </div>
            <div class="row-fluid">
                <div class="box  span12">
                     <div class="box-header" data-original-title>
                        <h2><i class="halflings-icon edit"></i><span class="break"></span> Assign Pages </h2>
                     </div>
                     <div class="box-content">
                         <div class="span12">&nbsp;</div>
                         <div class="span12">
                            <div class="control-group">
                                <label class="control-label" for="DDL_Modules">Modules</label>
                                <asp:DropDownList ID="DDL_Modules" runat="server"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="span12">
                            <div class="control-group">
                                <label class="control-label" for="DDL_Pages">Pages</label>
                                <asp:DropDownList ID="DDL_Pages" runat="server"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="span12">
                            <div class="control-group">
                                <label class="control-label" for="DDL_Assign2">Assign</label>
                                <asp:DropDownList ID="DDL_Assign2" runat="server">
                                    <asp:ListItem>-- Select--</asp:ListItem>
                                    <asp:ListItem Value="1">Assign</asp:ListItem>
                                    <asp:ListItem Value="0">Not Assign</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="span12">
                            <asp:Button ID="btn_SavPag" runat="server" CssClass="btn btn-info"  Text="Save Pages" OnClick="btn_SavPag_Click" />
                        </div>
                         <br />
                          <div class="span12">
                            <asp:GridView ID="GVSubMenu"  class="table table-striped table-bordered" runat="server" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:BoundField DataField="SubMenuNam" HeaderText="Page Name" SortExpression="SubMenuNam" />
                                    <asp:BoundField DataField="SubMenuDesc" HeaderText="Description" SortExpression="SubMenuDesc" />
                                </Columns>
                            </asp:GridView>
                        </div>
                     </div>    
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>    
</asp:Content>
