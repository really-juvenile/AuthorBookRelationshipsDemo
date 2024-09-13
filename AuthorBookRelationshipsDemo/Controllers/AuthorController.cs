using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuthorBookRelationshipsDemo.Data;
using AuthorBookRelationshipsDemo.Models;

namespace AuthorBookRelationshipsDemo.Controllers
{
    public class AuthorController : Controller
    {
        //Author Specific (authorized)
        public ActionResult Index()
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var users = session.Query<Author>().ToList();
                return View(users);
            }
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Author author)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    var existingUser = session.Query<Author>().FirstOrDefault(u => u.Id == author.Id);//edited Id instead of Name
                    author.AuthorDetails.Author = author;
                    session.Save(author);
                    txn.Commit();
                    return RedirectToAction("Index");
                }
            }
        }
        public ActionResult Edit(int id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var user = session.Query<Author>().SingleOrDefault(u => u.Id == id);
                return View(user);
            }


        }
        [HttpPost]
        public ActionResult Edit(Author author)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    author.AuthorDetails.Author = author;
                    session.Update(author);
                    txn.Commit();
                    return RedirectToAction("Index");
                }
            }
        }

        public ActionResult Delete(int id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var user = session.Get<Author>(id);
                session.Delete(user);
                return View(user);


            }
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteProduct(int id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var user = session.Get<Author>(id);
                    session.Delete(user);
                    transaction.Commit();
                    return RedirectToAction("Index");
                }
            }
        }
    }
}