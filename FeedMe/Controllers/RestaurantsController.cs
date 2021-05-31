using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FeedMe.Data;
using ourProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace FeedMe.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly FeedMeContext _context;

        public RestaurantsController(FeedMeContext context)
        {
            _context = context;
        }

        // GET: Restaurants
        public async Task<IActionResult> Index()
        {
            //return View(await _context.Restaurant.Include(c => c.Categories).Include(d => d.DeliveryCities).ToListAsync());
            return View(await _context.Restaurant.ToListAsync());
        }

        // GET: Restaurants/Details/5
        [Authorize(Roles = "Admin,rManager")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurant.FirstOrDefaultAsync(m => m.ID == id);

            //restaurant.Categories = new List<Category>();
            //restaurant.Categories.AddRange(_context.Category);

            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // GET: Restaurants/Create
        [Authorize(Roles = "Admin,rManager")]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Category, nameof(Category.ID), nameof(Category.Name));
            ViewBag.Cities = new SelectList(_context.City, nameof(City.ID), nameof(City.Name));
            return View();
        }

        // POST: Restaurants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Admin,rManager")]
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
                return RedirectToAction(nameof(Index));
            }
            return View(restaurant);
        }

        // GET: Restaurants/Edit/5
        [Authorize(Roles = "Admin,rManager")]
        public async Task<IActionResult> Edit(int? id)
        {
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
            return View(restaurant);
        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,rManager")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,RestaurantImage,Description,Address,PhoneNumber,Rate, Categories")] Restaurant restaurant)
        {
            if (id != restaurant.ID)
            {
                return NotFound();
            }

          

            if (ModelState.IsValid)
            {
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var restaurant = await _context.Restaurant.FindAsync(id);
            _context.Restaurant.Remove(restaurant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantExists(int id)
        {
            return _context.Restaurant.Any(e => e.ID == id);
        }
    }
}
