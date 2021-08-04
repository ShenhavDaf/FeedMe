using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FeedMe.Data;
using FeedMe.Models;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(Roles = "Admin,rManager,Client")]
        public async Task<IActionResult> Index()
        {
            var feedMeContext = _context.MyCart.Include(m => m.User).OrderBy(x => x.ID); // All carts
            if (User.IsInRole("Admin"))
                feedMeContext = feedMeContext.OrderBy(x => x.UserID); // All carts

            if (User.IsInRole("rManager") || User.IsInRole("Client"))
            {
                var userEmail = User.Claims.ToList()[0].Value;
                var curUsers = _context.User.Where(u => u.Email == userEmail);
                var userCarts = from c in feedMeContext
                                join u in curUsers
                                  on c.UserID equals u.Id
                                select c;

                return View(userCarts);
            }

            return View(await feedMeContext.ToListAsync());
            //return View(await _context.MyCart.ToListAsync());
        }

        public async Task<IActionResult> Search(string searchString)
        {
            var carts = from m in _context.MyCart.Include(m => m.User).OrderBy(x => x.TotalAmount)
                        select m;

            carts = carts.Where(s => (s.User.Name.Contains(searchString) || searchString == null));

            return View("Index", await carts.ToListAsync());
        }

        // GET: MyCarts/Details/5
        [Authorize(Roles = "Admin,rManager,Client")]
        public async Task<IActionResult> Details(int? id)
        {
            MyCart myCart = new MyCart();
            myCart.MyCartItems = new List<MyCartItem>();
            myCart.TotalAmount = 0;
            myCart.IsClose = false;

            // If there is a request for cart details from the restaurant page we will use cookies.
            if (id == null)
            {
                var userEmail = User.Claims.ToList()[0].Value;
                foreach (var user in _context.User) //Get the currect user that is log in.
                {
                    if (user.Email == userEmail)
                    {// לעבור על הרשימה של הקארטים של היוזר
                        foreach (var cart in _context.MyCart.Include(i=>i.MyCartItems)) // Get user cart values.
                        {
                            if (user.Id == cart.UserID)
                            {
                                //if (cart.IsClose == true)
                                //    user.MyCarts.Add(cart);
                                //else
                                if (cart.IsClose == false)
                                {
                                    myCart.UserID = cart.UserID;
                                    myCart.ID = cart.ID; // Here The cart doesn't receive the data on the cartItems.
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else //The details of the cart are sent by its ID number.
            {
                myCart.ID = (int)id;

                if (User.IsInRole("rManager") || User.IsInRole("Client"))
                {
                    var userEmail = User.Claims.ToList()[0].Value;
                    foreach (var user in _context.User) //Get the currect user that is log in.
                    {
                        if (user.Email == userEmail)
                        {// לעבור על הרשימה של הקארטים של היוזר
                            foreach (var cart in _context.MyCart) // Get user cart values.
                            {
                                if (user.Id == cart.UserID)
                                {
                                    var c = user.MyCarts.Where(x => x.ID == id);
                                    if (c.Count() <= 0)
                                        return NotFound();
                                }
                            }
                        }
                    }
                }
            }

            foreach (var myCartItem in _context.MyCartItem.Include(d=>d.Dish)) //Get cartItems data.
            {
                if (myCart.ID == myCartItem.MyCartID)
                {
                    if (myCartItem.SaveQ == false)//Checks if the buyer didn't approved the cart item than it won't add to the cart.
                    {
                        //myCart.TotalAmount -= myCartItem.Price;
                        //  _context.Update(myCart); //לבדוק אם צריךךךך
                        _context.Remove(myCartItem);
                        //_context.Update(myCart);
                        continue;
                    }

                    foreach (var dish in _context.Dish.Include(r=>r.Restaurant)) //Get dish data so we can see dish photo in the cart.
                    {
                        if (dish.ID == myCartItem.DishID)
                        {
                            myCartItem.Dish = dish;
                            //myCartItem.Price = dish.Price;
                            break;
                        }
                    }
                    //myCartItem.MyCart = myCart; //Save the data in the cart and the cartItem.
                    // myCartItem.MyCartID = myCart.ID;
                    myCart.TotalAmount += ((myCartItem.Price) * (myCartItem.Quantity));
                    myCart.MyCartItems.Add(myCartItem);
                    //_context.Update(myCart); //לבדוק אם צריךךךך
                }
            }

            await _context.SaveChangesAsync();

            if (myCart == null)
            {
                return NotFound();
            }

            return View(myCart);
        }

        // GET: MyCarts/Create
        public IActionResult Create()
        {
            return View();
            //ViewData["UserID"] = new SelectList(_context.User, "Id", "Address");
            //return View();
        }

        public async Task<IActionResult> Pay(int? id)
        {
            var myCart = await _context.MyCart.FindAsync(id);
            if (myCart == null || myCart.TotalAmount == 0)
            {
                return View("Create");
            }

            await _context.SaveChangesAsync();

            return View(myCart);
        }

        public async Task<IActionResult> Delivery(int? id)
        {
            var myCart = await _context.MyCart.FindAsync(id);
            foreach (var cart in _context.MyCart)
                if (cart.ID == id)
                {
                    cart.IsClose = true;
                    _context.Update(cart);
                    break;
                }

            await _context.SaveChangesAsync();

            return View(myCart);
        }

        // POST: MyCarts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,TotalAmount,UserID,IsClose")] MyCart myCart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(myCart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserID"] = new SelectList(_context.User, "Id", "Address", myCart.UserID);
            return View(myCart);

            //if (ModelState.IsValid)
            //{
            //    _context.Add(myCart);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(myCart);
        }

        // GET: MyCarts/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!(User.IsInRole("Admin")))
            {
                var userEmail = User.Claims.ToList()[0].Value;
                var user = _context.User.Where(u => u.Email == userEmail).FirstOrDefault();
                var allCarts = await _context.MyCart.Where(c => c.UserID == user.Id).ToListAsync();
                if (allCarts == null)
                    return NotFound();
                var curCart = allCarts.Where(e => e.ID == id).ToList();
                if (curCart == null || curCart.Count() <= 0)
                    return NotFound();
                if (!MyCartExists((int)id))
                {
                    return NotFound();
                }
                var isClose = curCart.First().IsClose;
                if (isClose)
                    return NotFound();
            }

            var myCart = await _context.MyCart.FindAsync(id);
            if (myCart == null)
            {
                return NotFound();
            }
            ViewData["ID"] = new SelectList(_context.MyCart, "ID", "ID", myCart.ID);
            ViewData["UserID"] = new SelectList(_context.User, "Id", "Name", myCart.UserID);
            return View(myCart);
        }

        // POST: MyCarts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,TotalAmount,UserID,IsClose")] MyCart myCart)
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
            ViewData["UserID"] = new SelectList(_context.User, "Id", "Address", myCart.UserID);
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
                .Include(m => m.User)
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
