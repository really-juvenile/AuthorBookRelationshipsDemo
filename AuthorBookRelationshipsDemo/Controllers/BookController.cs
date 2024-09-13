using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuthorBookRelationshipsDemo.Data;
using AuthorBookRelationshipsDemo.Models;
using NHibernate.Criterion;

namespace AuthorBookRelationshipsDemo.Controllers
{
    public class BookController : Controller
    {
        // GET: Book

        //Author Specific (Particular Books) (Authorized)
        public ActionResult Index()
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var books = session.Query<Book>().ToList();
                return View(books);
            }
        }

        
        public ActionResult BookDetails(int authId)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var books = session.Query<Book>().Where(a => a.Author.Id == authId).ToList();
                ViewBag.AuthorId = authId;

                return View(books);
            }
        }

        // Create Order
        public ActionResult Create(int authId)
        {
            var book = new Book { Author = new Author { Id = authId } };
            ViewBag.AuthorId = authId;
            return View(book);
        }

        [HttpPost]
        public ActionResult Create(Book book)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    var author = session.Get<Author>(book.Author.Id);
                    if (author == null) return HttpNotFound();

                    book.Author = author;
                    session.Save(book);
                    txn.Commit();
                    return RedirectToAction("BookDetails", new { authId = author.Id });
                }
            }
        }

        // Edit Order
        public ActionResult Edit(int id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var book = session.Get<Book>(id);
                ViewBag.AuthorId = book.Author.Id;
                if (book == null) return HttpNotFound();
                return View(book);
            }
        }

        [HttpPost]
        public ActionResult Edit(Book book)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    var existingBook = session.Get<Book>(book.Id);
                    if (existingBook == null) return HttpNotFound();

                    existingBook.Description = book.Description;
                    session.Update(existingBook);
                    txn.Commit();
                    return RedirectToAction("BookDetails", new { authId = existingBook.Author.Id });
                }
            }
        }

        // Delete Order
        public ActionResult Delete(int id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var book = session.Get<Book>(id);
                ViewBag.AuthorId = book.Author.Id;
                if (book == null) return HttpNotFound();
                return View(book);
            }
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    var book = session.Get<Book>(id);
                    if (book == null) return HttpNotFound();

                    session.Delete(book);
                    txn.Commit();
                    return RedirectToAction("BookDetails", new { authId = book.Author.Id });
                }
            }
        }
    }
}