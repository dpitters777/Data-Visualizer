<%@ Page Title="Data Visualizer" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DataVisualizer.aspx.cs" Inherits="DataVisualizer._Default" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron" id="mainSpace" runat="server" style="height: 967px;">
        <div id="leftside" style="float: left; width: 100%;">
            <h1>Foodmart Data Visualizer</h1>
            <p class="lead">Select a category, product, and store.</p>
            <p>Product Category: </p><asp:DropDownList ID="CategoryDropdown" runat="server" AutoPostBack="True" OnSelectedIndexChanged="CategoryDropdown_SelectedIndexChanged" AppendDataBoundItems="True">
            <asp:ListItem>--Select Category--</asp:ListItem>
            <asp:ListItem>All</asp:ListItem>
            </asp:DropDownList>
            <br />
            <p>Product: </p><asp:DropDownList ID="ProductDropdown" runat="server" AutoPostBack="True" Enabled="False" OnSelectedIndexChanged="ProductDropdown_SelectedIndexChanged">
            <asp:ListItem>--Select Product--</asp:ListItem>
            </asp:DropDownList>
            <br />
            <p>Store: </p><asp:DropDownList ID="StoreDropdown" runat="server" AutoPostBack="True" Enabled="False" AppendDataBoundItems="True">
            <asp:ListItem>--Select Store--</asp:ListItem>
            </asp:DropDownList>
            <br /><br />
            <asp:Button runat="server" class="btn btn-primary btn-lg" Text="Refresh Data" ID="refreshBtn" OnClick="refreshBtn_Click"></asp:Button>
        </div>
        <div id="noResultsMessage" runat="server">
            <p>No sales found for the selected product at the selected store. Please perform a new search.</p>
        </div>
        <div id="rightside" style="position: relative; float: left; top: 33px; left: 20px; width: 96%;">
            <asp:Chart ID="dataChart" runat="server" Height="450px" Visible="False" Width="941px">
            <series>
                <asp:Series Name="Series1">
                </asp:Series>
            </series>
            <chartareas>
                <asp:ChartArea Name="ChartArea1">
                </asp:ChartArea>
            </chartareas>
        </asp:Chart>
        </div>
    </div>
</asp:Content>
