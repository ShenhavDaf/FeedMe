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
        //public async Task<IActionResult> Details(int? id)
        public async Task<IActionResult> Details(int? id)
        {

            MyCart myCart = new MyCart();
            myCart.MyCartItems = new List<MyCartItem>();
            myCart.TotalAmount = 0;

            // If there is a request for cart details from the restaurant page we will use cookies.
            if (id == null)
            {
                var userEmail = User.Claims.ToList()[0].Value;

                foreach (var item in _context.User) //Get the currect user that is log in.
                {
                    if (item.Email == userEmail)
                    {
                        foreach (var cart in _context.MyCart) // Get user cart values.
                        {
                            if (item.Id == cart.UserID)
                            {
                                myCart = cart; // Here The cart doesn't receive the data on the cartItems.
                            }
                        }
                    }
                }
            }
            else //The details of the cart are sent by its ID number.
            {
                myCart.ID = (int)id;
            }

            foreach (var myCartItem in _context.MyCartItem) //Get cartItems data.
            {
                if (myCart.ID == myCartItem.MyCartID)
                {
                    foreach (var dish in _context.Dish) //Get dish data.
                    {
                        if (dish.ID == myCartItem.DishID)
                        {
                            myCartItem.Dish = dish;
                            myCartItem.DishID = dish.ID;
                            myCartItem.Price = dish.Price;
                            break;
                        }
                    }
                    myCartItem.MyCart = myCart; //Save the data in the cart and the cartItem.
                    myCartItem.MyCartID = myCart.ID;
                    myCart.TotalAmount += ((myCartItem.Price) * (myCartItem.Quantity));
                    myCart.MyCartItems.Add(myCartItem);
                }
            }

            if (myCart == null)
            {
                return NotFound();
            }

            return View(myCart);
        }

        /////// מפה עובדדדדדד
        //        MyCart myCart = new MyCart();
        //myCart.MyCartItems = new List<MyCartItem>();
        //myCart.ID = id;
        //myCart.TotalAmount = 0;

        //Find all the cartItems that belong to the customer's cart.
        //foreach (var myCartItem in _context.MyCartItem) 
        //{
        //    if (myCart.ID == myCartItem.MyCartID)
        //    {
        //        foreach (var dish in _context.Dish)
        //        {
        //            if (dish.ID == myCartItem.DishID)
        //            {
        //                myCartItem.Dish = dish;
        //                myCartItem.DishID = dish.ID;
        //                myCartItem.Price = dish.Price;
        //                break;
        //            }
        //        }
        //        myCartItem.MyCart = myCart; 
        //        myCartItem.MyCartID = myCart.ID;
        //        myCart.TotalAmount += ((myCartItem.Price) * (myCartItem.Quantity));
        //        myCart.MyCartItems.Add(myCartItem);
        //    }
        //}



        //if (id == null)
        //{
        //    return NotFound();
        //}

        //var myCart = await _context.MyCart.Include(r => r.MyCartItems).FirstOrDefaultAsync(m => m.ID == id);

        //if (myCart == null)
        //{
        //    return NotFound();
        //}

        //return View(myCart);



        //restaurant.Categories = new List<Category>();
        //restaurant.Categories.AddRange(_context.Category);

        //    if (myCart == null)
        //    {
        //        return NotFound();


        //        MyCart myCart = new MyCart();
        //        var userEmail = User.Claims.ToList()[0].Value;

        //        foreach (var item in _context.User)//get the currect user that is log in.
        //        {
        //            if (item.Email == userEmail)
        //            {
        //                foreach (var cart in _context.MyCart) // get user cart values.
        //                {
        //                    if (item.Id == cart.UserID)
        //                    {
        //                        item.MyCart = cart;
        //                        myCart = cart;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        //if (id == null)
        //{
        //    return NotFound();
        //}

        //   var myC = await _context.MyCart.Include(r => r.MyCartItems).FirstOrDefaultAsync(m => m.ID == id);

        //restaurant.Categories = new List<Category>();
        //restaurant.Categories.AddRange(_context.Category);



        //        //foreach (var cartItem in _context.MyCartItem)
        //        //{
        //        //    if (cartItem.SaveQ == false)
        //        //    {
        //        //        myCartItem = cartItem;

        //        //        foreach (var cart in _context.MyCart) // find cartItem cart
        //        //        {
        //        //            if(cartItem.MyCartID == cart.ID)
        //        //            {
        //        //                cartItem.MyCart = cart;
        //        //            }
        //        //        }

        //        //        myCartItem.SaveQ = true;
        //        //        if (quantity != 1)
        //        //        {
        //        //            myCartItem.Quantity = quantity;
        //        //            myCartItem.MyCart.TotalAmount += ((myCartItem.Price) * (quantity - 1));
        //        //        }
        //        //        break;
        //        //    }
        //        //}

        //        //foreach (var dish in _context.Dish)
        //        //{
        //        //    if (myCartItem.DishID == dish.ID)
        //        //    {
        //        //        myCartItem.Dish = dish;
        //        //        break;
        //        //    }
        //        //}

        //        //foreach (var restaurant in _context.Restaurant)
        //        //{
        //        //    if (myCartItem.Dish.RestaurantID == restaurant.ID)
        //        //    {
        //        //        myCartItem.Dish.Restaurant = restaurant;
        //        //        break;
        //        //    }
        //        //}

        //        //if (myCartItem.Dish.Restaurant == null)
        //        //{
        //        //    return NotFound();
        //        //}
        //        ////https://localhost:44376/MyCartItems/Details/96
        //        //// ourProject.Models.Restaurant restaurant1 = myCartItem.Dish.Restaurant;
        //        //// return View("Views/Restaurants/" + myCartItem.Dish.RestaurantID);
        //        //return RedirectToAction("Details", "Restaurants", new { id = myCartItem.Dish.RestaurantID });

        //        //return RedirectToAction("Details/" + myCartItem.Dish.RestaurantID, "Restaurants"/*, "/" + myCartItem.Dish.RestaurantID*/);

        //        // return View(myCartItem.Dish.Restaurant);


        //    }









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

        //public IActionResult saveQuantity(int quantity)
        //{
        //    var myCartItem = new MyCartItem();

        //    foreach (var cartItem in _context.MyCartItem)
        //    {
        //        if (cartItem.SaveQ == false)
        //        {
        //            myCartItem = cartItem;

        //            foreach (var cart in _context.MyCart) // find cartItem cart
        //            {
        //                if (cartItem.MyCartID == cart.ID)
        //                {
        //                    cartItem.MyCart = cart;
        //                }
        //            }

        //            myCartItem.SaveQ = true;
        //            if (quantity != 1)
        //            {
        //                myCartItem.Quantity = quantity;
        //                myCartItem.MyCart.TotalAmount += ((myCartItem.Price) * (quantity - 1));
        //            }
        //            break;
        //        }
        //    }

        //    foreach (var dish in _context.Dish)
        //    {
        //        if (myCartItem.DishID == dish.ID)
        //        {
        //            myCartItem.Dish = dish;
        //            break;
        //        }
        //    }

        //    foreach (var restaurant in _context.Restaurant)
        //    {
        //        if (myCartItem.Dish.RestaurantID == restaurant.ID)
        //        {
        //            myCartItem.Dish.Restaurant = restaurant;
        //            break;
        //        }
        //    }

        //    if (myCartItem.Dish.Restaurant == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(myCartItem.Dish.Restaurant);
        //}
    }
}
