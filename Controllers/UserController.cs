using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechTest.Models;
using Microsoft.AspNetCore.Authorization;

namespace TechTest.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly TechTestDbContext _contexto;

        public UserController(TechTestDbContext contexto)
        {
            _contexto = contexto;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
              return _contexto.PERSONA.AsNoTracking() != null ? 
                          View(await _contexto.PERSONA.ToListAsync()) :
                          Problem("Entity set 'TechTestDbContext.UserModel'  is null.");
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _contexto.PERSONA.AsNoTracking() == null)
            {
                return NotFound();
            }

            var userModel = await _contexto.PERSONA.AsNoTracking()
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (userModel == null)
            {
                return NotFound();
            }

            return View(userModel);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonId,Email,PersonName,PersonAge,PersonStatus,PersonSex,PersonDisDate,PersonModDate")] UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                userModel.PersonStatus = true;
                userModel.PersonDisDate = DateTime.Now;
                userModel.PersonModDate = DateTime.Now;
				_contexto.Add(userModel);
                await _contexto.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userModel);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _contexto.PERSONA.AsNoTracking() == null)
            {
                return NotFound();
            }

            var userModel = await _contexto.PERSONA.FindAsync(id);
            if (userModel == null)
            {
                return NotFound();
            }
            return View(userModel);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PersonId,Email,PersonName,PersonAge,PersonStatus,PersonSex,PersonDisDate")] UserModel userModel)
        {
            if (id != userModel.PersonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    userModel.PersonModDate= DateTime.Now;
					_contexto.Update(userModel);
                    await _contexto.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserModelExists(userModel.PersonId))
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
            return View(userModel);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _contexto.PERSONA.AsNoTracking() == null)
            {
                return NotFound();
            }

            var userModel = await _contexto.PERSONA.AsNoTracking()
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (userModel == null)
            {
                return NotFound();
            }

            return View(userModel);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_contexto.PERSONA == null)
            {
                return Problem("Entity set 'TechTestDbContext.UserModel'  is null.");
            }
            var userModel = await _contexto.PERSONA.FindAsync(id);
            if (userModel != null)
            {
                userModel.PersonStatus = false;
                userModel.PersonModDate = DateTime.Now;
				_contexto.PERSONA.Update(userModel);
            }
            
            await _contexto.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserModelExists(int id)
        {
          return (_contexto.PERSONA?.Any(e => e.PersonId == id)).GetValueOrDefault();
        }
    }
}
