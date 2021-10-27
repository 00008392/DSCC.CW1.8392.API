
using DSCC._8392.DAL.Repositories;
using DSCC._8392.Domain.Contracts;
using DSCC._8392.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DSCC._8392.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<Genre> _genreRepository;
        public BooksController(IRepository<Book> bookRepository, IRepository<Genre> genreRepository)
        {
            _bookRepository = bookRepository;
            _genreRepository = genreRepository;
        }
        // GET: api/books
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //get all books and include book genre
            var books = await _bookRepository.GetItemsAsync(b=>b.Genre);
            return new OkObjectResult(books);
        }

        //it was decided to put this method in book controller for simplicity and because 
        //genre is related to book. Ideally, it should be in separate controller along with 
        //other actions on genre besides GetAll
        [HttpGet("genres")]
        public async Task<IActionResult> GetGenres()
        {
            //get all genres 
            var genres = await _genreRepository.GetItemsAsync();
            return new OkObjectResult(genres);
        }
        // GET api/books/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            //get particular book and include genre
            var book = await _bookRepository.GetItemByIdAsync(id, b=>b.Genre);
            if (book == null)
            {
                return NotFound();
            }
            return new OkObjectResult(book);
        }

        // POST api/books
        [HttpPost]
        public async Task<IActionResult> Post(Book book)
        {
            //create book
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                //check if all fields are correct
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //check if genre is valid
                if(!_genreRepository.IfExists(book.GenreId))
                {
                    return BadRequest("Not existing genre");
                }
                try
                {
                    await _bookRepository.InsertItemAsync(book);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
                finally
                {
                    scope.Complete();
                }
            }
            return CreatedAtAction(nameof(Get), new { id = book.Id }, book);
        }

        // PUT api/books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Book book)
        {
            //edit particular book
            //check if all fields are correct
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (book != null)
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    //check if genre is valid
                    if (!_genreRepository.IfExists(book.GenreId))
                    {
                        return BadRequest("Not existing genre");
                    }
                    try
                    {
                        await _bookRepository.UpdateItemAsync(book);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                    finally
                    {
                        scope.Complete();
                    }
                    return new OkResult();
                }
            }
            return BadRequest();
        }

        // DELETE api/books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            //delete particular book
            var book = await _bookRepository.GetItemByIdAsync(id);
            if (book==null)
            {
                return NotFound();
            }

            await _bookRepository.DeleteItemAsync(book);

            return new OkResult();
        }
    }
}
