using Ede.Uof.EIP.Organization.Util;
using Ede.Uof.EIP.SystemInfo;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page.Common;
using Ede.Uof.WKF.Design;
using KYTLog;
using KYTUtilLibs.Utils;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web.UI;
using UOFAssist.WKF;

/**
* 修改時間：2020/11/11
* 修改人員：梁夢慈
* 修改項目：
    * 1. 將檔案選擇(ktxtSDOC_NAME)顯示全部名稱；將文件名稱(ktxtDOC_NAME)顯示全部名稱
    * 2. 文管新增時，要去判斷檔案名稱是否重複，若重複時，跳出提醒 "檔案名稱已重複"
    * 3. 檔案選擇後改為帶入文件名稱
* 修改原因：
    * 1. 調整欄位，USER希望能一次顯示出來
    * 2. 新增規格
    * 2. 新增規格
* 修改位置： 
    * 1.「前端網頁中」，新增CSS <-- 針對視窗調整max-width (參考UC_ZENITHTEK_MATNR.ascx常昇寫的)
    * 2. 「前端網頁中」，新增驗證方法(checkDOC_NAME)
    * 3. 「ibtnSDOC_NAME_Click」，跳窗取回後，文件名稱ktxtSDOC_NAME.Text給予["DOC_NAME"]的值
* **/

/**
* 修改時間：2020/11/06
* 修改人員：梁夢慈
* 修改項目：
    * 1. 新增時，預設文管目錄(Default_FOLDER_ID)
* 修改原因：
    * 1. 漏寫
* 修改位置： 
    * 1.「SetField()」中，於起單時給予預設值
* **/

/**
* 修改時間：2020/09/25
* 修改人員：梁夢慈
* 修改項目：
    * 1. 取文管目錄結構方法內是給UserGUID
* 修改原因：
    * 1. 測試沒有帶出目錄名稱
* 修改位置： 
    * 1.「ibtnSDOC_NAME_Click」中，dmsfolder.GetFolders(user.UserGUID, Current.Culture);  第一個參數給UserGUID
* **/

/**
* 修改時間：2020/09/24
* 修改人員：梁夢慈
* 修改項目：
    * 1. 檔案選擇標題文字，在「新增」時完全隱藏(包含輸入框線也不要有)
* 修改原因：
    * 1. 畫面調整
* 修改位置： 
    * 1.「前端網頁」中，「檔案選擇」標題改為Label，在後端「krdobtnACTION_SelectedIndexChanged」控制顯示/隱藏
* **/

/**
* 修改時間：2020/09/22
* 修改人員：梁夢慈
* 修改項目：
    * 1. 前端送單檢查上傳的檔案數量，只要檢查isoFile裡的數量即可
* 修改原因：
    * 1. 沒有指定名稱取出來的結果，會包含所有的檔案數量
* 修改位置： 
    * 1.「前端網頁」->「checkFileUpload」、「checkFileExceed」中，判斷符合節點(isoFile)的檔案數量
* **/

/// <summary>
/// 統一文管文件申請
/// </summary>
public partial class WKF_OptionalFields_UC_PIC_ISO : WKF_FormManagement_VersionFieldUserControl_VersionFieldUC
{
    /// <summary>
    /// 資料庫連通字串
    /// </summary>
    string ConnectionString;

    /// <summary>
    /// 當前FieldMode
    /// </summary>
    FieldMode FormFieldMode;

    /// <summary>
    /// 當前FieldMode
    /// </summary>
    static FieldMode FormFieldMode_WebMethod;

    /// <summary>
    /// KYT控制元件
    /// </summary>
    KYTController kytController;

    #region ==============公開方法及屬性==============
    //表單設計時
    //如果為False時,表示是在表單設計時
    private bool m_ShowGetValueButton = true;
    public bool ShowGetValueButton
    {
        get { return this.m_ShowGetValueButton; }
        set { this.m_ShowGetValueButton = value; }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        //這裡不用修改
        //欄位的初始化資料都到SetField Method去做
        SetField(m_versionField);
    }

    /// <summary>
    /// 外掛欄位的條件值
    /// </summary>
    public override string ConditionValue
    {
        get
        {
            //回傳字串
            //此字串的內容將會被表單拿來當做條件判斷的值
            return string.Empty;
        }
    }

