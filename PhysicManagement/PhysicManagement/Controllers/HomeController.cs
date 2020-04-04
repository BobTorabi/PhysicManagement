﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhysicManagement.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return RedirectPermanent("~/Home/Dashboard");
        }
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}