using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Areas.Admin.Infrastructure.Cache;
using Nop.Core.Caching;
using Nop.Services.Catalog;
using Nop.Services.Vendors;
using Nop.Services.TableOfContent;
using Nop.Core.Domain.Customers;

namespace Nop.Web.Areas.Admin.Helpers
{
    /// <summary>
    /// Select list helper
    /// </summary>
    public static class SelectListHelper
    {
        /// <summary>
        /// Get category list
        /// </summary>
        /// <param name="categoryService">Category service</param>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Category list</returns>
        public static List<SelectListItem> GetCategoryList(ICategoryService categoryService, ICacheManager cacheManager, bool showHidden = false)
        {
            if (categoryService == null)
                throw new ArgumentNullException(nameof(categoryService));

            if (cacheManager == null)
                throw new ArgumentNullException(nameof(cacheManager));

            var cacheKey = string.Format(NopModelCacheDefaults.CategoriesListKey, showHidden);
            var listItems = cacheManager.Get(cacheKey, () =>
            {
                var categories = categoryService.GetAllCategories(showHidden: showHidden);
                return categories.Select(c => new SelectListItem
                {
                    Text = categoryService.GetFormattedBreadCrumb(c, categories),
                    Value = c.Id.ToString()
                });
            });

            var result = new List<SelectListItem>();
            //clone the list to ensure that "selected" property is not set
            foreach (var item in listItems)
            {
                result.Add(new SelectListItem
                {
                    Text = item.Text,
                    Value = item.Value
                });
            }

            return result;
        }

        /// <summary>
        /// Get manufacturer list
        /// </summary>
        /// <param name="manufacturerService">Manufacturer service</param>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Manufacturer list</returns>
        public static List<SelectListItem> GetManufacturerList(IManufacturerService manufacturerService, ICacheManager cacheManager, bool showHidden = false)
        {
            if (manufacturerService == null)
                throw new ArgumentNullException(nameof(manufacturerService));

            if (cacheManager == null)
                throw new ArgumentNullException(nameof(cacheManager));

            var cacheKey = string.Format(NopModelCacheDefaults.ManufacturersListKey, showHidden);
            var listItems = cacheManager.Get(cacheKey, () =>
            {
                var manufacturers = manufacturerService.GetAllManufacturers(showHidden: showHidden);
                return manufacturers.Select(m => new SelectListItem
                {
                    Text = m.Name,
                    Value = m.Id.ToString()
                });
            });

            var result = new List<SelectListItem>();
            //clone the list to ensure that "selected" property is not set
            foreach (var item in listItems)
            {
                result.Add(new SelectListItem
                {
                    Text = item.Text,
                    Value = item.Value
                });
            }

            return result;
        }

        /// <summary>
        /// Get vendor list
        /// </summary>
        /// <param name="vendorService">Vendor service</param>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Vendor list</returns>
        public static List<SelectListItem> GetVendorList(IVendorService vendorService, ICacheManager cacheManager, bool showHidden = false)
        {
            if (vendorService == null)
                throw new ArgumentNullException(nameof(vendorService));

            if (cacheManager == null)
                throw new ArgumentNullException(nameof(cacheManager));

            var cacheKey = string.Format(NopModelCacheDefaults.VendorsListKey, showHidden);
            var listItems = cacheManager.Get(cacheKey, () =>
            {
                var vendors = vendorService.GetAllVendors(showHidden: showHidden);
                return vendors.Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                });
            });

            var result = new List<SelectListItem>();
            //clone the list to ensure that "selected" property is not set
            foreach (var item in listItems)
            {
                result.Add(new SelectListItem
                {
                    Text = item.Text,
                    Value = item.Value
                });
            }

            return result;
        }

        public static List<SelectListItem> GetBookDirList(IBookDirService bookDirService,Customer customer = null,int cateId = 0, int bookIds = 0, ICacheManager cacheManager = null, bool showHidden = false)
        {
            if (bookDirService == null)
                throw new ArgumentNullException(nameof(bookDirService));

            if (cacheManager == null)
                throw new ArgumentNullException(nameof(cacheManager));

            var cacheKey = string.Format(NopModelCacheDefaults.BookDirListKey, showHidden.ToString() + cateId.ToString() + bookIds.ToString());
            var listItems = cacheManager.Get(cacheKey, () =>
            {
                //   var vendors = bookDirService.GetAllVendors(showHidden: showHidden);
                var vendors = bookDirService.GetAllBookDirsData("",cateId, bookIds);
                return vendors.Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                });
            });

            var result = new List<SelectListItem>();
            //clone the list to ensure that "selected" property is not set
            foreach (var item in listItems)
            {
                result.Add(new SelectListItem
                {
                    Text = item.Text,
                    Value = item.Value
                });
            }

            return result;
        }


        /// <summary>
        /// 获取书籍列表
        /// </summary>
        /// <param name="bookservice"></param>
        /// <param name="cateids"></param>
        /// <param name="customer"></param>
        /// <param name="cacheManager"></param>
        /// <param name="showHidden"></param>
        /// <returns></returns>
        public static List<SelectListItem> GetBookList(IProductService bookservice, List<int> cateids, Customer customer = null, ICacheManager cacheManager = null, bool showHidden = false)
        {
            if (bookservice == null)
                throw new ArgumentNullException(nameof(bookservice));

            if (cacheManager == null)
                throw new ArgumentNullException(nameof(cacheManager));

            var cacheKey = string.Format(NopModelCacheDefaults.BookListKey, showHidden.ToString()+ string.Join(",",cateids));
            var listItems = cacheManager.Get(cacheKey, () =>
            {
                //   var vendors = bookDirService.GetAllVendors(showHidden: showHidden);
                var products = bookservice.SearchProducts(showHidden: true,
                 categoryIds: cateids,
                 manufacturerId: 0,
                 storeId: 0,
                 vendorId: 0,
                 warehouseId: 0,
                 productType: null,
                 keywords: null,
                 pageIndex: 0,
                 pageSize: int.MaxValue);
                return products.Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                });
            });

            var result = new List<SelectListItem>();
            //clone the list to ensure that "selected" property is not set
            foreach (var item in listItems)
            {
                result.Add(new SelectListItem
                {
                    Text = item.Text,
                    Value = item.Value
                });
            }

            return result;
        }

    }
}