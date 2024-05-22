<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserUpOneLevelUC.ascx.cs" Inherits="WKF_OptionalFields_UserUpOneLevelUC" %>
<%@ Reference Control="~/WKF/FormManagement/VersionFieldUserControl/VersionFieldUC.ascx" %>



<asp:label id="lblHasNoAuthority" runat="server" text="無填寫權限" forecolor="Red" visible="False" meta:resourcekey="lblHasNoAuthorityResource1"></asp:label>
<asp:label id="lblToolTipMsg" runat="server" text="不允許修改(唯讀)" visible="False" meta:resourcekey="lblToolTipMsgResource1"></asp:label>
<asp:label id="lblModifier" runat="server" visible="False" meta:resourcekey="lblModifierResource1"></asp:label>
<asp:label id="lblMsgSigner" runat="server" text="填寫者" visible="False" meta:resourcekey="lblMsgSignerResource1"></asp:label>
<asp:label id="lblAuthorityMsg" runat="server" text="具填寫權限人員" visible="False" meta:resourcekey="lblAuthorityMsgResource1"></asp:label>

<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Ede" %>

<asp:hiddenfield id="HiddenField1" runat="server" />
<ede:grid id="Grid2" runat="server" autogeneratecolumns="false" autogeneratecheckboxcolumn="false">
    <columns>
        <asp:templatefield headertext="申請者上一層單位">
            <itemtemplate>
                <%# Eval("團隊") %>
            </itemtemplate>
        </asp:templatefield>
    </columns>
</ede:grid>
