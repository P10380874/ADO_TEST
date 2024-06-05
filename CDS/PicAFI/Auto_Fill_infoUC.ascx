<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Auto_Fill_infoUC.ascx.cs" Inherits="WKF_OptionalFields_Auto_Fill_infoUC" %>
<%@ Reference Control="~/WKF/FormManagement/VersionFieldUserControl/VersionFieldUC.ascx" %>
<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Ede" %>

<asp:Label ID="lblHasNoAuthority" runat="server" Text="無填寫權限" ForeColor="Red" Visible="False" meta:resourcekey="lblHasNoAuthorityResource1"></asp:Label>
<asp:Label ID="lblToolTipMsg" runat="server" Text="不允許修改(唯讀)" Visible="False" meta:resourcekey="lblToolTipMsgResource1"></asp:Label>
<asp:Label ID="lblModifier" runat="server" Visible="False" meta:resourcekey="lblModifierResource1"></asp:Label>
<asp:Label ID="lblMsgSigner" runat="server" Text="填寫者" Visible="False" meta:resourcekey="lblMsgSignerResource1"></asp:Label>
<asp:Label ID="lblAuthorityMsg" runat="server" Text="具填寫權限人員" Visible="False" meta:resourcekey="lblAuthorityMsgResource1"></asp:Label>
  <%--防止ID因為送單跑掉用HiddenField1--%>
   <asp:HiddenField ID="HiddenField1" runat="server" />
<Ede:Grid ID="Grid1" runat="server" AutoGenerateColumns="false" AutoGenerateCheckBoxColumn="false">
    <Columns>
        <asp:TemplateField HeaderText="姓名">
            <ItemTemplate>
                <%# Eval("姓名") %>
                <%--這邊直接綁資料庫的資料欄位名稱--%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="員編">
            <ItemTemplate>
                <%# Eval("員編") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="職級">
            <ItemTemplate>
                <%# Eval("職級") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="直屬單位">
            <ItemTemplate>
                <%# Eval("直屬單位") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="上一層單位">
            <ItemTemplate>
                <%# Eval("上一層單位") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="上二層單位">
            <ItemTemplate>
                <%# Eval("上二層單位") %>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</Ede:Grid>