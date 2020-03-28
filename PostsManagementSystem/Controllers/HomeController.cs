using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PostsManagementSystem.Data;
using PostsManagementSystem.Models;
using PostsManagementSystem.Models.ViewModels;

namespace PostsManagementSystem.Controllers
{
    [Authorize]
    [Route("/[Action]")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("/")]
        [Route("/[Action]")]
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Instructions()
        {
            AppInfo instructions = _context.AppInfo
                .Where(a => a.Name == "Instructions")
                .FirstOrDefault();

            return View(instructions);
        }

        [HttpPost]
        public ActionResult Instructions(IFormCollection collection)
        {
            AppInfo instructions = _context.AppInfo
                .Where(a => a.Name == "Instructions")
                .FirstOrDefault();

            instructions.Content = collection["Content"];
            instructions.ContentAR = collection["ContentAR"];

            _context.Update(instructions);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        //public ActionResult ContactUs()
        //{
        //    AppInfo contactUs = _context.AppInfo
        //        .Where(a => a.Name == "Contact Us")
        //        .FirstOrDefault();

        //    return View(contactUs);
        //}

        //[HttpPost]
        //public ActionResult ContactUs(IFormCollection collection)
        //{
        //    AppInfo contactUs = _context.AppInfo
        //        .Where(a => a.Name == "Contact Us")
        //        .FirstOrDefault();

        //    contactUs.Content = collection["Content"];
        //    contactUs.ContentAR = collection["ContentAR"];

        //    _context.Update(contactUs);
        //    _context.SaveChanges();

        //    return RedirectToAction("Index", "Home");
        //}

        public ActionResult AboutApplication()
        {
            AboutVM aboutVM = new AboutVM();

            aboutVM.AppInfo = _context.AppInfo
                .Where(a => a.Name == "About Application")
                .FirstOrDefault();

            aboutVM.SocialMediaAccounts = _context.SocialMediaAccounts
                .Where(s => s.AppInfo.Name == "About Application")
                .ToList();

            return View(aboutVM);
        }

        [HttpPost]
        public ActionResult AboutApplication(IFormCollection collection)
        {
            AppInfo aboutApplication = _context.AppInfo
                .Where(a => a.Name == "About Application")
                .FirstOrDefault();


            aboutApplication.Content = collection["AppInfo.Content"];
            aboutApplication.ContentAR = collection["AppInfo.ContentAR"];

            _context.Update(aboutApplication);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult CreateAccount(int id)
        {
            SocialMediaAccount account = new SocialMediaAccount();
            account.AppInfoId = id;

            return View(account);
        }

        [HttpPost]
        public ActionResult CreateAccount(IFormCollection collection)
        {
            SocialMediaAccount account = new SocialMediaAccount();
            account.Name = collection["Name"];
            account.Link = collection["Link"];
            account.AppInfoId = Int32.Parse(collection["AppInfoId"]);

            _context.Add(account);
            _context.SaveChanges();

            AppInfo temp = _context.AppInfo.Find(account.AppInfoId);

            if (temp.Name == "About Application")
                return RedirectToAction("AboutApplication");
            else
                return RedirectToAction("AboutOwner");
        }

        public ActionResult EditAccount(int id)
        {
            SocialMediaAccount account = _context.SocialMediaAccounts.Find(id);

            return View(account);
        }

        [HttpPost]
        public ActionResult EditAccount(int id, IFormCollection collection)
        {
            SocialMediaAccount account = _context.SocialMediaAccounts.Find(id);
            int appInfoId = account.AppInfoId;

            account.Name = collection["Name"];
            account.Link = collection["Link"];

            _context.Update(account);
            _context.SaveChanges();

            AppInfo appInfo = _context.AppInfo.Find(appInfoId);

            if (appInfo.Name == "About Application")
                return RedirectToAction("AboutApplication");
            else
                return RedirectToAction("AboutOwner");
        }

        public ActionResult DeleteAccount(int id)
        {
            SocialMediaAccount account = _context.SocialMediaAccounts.Find(id);
            int appInfoId = account.AppInfoId;

            _context.SocialMediaAccounts.Remove(account);
            _context.SaveChanges();

            AppInfo appInfo = _context.AppInfo.Find(appInfoId);

            if (appInfo.Name == "About Application")
                return RedirectToAction("AboutApplication");
            else
                return RedirectToAction("AboutOwner");
        }

        public ActionResult AboutOwner()
        {
            AboutVM aboutVM = new AboutVM();

            aboutVM.AppInfo = _context.AppInfo
                .Where(a => a.Name == "About Owner")
                .FirstOrDefault();

            aboutVM.SocialMediaAccounts = _context.SocialMediaAccounts
                .Where(s => s.AppInfo.Name == "About Owner")
                .ToList();

            return View(aboutVM);
        }

        [HttpPost]
        public ActionResult AboutOwner(IFormCollection collection)
        {
            AppInfo aboutOwner = _context.AppInfo
                .Where(a => a.Name == "About Owner")
                .FirstOrDefault();


            aboutOwner.Content = collection["AppInfo.Content"];
            aboutOwner.ContentAR = collection["AppInfo.ContentAR"];

            _context.Update(aboutOwner);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
