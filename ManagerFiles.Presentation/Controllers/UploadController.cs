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

        public async Task<JsonResult> CopyOrMoveFilesAsync(bool justCopy, string[] files) 
        {
            try
            {
                await _filePersistenceService.CopyOrMoveFilesAsync(justCopy, files);

                var folderAndFiles = await _filePersistenceService.GetFoldersAndFilesAsync();

                return Json(new { error = true, data = folderAndFiles });
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

        public async Task<JsonResult> UploadFile()
        {
            try
            {               
                var counter = 0;
                var currentCount = 0;                
                
                var postedFile = Request.Form.Files;

                await Task.Delay(500);
                
                currentCount++;
                
                SendFeedBack(currentCount, counter);

                if (postedFile.Count <= 0 || postedFile == null)
                {
                    return Json(new { error = true, message = "Empty File was uploaded" });
                }

                if (postedFile[0] == null || postedFile[0].Length <= 0)
                {
                    return Json(new { error = true, message = "Empty File was uploaded" });
                }

                await Task.Delay(500);
                
                currentCount++;
                
                SendFeedBack(currentCount, counter);
                
                var fileInfo = new FileInfo(postedFile[0].FileName);

                await _filePersistenceService.SaveFileToOriginAsync(postedFile[0]);

                var folderAndFiles = _filePersistenceService.GetFoldersAndFilesAsync();

                return Json(new { error = false, data = folderAndFiles });

                //if (extention.ToLower() != ".csv")
                //{
                //    return Json(new { error = true, message = "invalid file format" });
                //}     

                //string[] arr = new string[] { "Task1.pdf", "employes.csv" };
                //await _filePersistenceService.CopyOrMoveFilesAsync(false, arr);                

                //await Task.Delay(500);

                //currentCount++;

                //SendFeedBack(currentCount, counter);

            }
            catch (Exception ex)
            {
                return JsonException(ex);
            }

        }

        private async void SendFeedBack(int currentCount, int UploadCount)
        {
            var totalCount = 4;
            var feedBackModel = new FeedbackModel()
            {
                currentCount = currentCount,
                currentPercent = (currentCount * 100 / totalCount).ToString(),
                UploadCount = UploadCount,
            };
            await _broadCastHubService.Clients.All.SendAsync("feedBack", feedBackModel);
        }
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
