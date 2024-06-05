<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SysCategoryUC.ascx.cs" Inherits="WKF_OptionalFields_SysCategoryUC" %>
<%@ Reference Control="~/WKF/FormManagement/VersionFieldUserControl/VersionFieldUC.ascx" %>



<asp:Label ID="lblHasNoAuthority" runat="server" Text="無填寫權限" ForeColor="Red" Visible="False" meta:resourcekey="lblHasNoAuthorityResource1"></asp:Label>
<asp:Label ID="lblToolTipMsg" runat="server" Text="不允許修改(唯讀)" Visible="False" meta:resourcekey="lblToolTipMsgResource1"></asp:Label>
<asp:Label ID="lblModifier" runat="server" Visible="False" meta:resourcekey="lblModifierResource1"></asp:Label>
<asp:Label ID="lblMsgSigner" runat="server" Text="填寫者" Visible="False" meta:resourcekey="lblMsgSignerResource1"></asp:Label>
<asp:Label ID="lblAuthorityMsg" runat="server" Text="具填寫權限人員" Visible="False" meta:resourcekey="lblAuthorityMsgResource1"></asp:Label>

<%--<asp:Label ID="Label1" runat="server" Text="可輸入關鍵字過濾資料，若查詢不到需要的選項，請到最下方選擇「其他」並自行輸入"></asp:Label>
<br />--%>

<asp:UpdatePanel ID="up_SysCategory" runat="server">
    <ContentTemplate>

        <asp:HiddenField ID="hf_AllData" runat="server" />

        <telerik:RadComboBox runat="server" ID="rcb_SysCategory" AutoPostBack="true" OnItemsRequested="rcb_SysCategory_ItemsRequested" CausesValidation="false"
            EnableLoadOnDemand="true" HighlightTemplatedItems="true" AppendDataBoundItems="true" OnSelectedIndexChanged="rcb_SysCategory_SelectedIndexChanged"
            AllowCustomText="true" DropDownWidth="350px" Width="150px" BorderStyle="Dashed" Text='<%#Bind("SYS_CATEGORY_NAME") %>'
            Height="250px" Filter="Contains" EmptyMessage="--- 請選擇 ---">
            <HeaderTemplate>
                <table style="width: 260px">
                    <tr>
                        <td style="width: 80px">部門
                        </td>
                        <td style="width: 180px">名稱
                        </td>
                    </tr>
                </table>
            </HeaderTemplate>
            <ItemTemplate>
                <table style="width: 260px">
                    <tr>
                        <td style="width: 80px;">
                            <%# DataBinder.Eval(Container, "Attributes['DEPARTMENT']") %>
                        </td>
                        <td style="width: 180px;">
                            <%# DataBinder.Eval(Container,"Text") %>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </telerik:RadComboBox>

        <asp:TextBox ID="txt_Else" runat="server" Visible="false" placeholder="請自行輸入系統別"></asp:TextBox>

    </ContentTemplate>
</asp:UpdatePanel>


