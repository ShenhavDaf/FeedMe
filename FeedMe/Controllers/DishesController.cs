using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using FeedMe.Data;
using FeedMe.Models;

namespace FeedMe.Controllers
{
    public class DishesController : Controller
    {
        private readonly FeedMeContext _context;

        public DishesController(FeedMeContext context)
        {
            _context = context;
        }

        // GET: Dishes
        public async Task<IActionResult> Index(string searchString)
        {

            var dish = from m in _context.Dish
                       select m;

            dish = dish.Where(s => (s.Name.Contains(searchString) || searchString == null) ||
           s.Description.Contains(searchString));

            //var feedMeContext = _context.Dish.Include(d => d.Restaurant);
            // return View(await feedMeContext.ToListAsync());
            return View(await dish.ToListAsync());
        }

        // GET: Dishes/Details/5
        [Authorize(Roles = "Admin,rManager")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish
                .Include(d => d.Restaurant)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // GET: Dishes/Create
        public IActionResult Create()
        {
            ViewBag.FoodType = new SelectList(Enum.GetNames(typeof(FoodType)));

            ViewData["RestaurantID"] = new SelectList(_context.Restaurant, "ID", nameof(Restaurant.Name));
            return View();
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,DishImage,Description,FoodType,Price,RestaurantID")] Dish dish, int restaurant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dish);
                await _context.SaveChangesAsync();
                //PostMessageToFacebook().Wait();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RestaurantID"] = new SelectList(_context.Restaurant, "ID", nameof(Restaurant.Name), dish.RestaurantID);
            return View(dish);
        }

        // GET: Dishes/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.FoodType = new SelectList(Enum.GetNames(typeof(FoodType)));
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }
            ViewData["RestaurantID"] = new SelectList(_context.Restaurant, "ID", nameof(Restaurant.Name), dish.RestaurantID);
            return View(dish);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,DishImage,Description,FoodType,Price,RestaurantID")] Dish dish)
        {
            if (id != dish.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(dish.ID))
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
            ViewData["RestaurantID"] = new SelectList(_context.Restaurant, "ID", nameof(Restaurant.Name), dish.RestaurantID);
            return View(dish);
        }

        // GET: Dishes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish
                .Include(d => d.Restaurant)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await _context.Dish.FindAsync(id);
            _context.Dish.Remove(dish);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
            return _context.Dish.Any(e => e.ID == id);
        }

        //public static async Task<string> PostMessageToFacebook()
        //{
        //    // Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
        //    // for details on configuring this project to bundle and minify static web assets.

        //    // Post to Facebook after new dish is validated and created in the database
        //    string pageId = "105380358425572";
        //    string accessToken = "EAAMUCFVTWL0BAMhxIJkFQRBOZCmyOffnTkAlonCOj8U8ILB2O943aBqpOOMIou6MEduKppMUM9TcO67yPcQaqEchD2pTvC4FPsJwkQ6SIZAzgbhFhIgrFN50w5QofVWQayq4sIf5AVqWg7fCxtxPEHDDZCtyLmBFczn1kqmMIyWZBQQHOTrf";

        //    string message = "Test 01082021_1";

        //    FacebookApi api = new FacebookApi(pageId, accessToken);
        //    string result = await api.PostMessage(message);

        //    Console.WriteLine(result);

        //    return result;
        //}


        public async Task<IActionResult> JoinDishRestaurant(int? id)
        {
            ViewJoin viewJoin = new ViewJoin();

            //viewJoin.Restaurants = null;
            viewJoin.Dishes = null;

            if (id == null)
            {
                return NotFound();
            }

            //var review = from r in _context.Review.Include(r => r.App).Include(r => r.Name).Include(r => r.UserName)
            //           join usr in _context.User on r.UserNameId equals usr.Id
            //           where id == r.UserNameId
            //           select r;




            var allRestaurants = await _context.Restaurant.ToListAsync();
            //var allDishes = await _context.Dish.ToListAsync();
            var allDishes = await _context.Dish.Where(d => d.ID == id).ToListAsync();

            //var joinResult =
            //        from r in allRestaurants // This is the column that connects the 2 tables.
            //        join d in allDishes
            //        on r.ID equals d.RestaurantID into result
            //        select result;

            var query1 = from d in allDishes
                         join r in allRestaurants
                             on d.RestaurantID equals r.ID
                         select new { d, r };

            var query2 = from d in allDishes
                         join r in allRestaurants
                             on d.RestaurantID equals r.ID into result
                         select result;

            var query3 = from d in allDishes
                         join r in allRestaurants
                             on d.RestaurantID equals r.ID
                         select d;

            var query4 = from d in allDishes
                         join r in allRestaurants
                             on d.RestaurantID equals r.ID
                         where id == d.RestaurantID
                         select d;
            // קח את כל שמות המסעדות, מזג אותן עם כל המנות, היכן שהאיידי של מסעדה במסדר נתונים של מנות זהה לאיידי של רשימת המסעדות
            //  transaction/photo = dish table. user/person = restaurant table

            //if (query1 == null)
            //{
            //    return NotFound();
            //}

            viewJoin.Restaurants = query2.Distinct().SelectMany(x => x).ToList();
            viewJoin.Dishes = query3.ToList();
            //viewJoin.Restaurants = rest.Distinct().Select(x => x).ToList();

            return View(viewJoin);
        }
    }
}
