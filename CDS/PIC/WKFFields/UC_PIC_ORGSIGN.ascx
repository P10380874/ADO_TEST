<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UC_PIC_ORGSIGN.ascx.cs" Inherits="WKF_OptionalFields_UC_PIC_ORGSIGN" %>
<%@ Reference Control="~/WKF/FormManagement/VersionFieldUserControl/VersionFieldUC.ascx" %>



<style>

    /* Custom */


    /* Custom End */
</style>


<script>


</script>


<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="container-fluid">
            <!--container-fluid-->

            <div class="row">
                <!--當下簽核人之主管-->
                <asp:HiddenField runat="server" ID="hidORGSIGN" />
            </div>
            </br>
            <div class="row">
                <asp:Label runat="server" ID="lblMSG" ForeColor="Red"></asp:Label>
            </div>
            <div class="row">
                <div class="col-md-8">
                    <asp:Label runat="server" ID="lblMessage" ForeColor="Red"></asp:Label>
                </div>
            </div>

        </div>
        <!--container-fluid-->
    </ContentTemplate>
</asp:UpdatePanel>

<asp:LinkButton ID="lnk_Edit" runat="server" OnClick="lnk_Edit_Click" Visible="False" CausesValidation="False" meta:resourcekey="lnk_EditResource1">[修改]</asp:LinkButton>
<asp:LinkButton ID="lnk_Cannel" runat="server" OnClick="lnk_Cannel_Click" Visible="False" CausesValidation="False" meta:resourcekey="lnk_CannelResource1">[取消]</asp:LinkButton>
<asp:LinkButton ID="lnk_Submit" runat="server" OnClick="lnk_Submit_Click" Visible="False" CausesValidation="False" meta:resourcekey="lnk_SubmitResource1">[確定]</asp:LinkButton>
<asp:Label ID="lblHasNoAuthority" runat="server" Text="無填寫權限" ForeColor="Red" Visible="False" meta:resourcekey="lblHasNoAuthorityResource1"></asp:Label>
<asp:Label ID="lblToolTipMsg" runat="server" Text="不允許修改(唯讀)" Visible="False" meta:resourcekey="lblToolTipMsgResource1"></asp:Label>
<asp:Label ID="lblModifier" runat="server" Visible="False" meta:resourcekey="lblModifierResource1"></asp:Label>
<asp:Label ID="lblMsgSigner" runat="server" Text="填寫者" Visible="False" meta:resourcekey="lblMsgSignerResource1"></asp:Label>
<asp:Label ID="lblAuthorityMsg" runat="server" Text="具填寫權限人員" Visible="False" meta:resourcekey="lblAuthorityMsgResource1"></asp:Label>
