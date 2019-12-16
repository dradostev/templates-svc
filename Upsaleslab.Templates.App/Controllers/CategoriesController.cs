using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Upsaleslab.Templates.App.Models;
using Upsaleslab.Templates.App.Requests;
using Upsaleslab.Templates.App.Services;

namespace Upsaleslab.Templates.App.Controllers
{
    [ApiController, Route("categories")]
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService _categoriesService;

        private Guid UserId => Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ??
                                          throw new InvalidOperationException());

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [HttpPost]
        public async Task<ActionResult<Category>> Post([FromBody] CreateCategory request) =>
            await _categoriesService.CreateCategoryAsync(request, UserId) switch
            {
                (Result.Successful, var template) => StatusCode(201, template),
                (Result.Conflict, null) => Conflict(),
                _ => (ActionResult<Category>) StatusCode(500)
            };
        
        [HttpPut("{catId}")]
        public async Task<ActionResult<Category>> Post(Guid catId, [FromBody] UpdateCategory request) =>
            await _categoriesService.UpdateCategoryAsync(catId, request, UserId) switch
            {
                (Result.Successful, var template) => StatusCode(201, template),
                (Result.Conflict, null) => Conflict(),
                (Result.NotFound, null) => NotFound(),
                (Result.Gone, null) => StatusCode(410),
                _ => (ActionResult<Category>) StatusCode(500)
            };
        
        [HttpDelete("{catId}")]
        public async Task<ActionResult> Delete(Guid catId, [FromBody] DeleteCategory request) =>
            await _categoriesService.DeleteCategoryAsync(catId, request, UserId) switch
            {
                Result.Successful => Ok(),
                Result.Conflict => Conflict(),
                Result.NotFound => NotFound(),
                Result.Gone => StatusCode(410),
                _ => StatusCode(500)
            };

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Get() =>
            Ok(await _categoriesService.ListCategoriesAsync());
    }
}