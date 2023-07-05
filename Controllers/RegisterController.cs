using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using System.Data;

namespace TechTest.Controllers
{
    [Authorize]
    public class RegisterController : Controller
    {
        private readonly TechTestDbContext _contexto;

        public RegisterController(TechTestDbContext contexto)
        {
            _contexto = contexto;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            return _contexto.USUARIO.AsNoTracking() != null ?
                        View(await _contexto.USUARIO.ToListAsync()) :
                        Problem("Entity set 'TechTestDbContext.UserModel'  is null.");
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _contexto.USUARIO.AsNoTracking() == null)
            {
                return NotFound();
            }

            var userModel = await _contexto.USUARIO.AsNoTracking()
                .FirstOrDefaultAsync(m => m.UserId == id);
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
        public async Task<IActionResult> Create([Bind("UserId,Email,UserPassword,UserStatus,UserDisDate,UserModDate")] LoginModel user)
        {
            //if (ModelState.IsValid)
            //{
            //    user.UserStatus = true;
            //    user.UserDisDate = DateTime.Now;
            //    user.UserModDate = DateTime.Now;
            //    _contexto.Add(user);
            //    await _contexto.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(user);
            bool register;
            string message;
            try
            {
                if (ModelState.IsValid)
                {
                    using (SqlConnection con = new(_contexto.Connection))
                    {
                        using (SqlCommand cmd = new("RegisterUser_SP", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = user.Email;
                            cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = user.UserPassword;
                            cmd.Parameters.Add("Registered", SqlDbType.Bit).Direction = ParameterDirection.Output;
                            cmd.Parameters.Add("Message", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();

                            register = Convert.ToBoolean(cmd.Parameters["Registered"].Value);
                            message = cmd.Parameters["Message"].Value.ToString();
                        }
                    }
                    return RedirectToAction("User", "Index");
                }
            }
            catch (Exception)
            {
                return View();
            }
            ViewData["error"] = "Error de credenciales";
            return View();
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _contexto.USUARIO.AsNoTracking() == null)
            {
                return NotFound();
            }

            var user = await _contexto.USUARIO.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Email,UserPassword,UserStatus,UserDisDate,UserModDate")] LoginModel user)
        {
            if (id != user.UserId)
            {   
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    user.UserModDate = DateTime.Now;
                    _contexto.Update(user);
                    await _contexto.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserModelExists(user.UserId))
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
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _contexto.USUARIO.AsNoTracking() == null)
            {
                return NotFound();
            }

            var user = await _contexto.USUARIO.AsNoTracking()
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_contexto.USUARIO == null)
            {
                return Problem("Entity set 'TechTestDbContext.UserModel'  is null.");
            }
            var user = await _contexto.USUARIO.FindAsync(id);
            if (user != null)
            {
                user.UserStatus = false;
                user.UserModDate = DateTime.Now;
                _contexto.USUARIO.Update(user);
            }

            await _contexto.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserModelExists(int id)
        {
            return (_contexto.USUARIO?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}