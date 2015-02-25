using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Realeyes.WebUI.Controllers
{
    using Realeyes.WebUI.Abstract;

    public class HomeController : Controller
    {
        private readonly ICurrencyExchangeRepository repo;

        public HomeController(ICurrencyExchangeRepository repo)
        {
            this.repo = repo;
        }

        //
        // GET: /Home/

        [HttpGet]
        public ActionResult Index()
        {
            return View(repo.GetAllPossibleCurrencies());
        }

        [HttpGet]
        public double GetLastExchangeRate(string cur1, string cur2)
        {
            if (string.IsNullOrWhiteSpace(cur1) || string.IsNullOrWhiteSpace(cur2)) return 1;

            return repo.GetLastExchangeRate(cur1, cur2);
        }
    }
}
