using Microsoft.AspNetCore.Mvc;
using Game.Models;

namespace Game.Controllers
{
    public class TalkController : Controller
    {
        private readonly LoveDBContext _context;
        public TalkController(LoveDBContext context)
        {
            _context = context;
        }

        public IActionResult TalkRoom()
        {
            return View();
        }
    }
}