    /// <summary>
    /// 是否被修改
    /// </summary>
    public override bool IsModified
    {
        get
        {
            //請自行判斷欄位內容是否有被修改
            //有修改回傳True
            //沒有修改回傳False
            return false;
        }
    }

    /// <summary>
    /// 查詢顯示的標題
    /// </summary>
    public override string DisplayTitle
    {
        get
        {
            //表單查詢或WebPart顯示的標題
            //回傳字串
            return string.Empty;
        }
    }

    /// <summary>
    /// 訊息通知的內容
    /// </summary>
    public override string Message
    {
        get
        {
            //表單訊息通知顯示的內容
            //回傳字串
            return string.Empty;
        }
    }


    /// <summary>
    /// 真實的值
    /// </summary>
    public override string RealValue
    {
        get
        {
            //回傳字串
            //取得表單欄位簽核者的UsetSet字串
            //內容必須符合EB UserSet的格式
            return string.Empty;
        }
        set
        {
            //這個屬性不用修改
            base.m_fieldValue = value;
        }
    }


    /// <summary>
    /// 欄位的內容
    /// </summary>
    public override string FieldValue
    {
        get
        {
            return kytController.FieldValue;
            //Dictionary<string, object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(HttpUtility.HtmlDecode(kytController.FieldValue));

            //return HttpUtility.HtmlEncode(JsonConvert.SerializeObject(dict)); // 表單資料序列化為JSON字串
        }
        set
        {
            //這個屬性不用修改
            base.m_fieldValue = value;
        }
    }

    /// <summary>
    /// 是否為第一次填寫
    /// </summary>
    public override bool IsFirstTimeWrite
    {
        get
        {
            //這裡請自行判斷是否為第一次填寫
            return false;
        }
        set
        {
            //這個屬性不用修改
            base.IsFirstTimeWrite = value;
        }
    }

