﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DialogMasterPage.master" CodeFile="SearchDOC.aspx.cs" Inherits="CDS_ZENITHTEK_WKFFields_QUERYWINDOW_SearchDOC
    " %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .GridPager a, .GridPager span {
            display: block;
            height: 25px;
            width: 25px;
            font-weight: bold;
            text-align: center;
            text-decoration: none;
        }

        .GridPager a {
            background-color: #f5f5f5;
            color: #969696;
            border: 1px solid #969696;
        }

        .GridPager span {
            background-color: #A1DCF2;
            color: #ff0000;
            border: 1px solid #3AC0F2;
        }

         table .grid th {
            border: 1px solid black;
            height: 25px;
            background-color: #336699;
            color: white;
            padding: 5px;
        }

        table .grid tr td {
            border: 1px solid black;
            height: 25px;
            padding: 5px;
        }

        table .grid tr:last-child td {
            border: 1px solid black;
            height: 25px;
            padding: 5px;
        }

        table .grid tr:last-child table {
            border: 0px;
        }
    </style>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
             <label>查詢檔案：</label>
             <asp:TextBox ID="txtSearch" runat="server" AutoPostBack="true" OnTextChanged="txtSearch_TextChanged"></asp:TextBox>
            </div>
            <asp:GridView ID="gvMain" CssClass="grid" runat="server" Width="100%" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" ForeColor="#333333" AllowPaging="true" PageSize="10" OnPageIndexChanging="gvMain_PageIndexChanging">
                <PagerStyle CssClass="GridPager" />
                <AlternatingRowStyle BackColor="#ffffcc" />
                <Columns>
                    <asp:TemplateField HeaderText="選擇" HeaderStyle-Width="1%" HeaderStyle-Wrap="false" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnGet" runat="server" OnClick="lbtnGet_Click">選擇</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="文件名稱" HeaderStyle-Width="1%" HeaderStyle-Wrap="false" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblDOC_NAME" ViewType="ReadOnly" FieldName="DOC_NAME" Text='<%#Bind("DOC_NAME")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
