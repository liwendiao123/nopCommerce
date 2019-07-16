using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Core.Domain.Customers;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Seo;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Departments;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Web.Areas.Admin.Factories
{
    public class DepartmentFactory : IDepartmentFactory
    {

        #region

        #region Fields

        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly ICustomerService _customerService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IDepartmentService _departmentService;

        #endregion

        #endregion

        public DepartmentFactory(
        IBaseAdminModelFactory baseAdminModelFactory,
        ICustomerService customerService,
        IDateTimeHelper dateTimeHelper,
        IGenericAttributeService genericAttributeService,
        ILocalizationService localizationService,
        ILocalizedModelFactory localizedModelFactory,
        IUrlRecordService urlRecordService,
        IDepartmentService departmentService

            )
        {
            _baseAdminModelFactory = baseAdminModelFactory;
            _customerService = customerService;
            _dateTimeHelper = dateTimeHelper;
            _genericAttributeService = genericAttributeService;
            _localizationService = localizationService;
            _localizedModelFactory = localizedModelFactory;
            _urlRecordService = urlRecordService;
            _departmentService = departmentService;
        }

        public DepartmentListModel PrepareDepartmentListModel(DepartmentSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get Deps

            var deps = _departmentService.GetAllDeps("");
           // var vendors = _vendorService.GetAllVendors(showHidden: true,
                //name: searchModel.SearchName,
                //pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

            //prepare list model
            var model = new DepartmentListModel().PrepareToGrid(searchModel, deps, () =>
            {
                //fill in model values from the entity
                return deps.Select(vendor =>
                {
                    var vendorModel = vendor.ToModel<DepartmentModel>();
                   // vendorModel.SeName = _urlRecordService.GetSeName(vendor, 0, true, false);

                    return vendorModel;
                });
            });

            return model;
        }

        public DepartmentModel PrepareDepModel(DepartmentModel model, Department department, bool excludeProperties = false)
        {
            Action<DepartmentModel, int> localizedModelConfiguration = null;

            if (department != null)
            {
                //fill in model values from the entity
                if (model == null)
                {
                    model = department.ToModel<DepartmentModel>();
                  //  model.SeName = _urlRecordService.GetSeName(vendor, 0, true, false);
                }

                //define localized model configuration action
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Name = _localizationService.GetLocalized(department, entity => entity.Name, languageId, false, false);
                    locale.VatCode = _localizationService.GetLocalized(department, entity => entity.VatCode, languageId, false, false);
                    locale.Tel = _localizationService.GetLocalized(department, entity => entity.Tel, languageId, false, false);
                    locale.Email = _localizationService.GetLocalized(department, entity => entity.Email, languageId, false, false);
                    locale.MainUrl = _localizationService.GetLocalized(department, entity => entity.MainUrl, languageId, false, false);
                    locale.ImgUrl = _localizationService.GetLocalized(department, entity => entity.ImgUrl, languageId, false, false);
                    locale.Desc = _localizationService.GetLocalized(department, entity => entity.Desc, languageId, false, false);
                    locale.ContactPerson = _localizationService.GetLocalized(department, entity => entity.ContactPerson, languageId, false, false);
                };
            }
                //prepare associated customers
              //  PrepareAssociatedCustomerModels(model.AssociatedCustomers, vendor);

                //prepare nested search models
               // PrepareVendorNoteSearchModel(model.VendorNoteSearchModel, vendor);
            

            //set default values for the new model
            if(department == null)
            {
              //  model.PageSize = 6;
                model.Active = true;
               // model.AllowCustomersToSelectPageSize = true;
              //  model.PageSizeOptions = _vendorSettings.DefaultVendorPageSizeOptions;
            }

            //prepare localized models
           // if (!excludeProperties)
              //  model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            //prepare model vendor attributes
           // PrepareVendorAttributeModels(model.VendorAttributes, vendor);

            //prepare address model
           // var address = _addressService.GetAddressById(vendor?.AddressId ?? 0);
            //if (!excludeProperties && address != null)
            //    model.Address = address.ToModel(model.Address);
            //PrepareAddressModel(model.Address, address);

            return model;
        }

        public DepartmentSearchModel PrepareDepSearchModel(DepartmentSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }
    }
}
