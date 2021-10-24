
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
    public class GenresController : ControllerBase
    {
        private readonly IRepository<Genre> _genreRepository;
        public GenresController(IRepository<Genre> genreRepository)
        {
            _genreRepository = genreRepository;
        }
        // GET: api/genres
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var genres = await _genreRepository.GetItemsAsync();
            return new OkObjectResult(genres);
        }

        // GET api/genres/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var genre = await _genreRepository.GetItemByIdAsync(id);
            if (genre == null)
            {
                return NotFound();
            }
            return new OkObjectResult(genre);
        }

        // POST api/genres
        [HttpPost]
        public async Task<IActionResult> Post(Genre genre)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                try
                {
                    await _genreRepository.InsertItemAsync(genre);
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
            return CreatedAtAction(nameof(Get), new { id = genre.Id }, genre);
        }

        // PUT api/genres/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Genre genre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (genre != null)
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    try
                    {
                        await _genreRepository.UpdateItemAsync(genre);
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

        // DELETE api/genres/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _genreRepository.GetItemByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            await _genreRepository.DeleteItemAsync(book);

            return new OkResult();
        }
    }
}
