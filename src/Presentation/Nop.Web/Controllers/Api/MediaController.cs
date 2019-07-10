using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Services.Media;

namespace Nop.Web.Controllers.Api
{
    public class MediaController : Controller
    {

     
        #region Fields

        private readonly IPictureService _pictureService;



        #endregion


        private MediaController(IPictureService pictureService)
        {
            _pictureService = pictureService;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        //do not validate request token (XSRF)
        //[AdminAntiForgery(true)]
        public virtual IActionResult AsyncUpload()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.UploadPictures))
            //    return Json(new { success = false, error = "You do not have required permissions" }, "text/plain");

            var httpPostedFile = Request.Form.Files.FirstOrDefault();
            if (httpPostedFile == null)
            {
                return Json(new
                {
                    success = false,
                    message = "No file uploaded"
                });
            }

            const string qqFileNameParameter = "qqfilename";

            var qqFileName = Request.Form.ContainsKey(qqFileNameParameter)
                ? Request.Form[qqFileNameParameter].ToString()
                : string.Empty;

            var picture = _pictureService.InsertPicture(httpPostedFile, qqFileName);

            //when returning JSON the mime-type must be set to text/plain
            //otherwise some browsers will pop-up a "Save As" dialog.
            return Json(new
            {
                code = 0,
                msg = "上传成功",
                data = new {
                    pictureId = picture.Id,
                    imageUrl = _pictureService.GetPictureUrl(picture, 100)

                }                         
            });
        }
    }
}