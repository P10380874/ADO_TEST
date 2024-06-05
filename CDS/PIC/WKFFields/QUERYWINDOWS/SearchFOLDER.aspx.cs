using Ede.Uof.EIP.SystemInfo;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page;
using Ede.Uof.Utility.Page.Common;
using KYTLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class CDS_PIC_WKFFields_QUERYWINDOW_SearchFOLDER : BasePage
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
            BuildFolderTree();
        }
        else // 如果是POSTBACK
        {
        }
    }




    /// <summary>
    /// 取得文管結構
    /// </summary>
    private void BuildFolderTree()
    {
        var dmsfolder = new Ede.Uof.DMS.DMSFolder2();
        var folderDt = dmsfolder.GetFolders(User.Identity.Name, Current.Culture);
        var rootNode = new Telerik.Web.UI.RadTreeNode("文件庫", "DMSRoot"); //lblDMSRoot.Text
        rootNode.ImageUrl = "~/Common/Images/Icon/icon_m15.png";
        rootNode.ExpandedImageUrl = "~/Common/Images/Icon/icon_m15.png";
        trvDMS.Nodes.Add(rootNode);

        ViewState["folderStructure"] = folderDt;

        DataRow[] rows = folderDt.Select("PARENT_ID='DMSRoot'");

        if (rows.Length > 0)
        {
            rootNode.Expanded = true;
            foreach (DataRow row in rows)
            {
                var childNode = new RadTreeNode(row["CHILD_NAME"].ToString(), row["CHILD_ID"].ToString());

                childNode.ImageUrl = "~/Common/Images/Icon/icon_m15.png";
                childNode.ExpandedImageUrl = "~/Common/Images/Icon/icon_m15.png";

                if (folderDt.Select(string.Format("PARENT_ID='{0}'", childNode.Value)).Length > 0)
                    childNode.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;


                rootNode.Nodes.Add(childNode);
            }
        }
    }

    /// <summary>
    /// 資料夾展開
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void trvDMS_NodeExpand(object sender, RadTreeNodeEventArgs e)
    {
        DataTable folderDT = (DataTable)ViewState["folderStructure"];

        DataRow[] rows = folderDT.Select(string.Format("PARENT_ID='{0}'", e.Node.Value));

        if (e.Node.Nodes.Count == 0)
        {
            foreach (DataRow row in rows)
            {
                RadTreeNode node = new RadTreeNode();
                node.ImageUrl = "~/Common/Images/Icon/icon_m15.png";
                node.ExpandedImageUrl = "~/Common/Images/Icon/icon_m15.png";
                node.Text = row["CHILD_NAME"].ToString();
                node.Value = row["CHILD_ID"].ToString();

                if (folderDT.Select(string.Format("PARENT_ID='{0}'", node.Value)).Length > 0)
                    node.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;

                e.Node.Nodes.Add(node);
            }
        }
    }

    /// <summary>
    /// 點選資料夾
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void trvDMS_NodeClick(object sender, RadTreeNodeEventArgs e)
    {
        txtFOLDER_NAME.Text = e.Node.Text;
        hidFOLDER_ID.Value = e.Node.Value;
        KYTUtilLibs.KYTDebugLog.Log(DebugLog.LogLevel.Debug, string.Format("SearchFOLDER::目錄名稱:{0}::目錄FOLDER_ID:{1}", txtFOLDER_NAME.Text, hidFOLDER_ID.Value));
    }

    /// <summary>
    /// 目錄_取回
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGet_Click(object sender, EventArgs e)
    {
        DataTable dtReturn = new DataTable();
        dtReturn.Columns.Add(new DataColumn("FOLDER_NAME", typeof(String)));
        dtReturn.Columns.Add(new DataColumn("FOLDER_ID", typeof(String)));
        DataRow ndr = dtReturn.NewRow();
        ndr["FOLDER_NAME"] = txtFOLDER_NAME.Text.Replace("\"", "&#&#&&");
        ndr["FOLDER_ID"] = hidFOLDER_ID.Value.Replace("\"", "&#&#&&");
        dtReturn.Rows.Add(ndr);
        Dialog.SetReturnValue2(Newtonsoft.Json.JsonConvert.SerializeObject(dtReturn));
        Dialog.Close(this);
    }
}