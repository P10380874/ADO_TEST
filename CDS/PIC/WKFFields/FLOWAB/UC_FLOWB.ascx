<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UC_FLOWB.ascx.cs" Inherits="WKF_OptionalFields_CDS_PIC_WKFFields_UC_FLOWB" %>
<%@ Reference Control="~/WKF/FormManagement/VersionFieldUserControl/VersionFieldUC.ascx" %>

<%--引用bootstrap --%>
<link href="<%=Page.ResolveUrl("~/CDS/bootstrap/css/bootstrap.min.css")%>" rel="stylesheet" />
<link href="<%=Page.ResolveUrl("~/CDS/KYTUtils/Assets/css/KYTI.css")%>" rel="stylesheet" />
<script src="<%=Page.ResolveUrl("~/CDS/bootstrap/js/popper.min.js")%>"></script>
<script src="<%=Page.ResolveUrl("~/CDS/bootstrap/js/bootstrap.min.js")%>"></script>


<style>

    /* Custom */

    /* 表格置中及換標題顏色 */
    table.tsGridView2 > tbody > tr > th {
        background-color: #87ceeb;
    }
	table.tsGridView2 > tbody > tr > th,
    table.tsGridView2 > tbody > tr > td {
        text-align: center;
    }

    /* Custom End */
</style>


<script>


</script>


<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-4">
                    <asp:GridView runat="server" ID="gvForms" CssClass="tsGridView2 horzFull" AutoGenerateColumns="false" ShowHeader="true" ShowHeaderWhenEmpty="true" 
                        OnRowDataBound="gvForms_RowDataBound">
                        <EmptyDataRowStyle HorizontalAlign="Center" />
                        <EmptyDataTemplate>
                            <asp:Label ID="Label00" runat="server" ForeColor="Red" Font-Bold="true" Text="目前沒有明細資料"></asp:Label>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="表單編號" ItemStyle-Width="15%" ItemStyle-Wrap="true">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnFLOWA_NO" runat="server" Width="120px" Text='<%#Bind("FLOWA_NO")%>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="表單狀態" ItemStyle-Width="25%" ItemStyle-Wrap="true">
                                <ItemTemplate>
                                    <!-- 協辦單簽核狀況，簽核中/結案核准/結案否決 -->
                                    <asp:Label ID="lblFLOWA_STATUS" runat="server" Width="80px" Text='<%#Bind("FLOWA_STATUS")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

        </div>
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
