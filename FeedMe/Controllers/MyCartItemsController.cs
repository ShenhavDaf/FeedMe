using System;
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
    public class MyCartItemsController : Controller
    {
        private readonly FeedMeContext _context;

        public MyCartItemsController(FeedMeContext context)
        {
            _context = context;
        }

        // GET: MyCartItems
        public async Task<IActionResult> Index()
        {
            var feedMeContext = _context.MyCartItem.Include(m => m.Dish).Include(m => m.MyCart);
            return View(await feedMeContext.ToListAsync());
        }

        // GET: MyCartItems/Details/5
        public async Task<IActionResult> Details(int id)
        {
            //We created a CartItem object in case the packet does not exist in our database
            MyCartItem c = new MyCartItem();
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
                    c.Dish.Price = item.Price;
                    c.Price = item.Price;
                    break;
                }
            }
            string s = HttpContext.Session.GetString("cart");
            if (s != null)
            {
                c.MyCartID = Int32.Parse(s);
                foreach (var item in _context.MyCart)
                {
                    if (item.ID == c.MyCartID)
                    {
                        c.MyCart = item;
                        c.MyCart.TotalAmount += c.Price;
                        c.MyCart.MyCartItems.Add(c);
                        break;
                    }
                }
            }
            else
            {
                MyCart cart = new MyCart();
                cart.MyCartItems = new List<MyCartItem>();
                cart.TotalAmount = c.Price;
                c.MyCartID = cart.ID;
                c.MyCart = cart;
                //  c.cart1.CartItems.Add(c);
                cart.MyCartItems.Add(c);
                string l = cart.ID.ToString();
                HttpContext.Session.SetString("cart", l);
            }


            _context.Add(c);
            await _context.SaveChangesAsync();

            /*    if (id == null)
                {
                    return NotFound();
                }*/

            var myCartItem = await _context.MyCartItem.Include(r => r.Dish).FirstOrDefaultAsync(m => m.ID == c.ID);

            if (myCartItem == null)
            {
                return NotFound();
            }

            return View(myCartItem);
        }
          /*  if (id == null)
            {
                return NotFound();
            }

            var myCartItem = await _context.MyCartItem
                .Include(m => m.Dish)
                .Include(m => m.MyCart)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (myCartItem == null)
            {
                return NotFound();
            }

            return View(myCartItem);
        }*/

        // GET: MyCartItems/Create
        public IActionResult Create()
        {
            ViewData["DishID"] = new SelectList(_context.Dish, "ID", "Description");
            ViewData["MyCartID"] = new SelectList(_context.Set<MyCart>(), "ID", "ID");
            return View();
        }

        // POST: MyCartItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,DishID,Quantity,Price,MyCartID")] MyCartItem myCartItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(myCartItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DishID"] = new SelectList(_context.Dish, "ID", "Description", myCartItem.DishID);
            ViewData["MyCartID"] = new SelectList(_context.Set<MyCart>(), "ID", "ID", myCartItem.MyCartID);
            return View(myCartItem);
        }

        // GET: MyCartItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myCartItem = await _context.MyCartItem.FindAsync(id);
            if (myCartItem == null)
            {
                return NotFound();
            }
            ViewData["DishID"] = new SelectList(_context.Dish, "ID", "Description", myCartItem.DishID);
            ViewData["MyCartID"] = new SelectList(_context.Set<MyCart>(), "ID", "ID", myCartItem.MyCartID);
            return View(myCartItem);
        }

        // POST: MyCartItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,DishID,Quantity,Price,MyCartID")] MyCartItem myCartItem)
        {
            if (id != myCartItem.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(myCartItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MyCartItemExists(myCartItem.ID))
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
            ViewData["DishID"] = new SelectList(_context.Dish, "ID", "Description", myCartItem.DishID);
            ViewData["MyCartID"] = new SelectList(_context.Set<MyCart>(), "ID", "ID", myCartItem.MyCartID);
            return View(myCartItem);
        }

        // GET: MyCartItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myCartItem = await _context.MyCartItem
                .Include(m => m.Dish)
                .Include(m => m.MyCart)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (myCartItem == null)
            {
                return NotFound();
            }

            return View(myCartItem);
        }

        // POST: MyCartItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var myCartItem = await _context.MyCartItem.FindAsync(id);
            _context.MyCartItem.Remove(myCartItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MyCartItemExists(int id)
        {
            return _context.MyCartItem.Any(e => e.ID == id);
        }
    }
}
