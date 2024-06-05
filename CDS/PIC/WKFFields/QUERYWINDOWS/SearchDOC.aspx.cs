using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page;
using Ede.Uof.Utility.Page.Common;
using KYTLog;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



/**
* 修改時間：2021/03/26
* 修改人員：梁夢慈
* 修改項目：
    * 1. 新增關鍵字搜尋欄位
* 修改原因：
    * 1. 新增規格
* 修改位置： 
    * 1.「RefreshgvMain()」中，SQL查詢語法中，當有輸入關鍵字時，新增對「DOC_NAME」的條件
    *   「前端網頁」中，新增搜尋欄位(txtSearch)
* **/

/**
* 修改時間：2020/11/10
* 修改人員：梁夢慈
* 修改項目：
    * 1. 對搜尋結果用名稱(DOC_NAME)遞增排序
* 修改原因：
    * 1. 新增規格
* 修改位置： 
    * 1.「RefreshgvMain()」中，新增遞增排序 ORDER BY DOC_NAME ASC
* **/


public partial class CDS_ZENITHTEK_WKFFields_QUERYWINDOW_SearchDOC : BasePage
{
    /// <summary>
    /// 資料庫連通字串
    /// </summary>
    private string ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        ((Master_DialogMasterPage)this.Master).Button2Text = ""; // Button2不顯示
        ((Master_DialogMasterPage)this.Master).Button1Text = ""; // Button1不顯示

        // 取得資料庫連通字串
        ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString;

        if (!Page.IsPostBack) // 首次載入網頁
        {
            gvMain.DataSource = RefreshgvMain();
            gvMain.DataBind();
        }
        else // 如果是POSTBACK
        {
        }
    }


    protected void gvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvMain.PageIndex = e.NewPageIndex;
        gvMain.DataSource = ViewState[gvMain.ID];
        gvMain.DataBind();
    }

    /// <summary>
    /// 取回事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnGet_Click(object sender, EventArgs e)
    {
        LinkButton lbSelect = sender as LinkButton; // 取得點擊按鈕
        GridViewRow gr = lbSelect.NamingContainer as GridViewRow; // 取得所在GridViewRow
        DataTable tblMATNR = ViewState[gvMain.ID] as DataTable; // 取得先前記住的table
        DataRow row = tblMATNR.Rows[gr.DataItemIndex]; // 取得對映DataRow
        DataTable dtReturn = new DataTable();
        DataRow ndr = dtReturn.NewRow();
        foreach (DataColumn dc in tblMATNR.Columns)
        {
            if (!dtReturn.Columns.Contains(dc.ColumnName))
                dtReturn.Columns.Add(new DataColumn(dc.ColumnName, dc.DataType));
            ndr[dc.ColumnName] = row[dc.ColumnName].ToString().Replace("\"", "&#&#&&");
        }
        dtReturn.Rows.Add(ndr);
        Dialog.SetReturnValue2(Newtonsoft.Json.JsonConvert.SerializeObject(dtReturn));
        Dialog.Close(this);
    }

    /// <summary>
    /// 輸入查詢條件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        gvMain.DataSource = RefreshgvMain();
        gvMain.DataBind();
    }

    /// <summary>
    /// 尋找檔案
    /// </summary>
    /// <returns></returns>
    private DataTable RefreshgvMain()
    {
        DataTable dt = new DataTable();
        List<string> listDOC_ID = new List<string>();
        string filter = "";
        if (!string.IsNullOrEmpty(txtSearch.Text))
            filter = " AND DOC_NAME LIKE '%' + @DOC_NAME + '%'";
        using (SqlDataAdapter sda = new SqlDataAdapter(string.Format(@"
            SELECT STORE.FILE_GROUP_ID,STORE.FILE_NAME,PRO.DOC_ID,SHADOW.DOC_NAME,FOLDER_ID FROM TB_DMS_DOC_PROPERTY AS PRO
		 LEFT JOIN TB_DMS_DOC_SHADOW AS SHADOW   ON PRO.DOC_ID     = SHADOW.DOC_ID
		 LEFT JOIN TB_EB_FILE_STORE  AS STORE    ON SHADOW.FILE_ID = STORE.FILE_GROUP_ID		 
         WHERE FOLDER_ID = @FOLDER_ID
         {0}
		 ORDER BY DOC_NAME ASC", filter
        ), ConnectionString))
        using (DataSet ds = new DataSet())
        {
            sda.SelectCommand.Parameters.AddWithValue("@FOLDER_ID", PICFLOW.PICFLOWConfiguration.Default_FOLDER_ID);
            sda.SelectCommand.Parameters.AddWithValue("@DOC_NAME", txtSearch.Text);

            try
            {
                sda.Fill(ds);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                KYTUtilLibs.KYTDebugLog.Log(DebugLog.LogLevel.Error, string.Format("SearchDOC::RefreshgvMain()::錯誤：{0}", ex.Message));
            }
        }

        ViewState[gvMain.ID] = dt;
        return dt;

    }

}