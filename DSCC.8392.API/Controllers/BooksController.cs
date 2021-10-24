
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
        public BooksController(IRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }
        // GET: api/books
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var books = await _bookRepository.GetItemsAsync(b=>b.Genre);
            return new OkObjectResult(books);
        }

        // GET api/books/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
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
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
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
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (book != null)
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

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
