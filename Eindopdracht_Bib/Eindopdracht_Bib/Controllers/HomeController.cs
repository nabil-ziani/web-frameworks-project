﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Eindopdracht_Bib.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.timestamp = DateTime.Now.ToString("HH:mm:ss");
            return View();
        }
    }
}
