using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Web.Models.Api.Ali
{
    public class AliSmsTemplateManager
    {
         static AliSmsTemplateManager()
        {
            Init();
           
        }


        public static List<AliSmsTemp> AliSmsTemps =  new List<AliSmsTemp>();


        public static AliSmsTemp GetAliSmsTemByType(int type)
        {
            var ast = new AliSmsTemp();


            var tem = AliSmsTemps.FirstOrDefault(x => x.Type == type);
            return tem;
        }


        private static void Init()
        {



            AliSmsTemps =   new List<AliSmsTemp>() {
                        new AliSmsTemp {
                            Type = 0,
                            MsgCode = "SMS_148990154",
                            MsgTemContent = "验证码{0}，您正在注册成为新用户，感谢您的支持！"
                        },
                         new AliSmsTemp {
                             Type = 1,
                            MsgCode = "SMS_148990153",
                             MsgTemContent ="验证码{0}，您正在尝试修改登录密码，请妥善保管账户信息。",
                        },
                            new AliSmsTemp {
                                Type = 2,
                            MsgCode = "SMS_148990156",
                             MsgTemContent ="验证码{0}，您正在登录，若非本人操作，请勿泄露。",
                        },
                        new AliSmsTemp
                        {
                            Type = -1,
                            MsgCode = "SMS_148990157",
                            MsgTemContent = "验证码{0}，您正在进行身份验证，打死不要告诉别人哦！"
                        },
                     
                       new AliSmsTemp {
                           Type = -1,
                            MsgCode = "SMS_148990155",
                            MsgTemContent = "验证码{0}，您正尝试异地登录，若非本人操作，请勿泄露。"
                        },
                     
                   
                       
                           new AliSmsTemp {
                               Type = -1,
                            MsgCode = "SMS_148990152",
                             MsgTemContent ="验证码{0}，您正在尝试变更重要信息，请妥善保管账户信息。",
                        }

                   };
        }

        


    }


    public class AliSmsTemp
    {
        public int Type { get; set; }


        public string MsgCode { get; set; }


        public string MsgTemContent { get; set; }
    }


}
