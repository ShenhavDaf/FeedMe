using FeedMe.Data;
using FeedMe.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FeedMe.Controllers
{
    public class HomeController : Controller
    {
        private readonly FeedMeContext _context;

        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public HomeController(FeedMeContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            var orderDish = _context.MyCartItem.Include(a => a.Dish);
            Dictionary<String, int> map = new Dictionary<String, int>();

            foreach (var item in orderDish)
            {
                if (map.ContainsKey(item.Dish.Name))
                {
                    map[item.Dish.Name]++;
                }
                else
                {
                    map.Add(item.Dish.Name, 1);
                }
            }


            map.OrderBy(a => a.Value);
            var list = map.Keys.ToList();
            list.Sort((pair1, pair2) => map[pair1].CompareTo(map[pair2]));



            var query = from key in list select new { label = key, y = map[key] };
            // query.OrderBy(a => a.label);
            ViewData["Graph"] = JsonConvert.SerializeObject(query);

            List<int> listY = new List<int>();
            listY.Add(map.Keys.Count);
            listY.Add(_context.Dish.Count() - map.Keys.Count);

            List<String> listX = new List<String>();
            listX.Add("Ordered dishes");
            listX.Add("Not ordered dishes");

            ViewData["GraphX"] = JsonConvert.SerializeObject(listX);
            ViewData["GraphY"] = JsonConvert.SerializeObject(listY);

            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Signup()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
