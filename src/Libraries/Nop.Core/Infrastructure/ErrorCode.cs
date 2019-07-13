using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Nop.Core.Infrastructure
{
    public  enum ErrorCode
    {
        #region 1-999 系统错误码
        [Description("操作成功.")]
        sys_success = 0,
        [Description("操作失败,请联系管理员.")]
        sys_fail = 1,
        [Description("参数值格式有误.")]
        sys_param_format_error = 2,
        [Description("应用授权验证失败.")]
        app_token_verify_fail = 11,
        #endregion

        #region
        [Description("请勿频繁发送验证码.")]
         mobile_sms_frequently = 100 ,
        #endregion
    }
}
