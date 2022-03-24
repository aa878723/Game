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

        [HttpGet]
        public IActionResult PersonalTalk(string friendAccount)
        {
            string? userAccount = HttpContext.Session.GetString("LoveGameAccount");//檢查使用者登入
            if (string.IsNullOrEmpty(userAccount))
                return RedirectToAction("Login", "LoveGames");

            var persontalTalkSelectList = new List<PersonalTalkEntry>();
            var friendRecords = _context.TbFriends.Where(x => x.Account == userAccount).ToList();
            foreach (var record in friendRecords)
            {
                var lastTalking = _context.PersonalTalkings
                    .Where(x =>
                        (x.Account == userAccount && x.Friend == record.Friend)
                        || (x.Account == record.Friend && x.Friend == userAccount)
                    )
                    .OrderByDescending(x => x.SendTime)
                    .FirstOrDefault();
                if (lastTalking != null)
                {
                    persontalTalkSelectList.Add(new PersonalTalkEntry
                    {
                        FriendAccount = record.Friend,
                        LastMessage = this.ShrinkMessage(lastTalking.Message),
                        LastSendTime = lastTalking.SendTime
                    });
                }
                else
                {
                    persontalTalkSelectList.Add(new PersonalTalkEntry
                    {
                        FriendAccount = record.Friend,
                        LastMessage = "[系統] 我們已經成為好友，快來聊天吧",
                        LastSendTime = new DateTime(2022, 1,1)
                    });
                }
            }

            var seletedTalkingMessages = _context.PersonalTalkings
                .Where(x => 
                    (x.Account == userAccount && x.Friend == friendAccount)
                    || (x.Account == friendAccount && x.Friend == userAccount)
                )
                .OrderBy(x => x.SendTime)
                .ToList();

            return View(new PersonalTalkingViewModel
            {
                UserAccount = userAccount,
                FriendAccount = friendAccount,
                PersonalTalkingSelectList = persontalTalkSelectList,
                SelectedTalkingMessage = seletedTalkingMessages
            });
        }

        [HttpPost]
        public IActionResult PersonalTalk(string friendAccount, string message)
        {
            string? userAccount = HttpContext.Session.GetString("LoveGameAccount");//檢查使用者登入
            if (string.IsNullOrEmpty(userAccount))
                return RedirectToAction("Login", "LoveGames");

            _context.PersonalTalkings.Add(new PersonalTalking
            {
                Account = userAccount,
                Friend = friendAccount,
                Message = message,
                Picture = "",
                SendTime = DateTime.Now
            });
            _context.SaveChanges();

            return Ok();
        }

        private string ShrinkMessage(string messge)
        {
            if (messge.Length < 18)
                return messge;
            else
            {
                messge = messge.Substring(0, 15) + "...";
                return messge;
            }
        }
    }
}
