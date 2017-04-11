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
    public class PosterController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            IList<Posters> posterList = new List<Posters>();
            PosterManager posterManager = new PosterManager();
            posterList = posterManager.GetAvaliablePosters();

            return View(posterList);
        }

        public ActionResult Search(string title, string category, double? minPrice, double? maxPrice)
        {
            IList<Posters> posterList = new List<Posters>();
            PosterManager posterManager = new PosterManager();
            posterList = posterManager.FilterPosters(title, category, minPrice, maxPrice);
            return View("Index", posterList);
        }

        // GET: Users/Details/5
        public ActionResult Details(int id)
        {
            PosterManager posterManager = new PosterManager();
            Posters poster = posterManager.GetPosterById(id);
            return View(poster);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        public ActionResult Create(Posters poster)
        {
            PosterManager posterManager = new PosterManager();
            posterManager.CreatePoster(poster);
            return RedirectToAction("Index");
        }


        // GET: Users/Edit/5
        public ActionResult Edit(int id)
        {
            PosterManager posterManager = new PosterManager();
            Posters poster = posterManager.GetPosterById(id);
            return View(poster);
        }

        // POST: Users/Edit/5
        [HttpPost]
        public ActionResult Edit(Posters poster)
        {
            PosterManager posterManager = new PosterManager();
            //userManager.UpdateUser(user);
            posterManager.UpdatePosterStoredProc(poster);
            return RedirectToAction("Index");
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int id)
        {
            PosterManager posterManager = new PosterManager();
            Posters poster = posterManager.GetPosterById(id);
            return View(poster);
        }

        // POST: Users/Delete/5
        [HttpPost]
        public ActionResult Delete(Posters poster)
        {
            PosterManager posterManager = new PosterManager();
            posterManager.DeletePoster(poster);
            return RedirectToAction("Index");
        }


        public ActionResult MyPosters()
        {
            IList<Posters> posterList = new List<Posters>();
            PosterManager posterManager = new PosterManager();
            posterList = posterManager.GetUserPosters();

            return View(posterList);
        }

        public ActionResult Purchase(int id)
        {
            ViewBag.errorText = "";
            PosterManager posterManager = new PosterManager();
            Posters poster = posterManager.GetPosterById(id);
            return View(poster);
        }

        [HttpPost]
        public ActionResult Purchase(Posters poster)
        {
            ViewBag.errorText = "";

            PosterManager posterManager = new PosterManager();
            int orderedQuantity = poster.quantity;
            int currentQuantity = posterManager.GetPosterById(poster.posterID).quantity;

            if (orderedQuantity > currentQuantity)
            {
                ViewBag.errorText = "Not Enough Items, Look for the number of itemss available!";
                return View(posterManager.GetPosterById(poster.posterID));
            }
            else
            {
                posterManager.Purchase(poster);
                return RedirectToAction("Index");
            }

            
        }

        public ActionResult CompanyReport()
        {
            //UsersManager userManager = new UsersManager();
            //IList<Users> users = userManager.GetAllUsers();
            ViewBag.Users = new UsersManager().GetAllUsers();
            IList<Report> rep = new List<Report>();
            return View(rep);
        }

        [HttpPost]
        public ActionResult CompanyReport(int id, double min, double max)
        {
            PosterManager posterManager = new PosterManager();
            ViewBag.Users = new UsersManager().GetAllUsers();
            IList<Report> rep = posterManager.TrackSalesReport(id, min, max);
            return View(rep);
        }

    }
}
