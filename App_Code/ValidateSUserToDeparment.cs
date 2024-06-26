﻿using Newtonsoft.Json;
using PicDep.UserToDepartment;
using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Xml;

/// <summary>
/// ValidateSUserToDeparment 的摘要描述
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
// [System.Web.Script.Services.ScriptService]
public class ValidateSUserToDeparment : System.Web.Services.WebService
{

    public ValidateSUserToDeparment()
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
            // 讀取節點資訊
            foreach (XmlNode item in FieldItem.ChildNodes)
            {
                //尋找專案欄位
                if (item.Attributes != null && item.Attributes["fieldId"].InnerText == "SUserToDeparment")
                {
                    string value = item.InnerText;
                    SUserToDepFieldData model = JsonConvert.DeserializeObject<SUserToDepFieldData>(value);
                    if (String.IsNullOrEmpty(model.NAME))
                    {
                        errors.Add("請輸入人員名稱！");
                    }
                    else
                    {

                        if (model.NAME == "000" && String.IsNullOrEmpty(model.else_text)) //選擇"其他"卻未輸入內容
                        {
                            errors.Add("請輸入人員名稱！");
                        }
                    }
                    return errors;
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