    /// <summary>
    /// 顯示時欄位初始值
    /// </summary>
    /// <param name="versionField">欄位集合</param>
    public override void SetField(Ede.Uof.WKF.Design.VersionField versionField)
    {
        FieldOptional fieldOptional = versionField as FieldOptional;

        if (fieldOptional != null)
        {
            #region ==============如果有修改，要顯示修改者資訊==============
            if (fieldOptional.Modifier != null)
            {
                lblModifier.Visible = true;
                lblModifier.ForeColor = System.Drawing.Color.Red;
                lblModifier.Text = String.Format("( {0}：{1} )", this.lblMsgSigner.Text, fieldOptional.Modifier.Name);
            }
            #endregion

            this.FormFieldMode = fieldOptional.FieldMode; // 記住本次 FieldMode
            FormFieldMode_WebMethod = fieldOptional.FieldMode;
            kytController = new KYTController(UpdatePanel1);

            // 取得資料庫連通字串
            ConnectionString = new DatabaseHelper().Command.Connection.ConnectionString;
            if (!Page.IsPostBack) // 網頁首次載入
            {
                kytController.SetAllViewType(KYTViewType.ReadOnly); // 設定所有KYT物件唯讀


                if (!string.IsNullOrEmpty(fieldOptional.FieldValue))
                {
                    //Dictionary<string, object> Dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(fieldOptional.FieldValue);
                    kytController.FieldValue = fieldOptional.FieldValue;
                }
                lblSDOC_NAME.Visible = krdobtnACTION.SelectedValue == "C" ? true : false;

                ibtnSDOC_NAME.Visible = false; // 檔案選擇_按鈕
                ibtnFOLDER.Visible = false; // 文管目錄_按鈕
                switch (fieldOptional.FieldMode) // 判斷FieldMode
                {
                    case FieldMode.Applicant: // 起單或退回申請者
                    case FieldMode.ReturnApplicant:
                        kytController.SetAllViewType(KYTViewType.Input); // 設定所有KYT物件可輸入

                        krdobtnACTION_SelectedIndexChanged(null, null);

                        // 取得檔案目錄名稱
                        string Default_FOLDER_ID = PICFLOW.PICFLOWConfiguration.Default_FOLDER_ID;
                        EBUser user = new UserUCO().GetEBUser(this.ApplicantGuid);
                        var dmsfolder = new Ede.Uof.DMS.DMSFolder2();
                        DataTable dtFolder = dmsfolder.GetFolders(user.UserGUID, Current.Culture);
                        foreach (DataRow dr in dtFolder.Rows)
                        {
                            if (dr["CHILD_ID"].ToString() == Default_FOLDER_ID)
                            {
                                ktxtFOLDER.Text = dr["CHILD_NAME"].ToString();
                                hidFOLDER_ID.Value = dr["CHILD_ID"].ToString();
                            }
                        }
                        string GroupCode = UOFUtils.UOFGroup.GetGroupCodeByDepartmentID(user.GroupID); //GROUP_CODE
                        KYTUtilLibs.KYTDebugLog.Log(DebugLog.LogLevel.Debug, string.Format("UC_TAIYUEN_FEE_MBUDGET::部門代碼:{0}", GroupCode));
                        kdpAppDate.Text = DateTime.Now.ToString(); //預先帶入當天日期
                        hidGROUPCODE.Value = GroupCode;
                        hidGourp_Name.Value = user.GroupName; //部門名稱
                        hidAPPLICANTDEPT.Value = user.GroupID;
                        hidAPPLICANTGUID.Value = user.UserGUID;
                        hidAPPLICANTACCOUNT.Value = user.Account;
                        hidAPPLICANTCOMP.Value = user.CompanyNo; // 申請者公司(group_code前四碼)

                        Dialog.Open2(ibtnSDOC_NAME, string.Format(@"~/CDS/PIC/WKFFields/QUERYWINDOWS/SearchDOC.aspx"), "選擇檔案", 500, 600, Dialog.PostBackType.AfterReturn, new { }.ToExpando());
                        Dialog.Open2(ibtnFOLDER, string.Format(@"~/CDS/PIC/WKFFields/QUERYWINDOWS/SearchFOLDER.aspx"), "選擇文管目錄", 400, 600, Dialog.PostBackType.AfterReturn, new { }.ToExpando());
                        break;
                    case FieldMode.Design: // 表單設計階段
                        break;
                    case FieldMode.Signin: // 表單簽核
                        break;
                    case FieldMode.Verify: // Verify
                        break;
                    case FieldMode.Print: // 表單列印
                        break;
                    case FieldMode.View: // 表單觀看
                        //btnPrint.Visible = this.GetFormIsEnd();
                        break;
                }
            }
            else
            {
                if (fieldOptional.FieldMode == FieldMode.Applicant ||
                    fieldOptional.FieldMode == FieldMode.ReturnApplicant)
                {
                    string GroupCode = KYTUtilLibs.Utils.UOFUtils.UOFGroup.GetGroupCodeByDepartmentID(this.ApplicantGroupId); //GROUP_CODE
                    string GroupName = KYTUtilLibs.Utils.UOFUtils.UOFGroup.GetGroupNameByDepartmentID(this.ApplicantGroupId); //GROUP_CODE
                    hidGROUPCODE.Value = GroupCode; // GROUPCODE
                }
            }
        }
    }
    #region 非控制項功能

    /// <summary>
    /// 是否為起單或退回申請者
    /// </summary>
    /// <returns></returns>
    private bool isApplicant()
    {
        return this.FormFieldMode == FieldMode.Applicant || this.FormFieldMode == FieldMode.ReturnApplicant;
    }

