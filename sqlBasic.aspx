<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="sqlBasic.aspx.cs" Inherits="sqlBasic" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ListView ID="ListView1" runat="server">

    
    
    <LayoutTemplate>
     <table border="1" cellpadding="1">
      <tr style="background-color:#E5E5FE">
       <th align="left"><asp:LinkButton ID="lnkCommand" runat="server">Command</asp:LinkButton></th>
       <th align="left"><asp:LinkButton ID="lnkDescriptions"  runat="server">Descriptions</asp:LinkButton></th>
       <th align="left"><asp:LinkButton ID="lnkExamples" runat="server">Examples</asp:LinkButton></th>
       <th></th>
      </tr>
      <tr id="itemPlaceholder" runat="server"></tr>
     </table>
    </LayoutTemplate>
    <ItemTemplate>
      <tr>
       <td><asp:Label runat="server" ID="lblId"><%#Eval("Command")%></asp:Label></td>
       <td><asp:Label runat="server" ID="lblName"><%#Eval("Descriptions")%></asp:Label></td>
       <td>

         <asp:ListView runat="server" ID="subMenu" ItemPlaceholderID="PlaceHolder2"  DataSource='<%# Eval("Examples") %>'>
          <LayoutTemplate>
              <asp:PlaceHolder runat="server" ID="PlaceHolder2" /> 
           </LayoutTemplate>
          <ItemTemplate>
              <li><asp:Label runat="server" ID="lbl_example"><%#Eval("example")%></asp:Label></li>
            </ItemTemplate>
        </asp:ListView>

       </td>
      </tr>
    </ItemTemplate>


    </asp:ListView>
</asp:Content>

