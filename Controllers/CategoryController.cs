using BlogApi.Data;
using BlogApi.Models;
using BlogApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogApi.Extensions;
using BlogApi.ViewModels.Categories;
using Microsoft.Extensions.Caching.Memory;

namespace BlogApi.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("v1/categories")]
        public async Task<IActionResult> GetAsync(
            [FromServices] BlogApiDataContext context,
            [FromServices] IMemoryCache cache)
        {
            try
            {
                var categories = cache.GetOrCreate("CategoriesCache", entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                    return GetCategories(context);
                });
                //var categories = await context.Categories.ToListAsync();
                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("0XE06 - Falha ao obter categorias"));
            }
        }
        private List<Category> GetCategories(BlogApiDataContext context)
        {
            return context.Categories.ToList();
        }
        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(
                [FromServices] BlogApiDataContext context,
                [FromRoute] int id)
        {
            try { 
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                return NotFound(new ResultViewModel<Category>("0XE12 - Categoria não encontrada"));
            }
            return Ok(new ResultViewModel<Category>(category));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("0XE13 - Falha interna no servidor"));
            }
        }

        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync(
                [FromServices] BlogApiDataContext context,
                [FromBody] EditorCategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));
            
            try
            {
                var category = new Category
                {
                    Id = 0,
                    Name = model.Name,
                    Slug = model.Slug.ToLower()
                };
                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();
                return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Category>("0XE01 - Não foi possível inserir a categoria"));
            } 
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("0XE11 - Falha interna no servidor"));
            }
        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync(
                [FromServices] BlogApiDataContext context,
                [FromBody] EditorCategoryViewModel model,
                [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound(new ResultViewModel<Category>("Categoria não encontrada"));

            try
            {
                category.Name = model.Name;
                category.Slug = model.Slug.ToLower();

                context.Categories.Update(category);
                await context.SaveChangesAsync();
                return Ok(category);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Category>("0XE02 - Não foi possível alterar a categoria"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("0XE12 - Falha interna no servidor"));
            }
        }

        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync(
                [FromServices] BlogApiDataContext context,
                [FromRoute] int id)
        {
            try 
            { 
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                    return NotFound();

                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                return Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Category>("0XE03 - Não foi possível excluir a categoria"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("0XE13 - Falha interna no servidor"));
            }
        }
    }
}

   