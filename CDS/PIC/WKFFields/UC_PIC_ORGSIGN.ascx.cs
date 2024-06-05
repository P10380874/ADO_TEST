using Ede.Uof.EIP.Organization;
using Ede.Uof.EIP.Organization.Util;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page.Common;
using Ede.Uof.WKF.Design;
using KYTLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using UOFAssist.WKF;
using KYTUtilLibs;
using Newtonsoft.Json;
using System.Data.SqlClient;

/**
* 修改時間：2020/08/20
* 修改人員：梁夢慈
* 修改項目：
    * 1. 取得當下所有簽核人員後，要再取得該主管加入組織欄位站點
* 發生原因：
    * 1. 會繼續簽給當下所有簽核人員，因為少了一個步驟(取得該主管)
* 修改位置：
    * 1. 「RealValue」中，取得當下所有簽核人員之主管
* **/

/**
* 修改時間：2020/08/19
* 修改人員：梁夢慈
* 修改項目：
    * 1. 取得當下所有簽核人員的主管 (會簽)
* 發生原因：
    * 1. 規格書邏輯理解錯誤
* 修改位置：
    * 1. 「RealValue」中，取得當下所有簽核人員之主管
* **/

/// <summary>
/// 統一-組織欄位簽核
/// </summary>
public partial class WKF_OptionalFields_UC_PIC_ORGSIGN : WKF_FormManagement_VersionFieldUserControl_VersionFieldUC
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

            return String.Empty;
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
            return String.Empty;
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
            return String.Empty;
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

            List<string> lisSign = new List<string>();

            // 找出所有簽核人員之主管
            DataTable dtSign = new DataTable();
            if (taskObj != null && taskObj.CurrentSite.SiteCode == "ORGSIGN")
            {
                using (SqlDataAdapter sda = new SqlDataAdapter(@"
                            SELECT * , 
		                            ISNULL((SELECT ACCOUNT FROM TB_EB_USER WHERE USER_GUID = A.SIGNER), NULL) AS 'ACCOUNT',
		                            ISNULL((SELECT NAME FROM TB_EB_USER WHERE USER_GUID = A.SIGNER), NULL) AS 'NAME'
                            FROM (
                            SELECT CASE WHEN ACTUAL_SIGNER IS NULL 
			                                           THEN ORIGINAL_SIGNER 
					  	                               ELSE ACTUAL_SIGNER 
						                                END AS 'SIGNER'  -- 實際簽核人

					                              FROM TB_WKF_TASK_NODE AS A
					                             WHERE SITE_ID = @SITE_ID) A
                            ", new DatabaseHelper().Command.Connection.ConnectionString))
                using (DataSet ds = new DataSet())
                {
                    sda.SelectCommand.Parameters.AddWithValue("@SITE_ID", taskObj.CurrentSite.SiteId);
                    try
                    {
                        if (sda.Fill(ds) > 0)
                        {
                            dtSign = ds.Tables[0];
                        }
                    }
                    catch (Exception e)
                    {
                        KYTUtilLibs.KYTDebugLog.Log(KYTLog.DebugLog.LogLevel.Error, string.Format(@"UC_PIC_ORGSIGN::取得現在所有簽核人員:{0}", e.Message));
                    }
                }
                foreach (DataRow dr in dtSign.Rows)
                {
                    UserSet us = KYTUtilLibs.Utils.UOFUtils.UOFUser.GetUserSuperior(dr["SIGNER"].ToString());
                    EBUser ebUser = new UserUCO().GetEBUser(us.Items[0].Key);
                    if (!lisSign.Contains(ebUser.Account)) lisSign.Add(ebUser.Account);
                }
            }
            string[] SignPerson = lisSign.ToArray();

            string rv = new KYTUtilLibs.Utils.UOFUtils.UOFForm.RealValue()
                    .AddPersonsByAccount("ORGSIGN", SignPerson) // 簽核人主管
                    .ToString();

            KYTDebugLog.Log(DebugLog.LogLevel.DetailInfo, string.Format(@"UC_PIC_ORGSIGN::RealValue:{0}", rv));
            return rv;
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
            //回傳字串
            //取得表單欄位填寫的內容
            Dictionary<string, object> formDict = new Dictionary<string, object>
            {
                //{ "ktxtSTOTAL" , ktxtSTOTAL.Text },
                //{ "ktxtDTOTAL" , ktxtDTOTAL.Text },
                //{ "ktxtSUM" , ktxtSUM.Text }
            };
            Dictionary<string, object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(HttpUtility.HtmlDecode(kytController.FieldValue));
            foreach (KeyValuePair<string, object> item in formDict) dict.Add(item.Key, item.Value);

            return HttpUtility.HtmlEncode(JsonConvert.SerializeObject(dict)); // 表單資料序列化為JSON字串



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
            #region ==============屬性說明==============『』
            //fieldOptional.IsRequiredField『是否為必填欄位,如果是必填(True),如果不是必填(False)』
            //fieldOptional.DisplayOnly『是否為純顯示,如果是(True),如果不是(False),一般在觀看表單及列印表單時,屬性為True』
            //fieldOptional.HasAuthority『是否有填寫權限,如果有填寫權限(True),如果沒有填寫權限(False)』
            //fieldOptional.FieldValue『如果已有人填寫過欄位,則此屬性為記錄其內容』
            //fieldOptional.FieldDefault『如果欄位有預設值,則此屬性為記錄其內容』
            //fieldOptional.FieldModify『是否允許修改,如果允許(fieldOptional.FieldModify=FieldModifyType.yes),如果不允許(fieldOptional.FieldModify=FieldModifyType.no)』
            //fieldOptional.Modifier『如果欄位有被修改過,則Modifier的內容為EBUser,如果沒有被修改過,則會等於Null』
            #endregion

            #region ==============如果沒有填寫權限時,就要顯示有填寫權限人員的清單,只要把以下註解拿掉即可==============
            //if (!fieldOptional.HasAuthority『是否有填寫權限)
            //{
            //    string strItemName = String.Empty;
            //    Ede.Uof.EIP.Organization.Util.UserSet userSet = ((FieldOptional)versionField).FieldControlData;

            //    for (int i = 0; i < userSet.Items.Count; i++)
            //    {
            //        if (i == userSet.Items.Count - 1)
            //        {
            //            strItemName += userSet.Items[i].Name;
            //        }
            //        else
            //        {
            //            strItemName += userSet.Items[i].Name + "、";
            //        }
            //    }

            //    lblHasNoAuthority.ToolTip = lblAuthorityMsg.Text + "：" + strItemName;
            //}
            #endregion

            #region ==============如果有修改，要顯示修改者資訊==============
            if (fieldOptional.Modifier != null)
            {
                lblModifier.Visible = true;
                lblModifier.ForeColor = System.Drawing.Color.Red;
                lblModifier.Text = String.Format("( {0}：{1} )", this.lblMsgSigner.Text, fieldOptional.Modifier.Name);
            }
            #endregion

            this.FormFieldMode = fieldOptional.FieldMode; // 記住本次 FieldMode

            // 初始化kytcontroller
            kytController = new KYTController(UpdatePanel1);

            // 取得資料庫連通字串
            ConnectionString = new DatabaseHelper().Command.Connection.ConnectionString;

            if (!Page.IsPostBack) // 網頁首次載入
            {
                //表單初始化狀態
                if (!string.IsNullOrEmpty(fieldOptional.FieldValue)) //如果欄位有值
                {
                    kytController.FieldValue = fieldOptional.FieldValue;
                }

                kytController.SetAllViewType(KYTViewType.ReadOnly); // 設定所有KYT物件唯讀

                switch (fieldOptional.FieldMode) // 判斷FieldMode
                {
                    case FieldMode.Applicant: // 起單或退回申請者
                    case FieldMode.ReturnApplicant:
                        kytController.SetAllViewType(KYTViewType.Input);// 設定所有KYT物件可輸入 


                        break;
                    case FieldMode.Design: // 表單設計階段
                        break;
                    case FieldMode.Print: // 表單列印
                        break;
                    case FieldMode.Signin: // 表單簽核
                        break;
                    case FieldMode.Verify: // Verify
                        break;
                    case FieldMode.View: // 表單觀看
                        break;
                }
            }
        }
    }

    #region 非控制項功能

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

}
