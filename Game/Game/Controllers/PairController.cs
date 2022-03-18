using Microsoft.AspNetCore.Mvc;
using Game.Models;

namespace Game.Controllers
{
    public class PairController : Controller
    {
        private readonly LoveDBContext _context;
        public PairController(LoveDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Pair()
        {
            // 檢查登人狀態
            string? userAccount = HttpContext.Session.GetString("LoveGameAccount");
            if (string.IsNullOrEmpty(userAccount))
                return RedirectToAction("Login", "LoveGames");

            // 取得符合 user 性向的對像
            var user = _context.LoveGames.FirstOrDefault(x => x.Account == userAccount);
            var lovers = _context.LoveGames
                .Where(lover => lover.Account != userAccount && lover.Gender == user.SexualOrientation)
                .ToList();

            var aaa = new List<int>();
            var bbb = aaa.Where(x => x > 0);

            // 取得 user 已經滑過的紀錄
            var existingPairs = _context.Pairs
                .Where(x => x.Account == userAccount)
                .ToList();

            // 過濾已經滑過的對像，沒滑過的加到候選人
            var newLovers = new List<LoveGame>();
            foreach (var lover in lovers)
            {
                bool isNewLover = true;
                foreach (var pair in existingPairs)
                {
                    if (lover.Account == pair.Lover)
                    {
                        isNewLover = false;
                        break;
                    }
                }

                if (isNewLover)
                    newLovers.Add(lover);
            }

            // 如果過濾到沒半個代表通通滑過了，就把候選人換成全部
            if (newLovers.Count == 0)
            {
                newLovers = lovers;
            }

            // 從候選人隨機挑一個
            var rand = new Random();
            int randIndex = rand.Next(newLovers.Count);

            return View(newLovers[randIndex]);
        }

        [HttpPost]
        public IActionResult Pair(string lover, bool want)
        {
            // 檢查登人狀態
            string? userAccount = HttpContext.Session.GetString("LoveGameAccount");
            if (string.IsNullOrEmpty(userAccount))
                return RedirectToAction("Login", "LoveGames");

            var existingPair = _context.Pairs
                .FirstOrDefault(x => x.Account == userAccount && x.Lover == lover);
            if (existingPair == null)
            {
                _context.Pairs.Add(new Pair
                {
                    Account = userAccount,
                    Lover = lover,
                    Want = want,
                });
            }
            else
            {
                existingPair.Want = want;
                _context.Update(existingPair);
            }

            _context.SaveChanges();
            return Ok();
        }
    }
}
