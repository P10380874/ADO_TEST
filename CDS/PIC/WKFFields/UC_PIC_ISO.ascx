<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UC_PIC_ISO.ascx.cs" Inherits="WKF_OptionalFields_UC_PIC_ISO" %>
<%@ Reference Control="~/WKF/FormManagement/VersionFieldUserControl/VersionFieldUC.ascx" %>
<%@ Register Src="~/KYTControl/KYTCheckBox.ascx" TagPrefix="uc1" TagName="KYTCheckBox" %>
<%@ Register Src="~/KYTControl/KYTCheckBoxList.ascx" TagPrefix="uc1" TagName="KYTCheckBoxList" %>
<%@ Register Src="~/KYTControl/KYTDatePicker.ascx" TagPrefix="uc1" TagName="KYTDatePicker" %>
<%@ Register Src="~/KYTControl/KYTDateTimePicker.ascx" TagPrefix="uc1" TagName="KYTDateTimePicker" %>
<%@ Register Src="~/KYTControl/KYTDropDownList.ascx" TagPrefix="uc1" TagName="KYTDropDownList" %>
<%@ Register Src="~/KYTControl/KYTTextBox.ascx" TagPrefix="uc1" TagName="KYTTextBox" %>
<%@ Register Src="~/KYTControl/KYTTimePicker.ascx" TagPrefix="uc1" TagName="KYTTimePicker" %>
<%@ Register Src="~/KYTControl/KYTGridView.ascx" TagPrefix="uc1" TagName="KYTGridView" %>
<%@ Register Src="~/KYTControl/KYTRadioButtonList.ascx" TagPrefix="uc1" TagName="KYTRadioButtonList" %>

<link href="<%=Page.ResolveUrl("~/KYTControl/css/gemps.ui.css")%>" rel="stylesheet" />
<link href="<%=Page.ResolveUrl("~/KYTControl/css/font-awesome.min.css")%>" rel="stylesheet" />
<script src="<%=Page.ResolveUrl("~/KYTControl/js/gemps.ui.js")%>"></script>
<%--<link href="<%=Page.ResolveUrl("~/CDS/PIC/Assets/css/KYTI.css")%>" rel="stylesheet" />--%>
<link href="<%=Page.ResolveUrl("~/CDS/KYTUtils/Assets/css/KYTI.css")%>" rel="stylesheet" />

<!--引用bootstrap -->
<link href="<%=Page.ResolveUrl("~/CDS/bootstrap/css/bootstrap.min.css")%>" rel="stylesheet" />
<script src="<%=Page.ResolveUrl("~/CDS/bootstrap/js/popper.min.js")%>"></script>
<script src="<%=Page.ResolveUrl("~/CDS/bootstrap/js/bootstrap.min.js")%>"></script>


<style>
    table.inline > tbody > tr {
        display: inline;
        padding-right: 10px;
    }

    
    /* 特別針對小視窗修正 */
    div.row > div.col-md-3 {
        max-width: 39vw !important;
    }
        
    div.row > div.col-md-3 > input[type=text] {
        max-width: 100%;
    }

    /* 讓需要輸入的欄位不要分行 */
    div.row > div {
        display: inline-flex;
        align-items: center;
    }

</style>

<script>
    // 必填欄位不可為空白
    function checkMustInput(sender, args) {
        if ($("input[id*='krdobtnACTION_RadioButtonList1_0']").length <= 0) return;
        var FalseCount = 0;
        if ($("input[id*='krdobtnACTION_RadioButtonList1_0']").prop("checked")) { // 新增
            var items_A = $("input.ACTION_A");
            for (var i = 0; i < items_A.length; i++) {
                if (!items_A[i].value) {
                    items_A[i].style.setProperty("background-color", "#e6aeae", "important");
                    FalseCount += 1;
                }
            }
        }
        else if ($("input[id*='krdobtnACTION_RadioButtonList1_1']").prop("checked")) { // 修改
            var items_B = $("input.ACTION_B");
            debugger;
            var span_B = $("span[id*='ktxtSDOC_NAME']");
            for (var i = 0; i < items_B.length; i++) {
                if (!items_B[i].value) {
                    items_B[i].style.setProperty("background-color", "#e6aeae", "important");
                    FalseCount += 1;
                }
            }
            //for (var i = 0; i < span_B.length; i++) {
            //    if (!span_B[i].textContent) {
            //        span_B[i].style.setProperty("background-color", "#e6aeae", "important");
            //        FalseCount += 1;
            //    }
            //}
        }
        if (FalseCount > 0) {
            args.IsValid = false;
        }

    }

    /**
     * 檢查是否有上傳檔案
     * @param args
     */
    function checkFileUpload(sender, args) {
        //var file = $("[id*='FileCenter_RadListView1_itemContainer']")
        var file = $(".FieldHide")
        for (var i = 0; i < file.length; i++) {
            var sub = $(file[i]);
            if (file[i].innerText == "(isoFile)") {
                var id = sub.parent().parent().parent().parent().parent().parent().parent().find("[id*='FileCenter_RadListView1_itemContainer']")

                if (id.children().length == 0) {
                    args.IsValid = false;
                }
            }
        }
    }

    /**
     * 檢查是否上傳超過1個檔案
     * @param args
     */
    function checkFileExceed(sender, args) {
        //var file = $("[id*='FileCenter_RadListView1_itemContainer']")
        var file = $(".FieldHide")
        for (var i = 0; i < file.length; i++) {
            var sub = $(file[i]);
            if (file[i].innerText == "(isoFile)") {
                var id = sub.parent().parent().parent().parent().parent().parent().parent().find("[id*='FileCenter_RadListView1_itemContainer']")

                if (id.children().length > 1) {
                    args.IsValid = false;
                }
            }
        }
    }


    // 檢查文件名稱是否重複
    function checkDOC_NAME(sender, args) {
        var hidFOLDER_ID = $("input[id*=hidFOLDER_ID]");
        var ktxtDOC_NAME = $("input[id*=ktxtDOC_NAME]");
        debugger;
        var result = $uof.pageMethod.syncUc('<%=SyncUcUrl%>',
            "checkDOC_NAME", [hidFOLDER_ID.val(), ktxtDOC_NAME.val()]);
        if (result == 1) {
            ktxtDOC_NAME[0].style.backgroundColor = "#E6AEAE";
            args.IsValid = false;
        }

    }

