using OLX_3821.DataAccess;
using OLX_3821.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OLX_3821.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        //private static LoginController Instance { get; set; }

        ////method to access OrderingRespository
        ////Implements Singleton
        //public static LoginController getInstance
        //{
        //    get
        //    {
        //        if (Instance == null)
        //            Instance = new LoginController();
        //        return Instance;
        //    }
        //}

        // GET: Login
        public ActionResult Index()
        {
            return View(new Users());
        }

        [HttpPost]
        public ActionResult Login(Users user, string returnUrl)
        {
            UsersManager manager = new UsersManager();
            bool authenticated = manager.Authenticate(user.username, user.uPassword);
            if (authenticated)
            {
                //!!!!!!!!!!!!!!!!!!!!!!
                FormsAuthentication.SetAuthCookie(user.username, false);

                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Users");
                }
            }
            else
            {
                ModelState.AddModelError("", "Authentication failed!");
                return View("Index", user);
            }
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
    }

}