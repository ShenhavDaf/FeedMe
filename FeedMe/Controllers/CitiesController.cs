using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FeedMe.Data;
using Microsoft.AspNetCore.Authorization;
using FeedMe.Models;

namespace FeedMe.Controllers
{
    public class CitiesController : Controller
    {
        private readonly FeedMeContext _context;

        public CitiesController(FeedMeContext context)
        {
            _context = context;
        }

        // GET: Cities
        /* public async Task<IActionResult> Index()
         {
             return View(await _context.City.ToListAsync());
         }*/
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string searchString)
        {
            var cities = from m in _context.City.OrderBy(x => x.Name)
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                cities = cities.Where(s => s.Name.Contains(searchString));
            }
            return View(await cities.ToListAsync());
        }

        public JsonResult GetCitiesList()
        {
            List<City> citiesList = new List<City>();
            foreach (var item in _context.City)
            {
                citiesList.Add(item);
                //Console.WriteLine(item.Name);
            }
            return Json(citiesList);
        }


        // GET: Cities/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.City
                .FirstOrDefaultAsync(m => m.ID == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // GET: Cities/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("ID,Name")] City city)
        {
            if (ModelState.IsValid)
            {
                _context.Add(city);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(city);
        }

        // GET: Cities/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.City.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            return View(city);
        }

        // POST: Cities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name")] City city)
        {
            if (id != city.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(city);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityExists(city.ID))
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
            return View(city);
        }

        // GET: Cities/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.City
                .FirstOrDefaultAsync(m => m.ID == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // POST: Cities/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var city = await _context.City.FindAsync(id);
            _context.City.Remove(city);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        private bool CityExists(int id)
        {
            return _context.City.Any(e => e.ID == id);
        }
    }
}
