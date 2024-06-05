using Ede.Uof.EIP.SystemInfo;
using Ede.Uof.Utility.Page;
using Ede.Uof.Utility.Page.Common;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;
using PIC_SFM.System_File_Maintenance.SysCategory;
using PIC_SFM.System_File_Maintenance.System_File_MaintenanceUCO2;
using Telerik.Pdf;
using DocumentFormat.OpenXml.Wordprocessing;

public partial class CDS_PicOptionField_DialogSYS_F_M_Excel : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ((Master_DialogMasterPage)this.Master).Button1Text = "";
        ((Master_DialogMasterPage)this.Master).Button2Text = "儲存資料並返回";
        ((Master_DialogMasterPage)this.Master).Button2OnClick += CDS_PicOptionField_SysParameter_DialogExcel_Button2OnClick;
    }

    private void CDS_PicOptionField_SysParameter_DialogExcel_Button2OnClick()
    {
        List<SysCategoryModel> listModel = JsonConvert.DeserializeObject<List<SysCategoryModel>>(lbl_data.Text);
        System_File_MaintenanceUCO2 System_File_MaintenanceUCO2 = new System_File_MaintenanceUCO2();
        foreach (var model in listModel)
        {
            DateTime now = DateTime.Now;
            model.IMPORT_DATE = now;
            if (model.excel_validate == "新增")
            {
                model.CREATE_DATE = now;
                model.UPDATE_DATE = now;
                System_File_MaintenanceUCO2.Insert_SYS_CATEGORY(model);
            }
            else if (model.excel_validate == "更新")
            {
                model.UPDATE_DATE = now;
                System_File_MaintenanceUCO2.Update_SYS_CATEGORY(model);
            }
            else if (model.excel_validate == "重複")
            {
                model.UPDATE_DATE = now;
                System_File_MaintenanceUCO2.Update_ExcelRepeat_SYS_CATEGORY(model);
            }
        }
        Dialog.SetReturnValue2("Y");
        Dialog.Close(Page);
    }

    protected void btn_readExcel_Click(object sender, EventArgs e)
    {
        if (fileUploadExcel.HasFile)
        {
            string fileName = Path.GetFileName(fileUploadExcel.FileName);
            string fileExtension = Path.GetExtension(fileName);
            if (fileExtension == ".xlsx" || fileExtension == ".xls")
            {
                string folderPath = Path.Combine(ConfigurationManager.AppSettings["FileStorageFolder"], "FileCenter");
                string tempFilePath = Path.Combine(folderPath, fileName);
                fileUploadExcel.SaveAs(tempFilePath);

                using (FileStream file = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook;
                    if (fileExtension == ".xls") workbook = new HSSFWorkbook(file);
                    else workbook = new XSSFWorkbook(file);

                    ISheet sheet = workbook.GetSheetAt(0);
                    if (sheet != null)
                    {
                        List<SysCategoryModel> listModel = ReadSheet(sheet);
                        BindGrid(listModel);
                        lbl_data.Text = JsonConvert.SerializeObject(listModel);
                    }
                    File.Delete(tempFilePath);
                }
            }
            else
            {
                string alertMsg = "alert('請上傳 .xlsx 或 .xls 格式的檔案！')";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", alertMsg, true);
            }
        }
    }
    //"開始讀入資料"的那個按鈕
    private List<SysCategoryModel> ReadSheet(ISheet sheet)
    {
        bool? isEnabled = null;
        List<SysCategoryModel> result = new List<SysCategoryModel>();
        System_File_MaintenanceUCO2 System_File_MaintenanceUCO2 = new System_File_MaintenanceUCO2();
        DataTable dt = System_File_MaintenanceUCO2.Get_Fuzzy_SYS_CATEGORY(string.Empty, isEnabled);
        string user = Current.Name;

        for (int i = 0; i <= sheet.LastRowNum; i++)
        {
            if (i == 0) continue;

            IRow row = sheet.GetRow(i);
            if (row != null)
            {
                SysCategoryModel model = new SysCategoryModel();
                for (int j = 0; j < row.LastCellNum; j++)
                {
                    ICell cell = row.GetCell(j);
                    if (cell != null)
                    {
                        switch (j)
                        {
                            case 0:
                                model.SYS_CATEGORY_ID = cell.ToString();
                                break;
                            case 1:
                                model.SYS_CATEGORY_NAME = cell.ToString();
                                break;
                            case 2:
                                model.DEPARTMENT = cell.ToString();
                                break;
                            case 3:
                                model.UPDATE_USER = cell.ToString();
                                break;
                            case 4:
                                string dateStr = cell.ToString();
                                if (DateTime.TryParse(dateStr, out DateTime updateDate))
                                {
                                    model.UPDATE_DATE = updateDate.Date;
                                }
                                else
                                {
                                    // 轉換失敗時的處理邏輯，例如設定為某個預設日期或拋出錯誤訊息
                                    model.excel_validate = "錯誤";
                                }
                                break;
                            case 5:
                                string enabledString = cell.ToString();
                                if (string.Equals(enabledString, "False", StringComparison.OrdinalIgnoreCase))
                                {
                                    model.ENABLED = false;
                                }
                                else if (string.Equals(enabledString, "True", StringComparison.OrdinalIgnoreCase))
                                {
                                    model.ENABLED = true;
                                }
                                else
                                {
                                    // 處理轉換失敗的情況，例如可以設定默認值或者記錄錯誤信息
                                    model.excel_validate = "錯誤";
                                }
                                break;

                        }
                    }
                }
                if (ValidateModel(model))
                {
                    model.excel_validate = "錯誤";
                }
                else
                {
                    model.excel_validate = ValidateDuplicate(model, dt, result);
                    if (model.excel_validate == "更新" || model.excel_validate == "重複") model.UPDATE_USER = user;
                    else model.CREATE_USER = user;
                    //    if (IsUpdate_DB(model, dt))
                    //    {
                    //        model.excel_validate = "更新";
                    //        model.UPDATE_USER = user;
                    //    }
                    //    else if (IsDuplicate_List(model, result))
                    //    {
                    //        model.excel_validate = "重複";
                    //        model.UPDATE_USER = user;
                    //    }
                    //    else
                    //    {
                    //        model.excel_validate = "新增";
                    //        model.CREATE_USER = user;
                    //    }
                }
                model.excel_seq = i + 1;
                result.Add(model);
            }
        }
        return result;
    }
    /// <summary>
    /// 2024/01/26 整合:判斷為更新、重複還是錯誤性的資料
    /// </summary>
    /// <param name="model"></param>
    /// <param name="db_data"></param>
    /// <param name="resultList"></param>
    /// <returns></returns>
    private string ValidateDuplicate(SysCategoryModel model, DataTable db_data, List<SysCategoryModel> resultList)
    {
        string validateResult = "";

        // 使用 LINQ 查詢是否有符合條件的資料列
        var matchingRows = from DataRow row in db_data.Rows
                           where (string)row["SYS_CATEGORY_ID"] == model.SYS_CATEGORY_ID
                           select row;

        // 如果有匹配的資料列，則表示模型存在於 DataTable 中
        if (matchingRows.Any())
        {
            // 取得第一個匹配的資料列
            DataRow matchingRow = matchingRows.First();

            // 設定更新條件Key值
            model.SYS_CATEGORY_ID = (string)matchingRow["SYS_CATEGORY_ID"];
            validateResult = "更新";
        }
        else if (resultList.Any(item => model.SYS_CATEGORY_ID == item.SYS_CATEGORY_ID))
        {
            validateResult = "重複";
        }
        else
        {
            validateResult = "新增";
        }

        // 驗證 SYS_CATEGORY_NAME 是否重複
        bool haveError = false;
        var matchName_DB = from DataRow row in db_data.Rows
                           where (string)row["SYS_CATEGORY_NAME"] == model.SYS_CATEGORY_NAME
                           select row;

        if (matchName_DB.Any()) // 資料庫中是否有重複名稱
        {
            DataRow firstMatch = matchName_DB.First(); // 理論上資料庫中只會有1筆
            string sysCategoryIdInDB = (string)firstMatch["SYS_CATEGORY_ID"];
            if (model.SYS_CATEGORY_ID != sysCategoryIdInDB) // 如相等則代表為同資料更新 不同則為資料改名
            {
                haveError = true;
            }
        }
        else if (resultList.Any(item => item.SYS_CATEGORY_NAME == model.SYS_CATEGORY_NAME)) // Excel 中是否有相同名稱
        {
            haveError = true;
        }

        if (haveError) // 有重複的 SYS_CATEGORY_NAME
        {
            validateResult = "系統別重複";
        }

        return validateResult;
    }

    //private bool IsUpdate_DB(SysCategoryModel model, DataTable dataTable)
    //{
    //    var matchingRows = from DataRow row in dataTable.Rows
    //                       where (string)row["SYS_CATEGORY_ID"] == model.SYS_CATEGORY_ID
    //                       select row;

    //    if (matchingRows.Any())
    //    {
    //        DataRow matchingRow = matchingRows.First();
    //        model.SYS_CATEGORY_ID = (string)matchingRow["SYS_CATEGORY_ID"];
    //        return true;
    //    }
    //    return false;
    //}
    //private bool IsDuplicate_List(SysCategoryModel testModel, List<SysCategoryModel> resultList)
    //{
    //    return resultList.Any(model =>
    //    model.SYS_CATEGORY_ID == testModel.SYS_CATEGORY_ID);
    //}

    private bool ValidateModel(SysCategoryModel model)
    {
        bool result_haveError = false;
        //如果 SYS_CATEGORY_ID 或 SYS_CATEGORY_NAME 為空，則設定為有錯誤
        if (String.IsNullOrEmpty(model.SYS_CATEGORY_ID) || String.IsNullOrEmpty(model.SYS_CATEGORY_NAME)) //字軌檢測: 是空值或錯誤值
        {
            result_haveError = true;      
        }
        else result_haveError = false;
        return result_haveError;
    }


    private void BindGrid(List<SysCategoryModel> list)
    {
        grid_SearchResult.DataSource = list;
        grid_SearchResult.DataBind();
    }
}
