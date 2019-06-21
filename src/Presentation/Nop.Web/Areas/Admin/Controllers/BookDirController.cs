using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.TableOfContent;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Models.TableOfContent;

namespace Nop.Web.Areas.Admin.Controllers
{
    public class BookDirController : BaseAdminController
    {
       
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;     
        private readonly IUrlRecordService _urlRecordService;
        private readonly IBookDirService _bookDirService;
        private readonly IBookDirFactory _bookDirFactory;

        public BookDirController(IUrlRecordService urlRecordService, 
            IPermissionService permissionService,
            ICustomerActivityService customerActivityService,
            IEventPublisher eventPublisher, 
            ILocalizationService localizationService,
            INotificationService notificationService, 
            IBookDirService bookDirService,
            IBookDirFactory _bookDirFactory
            )
        {
            _urlRecordService = urlRecordService;
            _permissionService = permissionService;
            _customerActivityService = customerActivityService;
            _eventPublisher = eventPublisher;
            _localizationService = localizationService;
            _notificationService = notificationService;
            _bookDirService = bookDirService;

        }
        public IActionResult Index()
        {


            if (!_permissionService.Authorize(StandardPermissionProvider.ManagerBook))
                return AccessDeniedView();

            //prepare model
            var model = _bookDirFactory.PrepareBookDirSearchModel(new BookDirSearchModel(),new BookDirModel());

            return View(model);
        }
    }
}