    /// <summary>
    /// 本表單是否已經結案
    /// </summary>
    /// <returns></returns>
    private bool GetFormIsEnd()
    {
        if (this.FormFieldMode == FieldMode.Applicant || this.FormFieldMode == FieldMode.ReturnApplicant) return false;
        if (taskObj == null) return false;
        if (taskObj.CurrentDocument == null) return false;
        using (SqlDataAdapter sda = new SqlDataAdapter(@"
                SELECT *
	              FROM [Z_TY_FEE_MBUDGET]
                 WHERE TASK_STATUS = '2'
				   AND TASK_RESULT = '0'
                   AND DOC_NBR = @DOC_NBR
        ", ConnectionString))
        using (DataSet ds = new DataSet())
        {
            sda.SelectCommand.Parameters.AddWithValue("@DOC_NBR", taskObj.CurrentDocument.DocNbr);
            try
            {
                sda.Fill(ds);
                return ds.Tables[0].Rows.Count > 0;
            }
            catch (Exception ex) { throw new Exception(string.Format("UC_TAIYUEN_FEE_MBUDGET::ERROR::{0}::{1}", "GetFormIsEnd", ex.Message)); }
        }
    }

    public string SyncUcUrl
    {
        get { return KYTUtilLibs.Utils.WebUtils.GetWKFFieldRelativeUrl(this); }
    }


    /// <summary>
    /// 中繼表檢查發票
    /// </summary>
    /// <param name="invoice"></param>
    /// <returns></returns>
    [WebMethod]
    public static string checkDOC_NAME(string folder_id,string doc_name)
    {
        string result = "";
        if (FormFieldMode_WebMethod == FieldMode.Applicant ||
            FormFieldMode_WebMethod == FieldMode.ReturnApplicant)
        {
            string ConnectionString = new DatabaseHelper().Command.Connection.ConnectionString;
            using (SqlDataAdapter sda = new SqlDataAdapter(@"
            SELECT STORE.FILE_GROUP_ID,STORE.FILE_NAME,PRO.DOC_ID,SHADOW.DOC_NAME,FOLDER_ID FROM TB_DMS_DOC_PROPERTY AS PRO
		     LEFT JOIN TB_DMS_DOC_SHADOW AS SHADOW   ON PRO.DOC_ID     = SHADOW.DOC_ID
		     LEFT JOIN TB_EB_FILE_STORE  AS STORE    ON SHADOW.FILE_ID = STORE.FILE_GROUP_ID
		 
             WHERE FOLDER_ID = @FOLDER_ID
               AND DOC_NAME = @DOC_NAME
            ", ConnectionString))
            using (DataSet ds = new DataSet())
            {
                sda.SelectCommand.Parameters.AddWithValue("@FOLDER_ID", folder_id);
                sda.SelectCommand.Parameters.AddWithValue("@DOC_NAME", doc_name);
                try
                {
                    sda.Fill(ds);
                    //把來源檔帶入
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        result= "0"; // 沒有重複
                    }
                    else
                    {
                        result= "1"; // 已重複
                    }
                }
                catch (Exception ex)
                {
                    KYTUtilLibs.KYTDebugLog.Log(DebugLog.LogLevel.Error, string.Format("UC_PIC_ISO::checkDOC_NAME::錯誤:{0}", ex.Message));
                }
            }
        }
        else
        {
            result= "0";
        }
        return result;
    }



    #endregion 非控制
    #region ==============修改權限LinkButton的事件==============
    protected void lnk_Edit_Click(object sender, EventArgs e)
    {
        //這裡還要加入控制項的隱藏或顯示

        lnk_Cannel.Visible = true;
        lnk_Edit.Visible = false;
        lnk_Submit.Visible = true;
    }
    protected void lnk_Cannel_Click(object sender, EventArgs e)
    {
        //這裡還要加入控制項的隱藏或顯示

        SetField(m_versionField);

        lnk_Cannel.Visible = false;
        lnk_Edit.Visible = true;
        lnk_Submit.Visible = false;
    }
    protected void lnk_Submit_Click(object sender, EventArgs e)
    {
        //這裡還要加入控制項的隱藏或顯示

        lnk_Cannel.Visible = false;
        lnk_Edit.Visible = true;
        lnk_Submit.Visible = false;

        //儲存表單資料
        if (base._IFieldOOServer.Count == 0) return;
        ((IFieldCompetenceServer)base._IFieldOOServer[0]).SaveForm();
    }
    #endregion


    /// <summary>
    /// 作業_變更
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void krdobtnACTION_SelectedIndexChanged(object sender, EventArgs e)
    {
        ibtnSDOC_NAME.Visible = false; // 檔案選擇_按鈕
        ibtnFOLDER.Visible = false; // 文管目錄_按鈕
        lblSDOC_NAME.Visible = false; // 檔案選擇_必填顯示*
        lblSDOCNAME.Visible = false; // 檔案選擇_標題

        ktxtSDOC_NAME.ReadOnly = true; // 檔案選擇
        ktxtSDOC_NAME.ViewType = KYTViewType.ReadOnly; // 檔案選擇
        ktxtDOC_NAME.ViewType = KYTViewType.ReadOnly; // 文件名稱
        ktxtFOLDER.ReadOnly = true; // 文管目錄

        if (krdobtnACTION.SelectedValue == "A") // 新增
        {
            ktxtSDOC_NAME.Text = "";
            ibtnFOLDER.Visible = true; // 文管目錄_按鈕
            ktxtDOC_NAME.ViewType = KYTViewType.Input; // 文件名稱
        }
        else if (krdobtnACTION.SelectedValue == "C") // 修改
        {
            ktxtSDOC_NAME.ViewType = KYTViewType.Input; // 檔案選擇
            lblSDOC_NAME.Visible = true; // 檔案選擇_必填顯示*
            ibtnSDOC_NAME.Visible = true; // 檔案選擇_按鈕
            ktxtDOC_NAME.ViewType = KYTViewType.ReadOnly; // 文件名稱
            lblSDOCNAME.Visible = true; // 檔案選擇_標題
            ktxtFOLDER.Text = "";
            hidFOLDER_ID.Value = "";
        }
    }

    /// <summary>
    /// 檔案選擇
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ibtnSDOC_NAME_Click(object sender, ImageClickEventArgs e)
    {
        string value = Dialog.GetReturnValue();
        if (string.IsNullOrEmpty(value)) return;
        string returnValue = "";
        returnValue = value.Replace("/&#&#&&/g", "''");
        DataTable result = JsonConvert.DeserializeObject<DataTable>(returnValue);
        string doc_name = result.Rows[0]["DOC_NAME"].ToString();
        string[] split_name = doc_name.Split("_");
        string versoinName = "";
        if (split_name.Length >= 2) // 是否存在「"_"」拆解字串
        {
            int version = 0;
            if (int.TryParse(split_name[split_name.Length - 1].ToString(), out version)) // 判斷拆解取最後是否為「數字」
            {
                int.TryParse(split_name[split_name.Length - 1].ToString(), out version);
                for (int i = 0; i < split_name.Length - 1; i++) // 如果存在多個底線，則先組合好前面的，再加上版本數字
                {
                    versoinName += split_name[i] + "_";
                }
                versoinName += (version + 1).ToString();
            }
            else // 如果不是數字直接加上「_1」
            {
                versoinName = doc_name + "_1";
            }
        }
        else if (split_name.Length == 1)
        {
            versoinName = doc_name + "_1";
        }
        //ktxtSDOC_NAME.Text = result.Rows[0]["FILE_NAME"].ToString();
        ktxtSDOC_NAME.Text = result.Rows[0]["DOC_NAME"].ToString();
        hidSDOC_ID.Value = result.Rows[0]["FILE_GROUP_ID"].ToString();
        ktxtDOC_NAME.Text = versoinName; // 文件名稱 --> 以 「DOC_NAME 處理後 > FILENAME_1」
        hidDOC_ID.Value = result.Rows[0]["DOC_ID"].ToString();
        hidFILE_GROUP_ID.Value = result.Rows[0]["FILE_GROUP_ID"].ToString();

        // 取得檔案目錄名稱
        string Default_FOLDER_ID = PICFLOW.PICFLOWConfiguration.Default_FOLDER_ID;
        EBUser user = new UserUCO().GetEBUser(this.ApplicantGuid);
        var dmsfolder = new Ede.Uof.DMS.DMSFolder2();
        DataTable dtFolder = dmsfolder.GetFolders(user.UserGUID, Current.Culture);
        foreach (DataRow dr in dtFolder.Rows)
        {
            if (dr["CHILD_ID"].ToString() == Default_FOLDER_ID)
            {
                ktxtFOLDER.Text = dr["CHILD_NAME"].ToString();
                hidFOLDER_ID.Value = dr["CHILD_ID"].ToString();
            }
        }
    }

    /// <summary>
    /// 文管目錄_選擇
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ibtnFOLDER_Click(object sender, ImageClickEventArgs e)
    {
        string value = Dialog.GetReturnValue();
        if (string.IsNullOrEmpty(value)) return;
        string returnValue = "";
        returnValue = value.Replace("/&#&#&&/g", "''");
        DataTable result = JsonConvert.DeserializeObject<DataTable>(returnValue);
        ktxtFOLDER.Text = result.Rows[0]["FOLDER_NAME"].ToString();
        hidFOLDER_ID.Value = result.Rows[0]["FOLDER_ID"].ToString();
    }

}