using LoggerApp.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Diagnostics;
using System.Net;
using System.Web;

namespace LoggerApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string usrname)
        {
            GetIPAddress(usrname, "login");
            return View();
        }

        protected string GetIPAddress(string uname, string sesontyp)
        {
            string hostName2 = Dns.GetHostName();
            IPHostEntry hostEntry2 = Dns.GetHostEntry(hostName2);

            int v = 0;
            string hh = "";
            foreach (IPAddress address in hostEntry2.AddressList)
            {
                hh += (v > 0) ? "," + address.ToString() : address.ToString();
                v++;
            }
            Log_tbl log_Tbl = new Log_tbl();
            log_Tbl.ipaddress = hh;
            log_Tbl.HostName = hostName2;
            log_Tbl.UserName = uname;
            log_Tbl.Module = "DBW";
            log_Tbl.createdOn = DateTime.Now;
            log_Tbl.createdBy = uname;
            log_Tbl.remarks = sesontyp;

            string contentRootPath = _webHostEnvironment.ContentRootPath;
            string logpath = Path.Combine(contentRootPath, "Logs/logs.txt");

            var log = new LoggerConfiguration().WriteTo.File(logpath).CreateLogger();
      
            log.Information("User : " + log_Tbl.UserName + ", Module : " + log_Tbl.Module + ", inTime : " + log_Tbl.createdOn + " Host : " + log_Tbl.HostName + ", ipAddress : " + log_Tbl.ipaddress + ", Remarks : " + log_Tbl.remarks);
            return hh;
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
    }
}