using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Technical_Assessement_API.Data;
using Technical_Assessement_API.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Technical_Assessement_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public AuthorController(ApiContext context,IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment; 
        }

        // GET: api/Author
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthor()
        {
            return await _context.Author.ToListAsync();
        }

        // GET: api/Author/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            var author = await _context.Author.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            return author;
        }

        // PUT: api/Author/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, Author author)
        {
            if (id != author.ID)
            {
                return BadRequest();
            }

            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
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

        // POST: api/Author
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Author>> PostAuthor(Author author)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            //Model is valid
            else
            {
                //to add img first:add it to (file) like wwwroot file on mvc core to save in server then save its name to db
                //  to add it to Static file(IMAges)wwwroot :it's a static file & to get path use webhostenvironment interface 
                //---to deal with webhostenvironment  interface should inject it and register in DI---
                //first: catch file(img)from request form cus files route on form request
                //second: get path of www
                //third: copy this file to a file streaam which save it in wwwroot(Images)

                //-----------------------------------------------------

                //var files = HttpContext.Request.Form.Files;
                //string webRootPath = _webHostEnvironment.WebRootPath;
                ////creating

                //string upload = webRootPath + WebConstant.ImgPathAuthor;
                //string filename = Guid.NewGuid().ToString();
                //string extension = Path.GetExtension(files[0].FileName);

                //using (var filestream = new FileStream(Path.Combine(upload, filename + extension), FileMode.Create))
                //{
                //    //add img to wwwroot
                //    files[0].CopyTo(filestream);
                //}
                //add product to db
                ///author.Image = filename + extension;
                //------------------------------------------------------------
                _context.Author.Add(author);

                await _context.SaveChangesAsync();

                return CreatedAtAction("GetAuthor", new { id = author.ID }, author);
            }
        }

        // DELETE: api/Author/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Author.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Author.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthorExists(int id)
        {
            return _context.Author.Any(e => e.ID == id);
        }


       /// return Author's books
       // get : api/Author/AuthorsBooksNames
        [HttpGet]
        [Route("GetBooksNamesofanAuthor")]
        public ActionResult<IEnumerable<string>> GetBooksNamesofanAuthor(int id)
        {
            IEnumerable<AuthorsBooks> Authors_Books = _context.AuthorsBooks.Where(i => i.AuthorID == id);
            //List<Book> list_books = _context.Book.Where(i => i.ID );
            IEnumerable<int> books = Authors_Books.Select(i => i.BookID);
            IEnumerable<String> BooksNames = _context.Book.Select(i => i.Title);

            return BooksNames.ToList();
        }
    }
}
