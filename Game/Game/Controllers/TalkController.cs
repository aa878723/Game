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
            var rooms = _context.TalkingRooms.ToList();
            return View(rooms);
        }

        [HttpGet]
        public IActionResult Talk(int id)
        {
            string? account = HttpContext.Session.GetString("LoveGameAccount");//檢查使用者登入
            if (string.IsNullOrEmpty(account))
                return RedirectToAction("Login", "LoveGames");

            var room = _context.TalkingRooms.FirstOrDefault(x => x.Id == id);
            if (room == null)
                return RedirectToAction("Warning", "Home", new CommonWarningViewModel
                {
                    Summary = "無法加入聊天室",
                    Message = $"指定的聊天室 (id={id}) 不存在"
                });

            var messages = _context.Talkings
                .Where(x => x.Room == room.RoomName)
                .OrderBy(x => x.MessageTime);
            var viewModel = new TalkingViewModel { 
                Messages = messages,
                UserAccount = account,
                RoomName = room.RoomName,
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Talk(string roomName, string userAccount, string message)
        {
            _context.Talkings.Add(new Talking
            {
                Account = userAccount,
                Message = message,
                Room = roomName,
                MessageTime = DateTime.Now
            });
            _context.SaveChanges();

            return RedirectToAction("Talk");
        }
    }
}
