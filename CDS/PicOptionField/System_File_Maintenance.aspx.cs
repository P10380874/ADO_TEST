using Ede.Uof.EIP.SystemInfo;
using Ede.Uof.Utility.Page;
using Ede.Uof.Utility.Page.Common;
using PIC_SFM.System_File_Maintenance.System_File_MaintenanceUCO2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIC_SFM.System_File_Maintenance.SysCategory;
using Telerik.Pdf;
using NPOI.SS.Formula.Functions;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Security.Cryptography.Xml;
using CheckBox = DocumentFormat.OpenXml.Wordprocessing.CheckBox;
using DocumentFormat.OpenXml.Packaging;
using NPOI.XSSF.UserModel;
using System.IO;
using NPOI.SS.UserModel;


public partial class CDS_Grid_System_File_Maintenance : BasePage
{
    System_File_MaintenanceUCO2 uco2 = new System_File_MaintenanceUCO2();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            btnQuery_Click(sender, e);
            Dialog.Open2(btn_ReadExcel, "~/CDS/PicOptionField/DialogSYS_F_M_Excel.aspx", "匯入資訊", 1080, 720, Dialog.PostBackType.AfterReturn);
        }
    }


    //模糊查詢
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        //var dt = uco2.Get_Fuzzy_SYS_CATEGORY(txtKeyword.Text, txt_IsENABLED.SelectedValue);
        bool? isEnabled = null;

        if (bool.TryParse(txt_IsENABLED.SelectedValue, out var parsedValue))
        {
            isEnabled = parsedValue;
        }
        //if (isEnabled.HasValue && isEnabled.Value == true) lbl_test.Text = isEnabled.Value.ToString();
        //else lbl_test.Text = "N";
        var dt = uco2.Get_Fuzzy_SYS_CATEGORY(txtKeyword.Text, isEnabled);


        Grid1.DataSource = dt;
        Grid1.DataBind();

        //從資料庫撈出來的TABLE塞給
        //Grid的DataSource
        //再用Grid的  DataBind 將資料建出來

    }

    protected void Grid1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView row = (DataRowView)e.Row.DataItem;
            LinkButton lbtnOrderId = (LinkButton)e.Row.FindControl("lbtnOrderId");
            ExpandoObject param = new { SYS_CATEGORY_ID = row["SYS_CATEGORY_ID"].ToString() }.ToExpando();
        }
    }


    private void BindGrid()
    {
        //DataTable result = uco2.Get_Fuzzy_SYS_CATEGORY(txtKeyword.Text, txt_IsENABLED.SelectedValue);
        bool? isEnabled = null;

        if (bool.TryParse(txt_IsENABLED.SelectedValue, out var parsedValue))
        {
            isEnabled = parsedValue;
        }

        DataTable result = uco2.Get_Fuzzy_SYS_CATEGORY(txtKeyword.Text, isEnabled);

        string type = txt_IsENABLED.SelectedValue;
        Grid1.DataSource = result;
        Grid1.DataBind();
    }


    //新增欄位
    protected void btn_AddDetail_Click(object sender, EventArgs e)
    {
        bool? isEnabled = null;
        System_File_MaintenanceUCO2 uco2 = new System_File_MaintenanceUCO2();
        DataTable dt = uco2.Get_Fuzzy_SYS_CATEGORY(txtKeyword.Text, isEnabled);

        // 插入空白行
        DataRow newRow = dt.NewRow();
        newRow["DEPARTMENT"] = string.Empty;
        newRow["SYS_CATEGORY_ID"] = "自動編號";
        newRow["SYS_CATEGORY_NAME"] = string.Empty;
        newRow["UPDATE_USER"] = string.Empty;
        newRow["UPDATE_DATE"] = DateTime.Now.ToString();
        newRow["ENABLED"] = false;

        int newline = Grid1.PageIndex * Grid1.PageSize;
        dt.Rows.InsertAt(newRow, newline);
        Grid1.DataSource = dt;

        // 進入編輯模式
        Grid1.EditIndex = 0;
        Grid1.DataBind();
        
        // 新增欄位狀態
        ckb_IsNewGridLine.Checked = true;
        //lbl_test.Text = ckb_IsNewGridLine.Checked.ToString();
    }

    protected void btn_search_Click(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void grid_SearchResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        BindGrid();
    }

    //編輯欄位
    protected void grid_SearchResult_RowEditing(object sender, GridViewEditEventArgs e)
    {
        ckb_IsNewGridLine.Checked = false;
        Grid1.EditIndex = e.NewEditIndex;
        BindGrid();
        //鎖住更新ID欄位
        ((TextBox)Grid1.Rows[e.NewEditIndex].FindControl("txt_SYS_CATEGORY_ID")).Enabled = false;

    }
    //取消按鍵
    protected void grid_SearchResult_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        Grid1.EditIndex = -1;
        BindGrid();
    }


    //刪除欄位
    protected void btn_DeleteMutipleDetail_Click(object sender, EventArgs e)
    {
        var selects = Grid1.GetSelectedRowsKeys();
        List<string> selectKeys = new List<string>();
        foreach (var iod in selects)
        {
            foreach (DictionaryEntry item in iod)
            {
                selectKeys.Add(item.Value.ToString());
            }
        }
        if (selectKeys.Count < 1)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('請先勾選資料！');", true);
        }
        else
        {
            uco2.Delete_CATEGORY(selectKeys);
            Grid1.EditIndex = -1;
            BindGrid();
        }
    }

    //更新資料
    protected void grid_SearchResult_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string sup_department = ((TextBox)Grid1.Rows[e.RowIndex].FindControl("txt_DEPARTMENT")).Text;
        //string sup_id = ((TextBox)Grid1.Rows[e.RowIndex].FindControl("txt_SYS_CATEGORY_ID")).Text;
        string sup_name = ((TextBox)Grid1.Rows[e.RowIndex].FindControl("txt_SYS_CATEGORY_NAME")).Text;
        bool enable_type = ((System.Web.UI.WebControls.CheckBox)Grid1.Rows[e.RowIndex].FindControl("CheckBox_ENABLED")).Checked;

        if ( String.IsNullOrEmpty(sup_name))
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('請將內容填寫完整！')", true);
            return;
        }

        SysCategoryModel model = new SysCategoryModel
        {
            DEPARTMENT = sup_department,
            SYS_CATEGORY_NAME = sup_name,
            ENABLED = enable_type,
        };

        string user = Current.Name;
        DateTime now = DateTime.Now;
        string result = "";
        System_File_MaintenanceUCO2 uco2 = new System_File_MaintenanceUCO2();
        if (ckb_IsNewGridLine.Checked) //新增
        {
            if (IsSYS_CATEGORY_NAME_Exists(model.SYS_CATEGORY_NAME))
            {
                result = result.Replace('\'', ' ').Replace('(', ' ').Replace(')', ' ').Replace(',', ' ');
                string alertMsg = "alert('" + "系統別已存在！\\n" + result + "')";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", alertMsg, true);
            }
            else
            {
               // model.SYS_CATEGORY_NAME = result;
                model.CREATE_USER = user;
                model.CREATE_DATE = now;
                model.UPDATE_USER = user;
                model.UPDATE_DATE = now;
                result = uco2.Insert_SYS_CATEGORY(model);
                //ckb_IsNewGridLine.Checked = true;
            }
        }
        
        else //更新
        {
            string ID = Grid1.DataKeys[e.RowIndex]["SYS_CATEGORY_ID"].ToString();
            model.SYS_CATEGORY_ID = ID;
            //lbl_test.Text = ID;
            //更新前檢查系統別是否重複(除了自己之外的重複)
            if (IsSYS_CATEGORY_NAME_ExistsCount(model.SYS_CATEGORY_NAME, model.SYS_CATEGORY_ID))
            {
                result = result.Replace('\'', ' ').Replace('(', ' ').Replace(')', ' ').Replace(',', ' ');
                string alertMsg = "alert('" + "系統別已存在！\\n" + result + "')";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", alertMsg, true);
            }
            else
            {
                object dataKeyValue = Grid1.DataKeys[e.RowIndex].Value;
                model.SYS_CATEGORY_ID = Convert.ToString(dataKeyValue);
                //lbl_test.Text += " || " + dataKeyValue.ToString();
                model.UPDATE_USER = user;
                model.UPDATE_DATE = now;
                result = uco2.Update_SYS_CATEGORY(model);
            }
        }
        if (result == "Y")
        {
            ckb_IsNewGridLine.Checked = false;
            Grid1.EditIndex = -1;
            BindGrid();
        }
        else
        {
            result = result.Replace('\'', ' ').Replace('(', ' ').Replace(')', ' ').Replace(',', ' ');
            string alertMsg = "alert('" + "更新失敗！\\n" + result + "')";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", alertMsg, true);
        }

    }
    //private bool IsSYS_CATEGORY_IDExists(string sysCategoryId)
    //{
    //    // 根據你的資料庫結構和訪問方式，執行查詢檢查 SYS_CATEGORY_ID 是否已存在
    //    string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString;
    //    var m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connStr);
    //    string cmdTxt = "SELECT COUNT(*) FROM [dbo].[CU_SYS_CATEGORY] WHERE [SYS_CATEGORY_ID] = @SYS_CATEGORY_ID";
    //    m_db.AddParameter("@SYS_CATEGORY_ID", sysCategoryId);
    //    int count = Convert.ToInt32(m_db.ExecuteScalar(cmdTxt));
    //    m_db.Dispose();

    //    return count > 0;
    //}
    //判斷名稱是否重複
    private bool IsSYS_CATEGORY_NAME_Exists(string sysCategoryName)
    {
        // 根據你的資料庫結構和訪問方式，執行查詢檢查 SYS_CATEGORY_NAME 是否已存在
        string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString;
        var m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connStr);
        string cmdTxt = "SELECT COUNT(*) FROM [dbo].[CU_SYS_CATEGORY] WHERE [SYS_CATEGORY_NAME] = @SYS_CATEGORY_NAME";
        m_db.AddParameter("@SYS_CATEGORY_NAME", sysCategoryName);
        int count = Convert.ToInt32(m_db.ExecuteScalar(cmdTxt));
        m_db.Dispose();

        return count > 0;
    }
    //計算有幾筆重複並回傳
    private bool IsSYS_CATEGORY_NAME_ExistsCount(string sysCategoryName, string sysCategoryId)
    {
        // 根據你的資料庫結構和訪問方式，執行查詢檢查 SYS_CATEGORY_NAME 是否已存在
        string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString;
        var m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connStr);

        // 加入條件，排除正在修改的那一條記錄
        string cmdTxt = "SELECT COUNT(*) FROM [dbo].[CU_SYS_CATEGORY] WHERE [SYS_CATEGORY_NAME] = @SYS_CATEGORY_NAME AND [SYS_CATEGORY_ID] != @SYS_CATEGORY_ID";

        m_db.AddParameter("@SYS_CATEGORY_NAME", sysCategoryName);
        m_db.AddParameter("@SYS_CATEGORY_ID", sysCategoryId);

        int count = Convert.ToInt32(m_db.ExecuteScalar(cmdTxt));
        m_db.Dispose();

        return count > 0;
    }
    //判斷是否生效標籤
    protected bool IsPrintButtonChecked(object status)
    {
        bool result = false;

        if (status != null && String.Equals(status.ToString(), "True", StringComparison.OrdinalIgnoreCase))
        {
            result = true;
        }
        else if (status != null && String.Equals(status.ToString(), "False", StringComparison.OrdinalIgnoreCase))
        {
            result = false;
        }

        return result;
    }

    //匯入檔案
    protected void btn_ReadExcel_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Dialog.GetReturnValue()))
        {
            BindGrid();
        }
    }
    protected void Grid1_BeforeExport(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        System_File_MaintenanceUCO2 maintainUCO = new System_File_MaintenanceUCO2();
        //DataTable result = maintainUCO.Select_ExportExcel_SYS_CATEGORY(txtKeyword.Text, txt_IsENABLED.SelectedValue);
        bool? isEnabled = null;
        if (bool.TryParse(txt_IsENABLED.SelectedValue, out var parsedValue))
        {
            isEnabled = parsedValue;
        }
        DataTable result = maintainUCO.Select_ExportExcel_SYS_CATEGORY(txtKeyword.Text, isEnabled);
        e.Datasource = result;
    }
    //test
    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        System_File_MaintenanceUCO2 maintainUCO = new System_File_MaintenanceUCO2();
        bool? isEnabled = null;
        if (bool.TryParse(txt_IsENABLED.SelectedValue, out var parsedValue))
        {
            isEnabled = parsedValue;
        }
        DataTable result = maintainUCO.Select_ExportExcel_SYS_CATEGORY(txtKeyword.Text, isEnabled);

        // Export data to Excel
        ExportDataToExcel(result);
    }
    public void ExportDataToExcel(DataTable result)
    {
        // 創建一個新的Excel檔案
        IWorkbook workbook = new XSSFWorkbook();
        ISheet sheet = workbook.CreateSheet("Sheet1");

        // 創建表頭
        IRow headerRow = sheet.CreateRow(0);
        for (int i = 0; i < result.Columns.Count; i++)
        {
            headerRow.CreateCell(i).SetCellValue(result.Columns[i].ColumnName);
        }

        // 填充資料
        for (int i = 0; i < result.Rows.Count; i++)
        {
            IRow row = sheet.CreateRow(i + 1);
            for (int j = 0; j < result.Columns.Count; j++)
            {
                row.CreateCell(j).SetCellValue(result.Rows[i][j].ToString());
            }
        }
        // 將檔案寫入到回應中
        Response.Clear();
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.AddHeader("Content-Disposition", "attachment; filename=系統別資料檔.xlsx");
        byte[] byteArray = null; // 儲存位元組陣列
        try
        {
            using (MemoryStream ms = new MemoryStream()) // 使用 using 語句自動關閉 MemoryStream
            {
                workbook.Write(ms); // 將工作簿寫入記憶體流
                byteArray = ms.ToArray(); // 將記憶體流轉換為位元組陣列
            }
        }
        finally
        {
            // 在 finally 塊中無需關閉 MemoryStream，因為 using 語句會自動關閉它
        }
        Response.BinaryWrite(byteArray); // 使用 BinaryWrite 方法寫入位元組陣列
        Response.End(); // 結束回應
    }
}