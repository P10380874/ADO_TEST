<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DialogMasterPage.master" AutoEventWireup="true" CodeFile="DialogSYS_F_M_Excel.aspx.cs" Inherits="CDS_PicOptionField_DialogSYS_F_M_Excel" %>

<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Ede" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

<%--    <asp:Label ID="lbl_test" runat="server" Text="Label"></asp:Label> <br /><br />--%>

    <asp:FileUpload ID="fileUploadExcel" runat="server" />
    <br />
    <br />
    <asp:Button ID="btn_readExcel" runat="server" Text="開始讀取資料" OnClick="btn_readExcel_Click" />
    <br />
    <asp:Label ID="lbl_note1" runat="server" Text="※提醒: 讀取資料後請按下方的「儲存資料並返回」按鈕才會新增至資料庫<br>※檢測結果為「錯誤」的資料將不會進入資料庫，請自行修改" Font-Bold="true"></asp:Label>
    <br />
    <asp:Label ID="lbl_note2" runat="server" Text="※依(系統代碼)為準，讀取到相同的資料將標記為「重複」，儲存時以最後一筆為主" Font-Bold="true"></asp:Label>
    <br />
    <asp:Label ID="lbl_note3" runat="server" Text="※如系統代碼或系統別未填，則顯示錯誤" Font-Bold="true"></asp:Label>
    <br />
    <asp:Label ID="lbl_note4" runat="server" Text="※如系統別已存在，顯示重複，且不會寫入" Font-Bold="true"></asp:Label>
    <br /> 
    <Ede:Grid ID="grid_SearchResult" runat="server" AutoGenerateColumns="false" AutoGenerateCheckBoxColumn="false">
        <Columns>
            <asp:BoundField DataField="excel_seq" HeaderText="Excel行數" />
            <asp:BoundField DataField="excel_validate" HeaderText="檢測結果" />
            <asp:BoundField DataField="DEPARTMENT" HeaderText="部門" />
            <asp:BoundField DataField="SYS_CATEGORY_ID" HeaderText="系統代碼" />
            <asp:BoundField DataField="SYS_CATEGORY_NAME" HeaderText="系統別" />
            <asp:BoundField DataField="UPDATE_USER" HeaderText="異動人" />
            <asp:BoundField DataField="UPDATE_DATE" HeaderText="異動日" DataFormatString="{0:yyyy/MM/dd}" />
            <asp:BoundField DataField="ENABLED" HeaderText="生效註記" />
        </Columns>
    </Ede:Grid>

    <asp:Label ID="lbl_data" runat="server" Text="" Visible="false"></asp:Label>

</asp:Content>

