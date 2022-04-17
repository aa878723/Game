using Microsoft.AspNetCore.Mvc;
using Meals.Models;

namespace Meals.Controllers
{
    public class EasyFoodController : Controller
    {
        private readonly FoodDBContext _context;
        public EasyFoodController(FoodDBContext context)
        {
            _context = context;
            
        }
        public IActionResult Create()
        {
          
            return View();
        }

        [HttpPost]
        public IActionResult Create([Bind("Name,Price")]
            EasyFood foodModel)
        {
            _context.EasyFoods.Add(foodModel);
         
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Index()
        {
            var list=_context.EasyFoods.ToList();
            return View(list);
      
        }

    }
}
