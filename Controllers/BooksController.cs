using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Technical_Assessement_API.Data;
using Technical_Assessement_API.Models;

namespace Technical_Assessement_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ApiContext _context;

        public BooksController(ApiContext context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBook()
        {
            return await _context.Book.ToListAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Book.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.ID || !ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
               List< AuthorsBooks> authorsBooks = _context.AuthorsBooks.Where(a=>a.BookID==book.ID).ToList();

               
                for (int i = 0; i < authorsBooks.Count(); i++)
                {
                    authorsBooks[i].BookID = book.ID;
                    authorsBooks[i].AuthorID = book.Authorsid[i];
                    
                }

                _context.Book.Add(book);


                _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();

            }

        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                AuthorsBooks authorsBooks = new AuthorsBooks();
                
                List<int> au = book.Authorsid.ToList();
                for (int i = 0; i < au.Count ; i++)
                {
                    authorsBooks.BookID = book.ID;
                    authorsBooks.AuthorID = au[i];
                    _context.AuthorsBooks.Add(authorsBooks);

                }

                _context.Book.Add(book);

                await _context.SaveChangesAsync();

                return CreatedAtAction("GetBook", new { id = book.ID }, book);
            }
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Book.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //Get: api/Books/Authorofabook
        [HttpGet]
        [Route("GetAuthorNameOfABook")]
        public async Task<IEnumerable<string>> GetAuthorNameOfABook(int id)
        {
            //    IEnumerable<AuthorsBooks> authorsBooks  = _context.AuthorsBooks.Where(i => i.BookID == id);
            //    //_context.Author.Select(i=>i.Name).Include(authorsBooks.)
            //   IEnumerable<string> AuthorsNames= authorsBooks.Where(i=>i.BookID==id).Select(i=>i.Author.Name)

            //IEnumerable<string> AuthorsNames = await _context.AuthorsBooks.Where(i => i.BookID == id).Select(i => i.Author.Name).ToListAsync();
            return await _context.AuthorsBooks.Where(i => i.BookID == id).Select(i => i.Author.Name).ToListAsync();

            // return AuthorsNames;
        }
        [HttpGet]
        [Route("GetAllAuthorsName")]
        public async Task<IEnumerable<string>> GetAllAuthorsName()
        {

            return await _context.Author.Select(n=>n.Name).ToListAsync();
        }
        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.ID == id);
        }

    }
}
