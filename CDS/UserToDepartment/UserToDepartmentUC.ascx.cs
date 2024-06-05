using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Ede.Uof.WKF.Design;
using System.Collections.Generic;
using Ede.Uof.WKF.Utility;
using Ede.Uof.EIP.Organization.Util;
using Ede.Uof.WKF.Design.Data;
using Ede.Uof.WKF.VersionFields;
using Ede.Uof.EIP.SystemInfo;
using Newtonsoft.Json;
using System.Linq;
using PicDep.UserToDepartment;
using PicDep.UserToDepartment.UserToDepartmentModel;
using Ede.Uof.Utility.Page.Common;
using System.Dynamic;
using Ede.Uof.Utility.Page;
using PIC_SFM.System_File_Maintenance.System_File_MaintenanceUCO2;
using Microsoft.AspNet.SignalR.Messaging;
using DocumentFormat.OpenXml.Wordprocessing;


public partial class WKF_OptionalFields_UserToDepartmentUC : WKF_FormManagement_VersionFieldUserControl_VersionFieldUC
{
    UserToDepartmentUCO utduco = new UserToDepartmentUCO();
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
    public void Page_Load(object sender, EventArgs e)
    {
        //這裡不用修改
        //欄位的初始化資料都到SetField Method去做
        //BindGrid();
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
            //若實作產品標準的控制修改權限必需實作
            //一般是用 m_versionField.FieldValue (表單開啟前的值)
            //      和this.FieldValue (當前的值) 作比對
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
            string result = "";
            if (Grid1 == null || Grid2 == null || Grid3 == null)
            {
                result = String.Empty;
            }
            else
            {
                string uid = HiddenField1.Value;
                DataTable resultP = utduco.Select_UserToDepartment_P(uid);
                DataTable resultT = utduco.Select_UserToDepartment_T(uid);
                DataTable resultD = utduco.Select_UserToDepartment_D(uid);
                string Message_result = "申請人：" + resultP.Rows[0].Field<string>("NAME") + "/" + "直屬單位：" + resultP.Rows[0].Field<string>("專案") + "/" + "上一層單位：" + resultT.Rows[0].Field<string>("團隊") + "/" + "上二層單位：" + resultD.Rows[0].Field<string>("部門");
                result = Message_result;
            }
            return result;
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
            string result = "";
            if (Grid1 == null || Grid2 == null || Grid3 == null)
            {
                result = String.Empty;
            }
            else
            {
                string uid = HiddenField1.Value;
                DataTable resultP = utduco.Select_UserToDepartment_P(uid);
                DataTable resultT = utduco.Select_UserToDepartment_T(uid);
                DataTable resultD = utduco.Select_UserToDepartment_D(uid);
                string Message_result = "申請人：" + resultP.Rows[0].Field<string>("NAME") + "/" + "直屬單位：" + resultP.Rows[0].Field<string>("專案") + "/" + "上一層單位：" + resultT.Rows[0].Field<string>("團隊") + "/" + "上二層單位：" + resultD.Rows[0].Field<string>("部門");
                result = Message_result;
            }
            return result;
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
            return String.Empty;
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
            // 取得表單欄位填寫的內容
            return HiddenField1.Value;
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
            //若實作產品標準的控制修改權限必需實作
            //實作此屬性填寫者可修改也才會生效
            //一般是用 m_versionField.Filler == null(沒有記錄填寫者代表沒填過)
            //      和this.FieldValue (當前的值是否為預設的空白) 作比對
            return false;
        }
        set
        {
            //這個屬性不用修改
            base.IsFirstTimeWrite = value;
        }
    }

    /// <summary>
    /// 設定元件狀態
    /// </summary>
    /// <param name="Enabled">是否啟用輸入元件</param>
    public void EnabledControl(bool Enabled)
    {

    }

    /// <summary>
    /// 顯示時欄位初始值
    /// </summary>
    /// <param name="versionField">欄位集合</param>
    public override void SetField(Ede.Uof.WKF.Design.VersionField versionField)
    {
        FieldOptional fieldOptional = versionField as FieldOptional;
        if (!IsPostBack)
        {
            if (String.IsNullOrEmpty(fieldOptional.FieldValue))
            {
                HiddenField1.Value = ReturnUserID;//抓目前的使用者ID
            }
            else 
            {
                //lbl_test.Text = HiddenField1.Value;
                HiddenField1.Value = fieldOptional.FieldValue;//抓之前的使用者
            }
            if (!String.IsNullOrEmpty(HiddenField1.Value))
            {
                string uid = HiddenField1.Value;
                //lbl_test.Text = uid;
                DataTable resultP = utduco.Select_UserToDepartment_P(uid);
                DataTable resultT = utduco.Select_UserToDepartment_T(uid);
                DataTable resultD = utduco.Select_UserToDepartment_D(uid);
                //專案
                Grid1.DataSource = resultP;
                Grid1.DataBind();
                //團隊
                Grid2.DataSource = resultT;
                Grid2.DataBind();
                //部門
                Grid3.DataSource = resultD;
                Grid3.DataBind();
            }
        }
        if (fieldOptional != null)
        {
            //草稿
            if (!fieldOptional.IsAudit)
            {
                if (fieldOptional.HasAuthority)
                {
                    //有填寫權限的處理
                    EnabledControl(true);
                }
                else
                {
                    //沒填寫權限的處理
                    EnabledControl(false);
                }
            }
            //己送出
            if (fieldOptional.FieldValue != null)
            {

            }
            else
            {
                //有填過
                if (fieldOptional.Filler != null)
                {
                    //判斷填寫的站點和當前是否相同
                    if (base.taskObj != null && base.taskObj.CurrentSite != null &&
                        base.taskObj.CurrentSite.SiteId == fieldOptional.FillSiteId && fieldOptional.Filler.UserGUID == Ede.Uof.EIP.SystemInfo.Current.UserGUID)
                    {
                        //判斷填寫權限
                        if (fieldOptional.HasAuthority)
                        {
                            //有填寫權限的處理
                            EnabledControl(true);
                        }
                        else
                        {
                            //沒填寫權限的處理
                            EnabledControl(false);
                        }
                    }
                    else
                    {
                        //判斷修改權限
                        if (fieldOptional.AllowModify)
                        {
                            //有修改權限的處理
                            EnabledControl(true);
                        }
                        else
                        {
                            //沒修改權限的處理
                            EnabledControl(false);
                        }

                    }
                }
                else
                {
                    //判斷填寫權限
                    if (fieldOptional.HasAuthority)
                    {
                        //有填寫權限的處理
                        EnabledControl(true);
                    }
                    else
                    {
                        //沒填寫權限的處理
                        EnabledControl(false);
                    }

                }
            }



            switch (fieldOptional.FieldMode)
            {
                case FieldMode.Print:
                case FieldMode.View:
                    //觀看和列印都需作沒有權限的處理
                    EnabledControl(false);
                    break;

            }

            #region ==============屬性說明==============『』
            //fieldOptional.IsRequiredField『是否為必填欄位,如果是必填(True),如果不是必填(False)』
            //fieldOptional.DisplayOnly『是否為純顯示,如果是(True),如果不是(False),一般在觀看表單及列印表單時,屬性為True』
            //fieldOptional.HasAuthority『是否有填寫權限,如果有填寫權限(True),如果沒有填寫權限(False)』
            //fieldOptional.FieldValue『如果已有人填寫過欄位,則此屬性為記錄其內容』
            //fieldOptional.FieldDefault『如果欄位有預設值,則此屬性為記錄其內容』
            //fieldOptional.FieldModify『是否允許修改,如果允許(fieldOptional.FieldModify=FieldModifyType.yes),如果不允許(fieldOptional.FieldModify=FieldModifyType.no)』
            //fieldOptional.Modifier『如果欄位有被修改過,則Modifier的內容為EBUser,如果沒有被修改過,則會等於Null』
            #endregion

            #region ==============如果有修改，要顯示修改者資訊==============
            if (fieldOptional.Modifier != null)
            {
                lblModifier.Visible = true;
                lblModifier.ForeColor = System.Drawing.Color.FromArgb(0x52, 0x52, 0x52);
                lblModifier.Text = System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(fieldOptional.Modifier.Name, true);
            }
            #endregion
        }

    }

    public void BindGrid()
    {
        string uid = ReturnUserID;
        //lbl_test.Text = uid;
        DataTable resultP = utduco.Select_UserToDepartment_P(uid);
        DataTable resultT = utduco.Select_UserToDepartment_T(uid);
        DataTable resultD = utduco.Select_UserToDepartment_D(uid);
        //專案
        Grid1.DataSource = resultP;
        Grid1.DataBind();
        //團隊
        Grid2.DataSource = resultT;
        Grid2.DataBind();
        //部門
        Grid3.DataSource = resultD;
        Grid3.DataBind();

    }

    public string ReturnUserID
    {
        get
        {
            // 呼叫後端函式抓取使用者 ID
            UserUCO userUCO = new UserUCO();
            EBUser now_user = userUCO.GetEBUser(Current.UserGUID);
            // 回傳使用者 ID
            //"."後面加要取的欄位
            return now_user.UserGUID;
        }
    }
}