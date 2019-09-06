using System.Collections.Generic;
using Nop.Web.Framework.Models;
using Nop.Web.Models.Common;

namespace Nop.Web.Models.Checkout
{

    /// <summary>
    /// 账单地址模型
    /// </summary>
    public partial class CheckoutBillingAddressModel : BaseNopModel
    {
        public CheckoutBillingAddressModel()
        {

            
            ExistingAddresses = new List<AddressModel>();
            InvalidExistingAddresses = new List<AddressModel>();
            BillingNewAddress = new AddressModel();
        }


        /// <summary>
        /// 已存在地址
        /// </summary>
        public IList<AddressModel> ExistingAddresses { get; set; }
        /// <summary>
        /// 不可用地址
        /// </summary>
        public IList<AddressModel> InvalidExistingAddresses { get; set; }
        /// <summary>
        /// 计费新地址
        /// </summary>
        public AddressModel BillingNewAddress { get; set; }

        /// <summary>
        /// 运送到同一地址
        /// </summary>
        public bool ShipToSameAddress { get; set; }

        /// <summary>
        /// 装运到允许的相同地址
        /// </summary>
        public bool ShipToSameAddressAllowed { get; set; }

        /// <summary>
        /// Used on one-page checkout page
        /// 用于单页结帐页面
        /// </summary>
        public bool NewAddressPreselected { get; set; }
    }
}