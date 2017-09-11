<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ProjectMenu.aspx.cs" Inherits="Mgr_ProjectMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <ul class="breadcrumb">
        <li><a href="SelectProject.aspx">Home</a></li>
        <li class="active">Project</li>
    </ul>
    <div id="ProjectTitle" runat="server" class="ContentHead">
        <h1>
            <asp:Label runat="server" ID="ProjectName" />
        </h1>
        <p><b>Contract Number:</b><asp:Label runat="server" ID="ContractNumber" /></p>
        <p>
            <b>Funding Allocated:</b><asp:Label runat="server" ID="Funding" /><br />
            <b>Funds Expended:</b><asp:Label runat="server" ID="Expended" /><br />
            <b>Funds Remaining:</b><asp:Label runat="server" ID="Remaining" />
        </p>

    </div>

    <div class="row">
        <div class="col-lg-4">
            <div class="bs-component">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title">Quotes</h3>
                    </div>
                    <div class="panel-body">
                        <ul>
                            <li><a href="CreateRFQ.aspx">Create New RFQ</a></li>
                            <li><a href="RFQList.aspx">Review Quotes</a> </li>
                        </ul>
                    </div>
                </div>

            </div>
        </div>
        <div class="col-lg-4">
            <div class="bs-component">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title">Purchase Orders</h3>
                    </div>
                    <div class="panel-body">
                        <ul>
                            <li><a href="CreatePO.aspx">Create New PO</a></li>
                            <li><a href="POList.aspx">Review Purchase Orders</a></li>

                        </ul>
                    </div>
                </div>

            </div>
        </div>
        <div class="col-lg-4">
            <div class="bs-component">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title">Parts</h3>
                    </div>
                    <div class="panel-body">
                        <ul>
                            <li><a href="CreateProjectKit.aspx">Create New Kit</a></li>
                            <li><a href="ProjectKitList.aspx">Review Kits</a></li>
                            <li><a href="PartsCatalog.aspx">Parts Catalog</a></li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <h5><a href="SelectProject.aspx">Change Project</a></h5>
</asp:Content>

