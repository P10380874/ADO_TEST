using DocumentFormat.OpenXml.EMMA;
using Newtonsoft.Json;
using PicCDS.SysCategory;
//using PicCostCenter.CostCenter;
//using PicCustomerDB.CustomerDB;
using PicDep.UserToDepartment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;

/// <summary>
/// ValidateAll 的摘要描述
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
// [System.Web.Script.Services.ScriptService]
public class ValidateAll : System.Web.Services.WebService
{

    public ValidateAll()
    {

        //如果使用設計的元件，請取消註解下列一行
        //InitializeComponent(); 
    }

    [WebMethod]
    public string ValidateForm(string formInfo)
    {
        var errors = StartValidate(formInfo);

        XmlDocument returnXmlDoc = new XmlDocument();
        XmlElement returnValueElement = returnXmlDoc.CreateElement("ReturnValue");
        XmlElement statusElement = returnXmlDoc.CreateElement("Status");
        statusElement.InnerText = "1";
        XmlElement exceptionElement = returnXmlDoc.CreateElement("Exception");
        XmlElement messageElement = returnXmlDoc.CreateElement("Message");

        foreach (var error in errors)
        {
            messageElement.InnerText += error + "\n";
            statusElement.InnerText = "0";
        }

        exceptionElement.AppendChild(messageElement);
        returnValueElement.AppendChild(exceptionElement);

        returnValueElement.AppendChild(statusElement);
        returnXmlDoc.AppendChild(returnValueElement);

        return returnXmlDoc.OuterXml;
    }

    private List<string> StartValidate(string formInfo)
    {
        List<string> errors = new List<string>();

        XmlDocument xmlDoc = new XmlDocument();
        try
        {
            // 載入XML字串
            xmlDoc.LoadXml(formInfo);

            // 取得根節點
            XmlNode root = xmlDoc.DocumentElement;
            XmlNode FieldItem = root["FormFieldValue"];
            foreach (XmlNode item in FieldItem.ChildNodes)
            {
                // 尋找所有驗證欄位
                if (item.Attributes != null && 
                   (// 條件式判斷
                    //item.Attributes["fieldId"].InnerText == "CostCenter" ||
                    //item.Attributes["fieldId"].InnerText == "CustomerDB" ||
                    //item.Attributes["fieldId"].InnerText == "SysCategory"||
                    item.Attributes["fieldId"].InnerText == "SUserToDeparment"
                    ))
                {
                    string value = item.InnerText;
                    switch (item.Attributes["fieldId"].InnerText)
                    {// 對應欄位代號
                        //case "CostCenter":
                        //    CostCenterModel costCenterModel = JsonConvert.DeserializeObject<CostCenterModel>(value);
                        //    if (String.IsNullOrEmpty(costCenterModel.COST_CENTER_NUM))
                        //    {
                        //        errors.Add("請輸入成本中心！");
                        //    }
                        //    break;
                        //case "CustomerDB":
                        //    CustomerDBModel customerDBModel = JsonConvert.DeserializeObject<CustomerDBModel>(value);
                        //    if (String.IsNullOrEmpty(customerDBModel.CUSTOMER_ID))
                        //    {
                        //        errors.Add("請輸入客戶名稱！");
                        //    }
                        //    else
                        //    {
                        //        if (customerDBModel.CUSTOMER_ID == "000" && String.IsNullOrEmpty(customerDBModel.ELSE_TEXT))
                        //        {
                        //            errors.Add("請輸入客戶名稱！");
                        //        }
                        //    }
                        //    break;
                        //case "SysCategory":
                        //    SysCategoryModel sysCategoryModel = JsonConvert.DeserializeObject<SysCategoryModel>(value);
                        //    if (String.IsNullOrEmpty(sysCategoryModel.SYS_CATEGORY_ID))
                        //    {
                        //        errors.Add("請輸入系統別！");
                        //    }
                        //    else
                        //    {
                        //        if (sysCategoryModel.SYS_CATEGORY_ID == "000" && String.IsNullOrEmpty(sysCategoryModel.ELSE_TEXT))
                        //        {
                        //            errors.Add("請輸入系統別！");
                        //        }
                        //    }
                        //    break;
                        //加入
                        case "SUserToDeparment":
                            SUserToDepFieldData SUserToDeparmentModel = JsonConvert.DeserializeObject<SUserToDepFieldData>(value);
                            if (String.IsNullOrEmpty(SUserToDeparmentModel.NAME))
                            {
                                errors.Add("請輸入人員名稱！");
                            }
                            else
                            {

                                if (SUserToDeparmentModel.NAME == "000" && String.IsNullOrEmpty(SUserToDeparmentModel.else_text)) //選擇"其他"卻未輸入內容
                                {
                                    errors.Add("請輸入人員名稱！");
                                }
                            }
                            break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errors.Add(ex.ToString());
        }
        return errors;
    }
}
