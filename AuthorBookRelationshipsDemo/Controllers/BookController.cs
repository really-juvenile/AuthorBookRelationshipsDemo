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
    [Authorize]
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


        public ActionResult BookDetails(Guid authId)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var books = session.Query<Book>().Where(a => a.Author.Id == authId).ToList();
                ViewBag.AuthorId = authId;

                return View(books);
            }
        }

        // Create Order
        public ActionResult Create(Guid authId)
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
        //public ActionResult Delete(int id)
        //{
        //    using (var session = NHibernateHelper.CreateSession())
        //    {
        //        var authId = Session["AuthorId"];
        //        var author = session.Query<Author>().FirstOrDefault(a => a.Id == (Guid)authId);

        //        if (author == null)
        //        {
        //            return HttpNotFound("Author not found");
        //        }

        //        var targetBook = author.Books.FirstOrDefault(o => o.Id == id);

        //        if (targetBook == null)
        //        {
        //            return HttpNotFound("Book not found");
        //        }

        //        return View(targetBook);
        //    }
        //}


        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteBook(int id)
        //{
        //    using (var session = NHibernateHelper.CreateSession())
        //    {
        //        using (var txn = session.BeginTransaction())
        //        {

        //            var targetBook = session.Get<Book>(id);

        //            session.Delete(targetBook);

        //            txn.Commit();

        //            return RedirectToAction("BookDetails");
        //        }
        //    }

        //}

        //Delete Order
        public ActionResult Delete(Guid id)
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
        public ActionResult DeleteConfirmed(Guid id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var book = session.Get<Book>(id);

                if (book != null)
                {

                    using (var txn = session.BeginTransaction())
                    {

                        session.Delete(book);
                        txn.Commit();

                    }
                }
                return RedirectToAction("BookDetails", new { authId = book.Author.Id });
            }
        }
    }
}