using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FeedMe.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using FeedMe.Models;

namespace FeedMe.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly FeedMeContext _context;

        public RestaurantsController(FeedMeContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var allRestaurants = await _context.Restaurant.ToListAsync();
            //var allDishes = await _context.Dish.ToListAsync();

            var resList = new List<Restaurant>
            {

            };

            if (User.IsInRole("Admin") || User.IsInRole("rManager"))
            {
                var userEmail = User.Claims.ToList()[0].Value;
                foreach (var user in _context.User) //Get the currect user that is log in.
                {
                    if (user.Email == userEmail)
                    {
                        if (user.RestaurantId != null)
                        {
                            var allUsers = await _context.User.Where(u => u.Email == userEmail).ToListAsync();
                            var uRestaurant = from r in allRestaurants
                                              join u in allUsers
                                              on r.ID equals u.RestaurantId
                                              select r;

                            resList.Add(uRestaurant.First());
                            foreach (var res in allRestaurants)
                            {
                                if (res != resList[0])
                                    resList.Add(res);
                            }

                            return View(resList.ToList());
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            var restaurant = from m in _context.Restaurant
                             select m;

            return View(restaurant);
        }

        //Search by name, category, city and Description
        public async Task<IActionResult> Search(string searchString)
        {
            var restaurants = from m in _context.Restaurant
                              select m;

            restaurants = restaurants.Where(r => (r.Categories.Any(c => (c.Name.Contains(searchString))))|| r.Name.Contains(searchString)|| searchString==null || r.Description.Contains(searchString));

            return View("Index", await restaurants.ToListAsync());
        }

        // GET: Restaurants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurant.Include(r => r.Dishes).FirstOrDefaultAsync(m => m.ID == id);

            //restaurant.Categories = new List<Category>();
            //restaurant.Categories.AddRange(_context.Category);

            if (restaurant == null)
            {
                return NotFound();
            }
            ViewData["Restaurants"] = restaurant;

            return View(restaurant);
        }

        // GET: Restaurants/Create
        [Authorize(Roles = "Admin,rManager")]
        public IActionResult Create()
        {
            var userEmail = User.Claims.ToList()[0].Value;
            foreach (var user in _context.User) //Get the currect user that is log in.
            {
                if (user.Email == userEmail)
                {
                    if (user.RestaurantId != null)
                    {
                        // Return to new .cshtml page says you need to delete current restaurant in order to have a new one.
                        //return RedirectToAction(nameof(AlreadyOwnsRes));
                        return View("AlreadyOwnsRes");
                    }
                    else
                    {
                        break;
                    }
                }
            }

            ViewBag.Categories = new SelectList(_context.Category.OrderBy(x => x.Name).ToList(), nameof(Category.ID), nameof(Category.Name));
            ViewBag.Cities = new SelectList(_context.City.OrderBy(x => x.Name).ToList(), nameof(City.ID), nameof(City.Name));
            //var newList = _context.City.OrderBy(x => x.Name).ToList(); // ToList optional
            return View();
        }

        // POST: Restaurants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,rManager")]
        public async Task<IActionResult> Create([Bind("ID,Name,RestaurantImage,Description,Address,PhoneNumber,DeliveryCities, Categories")] Restaurant restaurant, int[] categories, int[] deliveryCities)
        {
            //if(HttpContext.Session.GetString("email") == null)
            //{
            //    return RedirectToAction("Login", "Users");
            //}
            if (ModelState.IsValid)
            {
                restaurant.Categories = new List<Category>();
                restaurant.Categories.AddRange(_context.Category.Where(x => categories.Contains(x.ID)));

                restaurant.DeliveryCities = new List<City>();
                restaurant.DeliveryCities.AddRange(_context.City.Where(x => deliveryCities.Contains(x.ID)));


                _context.Add(restaurant);
                await _context.SaveChangesAsync();
                ///
                var userEmail = User.Claims.ToList()[0].Value;
                foreach (var user in _context.User) //Get the currect user that is log in.
                {
                    if (user.Email == userEmail)
                    {
                        user.RestaurantId = restaurant.ID;
                        _context.Update(user);
                    }
                }
                ///
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(restaurant);
        }

        // GET: Restaurants/Edit/5
        [Authorize(Roles = "Admin,rManager")]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Cities = new SelectList(_context.City.OrderBy(x => x.Name).ToList(), nameof(City.ID), nameof(City.Name));

            ViewBag.Categories = new SelectList(_context.Category.OrderBy(x => x.Name).ToList(), nameof(Category.ID), nameof(Category.Name));

            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurant.FindAsync(id);
            //restaurant.Categories = _context.Category.Where()
            if (restaurant == null)
            {
                return NotFound();
            }

            //
            var userEmail = User.Claims.ToList()[0].Value;
            if (User.IsInRole("rManager"))
            {
                foreach (var user in _context.User) //Get the currect user that is log in.
                {
                    if (user.Email == userEmail)
                    {
                        if (user.RestaurantId != id)
                        {
                            return NotFound();
                        }
                    }
                }
            }
            //

            return View(restaurant);
        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,RestaurantImage,Description,Address,PhoneNumber,Rate, Categories")] Restaurant restaurant, int[] deliveryCities, int[] categories)
        {
            if (id != restaurant.ID)
            {
                return NotFound();
            }



            if (ModelState.IsValid)
            {
                restaurant.DeliveryCities = new List<City>();
                restaurant.DeliveryCities.AddRange(_context.City.Where(x => deliveryCities.Contains(x.ID)));
                restaurant.Categories = new List<Category>();
                restaurant.Categories.AddRange(_context.Category.Where(x => categories.Contains(x.ID)));

                try
                {
                    _context.Update(restaurant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantExists(restaurant.ID))
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
            return View(restaurant);
        }

        public async Task<IActionResult> EditRate(int id, [Bind("ID,Name,RestaurantImage,Description,Address,PhoneNumber,Rate, Categories")] Restaurant restaurant, int[] deliveryCities)
        {
            if (id != restaurant.ID)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                restaurant.DeliveryCities = new List<City>();
                restaurant.DeliveryCities.AddRange(_context.City.Where(x => deliveryCities.Contains(x.ID)));

                try
                {
                    _context.Update(restaurant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantExists(restaurant.ID))
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
            return View(restaurant);
        }

        // GET: Restaurants/Delete/5
        [Authorize(Roles = "Admin,rManager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (User.IsInRole("rManager"))
            {
                var resObj = await _context.Restaurant
                .FirstOrDefaultAsync(m => m.ID == id);

                var userEmail = User.Claims.ToList()[0].Value;
                foreach (var user in _context.User) //Get the currect user that is log in.
                {
                    if (user.Email == userEmail)
                    {
                        if (resObj != null && user.RestaurantId != resObj.ID)
                        {
                            return NotFound();
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurant
                .FirstOrDefaultAsync(m => m.ID == id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var restaurant = await _context.Restaurant.FindAsync(id);
            _context.Restaurant.Remove(restaurant);
            foreach(var user in _context.User)
            {
                if (user.RestaurantId == restaurant.ID)
                {
                    user.RestaurantId = null;
                    user.Restaurant = null;
                    _context.Update(user);
                    break;
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantExists(int id)
        {
            return _context.Restaurant.Any(e => e.ID == id);
        }


    }
}
