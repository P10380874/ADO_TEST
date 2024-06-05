<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DialogMasterPage.master" CodeFile="SearchFOLDER.aspx.cs" Inherits="CDS_PIC_WKFFields_QUERYWINDOW_SearchFOLDER" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <style>
        .Title {
            border-right: 1px solid #336699;
            padding: 9px;
            background-color: aliceblue;
            text-align: center;
            vertical-align: -webkit-baseline-middle;
        }

        .txt{
          margin: 5px;
        }

        .btn{
            margin: 5px;
            color: #fff;
            background-color: #5b98e6;
            border-color: #4d6584;
        }

    </style>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="border-bottom: 2px solid #336699;">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblTitle" Text="資料夾名稱：" CssClass="Title"></asp:Label>

                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtFOLDER_NAME" ReadOnly="true" CssClass="txt"></asp:TextBox>

                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnGet" class="btn" OnClick="btnGet_Click" Text="確定" />
                        </td>
                    </tr>
                    <tr>
                    </tr>
                </table>
                <asp:HiddenField runat="server" ID="hidFOLDER_ID" />
            </div>
            <telerik:RadTreeView runat="server" ID="trvDMS" OnNodeExpand="trvDMS_NodeExpand" OnNodeClick="trvDMS_NodeClick"></telerik:RadTreeView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
