/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FeedMe.Data;
using FeedMe.Models;
using Microsoft.AspNetCore.Http;

namespace FeedMe.Controllers
{
    public class CartItem1Controller : Controller
    {
        private readonly FeedMeContext _context;

        public CartItem1Controller(FeedMeContext context)
        {
            _context = context;
        }

        // GET: CartItem1
        public async Task<IActionResult> Index()
        {
            var feedMeContext = _context.CartItem1.Include(c => c.Dish);
            return View(await feedMeContext.ToListAsync());
        }

        // GET: CartItem1/Details/5
        public async Task<IActionResult> Details(int id)
        {
            //We created a CartItem object in case the packet does not exist in our database
            CartItem1 c = new CartItem1();
            c.Dish = new ourProject.Models.Dish();
            c.DishID = id;
        //  c.Dish.ID = id;
            c.Quantity = 1;
            foreach (var item in _context.Dish)
            {
                if (item.ID == id)
                {
                    c.Dish.Name = item.Name;
                    c.Dish.RestaurantID = item.RestaurantID;
                    c.Dish.DishImage = item.DishImage;
                    c.Dish.Description = item.Description;
                    c.Dish.FoodType = item.FoodType;
                    c.Dish.Price= item.Price;
                    c.Description = item.Description;
                    c.Price = item.Price;
                    break;
                }
            }
            string s = HttpContext.Session.GetString("cart");
            if (s != null)
            {        
                c.Cart1ID = Int32.Parse(s);
                foreach (var item in _context.Cart1)
                {
                    if(item.ID == c.Cart1ID)
                    {
                        c.Cart1 = item;
                        c.Cart1.TotalAmount += c.Price;
                        c.Cart1.CartItems.Add(c);
                        break;
                    }
                }
            }
            else {
                Cart1 cart = new Cart1();
                cart.CartItems = new List<CartItem1>();
                cart.TotalAmount = c.Price;
                c.Cart1ID = cart.ID;
                c.Cart1 = cart;
              //  c.cart1.CartItems.Add(c);
                cart.CartItems.Add(c);
                string l = cart.ID.ToString();
                HttpContext.Session.SetString("cart", l);
            }

            
            _context.Add(c);
            await _context.SaveChangesAsync();

        *//*    if (id == null)
            {
                return NotFound();
            }*//*

            var cartItem1 = await _context.CartItem1.Include(r => r.Dish).FirstOrDefaultAsync(m => m.ID == c.ID);
      
            if (cartItem1 == null)
            {
                return NotFound();
            }

            return View(cartItem1);
        }

        // GET: CartItem1/Create
        public IActionResult Create()
        {
            ViewData["DishID"] = new SelectList(_context.Dish, "ID", "Description");
            return View();
        }

        // POST: CartItem1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,DishID,Quantity,Price,Cart1ID,Description")] CartItem1 cartItem1)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cartItem1);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DishID"] = new SelectList(_context.Dish, "ID", "Description", cartItem1.DishID);
            return View(cartItem1);
        }

        // GET: CartItem1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartItem1 = await _context.CartItem1.FindAsync(id);
            if (cartItem1 == null)
            {
                return NotFound();
            }
            ViewData["DishID"] = new SelectList(_context.Dish, "ID", "Description", cartItem1.DishID);
            return View(cartItem1);
        }

        // POST: CartItem1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,DishID,Quantity,Price,Cart1ID,Description")] CartItem1 cartItem1)
        {
            if (id != cartItem1.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cartItem1);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartItem1Exists(cartItem1.ID))
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
            ViewData["DishID"] = new SelectList(_context.Dish, "ID", "Description", cartItem1.DishID);
            return View(cartItem1);
        }

        // GET: CartItem1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartItem1 = await _context.CartItem1
                .Include(c => c.Dish)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cartItem1 == null)
            {
                return NotFound();
            }

            return View(cartItem1);
        }

        // POST: CartItem1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cartItem1 = await _context.CartItem1.FindAsync(id);
            _context.CartItem1.Remove(cartItem1);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartItem1Exists(int id)
        {
            return _context.CartItem1.Any(e => e.ID == id);
        }
    }
}
*/