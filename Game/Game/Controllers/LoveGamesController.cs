#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Game.Models;

namespace Game.Controllers
{
    public class LoveGamesController : Controller
    {
        private readonly LoveDBContext _context;

        public LoveGamesController(LoveDBContext context)
        {
            _context = context;
        }

        // GET: LoveGames
        public IActionResult Index()
        {
            string accountCache = HttpContext.Session.GetString("LoveGameAccount");
            var user = _context.LoveGames.FirstOrDefault(x => x.Account == accountCache);

            // 符合以下其中一個條件就重新登入
            if (user == null // session 快取找不到使用者
                || user.LastLogin.Date != DateTime.Now.Date // 跨日 
                || user.LastLogin.AddHours(2) < DateTime.Now) // 最後一次登入超過兩小時
            {
                return View();
            }

            // 有快取，不用登入
            if (user.Role == "admin") // admin = 管理員
                return RedirectToAction("Index");
            else
                return RedirectToAction("Details"); // 個人頁面
        }

        // GET: LoveGames/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loveGame = await _context.LoveGames
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loveGame == null)
            {
                return NotFound();
            }

            decimal money = 0;
            var moneyRecord = _context.Money.FirstOrDefault(x => x.Account == loveGame.Account);
            if (moneyRecord != null)
            {
                money = moneyRecord.Qe;
            }

