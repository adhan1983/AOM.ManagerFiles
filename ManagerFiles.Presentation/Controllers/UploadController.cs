using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.SignalR;
using ManagerFiles.Presentation.Hubs;
using ManagerFiles.Presentation.Models;
using ManagerFiles.Presentation.ServicesInterfaces;
using Microsoft.AspNetCore.Http;

namespace FileUpload.Controllers
{
    public class UploadController : Controller
    {
        private readonly IHubContext<BroadCastHubService> _broadCastHubService;
        private readonly IFilePersistenceService _filePersistenceService;
        public UploadController(IFilePersistenceService filePersistenceService, IHubContext<BroadCastHubService> broadCastHubService)
        {
            _broadCastHubService = broadCastHubService;
            _filePersistenceService = filePersistenceService;
        }

        public async Task<IActionResult> Index()
        {           
            var result = await _filePersistenceService.GetFoldersAndFilesAsync();
            
            return View(result);
        }


        [HttpPost]
        public async Task<JsonResult> CopyOrMoveFiles([FromForm]CopyOrMovingFiles copyOrMovingFiles) 
        {
            try
            {               
                await _filePersistenceService.CopyOrMoveFilesAsync(copyOrMovingFiles.justCopy, copyOrMovingFiles.files);

                var folderAndFiles = await _filePersistenceService.GetFoldersAndFilesAsync();

                return Json(new { error = false, data = folderAndFiles });
            }
            catch (Exception ex)
            {
                return JsonException(ex);
            }
        }       

        public async Task<JsonResult> UploadFile()
        {
            try
            {             
                var postedFile = Request.Form.Files;
                
                if (postedFile.Count <= 0 || postedFile == null)
                {
                    return Json(new { error = true, message = "Empty File was not uploaded" });
                }

                if (postedFile[0] == null || postedFile[0].Length <= 0)
                {
                    return Json(new { error = true, message = "Empty File was not uploaded" });
                }

                await _filePersistenceService.SaveFileToOriginAsync(postedFile[0]);

                var folderAndFiles = _filePersistenceService.GetFoldersAndFilesAsync();

                return Json(new { error = false, data = folderAndFiles });               

            }
            catch (Exception ex)
            {
                return JsonException(ex);
            }

        }
        private JsonResult JsonException(Exception ex)
        {
            return Json(new
            {
                error = true,
                message = ex.InnerException != null ?
                ex.InnerException.Message : ex.Message
            });
        }
        
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
