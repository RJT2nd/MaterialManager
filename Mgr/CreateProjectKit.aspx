<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CreateProjectKit.aspx.cs" Inherits="Mgr_CreateProjectKit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <ul class="breadcrumb">
        <li><a href="SelectProject.aspx">Home</a></li>
        <li><a href="ProjectMenu.aspx">Project</a></li>
        <li><a href="ProjectKitList.aspx">Kit List</a></li>
         <li class="active">Create Kit</li>
    </ul>
    <div id ="SelectProjectTitle" runAt="server" class="ContentHead">
        <h1>Create New Kit</h1>
        <p><b>Project: </b><asp:Label runat="server" ID="ProjectName" /> </p>
        <p><b>Contract Number: </b><asp:Label runat="server" ID="ContractNumber" /></p> 
    </div>
    <asp:Panel runat="server" CssClass="jumbotron" DefaultButton="submitButton">
        <h2>Kit Details</h2>
        <hr />
        <div class="form-group row">
            <label for="projectKitDescription" class="col-sm-2 col-form-label">Description</label>
            <div class="col-sm-10">
                <asp:TextBox TextMode="MultiLine"  runat="server" ID="ProjectKitDescription"></asp:TextBox>
            </div>
        </div>
        <div class="form-group row">
            <div runat="server" id="projectKitDescriptionValidate" class="font-size-14px"></div>
        </div>
        <div class="form-group row">
            <div class="col-sm-offset-2 col-xs-3 col-sm-2 col-md-1 col-lg-1">
                <button class="btn btn-default"><a href="ProjectMenu.aspx">Cancel</a></button>
            </div>
            <div class="col-sm-2 col-md-1 col-xs-3 col-lg-1">
                <asp:Button Text="Next" ID="submitButton" runat="server" OnClick="Submit_Click" class="btn btn-primary" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>

