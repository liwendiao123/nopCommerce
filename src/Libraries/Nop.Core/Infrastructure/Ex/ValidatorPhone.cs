using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Nop.Core.Infrastructure.Ex
{
   public static class ValidatorPhone
    {
        public static bool CheckMobile(this string mobile)
        {

            String regex = "^((13[0-9])|(14[5,7])|(15[0-3,5-9])|(17[0,3,5-8])|(18[0-9])|166|198|199|(147))\\d{8}$";
            var result = Regex.IsMatch(mobile, regex);

            return result;

            // return Common.Global.CheckMobile(mobile);
        }
    }
}
