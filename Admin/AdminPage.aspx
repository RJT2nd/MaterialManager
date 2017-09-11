<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AdminPage.aspx.cs" Inherits="Admin_AdminPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server"> 
    <ul class="breadcrumb">
        <li><a href="/">Home</a></li>
        <li class="active">Admin</li>
    </ul>
    <h1>Administration</h1> 
    <hr /> 
    <ul class="nav nav-pills nav-stacked">
        <li><a href="UserAdmin.aspx">User Administration</a></li>
        <li><a href="ProjectAdmin.aspx">Project Administration</a></li>
    </ul>
    
    </asp:Content>