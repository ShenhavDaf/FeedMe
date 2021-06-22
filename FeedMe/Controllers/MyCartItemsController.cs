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
            MyCartItem c = new MyCartItem();
            c.Dish = new Dish();
            c.DishID = id;
            c.Quantity = 1;
            c.SaveQ = false;

            foreach (var item in _context.Dish) // get dish values.
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

            var userEmail = User.Claims.ToList()[0].Value;

            foreach (var item in _context.User)//get the currect user that is log in.
            {
                if (item.Email == userEmail)
                {
                    foreach (var cart in _context.MyCart) // get user cart values.
                    {
                        if (item.Id == cart.UserID)
                        {
                            item.MyCart = cart;
                        }
                    }
                    if (item.MyCart.MyCartItems == null)
                    {
                        item.MyCart.MyCartItems = new List<MyCartItem>();
                    }
                    item.MyCart.TotalAmount += c.Price; //update all new cartItem data.
                    c.MyCartID = item.MyCart.ID;
                    c.MyCart = item.MyCart;
                    item.MyCart.MyCartItems.Add(c);
                }

            }

            c.Dish = null; // So that the dishes won't created again in the dish database.
            _context.Add(c);
            await _context.SaveChangesAsync();

            var myCartItem = await _context.MyCartItem.Include(r => r.Dish).FirstOrDefaultAsync(m => m.ID == c.ID);

          

            if (myCartItem == null)
            {
                return NotFound();

            }
            return View(myCartItem);
        }

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
        //public async Task<IActionResult> Edit(int id, [Bind("ID,DishID,Quantity,Price,MyCartID")] MyCartItem myCartItem)
        public async Task<IActionResult> Edit(int quantity)
        {
            var myCartItem = new MyCartItem();

            foreach (var cartItem in _context.MyCartItem)
            {
                if (cartItem.SaveQ == false)
                {
                    myCartItem = cartItem;

                    foreach (var cart in _context.MyCart) // find cartItem cart
                    {
                        if (cartItem.MyCartID == cart.ID)
                        {
                            cartItem.MyCart = cart;
                        }
                    }

                    myCartItem.SaveQ = true;// so we wont change the quantity again.
                    if (quantity != 1) //if the quantity is 1 no need make changes.
                    {
                        myCartItem.Quantity = quantity;
                        myCartItem.MyCart.TotalAmount += ((myCartItem.Price) * (quantity - 1)); 
                    }
                    break;
                }
            }

            _context.Update(myCartItem); //update new quantity.
            await _context.SaveChangesAsync();

            foreach (var dish in _context.Dish) //find our dish
            {
                if (myCartItem.DishID == dish.ID)
                {
                    myCartItem.Dish = dish;
                    break;
                }
            }

            foreach (var restaurant in _context.Restaurant) //find the restaurant of our dish.
            {
                if (myCartItem.Dish.RestaurantID == restaurant.ID)
                {
                    myCartItem.Dish.Restaurant = restaurant;
                    break;
                }
            }

            if (myCartItem.Dish.Restaurant == null)
            {
                return NotFound();
            }

            //Show the restaurant that the dish belongs.
            return RedirectToAction("Details", "Restaurants", new { id = myCartItem.Dish.RestaurantID }); 

        }

        //if (id != myCartItem.ID)
        //{
        //    return NotFound();
        //}

        //if (ModelState.IsValid)
        //{
        //    try
        //    {
        //        _context.Update(myCartItem);
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!MyCartItemExists(myCartItem.ID))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //    return RedirectToAction(nameof(Index));
        //}
        //ViewData["DishID"] = new SelectList(_context.Dish, "ID", "Description", myCartItem.DishID);
        //ViewData["MyCartID"] = new SelectList(_context.Set<MyCart>(), "ID", "ID", myCartItem.MyCartID);
        //return View(myCartItem);

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
