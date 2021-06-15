using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FeedMe.Data;
using FeedMe.Models;

namespace FeedMe.Controllers
{
    public class Cart1Controller : Controller
    {
        private readonly FeedMeContext _context;

        public Cart1Controller(FeedMeContext context)
        {
            _context = context;
        }

        // GET: Cart1
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cart1.ToListAsync());
        }

        // GET: Cart1/Details/5
        public async Task<IActionResult> Details(int id)
        { 
            Cart1 c = null;
            //  Cart1 cart1 = new Cart1(); 
            foreach (var item in _context.CartItem1)
            {
                if(item.ID == id)
                {
                    foreach (var dish in _context.Dish)
                    {
                        if (item.DishID == dish.ID)//save dish values
                        {
                            item.Dish.Name = dish.Name;
                            item.Dish.Price = dish.Price;
                            item.Dish.RestaurantID = dish.RestaurantID;
                            item.Dish.FoodType = dish.FoodType;
                            item.Dish.DishImage = dish.DishImage;
                            item.Dish.Description = dish.Description;
                            break;
                        }
                    }

                    if(item.cart1 != null)
                    {
                        c = item.cart1;
                        item.cart1.TotalAmount += item.Price;
                        item.cart1.CartItems.Add(item);
                        _context.Add(item.cart1);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        c = new Cart1();
                        c.CartItems = new List<CartItem1>();
                        item.CartID = c.ID;
                        c.TotalAmount = item.Price;
                        c.CartItems.Add(item);                  
                    }
                    break;
                }

            }

            _context.Add(c);
            await _context.SaveChangesAsync();

         /*   if (id == null)
            {
                return NotFound();
            }*/

            var cart1 = await _context.Cart1.Include(r => r.CartItems).FirstOrDefaultAsync(m => m.ID == c.ID);
            //var cart1 = await _context.Cart1
            //    .FirstOrDefaultAsync(m => m.ID == id);
            //cart1 = await _context.Cart1.Include(r => r.CartItems).FirstOrDefaultAsync(m => m.ID == i);

            if (cart1 == null)
            {
                return NotFound();
            }

            return View(cart1);
        }

        // GET: Cart1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cart1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,TotalAmount")] Cart1 cart1)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart1);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cart1);
        }

        // GET: Cart1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart1 = await _context.Cart1.FindAsync(id);
            if (cart1 == null)
            {
                return NotFound();
            }
            return View(cart1);
        }

        // POST: Cart1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,TotalAmount")] Cart1 cart1)
        {
            if (id != cart1.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart1);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Cart1Exists(cart1.ID))
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
            return View(cart1);
        }

        // GET: Cart1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart1 = await _context.Cart1
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cart1 == null)
            {
                return NotFound();
            }

            return View(cart1);
        }

        // POST: Cart1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cart1 = await _context.Cart1.FindAsync(id);
            _context.Cart1.Remove(cart1);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Cart1Exists(int id)
        {
            return _context.Cart1.Any(e => e.ID == id);
        }
    }
}
