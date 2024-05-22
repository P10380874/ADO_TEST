using System;
using System.Data;
using Ede.Uof.WKF.Design;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using Telerik.Web.UI;
using PicDep.UserToDepartment;
using System.Text;
public partial class WKF_OptionalFields_SUserToDepartmentUC : WKF_FormManagement_VersionFieldUserControl_VersionFieldUC
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
            string result = "";
            if (!String.IsNullOrEmpty(HiddenDataTable.Value))
            {
                List<SUserToDepFieldData> list_All = JsonConvert.DeserializeObject<List<SUserToDepFieldData>>(HiddenDataTable.Value);
                if (list_All.Count > 0)
                {
                    // 從第一個元素中取出Name、專案、團隊和部門的數據
                    SUserToDepFieldData firstItem = list_All[0];
                    // 將這些數據組合成一個訊息
                    string Message_result = "人員名稱：" + firstItem.NAME + "/" + "專案：" + firstItem.專案 + "/" + "團隊：" + firstItem.團隊 + "/" + "部門：" + firstItem.部門;
                    return Message_result;
                }
            }
            else
            {
                result = "";
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
            if (!String.IsNullOrEmpty(HiddenDataTable.Value))
            {
                List<SUserToDepFieldData> list_All = JsonConvert.DeserializeObject<List<SUserToDepFieldData>>(HiddenDataTable.Value);
                if (list_All.Count > 0)
                {
                    // 從第一個元素中取出Name、專案、團隊和部門的數據
                    SUserToDepFieldData firstItem = list_All[0];
                    // 將這些數據組合成一個訊息
                    string Message_result = "人員名稱：" + firstItem.NAME + "/" + "專案：" + firstItem.專案 + "/" + "團隊：" + firstItem.團隊 + "/" + "部門：" + firstItem.部門;
                    return Message_result;
                }
            }
            else
            {
                result = "";
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
            SUserToDepFieldData resultData = new SUserToDepFieldData
            {
                NAME = rcb_SUserToDepartment.SelectedValue,
                USER_GUID = HiddenNAME.Value,
                ALL_Team = HiddenDataTable.Value,
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
        rcb_SUserToDepartment.Enabled = Enabled;
        txt_Else.Enabled = Enabled;
    }

    /// <summary>
    /// 顯示時欄位初始值
    /// </summary>
    /// <param name="versionField">欄位集合</param>
    public override void SetField(Ede.Uof.WKF.Design.VersionField versionField)
    {
        FieldOptional fieldOptional = versionField as FieldOptional;
        //控制站點權限
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
                //己送出
                //有填過
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
                        if (siteCode.Contains("AllowEditSUserToDepartment")) //站點代號
                        {
                            EnabledControl(true);
                        }
                    }
                }
            }
        }

        //非首次讀入
        if (!IsPostBack)
        {
            //儲存後顯示
            UserToDepartmentUCO usertodepartmentUCO = new UserToDepartmentUCO();
            DataTable dataTable = usertodepartmentUCO.Select_ComboBox_SUserToDepartmentPO();
            hf_AllData.Value = JsonConvert.SerializeObject(dataTable);
            if (!String.IsNullOrEmpty(fieldOptional.FieldValue))
            {
                SUserToDepFieldData fieldData = JsonConvert.DeserializeObject<SUserToDepFieldData>(fieldOptional.FieldValue);
                rcb_SUserToDepartment.SelectedValue = fieldData.NAME;
                rcb_SUserToDepartment.Text = fieldData.NAME;
                if (fieldData.NAME == "000")
                {
                    txt_Else.Text = fieldData.else_text;
                    txt_Else.Visible = true;
                }
                ///顯示grid
                ///存好fieldData.ALL_Team
                StoreValueIfNotEmpty(fieldData.ALL_Team);
                ////從JSON字符串中反序列化（解析）List<string>
                List<SUserToDepFieldData> list_All = JsonConvert.DeserializeObject<List<SUserToDepFieldData>>(fieldData.ALL_Team);
                //lbl_test.Text = HiddenDataTable.Value;
                //Label2.Text = fieldData.ALL_Team;
                Grid1.DataSource = list_All;
                Grid1.DataBind();
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
    protected void rcb_SUserToDepartment_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
    {
        RadComboBox radComboBox = sender as RadComboBox;

        DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(hf_AllData.Value);

        // 使用 LINQ 查詢，篩選符合條件的資料列
        var queryResult = from row in dataTable.AsEnumerable()
                          where row.Field<string>("NAME").Contains(e.Text)
                           || row.Field<string>("ACCOUNT").Contains(e.Text)
                          select row;

        radComboBox.Items.Clear();

        if (queryResult.Any())
        {
            // 取得查詢結果
            DataTable resultTable = queryResult.CopyToDataTable();

            foreach (DataRow dataRow in resultTable.Rows)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Value = dataRow["NAME"].ToString();
                item.Text = (string)dataRow["NAME"];
                item.Attributes.Add("ACCOUNT", dataRow["ACCOUNT"].ToString());
                radComboBox.Items.Add(item);
                item.DataBind();
            }
        }
        RadComboBoxItem item_else = new RadComboBoxItem();
        item_else.Value = "000";
        item_else.Text = "其他";
        item_else.Attributes.Add("ACCOUNT", "其他");
        radComboBox.Items.Add(item_else);
        item_else.DataBind();
    }

    public void rcb_SUserToDepartment_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (!String.IsNullOrEmpty(rcb_SUserToDepartment.SelectedValue))
        {
            //及時更新
            //直接抓現在選的值
            DataTable User_Guid = utduco.Get_USER_GUID_SUserToDepartmentPO(rcb_SUserToDepartment.SelectedValue);
            string uid = DataTableToString(User_Guid).TrimEnd();
            DataTable resultALL = utduco.Select_UserToDepartment_ALL(uid);
            //用list存起來
            List<SUserToDepFieldData> list_All = new List<SUserToDepFieldData>();
            foreach (DataRow row in resultALL.Rows)
            {
                // 建一個模型並填充數據
                SUserToDepFieldData model = new SUserToDepFieldData
                {
                    NAME = rcb_SUserToDepartment.SelectedValue,
                    USER_GUID = uid,
                    ALL_Team = DataTableToString(resultALL), //全部的資料
                    Name = row["Name"].ToString(),
                    專案 = row["專案"].ToString(),
                    團隊 = row["團隊"].ToString(),
                    部門 = row["部門"].ToString()
                };
                list_All.Add(model); // 將模型添加到列表中
            }
            Grid1.DataSource = list_All;
            Grid1.DataBind();
            //把值存起來
            HiddenNAME.Value = uid;
            HiddenDataTable.Value = JsonConvert.SerializeObject(list_All);
            //lbl_test.Text = HiddenDataTable.Value;
        }
        if (rcb_SUserToDepartment.SelectedValue == "000")
        {
            txt_Else.Visible = true;
        }
        else
        {
            txt_Else.Visible = false;
            txt_Else.Text = "";
        }
    }
    //DataTable轉String
    public static string DataTableToString(DataTable dt)
    {
        StringBuilder sb = new StringBuilder();
        // 輸出表頭
        //foreach (DataColumn dc in dt.Columns)
        //{
        //    sb.Append(dc.ColumnName).Append(",");
        //}
        //sb.Append("\r\n");

        // 輸出資料
        foreach (DataRow dr in dt.Rows)
        {
            foreach (object obj in dr.ItemArray)
            {
                sb.Append(obj);
            }
            //sb.Append("\r\n");
        }

        return sb.ToString();
    }
    public void StoreValueIfNotEmpty(string newValue)
    {
        // 檢查新的值是否為空
        if (!string.IsNullOrEmpty(newValue))
        {
            // 如果新的值不為空，則將其儲存到HiddenDataTable.Value中
            HiddenDataTable.Value = newValue;
        }
        // 如果新的值為空，則不做任何事情，保持上次的值
    }
}