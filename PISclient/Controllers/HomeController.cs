using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PISclient.Helper;
using PISclient.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PISclient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            Index_ModelView model = new Index_ModelView();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Index(Index_ModelView model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ApiModel apiModel = new ApiModel();
                    using (var ms = new MemoryStream())
                    {
                        model.fajl.CopyTo(ms);
                        apiModel.fajl = ms.ToArray();
                    }
                    apiModel.fileOwner = model.fileOwner;
                    apiModel.fileType = model.fileType;
                    apiModel.requestID = Guid.NewGuid();
                    apiModel.fileExtension = Path.GetExtension(model.fajl.FileName);
                    apiModel.fileName = Path.GetFileNameWithoutExtension(model.fajl.FileName);
                    string request = JsonConvert.SerializeObject(apiModel);

                    //HttpRequest - POST
                    string responsePOST = HttpRequest.MakeRequest("http://localhost:50641/service/POST", "Username neki", "Password neki", "POST", request);
                    responsePOST = responsePOST.Substring(1, responsePOST.Length - 2);

                    //HttpRequest - GET
                    string responseGET = HttpRequest.MakeRequest("http://localhost:50641/service/GET?requestID=" + responsePOST, "Username neki", "Password neki", "GET", "");
                    if (string.IsNullOrWhiteSpace(responseGET))
                    {
                        ModelState.AddModelError(string.Empty, "Samo PDF fajl ce biti prikazan!");
                    }
                    else
                    {
                        responseGET = responseGET.Substring(1, responseGET.Length - 2);
                        byte[] fajlResponse = Convert.FromBase64String(responseGET);

                        return new FileContentResult(fajlResponse, "application/pdf");
                    }
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Nije uspeo upload fajla.");
                }
            }
            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
