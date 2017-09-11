<%@ Page Title="" MasterPageFile="~/Site.master" Language="C#" AutoEventWireup="true" CodeFile="CreateRFQ.aspx.cs" Inherits="Mgr_CreateRFQ" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <ul class="breadcrumb">
        <li><a href="SelectProject.aspx">Home</a></li>
        <li><a href="ProjectMenu.aspx">Project</a></li>
        <li><a href="RFQList.aspx">RFQ List</a></li>
         <li class="active">Create RFQ</li>
    </ul>
    <div id ="SelectProjectTitle" runAt="server" class="ContentHead">
        <h1>Create New RFQ</h1>
        <p><b>Project: </b><asp:Label runat="server" ID="ProjectName" /> </p>
        <p><b>Contract Number: </b><asp:Label runat="server" ID="ContractNumber" /></p> 
    </div>
    <asp:Panel runat="server" CssClass="jumbotron" DefaultButton="submitButton">
        <h2>RFQ Details</h2>
        <hr />
        <div class="form-group row">
            <label for="RFQDateTime" class="col-sm-2 col-form-label">RFQ Date</label>
            <div class="col-sm-10">
                <asp:TextBox  runat="server" TextMode="DateTimeLocal" ID="RFQDateTime"></asp:TextBox>
            </div>
        </div>
        <div class="form-group row">
            <div runat="server" id="rfqDtValidate" class="font-size-14px"></div>
        </div>
        <div class="form-group row">
            <label for="RFQVendorDateTime" class="col-sm-2 col-form-label">RFQ Vendor Date</label>
            <div class="col-sm-10">
                <asp:TextBox  runat="server" TextMode="DateTimeLocal" ID="VendorDateTime"></asp:TextBox>
            </div>
        </div>
        <div class="form-group row">
            <div runat="server" id="vendorDtValidate" class="font-size-14px"></div>
        </div>
        <div class="form-group row">
            <div class="col-sm-offset-2 col-xs-3 col-sm-2 col-md-1 col-lg-1">
                <button class="btn btn-default" runat="server" onserverclick="Cancel_Clicked">Cancel</button>
            </div>
            <div class="col-sm-2 col-md-1 col-xs-3 col-lg-1">
                <asp:Button Text="Next" ID="submitButton" runat="server" OnClick="Submit_Click" class="btn btn-primary" />
            </div>
        </div>
    </asp:Panel>

</asp:Content>