            return View(new UserProfileViewModel(loveGame, money));
        }

        // GET: LoveGames/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LoveGames/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Gender,Birthday,Name,SexualOrientation,Career,Hobby,City,Account,Password, ProfileImage")] 
            CreateUserViewModel userModel)
        {
            if (ModelState.IsValid)
            {
                if (!await this.UploadFile(userModel.ProfileImage, userModel.Account))
                {
                    return RedirectToAction("Warning", "Home", new CommonWarningViewModel
                    {
                        Summary = "加入會員失敗",
                        Message = "無法上傳照片，請重新嘗試"
                    });
                }
                var newUser = userModel.ConvertToLoveGameEntity("user");
                _context.Add(newUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = newUser.Id});
            }
            return View(userModel);
        }
        private IActionResult Login()
        {
            string accountCache = HttpContext.Session.GetString("LoveGameAccount");
            var user = _context.LoveGames.FirstOrDefault(x => x.Account == accountCache);

            // 符合以下其中一個條件就重新登入
            if (user == null // session 快取找不到使用者
                || user.LastLogin.Date != DateTime.Now.Date // 跨日 
                || user.LastLogin.AddHours(2) < DateTime.Now) // 最後一次登入超過兩小時
            {
                return View();
            }

            // 有快取，不用登入
            if (user.Role == "admin") // admin = 管理員
                return RedirectToAction("Index");
            else
                return RedirectToAction("Details"); // 個人頁面
        }

        // POST: LoveGames/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("Account,Password")] LoveGame loveGame)
        {
            var user = _context.LoveGames.FirstOrDefault(x => x.Account == loveGame.Account);
            if (user == null)
                return RedirectToAction("Warning", "Home", new CommonWarningViewModel
                {
                    Summary = "登入失敗",
                    Message = "帳號或密碼錯誤"
                });

            if (user.Password != loveGame.Password)
                return RedirectToAction("Warning", "Home", new CommonWarningViewModel
                {
                    Summary = "登入失敗",
                    Message = "帳號或密碼錯誤"
                });

            user.LastLogin = DateTime.Now;
            _context.LoveGames.Update(user);
            _context.SaveChanges();
            // set session 代表更新登入快取
            HttpContext.Session.SetString("LoveGameAccount", loveGame.Account);

            // 判斷與領取登入獎勵
            var claimedReward = this.ClaimLoginReward(user.Account);
            if (claimedReward != null)
            {
                // 有領獎就顯示登入獎勵紀錄
                return View("LoginReward", claimedReward);
            }
            // 沒領獎就回傳使用者資訊頁面
            return RedirectToAction("Details", "LoveGames", new { id = user.Id});
        }
        public IActionResult Logout()
        {
            // 清除登入快取然後重新導向 Login 頁面
            HttpContext.Session.SetString("LoveGameAccount", "");
            return RedirectToAction("Login");
        }

        // GET: LoveGames/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loveGame = await _context.LoveGames.FindAsync(id);
            if (loveGame == null)
            {
                return NotFound();
            }
            return View(loveGame);
        }

        // POST: LoveGames/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Gender,Age,Birthday,Name,SexualOrientation,Career,Hobby,City,Account,Password,Role,LastLogin")] LoveGame loveGame)
        {
            if (id != loveGame.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loveGame);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoveGameExists(loveGame.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(loveGame);
        }

        // GET: LoveGames/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loveGame = await _context.LoveGames
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loveGame == null)
            {
                return NotFound();
            }

            return View(loveGame);
        }

        // POST: LoveGames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loveGame = await _context.LoveGames.FindAsync(id);
            _context.LoveGames.Remove(loveGame);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoveGameExists(int id)
        {
            return _context.LoveGames.Any(e => e.Id == id);
        }

        private async Task<bool> UploadFile(IFormFile ufile, string userAccount)
        {
            if (ufile != null && ufile.Length > 0)
            {
                var extension = Path.GetExtension(ufile.FileName);
                if (extension != ".jpg")
                    return false;

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images", $"{userAccount}.jpg");
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ufile.CopyToAsync(fileStream);
                }
                return true;
            }
            return false;
        }

        private LoginRewardViewModel ClaimLoginReward(string userAccount)
        {
            // 新觀念：資料庫的資料型別有分 date（不包含時分秒）跟 dateTime（包含時分秒）
            // 但在 C# 中只有 Datetime。DateTime.Now 會取得當下時間包含時分秒
            // DateTime.Today 則只會取出日期，不包含時分秒（捨棄掉，變 0 點 0 分 0 秒）
            var loginDate = DateTime.Today;
            var loginReward = _context.LoginRewards
                .FirstOrDefault(x => x.Account == userAccount && x.RewardDate == loginDate);

            // 已經有今天的紀錄代表領過了，不做事
            if (loginReward != null)
                return null;

            // 程式走到這代表還沒領今天的登入獎勵，所以再來要查看使用者的登入獎勵歷史
            var lastReward = _context.LoginRewards
                .Where(x => x.Account == userAccount)
                .OrderByDescending(x => x.RewardDate)
                .FirstOrDefault(); // 只取最後一筆，用來判斷有沒有連續登入

            if (lastReward == null) // 沒有歷史紀錄，是人生第一次登入，所以新增一筆
            {
                var todayReward = new LoginReward
                {
                    Account = userAccount,
                    ContinuousLoginDay = 1,
                    RewardMoney = 100,
                    RewardDate = loginDate,
                    TotalLoginDay = 1,
                };
                _context.LoginRewards.Add(todayReward);

                var moneyRecord = _context.Money.FirstOrDefault(x => x.Account == userAccount);
                if (moneyRecord == null) // 沒有金幣紀錄就新增一筆
                {
                    moneyRecord = new Money
                    {
                        Account = userAccount,
                        Card = "",
                        Qe = 100
                    };
                    _context.Money.Add(moneyRecord);
                }
                else // 有金幣紀錄就更新 QE
                {
                    moneyRecord.Qe += 100;
                }

                _context.SaveChanges();

                // 建立並回傳一個 View Model（在 LoginReward 中多加一個擁有的代幣總額
                return new LoginRewardViewModel(todayReward, moneyRecord.Qe);
            }
            else // 有歷史紀錄，計算連續登入天數然後發放獎勵
            {
                int totalLogin = lastReward.ContinuousLoginDay + 1; // 總天數直接增加
                int continuousLogin = 1; // 連續登入如果斷掉就重設成 1
                if (DateTime.Today.Date - lastReward.RewardDate.Date == TimeSpan.FromDays(1))
                {
                    // 有連續登入，把連續登入的變數用 lastReward 的天數 +1 蓋掉
                    continuousLogin = lastReward.ContinuousLoginDay + 1;
                }

                // 新增領獎紀錄
                decimal rewardMoney = (continuousLogin % 7) * 100;
                var todayReward = new LoginReward
                {
                    Account = userAccount,
                    ContinuousLoginDay = continuousLogin,
                    TotalLoginDay = totalLogin,
                    RewardMoney = continuousLogin * 100,
                    RewardDate = DateTime.Today
                };
                _context.LoginRewards.Add(todayReward);

                // 更新金幣紀錄
                var moneyRecord = _context.Money.FirstOrDefault(x => x.Account == userAccount);
                if (moneyRecord == null)
                    throw new ArgumentNullException($"找不到 {userAccount} 的金幣紀錄");
                moneyRecord.Qe += rewardMoney;

                _context.SaveChanges();
                // 建立並回傳一個 View Model（在 LoginReward 中多加一個擁有的代幣總額
                return new LoginRewardViewModel(todayReward, moneyRecord.Qe);
            }
        }
    }
}
