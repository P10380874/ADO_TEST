<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="System_File_Maintenance.aspx.cs" Inherits="CDS_Grid_System_File_Maintenance" %>

<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Ede" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

   <%--<asp:Label ID="lbl_test" runat="server" Text="Label"></asp:Label> <br /><br />--%>

    <asp:Label ID="Label2" runat="server" Text="※注意刪除僅用於建檔時，如系統別已生效或曾經生效請勿刪除。" Font-Bold="true"></asp:Label>
    <br>
    <asp:Label ID="Label3" runat="server" Text="※欄位支援模糊查詢，請直接輸入關鍵字搜尋部門、系統代碼、系統別、異動人、異動日等" Font-Bold="true"></asp:Label>
    <br>
    <table class="PopTable">
        <tr>
            <td>關鍵字</td>
            <td>
                <asp:TextBox ID="txtKeyword" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>是否生效</td>
            <td>
                <asp:DropDownList ID="txt_IsENABLED" runat="server">
                    <asp:ListItem Text="--不設定條件--" Value=""></asp:ListItem>
                    <asp:ListItem Text="生效" Value="True"></asp:ListItem>
                    <asp:ListItem Text="無效" Value="False"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Button ID="btnQuery" runat="server" Text="查詢" OnClick="btnQuery_Click" />
                <asp:Button ID="btn_AddDetail" runat="server" Text="新增" CssClass="btn-margin" OnClick="btn_AddDetail_Click" />
                <asp:Button ID="btn_DeleteMutipleDetail" runat="server" Text="刪除" CssClass="btn-margin" OnClick="btn_DeleteMutipleDetail_Click" />
                <asp:Button ID="btn_ReadExcel" runat="server" Text="匯入檔案" CssClass="btn-margin" OnClick="btn_ReadExcel_Click" />
               <%--test--%>
                <asp:Button ID="btnExportExcel" runat="server" Text="匯出Excel" OnClick="btnExportExcel_Click" />
               
            </td>
        </tr>
    </table>

    <Ede:Grid ID="Grid1" runat="server" AutoGenerateColumns="false" OnRowDataBound="Grid1_RowDataBound" DataKeyNames="SYS_CATEGORY_ID"
    AllowPaging="true" PageSize="20" OnPageIndexChanging="grid_SearchResult_PageIndexChanging"
    OnRowEditing="grid_SearchResult_RowEditing" OnRowCancelingEdit="grid_SearchResult_RowCancelingEdit"
    OnRowUpdating="grid_SearchResult_RowUpdating" OnBeforeExport="Grid1_BeforeExport" AutoGenerateCheckBox="false">
    <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource" DataSourceType="DataTable" ExcelExportFileName="系統別資料檔.xls" />

    <%--test--%>
   <%-- <Ede:Grid ID="Grid1" runat="server" AutoGenerateColumns="false" OnRowDataBound="Grid1_RowDataBound" DataKeyNames="SYS_CATEGORY_ID"
        AllowPaging="true" PageSize="20" OnPageIndexChanging="grid_SearchResult_PageIndexChanging"
        OnRowEditing="grid_SearchResult_RowEditing" OnRowCancelingEdit="grid_SearchResult_RowCancelingEdit"
        OnRowUpdating="grid_SearchResult_RowUpdating">--%>
       

        <Columns>
            <asp:TemplateField HeaderText="部門">
                <ItemTemplate>
                    <%# Eval("DEPARTMENT") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txt_DEPARTMENT" runat="server" Text='<%#Bind("DEPARTMENT") %>' Width="80px" MaxLength="8"></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>


            <asp:TemplateField HeaderText="系統代碼">
                <ItemTemplate>
                    <%# Eval("SYS_CATEGORY_ID") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txt_SYS_CATEGORY_ID" runat="server" Text='<%#Bind("SYS_CATEGORY_ID") %>' Width="80px"></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>


            <asp:TemplateField HeaderText="系統別">
                <ItemTemplate>
                    <%# Eval("SYS_CATEGORY_NAME") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txt_SYS_CATEGORY_NAME" runat="server" Text='<%#Bind("SYS_CATEGORY_NAME") %>' Width="300px"></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>


            <asp:TemplateField HeaderText="異動人">
                <ItemTemplate>
                    <%# Eval("UPDATE_USER") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:Label ID="txt_CREATE_USER" runat="server" Text='<%#Bind("UPDATE_USER") %>' Width="300px"></asp:Label>
                </EditItemTemplate>
            </asp:TemplateField>


            <asp:TemplateField HeaderText="異動日">
                <ItemTemplate>
                    <%# Eval("UPDATE_DATE", "{0:yyyy/MM/dd}") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:Label ID="txt_CREATE_DATE" runat="server" Text='<%# Eval("UPDATE_DATE", "{0:yyyy/MM/dd}") %>'></asp:Label>

                </EditItemTemplate>
            </asp:TemplateField>


            <asp:TemplateField HeaderText="生效註記">
                <ItemTemplate>
                    <asp:CheckBox ID="CheckBox_ENABLED" runat="server" Checked='<%# IsPrintButtonChecked(Eval("ENABLED")) %>' />
                </ItemTemplate>
            </asp:TemplateField>


            <asp:TemplateField HeaderText="功能按鈕" HeaderStyle-Width="80px">
                <ItemTemplate>
                    <asp:LinkButton ID="btn_GridEdit" runat="server" Text="編輯" ForeColor="DarkBlue" BorderWidth="2" BackColor="White" CommandName="Edit"></asp:LinkButton>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:LinkButton ID="btn_GridUpdate" runat="server" Text="更新" CommandName="Update" BorderWidth="2" BackColor="White" ForeColor="DarkGreen" />
                    <asp:LinkButton ID="btn_GridCancel" runat="server" Text="取消" CommandName="Cancel" BorderWidth="2" BackColor="White" />
                </EditItemTemplate>
            </asp:TemplateField>
        </Columns>
    </Ede:Grid>
    <asp:CheckBox ID="ckb_IsNewGridLine" runat="server" Visible="false" />

    <script type="text/javascript">
        function Confirm() {
            if (confirm("確定要刪除這些資料嗎?")) {
                $("#<%= btn_DeleteMutipleDetail.ClientID%>").click();
            } else {
            }
        }
    </script>
</asp:Content>

