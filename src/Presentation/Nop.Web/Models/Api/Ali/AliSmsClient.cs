using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Profile;

namespace Nop.Web.Models.Api.Ali
{
    public class AliSmsClient
    {

        //以下字段换成自己的
        string _product = "Dysmsapi";//短信API产品名称
        string _domain = "dysmsapi.aliyuncs.com";//短信API产品域名
        string _accessKeyId = "xxxx";//你的accessKeyId
        string _accessKeySecret = "xxxxx";//你的accessKeySecret


      

        public  void SendSms(string phone,string code)
        {
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", _accessKeyId, _accessKeySecret);
            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", _product, _domain);

            IAcsClient acsClient = new DefaultAcsClient(profile);

            //var request = new SendSmsRequest();

            //request.SignName = signName;
            //request.TemplateCode = templateCode;
            //request.PhoneNumbers = phoneNumbers;
            //request.TemplateParam = templateParam;

        }
    }


    public class SmsTemplateCode
    {
        const string temcode1 = "SMS_73985016";
    }
}
