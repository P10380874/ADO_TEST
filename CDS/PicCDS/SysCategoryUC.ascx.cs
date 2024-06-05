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
using System.Xml;
using PicCDS.SysCategory;
using Telerik.Web.UI;
using Newtonsoft.Json;
using System.Linq;

public partial class WKF_OptionalFields_SysCategoryUC : WKF_FormManagement_VersionFieldUserControl_VersionFieldUC
{

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
            string result = "";
            if (rcb_SysCategory.SelectedValue == "000")
            {
                result = rcb_SysCategory.Text + "-" + txt_Else.Text;
            }
            else
            {
                result = rcb_SysCategory.Text;
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
            //取得表單欄位填寫的內容
            SysCatagoryFieldData resultData = new SysCatagoryFieldData 
            {
                categoryID = rcb_SysCategory.SelectedValue,
                categoryName = rcb_SysCategory.Text,
                else_text = txt_Else.Text
            };
            return JsonConvert.SerializeObject(resultData);
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
        rcb_SysCategory.Enabled = Enabled;
        txt_Else.Enabled = Enabled;
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
            else
            {
                //退回至申請-可修改
                if (fieldOptional.FieldMode == Ede.Uof.WKF.Design.FieldMode.ReturnApplicant)
                {
                    EnabledControl(true);
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
                    //根據站點代號決定是否開啟修改
                    if (base.taskObj != null && base.taskObj.CurrentSite != null)
                    {
                        string siteCode = base.taskObj.CurrentSite.SiteCode;
                        if (siteCode.Contains("AllowEditSysCategory")) //站點代號
                        {
                            EnabledControl(true);
                        }
                    }
                }
            }
        }

        if (!IsPostBack)
        {
            SysCategoryUCO sysCategoryUCO = new SysCategoryUCO();
            DataTable dataTable = sysCategoryUCO.Select_ComboBox_SysCategory();
            hf_AllData.Value = JsonConvert.SerializeObject(dataTable);

            if (!String.IsNullOrEmpty(fieldOptional.FieldValue))
            {
                SysCatagoryFieldData fieldData = JsonConvert.DeserializeObject<SysCatagoryFieldData>(fieldOptional.FieldValue);
                rcb_SysCategory.SelectedValue = fieldData.categoryID;
                rcb_SysCategory.Text = fieldData.categoryName;
                if(fieldData.categoryID == "000")
                {
                    txt_Else.Text = fieldData.else_text;
                    txt_Else.Visible = true;
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

    protected void rcb_SysCategory_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
    {
        RadComboBox radComboBox = sender as RadComboBox;

        DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(hf_AllData.Value);

        // 使用 LINQ 查詢，篩選符合條件的資料列
        var queryResult = from row in dataTable.AsEnumerable()
                          where row.Field<string>("SYS_CATEGORY_NAME").Contains(e.Text)
                             || row.Field<string>("DEPARTMENT").Contains(e.Text)
                          select row;

        radComboBox.Items.Clear();

        if (queryResult.Any())
        {
            // 取得查詢結果
            DataTable resultTable = queryResult.CopyToDataTable();

            foreach (DataRow dataRow in resultTable.Rows)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Value = dataRow["SYS_CATEGORY_ID"].ToString();
                item.Text = (string)dataRow["SYS_CATEGORY_NAME"];
                item.Attributes.Add("DEPARTMENT", dataRow["DEPARTMENT"].ToString());
                radComboBox.Items.Add(item);
                item.DataBind();
            }
        }
        RadComboBoxItem item_else = new RadComboBoxItem();
        item_else.Value = "000";
        item_else.Text = "其他";
        item_else.Attributes.Add("DEPARTMENT", "其他");
        radComboBox.Items.Add(item_else);
        item_else.DataBind();
    }

    protected void rcb_SysCategory_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (rcb_SysCategory.SelectedValue == "000")
        {
            txt_Else.Visible = true;
        }
        else
        {
            txt_Else.Visible = false;
            txt_Else.Text = "";
        }
    }
}
