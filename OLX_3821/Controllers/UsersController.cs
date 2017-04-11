using OLX_3821.DataAccess;
using OLX_3821.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OLX_3821.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {

            Users user = new Users();
            UsersManager userManager = new UsersManager();
            user = userManager.GetCurrentUser();
            IList<Users> currentUser = new List<Users>();
            currentUser.Add(user);

            return View(currentUser);
        }


        // GET: Users/Details/5
        public ActionResult Details(int id)
        {
            UsersManager userManager = new UsersManager();
            Users user = userManager.GetUserById(id);
            return View(user);
        }

        // GET: Users/Create
        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Create(Users user)
        {
            UsersManager userManager = new UsersManager();
            //userManager.CreateUser(user);
            userManager.CreateUserStoredProc(user);
            return RedirectToAction("Index");
        }


        // GET: Users/Edit/5
        public ActionResult Edit(int id)
        {
            UsersManager userManager = new UsersManager();
            Users user = userManager.GetUserById(id);
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        public ActionResult Edit(Users user)
        {
            UsersManager userManager = new UsersManager();
            //userManager.UpdateUser(user);
            userManager.UpdateUserStoredProc(user);
            return RedirectToAction("Index");
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int id)
        {
            UsersManager userManager = new UsersManager();
            Users user = userManager.GetUserById(id);
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost]
        public ActionResult Delete(Users user)
        {
            UsersManager userManager = new UsersManager();
            userManager.DeleteUser(user);
            return RedirectToAction("Index");
        }

        public ActionResult Payment()
        {
            UsersManager userManager = new UsersManager();
            Users user = userManager.GetCurrentUser();
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        public ActionResult Payment(Users user)
        {
            UsersManager userManager = new UsersManager();
            //userManager.UpdateUser(user);
            userManager.MakePayment(user);
            return RedirectToAction("Index");
        }

    }
}
