using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Stores;
using Nop.Services.Messages;
using System.Collections.Generic;

namespace Nop.Plugin.SMS.Aliyun.Services
{
    public partial interface IShortMessageTokenProvider
    {
        void AddStoreTokens(IList<Token> tokens, Store store);

        void AddCustomerTokens(IList<Token> tokens, Customer customer);
    }
}
