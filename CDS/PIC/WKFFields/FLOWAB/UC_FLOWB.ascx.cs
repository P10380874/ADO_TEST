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
using System.Web.UI.WebControls;
using Newtonsoft.Json.Linq;

/**
* 修改時間：2020/09/11
* 修改人員：陳緯榕
* 修改項目：
    * 表單狀態的險是要去查詢有沒有個人化設定的
* 發生原因：
    * 新規格
* 修改位置：
    * 「GetAllRelationForms」中，SQL內，使用〈TASK_ID〉查詢〈TB_WKF_TASK_NODE-CUSTOM_WORDS〉
    * 「gvForms_RowDataBound」中，當〈CUSTOM_WORDS〉能夠當作一個〈JObject〉拆解出〈zh-TW〉時，〈lblFLOWB_STATUS〉填入其內容；反之填入〈FLOWB_STATUS〉當其內容
* **/

/**
* 修改時間：2020/09/11
* 修改人員：高常昇
* 修改項目：
    * 調整前端畫面表格，需要置中且顏色調淡
* 修改位置：
    * 「前端網頁」中，Style內新增「表格置中及換標題顏色」區塊，及新增KYTI.css的引用
* **/

/// <summary>
/// 統一 - 用印申請單
/// </summary>
public partial class WKF_OptionalFields_CDS_PIC_WKFFields_UC_FLOWB : WKF_FormManagement_VersionFieldUserControl_VersionFieldUC
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
            return string.Empty;
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

            // 取得資料庫連通字串
            ConnectionString = new DatabaseHelper().Command.Connection.ConnectionString;

            if (!Page.IsPostBack) // 網頁首次載入
            {
                bool isSign = false;
                switch (fieldOptional.FieldMode) // 判斷FieldMode
                {
                    case FieldMode.Applicant: // 起單或退回申請者
                    case FieldMode.ReturnApplicant:
                        if (this.FormFieldMode == FieldMode.Applicant) // 剛起單
                        {
                        }
                        if (taskObj != null)
                            isSign = true;
                        break;
                    case FieldMode.Design: // 表單設計階段
                        break;
                    case FieldMode.Print: // 表單列印
                        isSign = true;
                        break;
                    case FieldMode.Signin: // 表單簽核
                        isSign = true;
                        break;
                    case FieldMode.Verify: // Verify
                        break;
                    case FieldMode.View: // 表單觀看
                        isSign = true;
                        break;
                }
                if (isSign)
                {
                    if (taskObj != null)
                    {
                        // 會辦狀態
                        ViewState["gvForms"] = GetAllRelationForms();
                        gvForms.DataSource = (DataTable)ViewState["gvForms"];
                        gvForms.DataBind();
                    }

                }
            }
        }
    }

    #region 非控制項功能

    /// <summary>
    /// 建立會辦狀態表
    /// </summary>
    /// <returns></returns>
    private DataTable CreateAllRelationForms()
    {
        DataTable dtReturn = new DataTable();
        dtReturn.Columns.Add(new DataColumn("FLOWA_NO", typeof(String))); // 表單單號
        dtReturn.Columns.Add(new DataColumn("FLOWA_TASK_ID", typeof(String))); // A單的TASK_ID
        dtReturn.Columns.Add(new DataColumn("FLOWA_STATUS", typeof(String))); // 簽核狀態

        return dtReturn;
    }

    /// <summary>
    /// 找到所有關聯的會辦狀態
    /// </summary>
    /// <returns></returns>
    private DataTable GetAllRelationForms()
    {
        DataTable dtReturn = CreateAllRelationForms();

        using (SqlDataAdapter sda = new SqlDataAdapter(@"
               SELECT FLOWA_NBR AS 'FLOWA_NO', -- 找到所有關聯的資料
                      TASK_ID AS 'FLOWA_TASK_ID',
                      ISNULL((SELECT TOP 1 CUSTOM_WORDS 
				                        FROM TB_WKF_TASK_NODE 
				                       WHERE TASK_ID = ZPIC_AB_TASK.TASK_ID 
				                         AND FINISH_TIME IS NULL), '') AS 'CUSTOM_WORDS', -- 用TASK_ID 取出尚未結案的一筆節點的CUSTOM_WORD
		              CASE WHEN TASK_STATUS_A = 1  -- 簽核狀態
				            AND TASK_RESULT_A IS NULL 
			               THEN '簽核中'
			               WHEN TASK_STATUS_A = 2 
				            AND TASK_RESULT_A = 0 
			               THEN '核准'
			               WHEN TASK_STATUS_A = 2 
				            AND TASK_RESULT_A = 1 
			               THEN '否決'
			               WHEN TASK_STATUS_A = 2 
				            AND TASK_RESULT_A = 2 
		   	               THEN '作廢'
			               WHEN TASK_STATUS_A = 3 
			               THEN '異常'
			               WHEN TASK_STATUS_A = 4 
			               THEN '退回'
						   WHEN TASK_STATUS_A = 5 
			               THEN '作廢'
			               ELSE '錯誤' 
			                END AS 'FLOWA_STATUS'
                  FROM ZPIC_AB_TASK
	             WHERE FLOWB_NBR = @FLOWB_NBR
              ORDER BY FLOWA_NBR ASC
            ", ConnectionString))
        using (DataSet ds = new DataSet())
        {
            sda.SelectCommand.Parameters.AddWithValue("@FLOWB_NBR", taskObj.FormNumber);
            try
            {
                if (sda.Fill(ds) > 0)
                    dtReturn = ds.Tables[0];
            }
            catch (Exception e)
            {
                DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_FLOWB.GetAllRelationForms.SELECT.ERROR:{0}", e.Message));
            }
        }
        return dtReturn;
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


    protected void gvForms_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        GridViewRow gr = e.Row;
        if (gr.RowType == DataControlRowType.DataRow)
        {
            DataTable dtSource = ViewState["gvForms"] as DataTable;
            DataRow row = dtSource.Rows[gr.DataItemIndex];

            LinkButton lbtnFLOWA_NO = gr.FindControl("lbtnFLOWA_NO") as LinkButton;
            Label lblFLOWA_STATUS = gr.FindControl("lblFLOWA_STATUS") as Label;

            try
            {
                JObject joComment = JObject.Parse(row["CUSTOM_WORDS"].ToString());
                string _status = joComment["zh-TW"].ToString();
                lblFLOWA_STATUS.Text = !string.IsNullOrEmpty(_status) ? _status : row["FLOWA_STATUS"].ToString();
            }
            catch
            {
                lblFLOWA_STATUS.Text = row["FLOWA_STATUS"].ToString();
            }
            string url = "";
            if (base.MobileUI) // 現在是APP
                url = "~/WKF/FormUse/Mobile/ViewForm.aspx";
            else
                url = "~/WKF/FormUse/ViewForm.aspx";
            Dialog.Open2(lbtnFLOWA_NO, url, row["FLOWA_NO"].ToString(), 960, 720, Dialog.PostBackType.None, new { TASK_ID = row["FLOWA_TASK_ID"].ToString() }.ToExpando());
        }
    }
}
