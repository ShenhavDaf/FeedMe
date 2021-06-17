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
    public class MyCartsController : Controller
    {
        private readonly FeedMeContext _context;

        public MyCartsController(FeedMeContext context)
        {
            _context = context;
        }

        // GET: MyCarts
        public async Task<IActionResult> Index()
        {
            return View(await _context.MyCart.ToListAsync());
        }

        // GET: MyCarts/Details/5
        public async Task<IActionResult> Details(int id)
        {
            MyCartItem myCartItem = new MyCartItem();
            myCartItem.Quantity = 1;

            foreach (var dish in _context.Dish)
            {
                if (id == dish.ID)//save dish values
                {
                    dish.Name = dish.Name;
                    dish.Price = dish.Price;
                    dish.RestaurantID = dish.RestaurantID;
                    dish.FoodType = dish.FoodType;
                    dish.DishImage = dish.DishImage;
                    dish.Description = dish.Description;
                    myCartItem.Dish = dish;
                    myCartItem.DishID = id;
                    myCartItem.Price = dish.Price;
                    break;
                }
            }

            foreach (var item in _context.User)
            {
                if(item.Email == User.Claims.ToList()[0].Value) 
                { 
                    if(item.MyCart == null)
                    {
                        item.MyCart.MyCartItems = new List<MyCartItem>();
                        item.MyCart.TotalAmount = myCartItem.Price;
                    }
                    else
                    {
                        item.MyCart.TotalAmount += myCartItem.Price;
                    }
                    myCartItem.MyCartID = item.MyCart.ID;
                    myCartItem.MyCart = item.MyCart;
                    item.MyCart.MyCartItems.Add(myCartItem);
                   
                }

            }
            
            MyCart c = null;
            //  Cart1 cart1 = new Cart1(); 
            foreach (var item in _context.MyCartItem)
            {
                if (item.ID == id)
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

                    if (item.MyCart != null)
                    {
                        c = item.MyCart;
                        item.MyCart.TotalAmount += item.Price;
                        item.MyCart.MyCartItems.Add(item);
                        _context.Add(item.MyCart);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        c = new MyCart();
                        c.MyCartItems = new List<MyCartItem>();
                        item.MyCartID = c.ID;
                        c.TotalAmount = item.Price;
                        c.MyCartItems.Add(item);
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

            var myCart = await _context.MyCart.Include(r => r.MyCartItems).FirstOrDefaultAsync(m => m.ID == c.ID);
            //var cart1 = await _context.Cart1
            //    .FirstOrDefaultAsync(m => m.ID == id);
            //cart1 = await _context.Cart1.Include(r => r.CartItems).FirstOrDefaultAsync(m => m.ID == i);

            if (myCart == null)
            {
                return NotFound();
            }

            return View(myCart);
        }
        /* if (id == null)
         {
             return NotFound();
         }

         var myCart = await _context.MyCart
             .FirstOrDefaultAsync(m => m.ID == id);
         if (myCart == null)
         {
             return NotFound();
         }

         return View(myCart);
     }*/

        // GET: MyCarts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MyCarts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,TotalAmount")] MyCart myCart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(myCart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(myCart);
        }

        // GET: MyCarts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myCart = await _context.MyCart.FindAsync(id);
            if (myCart == null)
            {
                return NotFound();
            }
            return View(myCart);
        }

        // POST: MyCarts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,TotalAmount")] MyCart myCart)
        {
            if (id != myCart.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(myCart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MyCartExists(myCart.ID))
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
            return View(myCart);
        }

        // GET: MyCarts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myCart = await _context.MyCart
                .FirstOrDefaultAsync(m => m.ID == id);
            if (myCart == null)
            {
                return NotFound();
            }

            return View(myCart);
        }

        // POST: MyCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var myCart = await _context.MyCart.FindAsync(id);
            _context.MyCart.Remove(myCart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MyCartExists(int id)
        {
            return _context.MyCart.Any(e => e.ID == id);
        }
    }
}
