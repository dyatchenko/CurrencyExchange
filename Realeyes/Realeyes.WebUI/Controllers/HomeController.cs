namespace Realeyes.WebUI.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
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
            return View();
        }

        [HttpGet]
        public async Task<double> GetLastExchangeRate(string cur1, string cur2)
        {
            if (string.IsNullOrWhiteSpace(cur1) || string.IsNullOrWhiteSpace(cur2)) return 1;

            return await repo.GetLastExchangeRate(cur1, cur2);
        }

        [HttpPost]
        public async Task<JsonResult> GetAllPossibleCurrencies()
        {
            return Json(await repo.GetAllPossibleCurrencies());
        }

        [HttpPost]
        public async Task<JsonResult> GetExchangeHistory(string cur1, string cur2, DateTime? date1, DateTime? date2)
        {
            if (string.IsNullOrWhiteSpace(cur1) || string.IsNullOrWhiteSpace(cur2) || date1 == null
                || date2 == null) return Json(new double[0]);

            return Json(await repo.GetCurrenciesExchangeHistory(cur1, cur2, date1.Value, date2.Value));
        }
    }
}
