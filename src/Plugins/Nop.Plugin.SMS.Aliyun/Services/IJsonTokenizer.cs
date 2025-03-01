﻿using Nop.Services.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.SMS.Aliyun.Services
{
    public partial interface IJsonTokenizer
    {
        /// <summary>
        /// Replace all of the token key occurrences inside the specified template text with corresponded token values
        /// </summary>
        /// <param name="template">The template with token keys inside</param>
        /// <param name="tokens">The sequence of tokens to use</param>
        /// <param name="htmlEncode">The value indicating whether tokens should be HTML encoded</param>
        /// <returns>Text with all token keys replaces by token value</returns>
        string Replace(string template, IEnumerable<Token> tokens, bool htmlEncode);
    }
}
