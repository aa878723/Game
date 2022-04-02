using Microsoft.AspNetCore.Mvc;
using Game.Models;

namespace Game.Controllers
{
    public class MoneyController : Controller
    {
        private readonly LoveDBContext _context;
        private static List<OrderOptionItem> orderList = new List<OrderOptionItem>
        {
            new OrderOptionItem { OrderType = "NT99", GameMoney = 300, Price = 99, Title = "超值特惠包", Description = "試水溫神器，不到一百塊就能體驗 Love Game 交友特權" },
            new OrderOptionItem { OrderType = "NT299", GameMoney = 1000, Price = 299, Title = "小資脫單包", Description = "告別單身生活，一個月少喝 10 杯飲料，能瘦身又能脫離單身"},
            new OrderOptionItem { OrderType = "NT3000", GameMoney = 12000, Price = 3000, Title = "霸道總裁包", Description = "霸道總裁專屬方案，價格只是個數字，總裁要的是尊爵不凡的服務"},
            new OrderOptionItem { OrderType = "NT8000", GameMoney = 30000, Price = 8000, Title = "御用皇帝包", Description = "皇帝祖宗專屬方案，價格是什麼東西，朕要的是瘋狂的撒幣"},
        };

        public MoneyController(LoveDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Orders()
        {
            // 確認登入狀態
            string? userAccount = HttpContext.Session.GetString("LoveGameAccount");
            if (string.IsNullOrEmpty(userAccount))
                return RedirectToAction("Login", "LoveGames");

            var moneyRecord = _context.Money.First(x => x.Account == userAccount);
            return View(new OrderOptionViewModel
            {
                Account = userAccount,
                TotalGameMoney = moneyRecord == null ? 0 : moneyRecord.Qe,
                OrderOptions = orderList
            });
        }

        [HttpPost]
        public IActionResult Purchase(string orderType)
        {
            // 確認登入狀態
            string? userAccount = HttpContext.Session.GetString("LoveGameAccount");
            if (string.IsNullOrEmpty(userAccount))
                return RedirectToAction("Login", "LoveGames");

            // 確認訂單 type 是否有效
            if (string.IsNullOrEmpty(orderType))
            {
                return RedirectToAction("Warning", "Home", new CommonWarningViewModel
                {
                    Summary = "儲值失敗",
                    Message = "訂單類型不可為空"
                });
            }

            var order = orderList.FirstOrDefault(x => x.OrderType == orderType);
            if (order == null)
            {
                return RedirectToAction("Warning", "Home", new CommonWarningViewModel
                {
                    Summary = "儲值失敗",
                    Message = "無效的訂單類型"
                });
            }

            // 建立訂單
            var orderEntity = new Order
            {
                Account = userAccount,
                Amount = order.GameMoney,
                OrderNumber = $"{order.OrderType}-{DateTime.Now.ToString("yyyyMMddHHmmss")}",
                OrderTime = DateTime.Now
            };


            // 更新使用者擁有的遊戲幣
            var moneyRecord = _context.Money.FirstOrDefault(x => x.Account == userAccount);
            if (moneyRecord == null)
            {
                _context.Money.Add(new Money
                {
                    Account = userAccount,
                    Card = "",
                    Qe = order.GameMoney
                });
            }
            else
            {
                moneyRecord.Qe += order.GameMoney;
            }

            // 儲存交易紀錄
            _context.Orders.Add(orderEntity);
            _context.SaveChanges();

            var user = _context.LoveGames.First(x => x.Account == userAccount);
            return Json(user.Id);
        }
    }
}
