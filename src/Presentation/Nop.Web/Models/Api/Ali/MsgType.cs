using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Web.Models.Api.Ali
{

   /// <summary>
   /// 消息类型
   /// </summary>
    public enum MsgType
    {
        /// <summary>
        /// 注册
        /// </summary>
        Register = 0,
        /// <summary>
        /// 重置密码
        /// </summary>
        ResetPassword = 1,
        /// <summary>
        /// 登录
        /// </summary>
        Login = 2,

        /// <summary>
        /// 更新角色
        /// </summary>
        UpdateRole = 3
    }
}
