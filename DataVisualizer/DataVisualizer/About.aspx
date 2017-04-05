<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="DataVisualizer.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>This web application displays database information in a sorted, graphical format. The information displayed is product sales over time, for a selected product at a selected store. Data is retrieved from the Foodmart database.</h3>
    <p>Created by <a href="Contact.aspx">David Pitters</a></p>
</asp:Content>
