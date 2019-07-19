using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nop.Services.Media;

namespace Nop.Web.Controllers.Api
{
    public class MediaController : BasePublicController
    {

     
        #region Fields
        private readonly IPictureService _pictureService;
        #endregion
        public MediaController(IPictureService pictureService)
        {
            _pictureService = pictureService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Test()
        {
            return Json(new
            {
                code = 0,
                test = ""

            });
        }



        [HttpPost]
        [DisableRequestSizeLimit]
        //do not validate request token (XSRF)
        //[AdminAntiForgery(true)]
        public virtual IActionResult AsyncUpload()
        {        
            //if (!_permissionService.Authorize(StandardPermissionProvider.UploadPictures))
            //    return Json(new { success = false, error = "You do not have required permissions" }, "text/plain");
            var httpPostedFile = Request.Form.Files.FirstOrDefault();
           
            const string qqFileNameParameter = "qqfilename";

            var qqFileName = Request.Form.ContainsKey(qqFileNameParameter)
                ? Request.Form[qqFileNameParameter].ToString()
                : Guid.NewGuid().ToString();


            try
            {
                var picture = _pictureService.InsertPicture(httpPostedFile, qqFileName);

                return Json(new
                {
                    code = 0,
                    msg = "上传成功",
                    data = new
                    {
                        pictureId = picture.Id,
                        imageUrl = _pictureService.GetPictureUrl(picture, 100)

                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 0,
                    msg = ex.Message,
                    data = new
                    {
                        pictureId = "-1",
                        imageUrl = ""

                    }
                });
            }
           

            //when returning JSON the mime-type must be set to text/plain
            //otherwise some browsers will pop-up a "Save As" dialog.
     
        }


        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size, filePath });
        }
    }
}