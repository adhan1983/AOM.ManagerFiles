using ManagerFiles.Presentation.Contants;
using ManagerFiles.Presentation.Hubs;
using ManagerFiles.Presentation.Models;
using ManagerFiles.Presentation.ServicesInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ManagerFiles.Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;        
        private readonly IHubContext<BroadCastHubService> _broadCastHubService;
        private readonly IFilePersistenceService _filePersistenceService;


        public HomeController(IFilePersistenceService filePersistenceService, IHubContext<BroadCastHubService> broadCastHubService, ILogger<HomeController> logger)
        {
            _logger = logger;
            _filePersistenceService = filePersistenceService;
            _broadCastHubService = broadCastHubService;
        }

        public async Task<ActionResult> Index()
        {

            var result = await _filePersistenceService.GetFoldersAndFilesAsync();

            //var counter = 0;
            //var currentCount = 0; 

            //await Task.Delay(500);
            //currentCount++;
            //for (int i = 0; i < 5; i++)
            //{
            //    SendFeedBack(currentCount, counter);
            //    currentCount++;
            //    await Task.Delay(500);
            //}


            //var result = _filesAplicationService.GetFilesAsync();

            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
            await _broadCastHubService.Clients.All.SendAsync(ManagerFilesConstants.FEEDBACK, feedBackModel);
        }
    }
}
