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
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myCart = await _context.MyCart.Include(r => r.MyCartItems).FirstOrDefaultAsync(m => m.ID == id);

            foreach (var myCartItem in _context.MyCartItem)
            {
                if (myCart.ID == myCartItem.MyCartID)
                {
                    foreach(var dish in _context.Dish)
                    {
                        if(dish.ID == myCartItem.DishID)
                        {
                            myCartItem.Dish = dish;
                            myCartItem.DishID = dish.ID;
                            myCartItem.Price = dish.Price;
                            myCartItem.Quantity = 1; ////////////////////////////// לטפל
                            break;
                        }
                    }
                    //myCartItem.MyCart = myCart;
                    //myCartItem.MyCartID = myCart.ID;
                    //myCart.MyCartItems = new List<MyCartItem>();
                    //myCart.MyCartItems.Add(myCartItem);
/*                    _context.Add(myCartItem);
                    await _context.SaveChangesAsync();*/
                }
            }


            //restaurant.Categories = new List<Category>();
            //restaurant.Categories.AddRange(_context.Category);

            if (myCart == null)
            {
                return NotFound();
            }

            return View(myCart);
            //MyCart myCart = new MyCart();
            //var userEmail = User.Claims.ToList()[0].Value;

            //foreach (var item in _context.User)//get the currect user that is log in.
            //{
            //    if (item.Email == userEmail)
            //    {
            //        foreach (var cart in _context.MyCart) // get user cart values.
            //        {
            //            if (item.Id == cart.UserID)
            //            {
            //                item.MyCart = cart;
            //                myCart = cart;
            //            }
            //        }
            //    }
            //}
            ////if (id == null)
            ////{
            ////    return NotFound();
            ////}

            ////var myCart = await _context.MyCart.Include(r => r.MyCartItems).FirstOrDefaultAsync(m => m.ID == id);

            ////restaurant.Categories = new List<Category>();
            ////restaurant.Categories.AddRange(_context.Category);

            //if (myCart == null)
            //{
            //    return NotFound();
            //}

            //return View(myCart);
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

        public IActionResult Pay()
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
