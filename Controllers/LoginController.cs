using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using Microsoft.Win32;
using System.Data.SqlClient;
using System.Web;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using TechTest.Models;
using Microsoft.Data.SqlClient;

namespace TechTest.Controllers
{
    
    public class LoginController : Controller
    {

        private readonly TechTestDbContext _contexto;

        public LoginController(TechTestDbContext contexto)
        {
            _contexto = contexto;
        }

        public IActionResult Login()
        {
            ClaimsPrincipal c = HttpContext.User;
            if (c.Identity != null)
            {
                if (c.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

		[HttpPost]
		public async Task<IActionResult> Login(LoginModel user)
		{
			try
			{
				using (SqlConnection cn = new(_contexto.Connection))
				{
					using (SqlCommand cmd = new SqlCommand("sp_ValidateUser", cn))
					{
						cmd.CommandType = System.Data.CommandType.StoredProcedure;
						cmd.Parameters.Add("@Email", System.Data.SqlDbType.VarChar).Value = user.Email;
						cmd.Parameters.Add("@Password", System.Data.SqlDbType.VarChar).Value = user.UserPassword;
						cmd.CommandType = CommandType.StoredProcedure;

						cn.Open();
						var dr = cmd.ExecuteReader();
						while (dr.Read())
						{
							if (user.Email != null && user.UserStatus != true)
							{
								List<Claim> claims = new List<Claim>()
						{
							new Claim(ClaimTypes.NameIdentifier, user.Email)
						};
								ClaimsIdentity ci = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
								AuthenticationProperties p = new();
								p.AllowRefresh = true;
								await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci), p);
								return RedirectToAction("Index", "Home");
							}
							else
							{
								ViewData["Message"] = "Credenciales incorrectas o cuenta no registrada.";
							}
						}
						cn.Close();
					}

					return View();
				}
			}
			catch (Exception e)
			{
				ViewBag.Error = e.Message;
				return View();
			}
		}


		[HttpPost]
		public ActionResult Registrar(LoginModel user)
		{
			bool register;
			string message;
			try
			{
				if (ModelState.IsValid)
				{
					using (SqlConnection con = new(_contexto.Connection))
					{
						using (SqlCommand cmd = new("Sp_RegisterUser", con))
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
					return RedirectToAction("Home", "Index");
				}
			}
			catch (Exception)
			{
				return View();
			}
			ViewData["error"] = "Error de credenciales";
			return View();
		}





	}
}