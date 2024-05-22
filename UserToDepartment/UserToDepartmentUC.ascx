<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserToDepartmentUC.ascx.cs" Inherits="WKF_OptionalFields_UserToDepartmentUC" %>
<%@ Reference Control="~/WKF/FormManagement/VersionFieldUserControl/VersionFieldUC.ascx" %>
<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Ede" %>


<asp:Label ID="lblHasNoAuthority" runat="server" Text="無填寫權限" ForeColor="Red" Visible="False" meta:resourcekey="lblHasNoAuthorityResource1"></asp:Label>
<asp:Label ID="lblToolTipMsg" runat="server" Text="不允許修改(唯讀)" Visible="False" meta:resourcekey="lblToolTipMsgResource1"></asp:Label>
<asp:Label ID="lblModifier" runat="server" Visible="False" meta:resourcekey="lblModifierResource1"></asp:Label>
<asp:Label ID="lblMsgSigner" runat="server" Text="填寫者" Visible="False" meta:resourcekey="lblMsgSignerResource1"></asp:Label>
<asp:Label ID="lblAuthorityMsg" runat="server" Text="具填寫權限人員" Visible="False" meta:resourcekey="lblAuthorityMsgResource1"></asp:Label>

<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Ede" %>

<asp:Panel CssClass="pnlGrids" runat="server">
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <Ede:Grid ID="Grid1" runat="server" AutoGenerateColumns="false" AutoGenerateCheckBoxColumn="false">
        <Columns>
            <asp:TemplateField HeaderText="員工姓名">
                <ItemTemplate>
                    <%# Eval("Name") %>
                    <%--這邊直接綁資料庫的資料欄位名稱--%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="直屬單位">
                <ItemTemplate>
                    <%# Eval("專案") %>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </Ede:Grid>
    <Ede:Grid ID="Grid2" runat="server" AutoGenerateColumns="false" AutoGenerateCheckBoxColumn="false">
        <Columns>
            <asp:TemplateField HeaderText="上一層單位">
                <ItemTemplate>
                    <%# Eval("團隊") %>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </Ede:Grid>

    <Ede:Grid ID="Grid3" runat="server" AutoGenerateColumns="false" AutoGenerateCheckBoxColumn="false">
        <Columns>
            <asp:TemplateField HeaderText="上二層單位">
                <ItemTemplate>
                    <%# Eval("部門") %>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </Ede:Grid>
</asp:Panel>

<style type="text/css">
    .pnlGrids
        {   
       display: flex;
        flex-direction: row;

        }
</style>
