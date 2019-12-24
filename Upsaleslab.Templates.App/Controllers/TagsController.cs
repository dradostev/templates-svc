using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Upsaleslab.Templates.App.Models;
using Upsaleslab.Templates.App.Requests;
using Upsaleslab.Templates.App.Services;

namespace Upsaleslab.Templates.App.Controllers
{
    [ApiController, Route("tags")]
    public class TagsController : Controller
    {
        private readonly ITagService _tagService;

        private Guid UserId => Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ??
                                          throw new InvalidOperationException());

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpPost, Authorize(Roles = "admin")]
        public async Task<ActionResult<Tag>> Post([FromBody] CreateTag request) =>
            await _tagService.CreateAsync(request, UserId) switch
            {
                (Result.Successful, var template) => StatusCode(201, template),
                (Result.Conflict, null) => Conflict(),
                _ => (ActionResult<Tag>) StatusCode(500)
            };
        
        [HttpPut("{tagId}"), Authorize(Roles = "admin")]
        public async Task<ActionResult<Tag>> Post(Guid tagId, [FromBody] UpdateTag request) =>
            await _tagService.UpdateAsync(tagId, request, UserId) switch
            {
                (Result.Successful, var template) => StatusCode(201, template),
                (Result.Conflict, null) => Conflict(),
                (Result.NotFound, null) => NotFound(),
                (Result.Gone, null) => StatusCode(410),
                _ => (ActionResult<Tag>) StatusCode(500)
            };
        
        [HttpDelete("{tagId}"), Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(Guid tagId, [FromBody] DeleteTag request) =>
            await _tagService.DeleteAsync(tagId, request, UserId) switch
            {
                Result.Successful => Ok(),
                Result.Conflict => Conflict(),
                Result.NotFound => NotFound(),
                Result.Gone => StatusCode(410),
                _ => StatusCode(500)
            };

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> Get() =>
            Ok(await _tagService.ListAsync());
    }
}