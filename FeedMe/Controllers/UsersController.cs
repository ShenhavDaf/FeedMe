using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FeedMe.Data;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using FeedMe.Models;

namespace FeedMe.Controllers
{
    public class UsersController : Controller
    {
        private readonly FeedMeContext _context;

        public UsersController(FeedMeContext context)
        {
            //var feedMeContext = _context.User.Include(u => u.Restaurant);
            _context = context;
        }

        // LOGOUT FROM USER
        public async Task<IActionResult> Logout()
        {
            //LOGOUT VIA SESSION: HttpContext.Session.Clear();

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        // REDIRECT TO VIEW OF LOGIN
        // GET: Users/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Users/Login
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Id,Email,Password,Name,Address,PhoneNumber,BirthdayDate")] User user, string ReturnUrl)
        {
            //var qName = _context.User.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);
            /*user.Name = "a";
            user.PhoneNumber = "d";*/

            if (ModelState.IsValid)
            {   // Search inside User's DB the user with the same Email & Password
                var q = from u in _context.User
                        where u.Email == user.Email && u.Password == user.Password
                        select u;
                //var q = _context.User.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);
                int count = await q.CountAsync(); // Check if the user that we searched for is exist in the DB

                // _context.Update(user);
                //  await _context.SaveChangesAsync(); 

                if (count > 0)
                {
                    foreach (var u in _context.User)//Find the current user id.
                        if (u.Email == user.Email)
                            user.Id = u.Id;

                    if (user.MyCarts == null) //If the buyer didn't finish his buy on register day or the times after.
                    {
                        user.MyCarts = new List<MyCart>();
                    }

                    foreach (var cart in _context.MyCart) //Check if the user had open carts that he didnt paid on.
                    {
                        if (cart.UserID == user.Id)
                        {
                            if (cart.IsClose == true)
                                user.MyCarts.Add(cart);
                            else
                            {
                                _context.Remove(cart);
                            }
                        }
                    }

                    MyCart myCart = new MyCart();
                    myCart.UserID = user.Id;
                    myCart.MyCartItems = new List<MyCartItem>();
                    myCart.TotalAmount = 0;
                    myCart.IsClose = false;
                    user.MyCarts.Add(myCart);

                    _context.Update(myCart);
                    await _context.SaveChangesAsync();

                    //HttpContext.Session.SetString("email", q.First().Email);
                    //HttpContext.Session.SetString("type", q.First().Type.ToString());
                    Signin(q.First());
                    //_context.Add(user);
                    //await _context.SaveChangesAsync();

                    if (!String.IsNullOrEmpty(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                        //return RedirectToAction(WebUtility.UrlEncode(ReturnUrl));
                    }
                    else
                    {
                        return RedirectToAction(nameof(Index), "Home");
                    }
                }
                else
                {
                    ViewData["Error"] = "Username and/or password are incorrect.";
                }
            }
            return View(user);
        }

        private async void Signin(User account)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Role, account.Type.ToString()),
                new Claim(ClaimTypes.Name, account.Name),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
            };

            // The action takes all 3 data from above and use it to create cookie authentication
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        // GET: Users/AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }

        // GET: Users/Birthday
        public IActionResult Birthday()
        {
            return View();
        }


        // GET: Users/Register
        public IActionResult Register()
        {
            ViewData["RestaurantId"] = new SelectList(_context.Restaurant, "ID", "Address");
            return View();
        }

        // POST: Users/Register
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,Email,Password,Name,Address,PhoneNumber,BirthdayDate,RestaurantId")] User user)
        {
            //user.MyCart = new MyCart();
            //user.MyCart.MyCartItems = new List<MyCartItem>();
            //user.MyCart.UserID = user.Id;
            //user.MyCart.TotalAmount = 0;

            if (ModelState.IsValid)
            {
                var q = _context.User.FirstOrDefault(u => u.Email == user.Email);

                if (q == null)
                {
                    user.MyCarts = new List<MyCart>();
                    MyCart myCart = new MyCart();
                    myCart.UserID = user.Id;
                    myCart.MyCartItems = new List<MyCartItem>();
                    myCart.TotalAmount = 0;
                    myCart.IsClose = false;
                    user.MyCarts.Add(myCart);

                    user.Type = UserType.Client;
                    _context.Add(user);
                    await _context.SaveChangesAsync();

                    var u = _context.User.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);
                    Signin(u);

                    return RedirectToAction(nameof(Index), "Home");
                }
                else
                {
                    ViewData["Error"] = "Unable to comply; cannot register this user.";
                }
            }
            ViewData["RestaurantId"] = new SelectList(_context.Restaurant, "ID", "Address", user.RestaurantId);
            return View(user);
        }


        // GET: Users
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var res = _context.User.Include(x => x.Restaurant);
            return View(await res.ToListAsync());
        }

        //Sherch by name, address and phone number
        public async Task<IActionResult> Search(string query)
        {
            var user = from m in _context.User
                       select m;

            user = user.Where(s => (s.Name.Contains(query) || query == null) ||
            s.PhoneNumber.Contains(query) || s.Address.Contains(query));

            return View("Index", await user.ToListAsync());
        }


        // GET: Users/Details/5
        [Authorize(Roles = "Admin,rManager,Client")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .Include(u => u.Restaurant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            var userEmail = User.Claims.ToList()[0].Value;
            if (User.IsInRole("rManager") || User.IsInRole("Client"))
            {
                foreach (var u in _context.User) //Get the currect user that is log in.
                {
                    if (u.Email == userEmail)
                    {
                        if (u.Id != id)
                        {
                            return NotFound();
                        }
                    }
                }
            }
            return View(user);
        }

        public async Task<IActionResult> GetUserID()
        {
            var userEmail = User.Claims.ToList()[0].Value;
            if (User.IsInRole("Admin") || User.IsInRole("rManager") || User.IsInRole("Client"))
            {
                foreach (var u in _context.User) //Get the currect user that is log in.
                {
                    if (u.Email == userEmail)
                    {
                        return RedirectToAction("Details", "Users", new { id = u.Id });
                        break;
                    }
                }
            }
            return View("Details");
        }



        // GET: Users/Edit/5
        [Authorize(Roles = "Admin,rManager,Client")]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.UserType = new SelectList(Enum.GetNames(typeof(UserType)));
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["RestaurantId"] = new SelectList(_context.Restaurant, "ID", "Address", user.RestaurantId);

            var userEmail = User.Claims.ToList()[0].Value;
            if (User.IsInRole("rManager") || User.IsInRole("Client"))
            {
                foreach (var u in _context.User) //Get the currect user that is log in.
                {
                    if (u.Email == userEmail)
                    {
                        if (u.Id != id)
                        {
                            return NotFound();
                        }
                    }
                }
            }

            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,rManager,Client")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Password,Name,Address,PhoneNumber,BirthdayDate,Type,RestaurantId")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (User.IsInRole("rManager"))
                {
                    user.Type = UserType.rManager;
                }
                if (User.IsInRole("Client"))
                {
                    user.Type = UserType.Client;
                }

                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (!User.IsInRole("Admin"))
                {
                    return RedirectToAction("Details", "Users", new { id = user.Id });
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RestaurantId"] = new SelectList(_context.Restaurant, "ID", "Address", user.RestaurantId);

            return View(user);
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .Include(u => u.Restaurant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
