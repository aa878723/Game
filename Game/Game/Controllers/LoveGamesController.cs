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
        public async Task<IActionResult> Index()
        {
            return View(await _context.LoveGames.ToListAsync());
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
        public IActionResult Login()
        {
            string accountCache = HttpContext.Session.GetString("LoveGameAccount");
            var user = _context.LoveGames.FirstOrDefault(x => x.Account == accountCache);
            if (user != null && user.LastLogin > DateTime.Now.AddHours(-2.0))
            {
                if (user.Role == "admin") // admin = 管理員
                    return RedirectToAction("Index");
                else
                    return RedirectToAction("Details"); // 個人頁面
            }
            return View();
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
            HttpContext.Session.SetString("LoveGameAccount", loveGame.Account);

            return RedirectToAction("Details", "LoveGames", new { id = user.Id});
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
    }
}
