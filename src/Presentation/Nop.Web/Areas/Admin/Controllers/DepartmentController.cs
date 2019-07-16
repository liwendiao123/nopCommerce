using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Customers;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Departments;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Web.Areas.Admin.Controllers
{
    public class DepartmentController : BaseAdminController
    {

        #region Field
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IDepartmentService _departmentService;
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly IPictureService _pictureService;
        private readonly IDepartmentFactory _departmentFactory;
        private readonly ICustomerActivityService _customerActivityService;
        #endregion



        public DepartmentController(
            IDepartmentService departmentService,
            ILocalizedEntityService localizedEntityService,
            ILocalizationService localizationService,
            INotificationService notificationService,
            IPermissionService permissionService,
            IPictureService pictureService,
            IDepartmentFactory departmentFactory,
            ICustomerActivityService customerActivityService
            )
        {
            _departmentService = departmentService;
            _localizedEntityService = localizedEntityService;
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _pictureService = pictureService;
            _departmentFactory = departmentFactory;
            _customerActivityService = customerActivityService;
        }
  

        #region Departments

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageVendors))
                return AccessDeniedView();

            //prepare model
            var model = _departmentFactory.PrepareDepSearchModel(new DepartmentSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(DepartmentSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.BookModelManage))
                return AccessDeniedDataTablesJson();

            //prepare model
            var model = _departmentFactory.PrepareDepartmentListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageVendors))
                return AccessDeniedView();

            //prepare model
            var model = _departmentFactory.PrepareDepModel(new DepartmentModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual IActionResult Create(DepartmentModel model, bool continueEditing, IFormCollection form)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.BookModelManage))
                return AccessDeniedView();

            //parse vendor attributes
            //var vendorAttributesXml = ParseVendorAttributes(form);
            //_vendorAttributeParser.GetAttributeWarnings(vendorAttributesXml).ToList()
            //    .ForEach(warning => ModelState.AddModelError(string.Empty, warning));

            if (ModelState.IsValid)
            {
                var vendor = model.ToEntity<Department>();
                // .InsertVendor(vendor);
                vendor.CreatedOnUtc = DateTime.Now;
                vendor.UpdatedOnUtc = DateTime.Now;

                _departmentService.InsertDep(vendor);

                //activity log
                _customerActivityService.InsertActivity("AddNewVendor",
                    string.Format(_localizationService.GetResource("ActivityLog.AddNewVendor"), vendor.Id), vendor);

                //search engine name
                //model.SeName = _urlRecordService.ValidateSeName(vendor, model.SeName, vendor.Name, true);
                //_urlRecordService.SaveSlug(vendor, model.SeName, 0);

                //address
                //var address = model.Address.ToEntity<Address>();
                //address.CreatedOnUtc = DateTime.UtcNow;

                //some validation
              

                //locales
                UpdateLocales(vendor, model);

                //update picture seo file name
              //  UpdatePictureSeoNames(vendor);

                _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Vendors.Added"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = vendor.Id });
            }

            //prepare model
            model = _departmentFactory.PrepareDepModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageVendors))
                return AccessDeniedView();

            //try to get a vendor with the specified id
            var dep = _departmentService.GetDepById(id);
            if (dep == null || dep.Deleted)
                return RedirectToAction("List");

            //prepare model
            var model = _departmentFactory.PrepareDepModel(null, dep);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(DepartmentModel model, bool continueEditing, IFormCollection form)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageVendors))
                return AccessDeniedView();

            //try to get a vendor with the specified id
            //var vendor = _vendorService.GetVendorById(model.Id);
            var vendor = _departmentService.GetDepById(model.Id);
            if (vendor == null || vendor.Deleted)
                return RedirectToAction("List");

            //parse vendor attributes
            //var vendorAttributesXml = ParseVendorAttributes(form);
            //_vendorAttributeParser.GetAttributeWarnings(vendorAttributesXml).ToList()
            //    .ForEach(warning => ModelState.AddModelError(string.Empty, warning));

            if (ModelState.IsValid)
            {
               // var prevPictureId = vendor.PictureId;
                vendor = model.ToEntity(vendor);
                //   _vendorService.UpdateVendor(vendor);


                _departmentService.UpdateDep(vendor);

                //vendor attributes
              //  _genericAttributeService.SaveAttribute(vendor, NopVendorDefaults.VendorAttributes, vendorAttributesXml);

                //activity log
                _customerActivityService.InsertActivity("EditDepartment",
                    string.Format(_localizationService.GetResource("ActivityLog.EditDepartment"), vendor.Id), vendor);

                //search engine name
                //model.SeName = _urlRecordService.ValidateSeName(vendor, model.SeName, vendor.Name, true);
                //_urlRecordService.SaveSlug(vendor, model.SeName, 0);

   
      

                //locales
                UpdateLocales(vendor, model);

                //delete an old picture (if deleted or updated)
                //if (prevPictureId > 0 && prevPictureId != vendor.PictureId)
                //{
                //    var prevPicture = _pictureService.GetPictureById(prevPictureId);
                //    if (prevPicture != null)
                //        _pictureService.DeletePicture(prevPicture);
                //}
                //update picture seo file name
              //  UpdatePictureSeoNames(vendor);

                _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Vendors.Updated"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = vendor.Id });
            }

            //prepare model
            model =_departmentFactory.PrepareDepModel(model, vendor, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageVendors))
                return AccessDeniedView();

            //try to get a vendor with the specified id
            var vendor =_departmentService.GetDepById(id);
            if (vendor == null)
                return RedirectToAction("List");

            //clear associated customer references
            //var associatedCustomers = _customerService.GetAllCustomers(vendorId: vendor.Id);
            //foreach (var customer in associatedCustomers)
            //{
            //    customer.VendorId = 0;
            //    _customerService.UpdateCustomer(customer);
            //}

            //delete a Department
            //  _vendorService.DeleteVendor(vendor);
            _departmentService.DeleteDep(vendor);
            //activity log
            _customerActivityService.InsertActivity("DeleteDepartment",
                string.Format(_localizationService.GetResource("ActivityLog.DeleteDepartment"), vendor.Id), vendor);

            _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Department.Deleted"));

            return RedirectToAction("List");
        }


        #endregion
        protected virtual void UpdateLocales(Department vendor, DepartmentModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(vendor,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(vendor,
                    x => x.VatCode,
                    localized.VatCode,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(vendor,
                    x => x.Tel,
                    localized.Tel,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(vendor,
                    x => x.Email,
                    localized.Email,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(vendor,
                    x => x.MainUrl,
                    localized.MainUrl,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(vendor,
                          x => x.ImgUrl,
                          localized.ImgUrl,
                          localized.LanguageId);
                _localizedEntityService.SaveLocalizedValue(vendor,
                   x => x.Desc,
                   localized.Desc,
                   localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(vendor,
                             x => x.ContactPerson,
                             localized.ContactPerson,
                             localized.LanguageId);

                //search engine name
                //var seName = _urlRecordService.ValidateSeName(vendor, localized.SeName, localized.Name, false);
                //_urlRecordService.SaveSlug(vendor, seName, localized.LanguageId);
            }
        }
    }
}