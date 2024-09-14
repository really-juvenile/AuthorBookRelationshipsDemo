using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using AuthorBookRelationshipsDemo.Data;
using System.Web.Security;
using AuthorBookRelationshipsDemo.ViewModels;
using AuthorBookRelationshipsDemo.Models;

namespace AuthorBookRelationshipsDemo.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        //login register and (GetAllbooks)book list(by all authors) unauthorized
        //Book name, auhtor name

        // Login action
        public ActionResult Login(LoginVM loginVM)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    // Look for the user by username
                    var user = session.Query<Author>().SingleOrDefault(u => u.UserName == loginVM.UserName);

                    // Check if user exists and password matches the stored hashed password
                    if (user != null && BCrypt.Net.BCrypt.Verify(loginVM.Password, user.Password))
                    {
                        // If the password is valid, set the auth cookie and redirect
                        FormsAuthentication.SetAuthCookie(loginVM.UserName, true);
                        return RedirectToAction("Index", "Author");
                    }

                    // If login fails, show error message
                    ModelState.AddModelError("", "UserName/Password doesn't match");
                    return View();
                }
            }
        }

        // Registration action
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Author author)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    // Hash the password using BCrypt
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(author.Password);

                    // Set the hashed password
                    author.Password = hashedPassword;

                    // Ensure AuthorDetails is correctly associated
                    if (author.AuthorDetails != null)
                    {
                        author.AuthorDetails.Author = author;
                    }

                    // Save the author to the database
                    session.Save(author);
                    txn.Commit();

                    return RedirectToAction("Login");
                }
            }
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");

        }
    }
}