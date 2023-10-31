install Nugets
Serilog.Sinks.File
Serilog

protected string GetIPAddress(string uname, string sesontyp)
{
    string hostName2 = Dns.GetHostName();
    IPHostEntry hostEntry2 = Dns.GetHostEntry(hostName2);
    //List<string> ipAddresses = new List<string>();

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
    var jj = (sesontyp == "login") ? (log_Tbl.intime = DateTime.Now, log_Tbl.outtime = null) : (log_Tbl.intime = null, log_Tbl.outtime = DateTime.Now);

    //int logid = applicationRepository.AddLog(log_Tbl);
    var logpath = System.Web.Hosting.HostingEnvironment.MapPath("~/Logs/log.txt");
    var log = new LoggerConfiguration().WriteTo.File(logpath).CreateLogger();
    //log.Information("User : " + log_Tbl.UserName + ", Module : " + log_Tbl.Module + ", inTime : " + log_Tbl.intime + ", outTime : " + log_Tbl.outtime + ", Host : " + log_Tbl.HostName + ", ipAddress : " + log_Tbl.ipaddress + ", Remarks : " + log_Tbl.remarks);
    log.Information("User : " + log_Tbl.UserName + ", Module : " + log_Tbl.Module + ", inTime : " + log_Tbl.createdOn + " Host : " + log_Tbl.HostName + ", ipAddress : " + log_Tbl.ipaddress + ", Remarks : " + log_Tbl.remarks);
    return hh;
}




[HttpPost]
public ActionResult Login(string uname, string password, string rememberme,string scraplogin)
{
    //var usrid = db.ManageUsers.Where(x => x.UserName.Equals(myCookie.Value)).FirstOrDefault();
    GetIPAddress(uname,"login");
    
    try
    {
        string pwd = EncodeDecode.EncodeBase64(password);

        var data = applicationRepository.CheckLogin(uname, pwd);

        if (data != null && data.UserName.Equals(uname, StringComparison.Ordinal) && data.Project_Typeid != 3)
        {
            if (rememberme == "1")
            {
                HttpCookie cookie = new HttpCookie("mybigcookie");
                cookie.Values.Add("uname", uname);
                cookie.Values.Add("password", password);
                cookie.Expires = DateTime.Now.AddYears(50);
                Response.Cookies.Add(cookie);
            }
            Session["projecttype"] = data.Project_Typeid;
            Session["usertype"] = data.UserType_Id;
            Session["username"] = data.UserName;
            Session["useriid"] = data.User_Id;
            Session["division"] = data.DivisionID;
            Session["subdivision"] = data.SubdivisionID;
            Session["Section"] = data.SectionID;
            Session["circle"] = data.CircleID;
            Session["district"] = data.DistrictID;
            Session["block"] = data.BlockID;
            Session["scraplogin"] = scraplogin;
            if (data.UserType_Id == 8 && data.Project_Typeid == 1)
            {
                return RedirectToAction("Dashboard", "Finance");
            }
            else if (data.UserType_Id == 1 && data.Project_Typeid == 1)
            {
                if(scraplogin == "123")
                {
                    return RedirectToAction("Dashboard", "Scrap");
                }
                else
                {
                    return RedirectToAction("Dashboard", "Divison");
                }
                
              
            }
            else if (data.UserType_Id == 10 && data.Project_Typeid == 1)
            {
                if (scraplogin == "123")
                {
                    return RedirectToAction("Dashboard", "Scrap");
                }
                else
                {
                    return RedirectToAction("Dashboard", "SubDivison");
                }
            }
            else if (data.UserType_Id == 2 && data.Project_Typeid == 1)
            {
                if (scraplogin == "123")
                {
                    return RedirectToAction("Dashboard", "Scrap");
                }
                else
                {
                    return RedirectToAction("Dashboard", "Section");
                }
            }
            else if (data.UserType_Id == 32 && data.Project_Typeid == 1)
            {
                return RedirectToAction("Dashboard", "Designoff");
            }
            else if (data.UserType_Id == 6 && data.Project_Typeid == 1)
            {
                return RedirectToAction("Dashboard", "Designoff");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        else if (data != null && data.IsDelete == true)
        {
            TempData["Message"] = "User does not exist";
            return View();
        }

        else if (data == null)
        {
            TempData["Message"] = "Incorrect Username Or Password";
            return View();
        }
        else
        {
            TempData["Message"] = "User does not exist";
            return View();
        }
    }
    catch (Exception ex)
    {
        throw ex;
    }
}

public ActionResult logout()
{
    Session.Abandon();
    GetIPAddress("user", "logout");
    return RedirectToAction("Login", "Home");
}


