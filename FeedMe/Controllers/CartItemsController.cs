using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FeedMe.Data;
using FeedMe.Models;
using ourProject.Models;

namespace FeedMe.Controllers
{
    public class CartItemsController : Controller
    {
        private readonly FeedMeContext _context;

        public CartItemsController(FeedMeContext context)
        {
            _context = context;
        }

        // GET: CartItems
        public async Task<IActionResult> Index()
        {
            var feedMeContext = _context.CartItem.Include(c => c.Dish);
            return View(await feedMeContext.ToListAsync());
        }

        // GET: CartItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItem
                .Include(c => c.Dish)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cartItem == null)
            {
                return NotFound();
            }

            return View(cartItem);
        }

        // GET: CartItems/Create
/*        public IActionResult Create(int id)
        {
            ViewData["DishID"] = id;
            ViewData["Dish"] = _context.Dish.Include(m => m.ID == id);
            foreach (var item in _context.Dish)
            {
                if (item.ID == id)
                {
                    ViewData["dish"] = item;
                }
                *//* ViewData["Quantity"] = new SelectList(_context.CartItem, "ID", "Quantity");
                 ViewData["Price"] = new SelectList(_context.Dish, "ID", "Price");*//*


            }
            return View();
        }*/
            // GET: CartItems/Create
/*            public IActionResult Create()
            {
                ViewData["DishID"] = new SelectList(_context.Dish, "ID", "Name");
                return View();
            }*/

            // POST: CartItems/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            /*[ValidateAntiForgeryToken]*/
            public async Task<IActionResult> Create([Bind("ID,DishID,Quantity,Price")] CartItem cartItem)
        {
            if (ModelState.IsValid)
            {
                //dish.Restaurant = new Restaurant();
                /*cartItem.Dish = new Dish();
                cartItem.Dish = _context.Dish.Any(m => m.ID == dish);
                cartItem.Dish = _context.Dish.FirstOrDefault(m => m.ID == dish);*/
                //dish.Restaurant = _context.Restaurant.Any(e => e.ID == restaurant);
                //dish.Restaurant = _context.Restaurant.FirstOrDefaultAsync(m => m.ID == restaurant);
                //dish.Restaurant = new Restaurant();
                //dish.Restaurant.Name = _context.Restaurant.Where(x => dish.Contains(x.RestaurantID));
                //restaurant.Categories = new List<Category>();
                //restaurant.Categories.AddRange(_context.Category.Where(x => categories.Contains(x.ID)));
                _context.Add(cartItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DishID"] = new SelectList(_context.Dish, "ID", "Name", cartItem.DishID);
            return Json(cartItem);
        }

        // GET: CartItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItem.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }
            ViewData["DishID"] = new SelectList(_context.Dish, "ID", "Name", cartItem.DishID);
            return View(cartItem);
        }

        // POST: CartItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,DishID,Quantity,Price")] CartItem cartItem)
        {
            if (id != cartItem.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cartItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartItemExists(cartItem.ID))
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
            ViewData["DishID"] = new SelectList(_context.Dish, "ID", "Name", cartItem.DishID);
            return View(cartItem);
        }

        // GET: CartItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItem
                .Include(c => c.Dish)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cartItem == null)
            {
                return NotFound();
            }

            return View(cartItem);
        }

        // POST: CartItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cartItem = await _context.CartItem.FindAsync(id);
            _context.CartItem.Remove(cartItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartItemExists(int id)
        {
            return _context.CartItem.Any(e => e.ID == id);
        }
    }
}