</script>
<!--統一文管文件申請-->
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="container-fluid">
            <!--container-fluid-->
            <div class="row" style="display: none">
                <div class="col-md-1  bg-light divtitle textRight">
                    申請日期
                </div>
                <div class="col-md-2">
                    <uc1:KYTDatePicker runat="server" ID="kdpAppDate" />
                </div>
                <div class="col-md-2  bg-light divtitle textRight">
                    申請人
                </div>
                <div class="col-md-3">
                    <asp:HiddenField runat="server" ID="hidAPPLICANTGUID" />
                    <asp:HiddenField runat="server" ID="hidAPPLICANTACCOUNT" />
                    <asp:HiddenField runat="server" ID="hidAPPLICANTCOMP" />
                    <asp:HiddenField runat="server" ID="hidAPPLICANTDEPT" />
                    <asp:HiddenField runat="server" ID="hidGROUPCODE" />
                    <asp:HiddenField runat="server" ID="hidGourp_Name" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-1  bg-light divtitle textRight">
                    作業
                </div>
                <div class="col-md-3">
                    <uc1:KYTRadioButtonList runat="server" ID="krdobtnACTION" RadioButtonListCssClass="inline" OnSelectedIndexChanged="krdobtnACTION_SelectedIndexChanged">
                        <asp:ListItem Value="A" Selected="True">新增</asp:ListItem>
                        <asp:ListItem Value="C">修改</asp:ListItem>
                    </uc1:KYTRadioButtonList>
                </div>
                <div class="col-md-1 bg-light divtitle textRight">
                    <span runat="server" id="lblSDOC_NAME" class="color_red">*</span>
                    <asp:Label runat="server" ID="lblSDOCNAME" Text="檔案選擇"></asp:Label>
                </div>
                <div class="col-md-3">
                    <uc1:KYTTextBox runat="server" ID="ktxtSDOC_NAME" Width="550px" TextBoxCssClass="ACTION_B" />
                    <asp:HiddenField runat="server" ID="hidSDOC_ID" />
                    <asp:HiddenField runat="server" ID="hidFILE_GROUP_ID" />
                    <asp:ImageButton runat="server" ID="ibtnSDOC_NAME" ImageUrl="~/Common/Images/SearchBtn.gif" OnClick="ibtnSDOC_NAME_Click" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-1 bg-light divtitle textRight">
                    <span runat="server" id="lblDOC_NAME" class="color_red">*</span>
                    文件名稱
                </div>
                <div class="col-md-3">
                    <uc1:KYTTextBox runat="server" ID="ktxtDOC_NAME" Width="550px" TextBoxCssClass="ACTION_A" />
                    <asp:HiddenField runat="server" ID="hidDOC_ID" />
                    <asp:HiddenField runat="server" ID="hidIsSameDOC_NAME" />
                </div>
                <div class="col-md-1 bg-light divtitle textRight">
                    <span runat="server" id="lblFOLDER" class="color_red">*</span>
                    文管目錄
                </div>
                <div class="col-md-3">
                    <uc1:KYTTextBox runat="server" ID="ktxtFOLDER" TextBoxCssClass="ACTION_A" />
                    <asp:HiddenField runat="server" ID="hidFOLDER_ID" />
                    <asp:ImageButton runat="server" ID="ibtnFOLDER" ImageUrl="~/Common/Images/SearchBtn.gif" OnClick="ibtnFOLDER_Click" />
                </div>
            </div>


            <div class="row">
                <div class="col-md-8">
                    <asp:CustomValidator ID="CustomValidator1" runat="server" Font-Bold="true" ForeColor="Red" Display="Dynamic" ErrorMessage="[請填寫必填欄位]" ClientValidationFunction="checkMustInput"></asp:CustomValidator>
                    <asp:CustomValidator ID="CustomValidator3" runat="server" Font-Bold="true" ForeColor="Red" Display="Dynamic" ErrorMessage="[請上傳檔案]" ClientValidationFunction="checkFileUpload"></asp:CustomValidator>
                    <asp:CustomValidator ID="CustomValidator2" runat="server" Font-Bold="true" ForeColor="Red" Display="Dynamic" ErrorMessage="[最多上傳一個檔案]" ClientValidationFunction="checkFileExceed"></asp:CustomValidator>
                    <asp:CustomValidator ID="CustomValidator4" runat="server" Font-Bold="true" ForeColor="Red" Display="Dynamic" ErrorMessage="[文件名稱已重複]" ClientValidationFunction="checkDOC_NAME"></asp:CustomValidator>
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
