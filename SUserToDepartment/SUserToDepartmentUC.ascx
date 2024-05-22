<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SUserToDepartmentUC.ascx.cs" Inherits="WKF_OptionalFields_SUserToDepartmentUC" %>
<%@ Reference Control="~/WKF/FormManagement/VersionFieldUserControl/VersionFieldUC.ascx" %>
<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Ede" %>


<asp:Label ID="lblHasNoAuthority" runat="server" Text="無填寫權限" ForeColor="Red" Visible="False" meta:resourcekey="lblHasNoAuthorityResource1"></asp:Label>
<asp:Label ID="lblToolTipMsg" runat="server" Text="不允許修改(唯讀)" Visible="False" meta:resourcekey="lblToolTipMsgResource1"></asp:Label>
<asp:Label ID="lblModifier" runat="server" Visible="False" meta:resourcekey="lblModifierResource1"></asp:Label>
<asp:Label ID="lblMsgSigner" runat="server" Text="填寫者" Visible="False" meta:resourcekey="lblMsgSignerResource1"></asp:Label>
<asp:Label ID="lblAuthorityMsg" runat="server" Text="具填寫權限人員" Visible="False" meta:resourcekey="lblAuthorityMsgResource1"></asp:Label>

<%--<asp:Label ID="lbl_test" runat="server" Text="Label"></asp:Label>
<br />
<br />
<asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
<br />
<br />
<asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
<br />
<br />--%>

<asp:UpdatePanel ID="up_SUserToDepartment" runat="server">
    <ContentTemplate>
        <asp:HiddenField ID="hf_AllData" runat="server" />
        <asp:HiddenField ID="HiddenNAME" runat="server" />
        <asp:HiddenField ID="HiddenDataTable" runat="server" />
        <asp:HiddenField ID="HiddenReturn" runat="server" />



        <telerik:RadComboBox runat="server" ID="rcb_SUserToDepartment" AutoPostBack="true" OnItemsRequested="rcb_SUserToDepartment_ItemsRequested" CausesValidation="false"
            EnableLoadOnDemand="true" HighlightTemplatedItems="true" AppendDataBoundItems="true" OnSelectedIndexChanged="rcb_SUserToDepartment_SelectedIndexChanged"
            AllowCustomText="true" DropDownWidth="350px" Width="150px" BorderStyle="Dashed" Text='<%#Bind("NAME") %>'
            Height="250px" Filter="Contains" EmptyMessage="--- 請選擇 ---">
            <HeaderTemplate>
                <table style="width: 260px">
                    <tr>
                        <td style="width: 80px">員工編號
                        </td>
                        <td style="width: 180px">人員名稱
                        </td>
                    </tr>
                </table>
            </HeaderTemplate>
            <ItemTemplate>
                <table style="width: 260px">
                    <tr>
                        <td style="width: 80px;">
                            <%# DataBinder.Eval(Container, "Attributes['ACCOUNT']") %>
                        </td>
                        <td style="width: 180px;">
                            <%# DataBinder.Eval(Container,"Text") %>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </telerik:RadComboBox>
        <asp:TextBox ID="txt_Else" runat="server" Visible="false" placeholder="請輸入姓名"></asp:TextBox>
        <asp:Panel CssClass="pnlGrids" runat="server">
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
                    <asp:TemplateField HeaderText="上一層單位">
                        <ItemTemplate>
                            <%# Eval("團隊") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="上二層單位">
                        <ItemTemplate>
                            <%# Eval("部門") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </Ede:Grid>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>


<style type="text/css">
    .pnlGrids {
        display: flex;
        flex-direction: row;
    }
</style>
