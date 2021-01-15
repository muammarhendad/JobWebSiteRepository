using Job_Offers_Website.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
	[RequireHttps]
	public class HomeController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();   
		public ActionResult Index()
		{
			return View(db.Categories.ToList());
		}

		public ActionResult Details(int JobId)
		{
			var job = db.Jobs.Find(JobId);
			if(job==null)
			{
				return HttpNotFound();
			}
			Session["JobId"] = JobId;
			return View(job);

		}
		[Authorize]
		public ActionResult Apply()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Apply(string Message)
		{
			var UserId = User.Identity.GetUserId();
			var JobId = (int)Session["JobId"];
			var check = db.ApplyForJobs.Where(a => a.JobId == JobId && a.UserId == UserId).ToList();
			if (check.Count<1)
			{
				 var job = new ApplyForJob();
				 job.JobId = JobId;
				 job.UserId = UserId;
				 job.Message = Message;
				 job.ApplyDate = DateTime.Now;
				 db.ApplyForJobs.Add(job);
				db.SaveChanges();
				ViewBag.Result = "Success, You Apply For This Job ";

			}
			else
			{
				ViewBag.Result = "Error, You are Aready Apply For This Job!!! ";
			}


			return View();
		}


	  [Authorize]
		public ActionResult GetJobsByUser()
		{
			var UserId = User.Identity.GetUserId();
			var jobs = db.ApplyForJobs.Where(a => a.UserId == UserId);
			return View(jobs.ToList());
		}

		public ActionResult DetailsOfJob(int id)
		{
			var job = db.ApplyForJobs.Find(id);
			if (job == null)
			{
				return HttpNotFound();
			}
		
			return View(job);

		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}