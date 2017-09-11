<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ViewJobBossMaterialData.aspx.cs" Inherits="Mgr_ViewJobBossData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
     <ul class="breadcrumb">
        <li><a href="SelectProject.aspx">Home</a></li>
        <li><a href="ProjectMenu.aspx">Project</a></li>
         <li><a href="PartsCatalog.aspx">Catalog</a></li>
         <li class="active">JobBoss Material Data</li>
    </ul>
        <div id ="SelectProjectTitle" runAt="server" class="ContentHead">
        <h1>JobBoss Material Data</h1>
            <p><b>Part Number:</b><asp:Label runat="server" ID="PartNumber" /> </p>
        <p><b>NSN:</b><asp:Label runat="server" ID="NSN" /></p> 
        </div>

     <asp:DetailsView ID="JobBossDetails" runat="server" DataKeyNames="Id" SelectMethod="JobBossDetails_GetItem" autoGenerateEditButton="false" AutoGenerateInsertButton="false"
        EnableViewState="false" AutoGenerateRows="false" ItemType="JBXML.Respond.JBXMLJBXMLRespondMaterialQueryRs" CssClass="table table-striped table-bordered table-hover" CellPadding="4">
     <Fields>
         
            <asp:BoundField DataField="ID" HeaderText="Material ID" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" SortExpression="UserName"  HeaderStyle-CssClass="info" />
            <asp:BoundField DataField="LastUpdated" HeaderText="Last Updated"  HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" HeaderStyle-CssClass="info" />
            <asp:BoundField DataField="MaterialType" HeaderText="Material Type"  HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" HeaderStyle-CssClass="info" />
         <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" HeaderStyle-CssClass="info" />
         <asp:BoundField DataField="VendorReferenceNbr" HeaderText="Vendor Reference Number" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" HeaderStyle-CssClass="info" />
         <asp:BoundField DataField="DefaultSource" HeaderText="Default Source" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" HeaderStyle-CssClass="info" />
         <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" HeaderStyle-CssClass="info" />
         <asp:TemplateField HeaderText="Standard Cost" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" HeaderStyle-CssClass="info">
             <ItemTemplate>
                 <asp:Label runat="server" Text='<%# String.Format("{0:C} {1}", Eval("StandardCost"), Eval("PriceUnit")) %>'></asp:Label>
             </ItemTemplate>
         </asp:TemplateField>
         <asp:TemplateField HeaderText="Average Cost" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" HeaderStyle-CssClass="info">
             <ItemTemplate>
                 <asp:Label runat="server" Text='<%# String.Format("{0:C} {1}", Eval("AverageCost"), Eval("PriceUnit")) %>'></asp:Label>
             </ItemTemplate>
         </asp:TemplateField>
         <asp:TemplateField HeaderText="Last Cost" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" HeaderStyle-CssClass="info">
             <ItemTemplate>
                 <asp:Label runat="server" Text='<%# String.Format("{0:C} {1}", Eval("LastCost"), Eval("PriceUnit")) %>'></asp:Label>
             </ItemTemplate>
         </asp:TemplateField>
         <asp:TemplateField HeaderText="Sell Price" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" HeaderStyle-CssClass="info">
             <ItemTemplate>
                 <asp:Label runat="server" Text='<%# String.Format("{0:C} {1}", Eval("SellPrice"), Eval("PriceUnit")) %>'></asp:Label>
             </ItemTemplate>
         </asp:TemplateField>
         <asp:BoundField DataField="LeadDays" HeaderText="Lead Days" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" HeaderStyle-CssClass="info" />
         <asp:BoundField DataField="ReorderPoint" HeaderText="Reorder Point" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" HeaderStyle-CssClass="info" />
         <asp:BoundField DataField="ReorderQty" HeaderText="Reorder Quantity" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" HeaderStyle-CssClass="info" />
         <asp:BoundField DataField="OnHandQty" HeaderText="On Hand Quantity" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" HeaderStyle-CssClass="info" />
         <asp:BoundField DataField="OnOrderQty" HeaderText="On Order Quantity" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" HeaderStyle-CssClass="info" />
         <asp:BoundField DataField="AllocatedQty" HeaderText="Allocated Quantity" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" HeaderStyle-CssClass="info" />
         <asp:BoundField DataField="InProductionQty" HeaderText="In Production Quantity" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" HeaderStyle-CssClass="info" />
         <asp:BoundField DataField="StockUnit" HeaderText="Stock Unit" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" HeaderStyle-CssClass="info" />
         <asp:BoundField DataField="PurchaseUnit" HeaderText="Purchase Unit" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" HeaderStyle-CssClass="info" />     
         <asp:BoundField DataField="CostUnit" HeaderText="Cost Unit" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" HeaderStyle-CssClass="info" />
         <asp:BoundField DataField="PriceUnit" HeaderText="Price Unit" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" HeaderStyle-CssClass="info" />                    
          <asp:BoundField DataField="ExtendedDescription" HeaderText="Extended Description" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" HeaderStyle-CssClass="info" /> 
               <asp:BoundField DataField="Notes" HeaderText="Notes" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" HeaderStyle-CssClass="info" /> 
            
         </Fields>


    </asp:DetailsView>
</asp:Content>

