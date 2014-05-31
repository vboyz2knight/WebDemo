<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Respiratory.aspx.cs" Inherits="Respiratory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<table style="width:100%;">
    <tr>
        <td>
            <div style="float: left; overflow: hidden">
            <asp:TreeView ID="TreeView1" runat="server" DataSourceID="SiteMapDataSource1" ExpandDepth="1"> </asp:TreeView>
            <asp:SiteMapDataSource ID="SiteMapDataSource1" runat="server" SiteMapProvider="RespiratorySitemapProvider"  />
            </div>>
        </td>
        <td>
            <div style="float: right; overflow: hidden">
            <asp:UpdatePanel ID="RespiratoryCalc" runat="server">
                <ContentTemplate>              
                    <fieldset>            
                        <asp:PlaceHolder ID="PlaceHolderEquation" runat="server"></asp:PlaceHolder>            
                    
                    </fieldset>
                    <br /> &nbsp;<asp:Label ID="lblError" runat="server"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
            </div>
        </td>
    </tr>
</table>
    

    

</asp:Content>

