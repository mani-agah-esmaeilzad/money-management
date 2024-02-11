using Microsoft.AspNetCore.Mvc;
using MoneyManagement.Models;

namespace MoneyManagement.Controllers
{
    public class AddCardController : Controller
    {
        _Context context = new _Context();
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(string number)
        {
            Cards card = new Cards();
            card.CardNumber = number;
            card.CardId = Guid.NewGuid();
            context.CardTbl.Add(card);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
