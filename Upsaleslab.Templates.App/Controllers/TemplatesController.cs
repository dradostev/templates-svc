using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Upsaleslab.Projects.App.Models;
using Upsaleslab.Templates.App.Models;
using Upsaleslab.Templates.App.Requests;
using Upsaleslab.Templates.App.Services;

namespace Upsaleslab.Templates.App.Controllers
{
    [ApiController, Route("/")]
    public class TemplatesController : Controller
    {
        private readonly ITemplateService _templateService;

        private Guid UserId => Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ??
                                          throw new InvalidOperationException());

        public TemplatesController(ITemplateService templateService)
        {
            _templateService = templateService;
        }

        [HttpPost, Authorize(Roles = "admin")]
        public async Task<ActionResult<Template>> Post([FromBody] CreateTemplate request) =>
            await _templateService.CreateTemplateAsync(request, UserId) switch
            {
                (Result.Successful, var template) => StatusCode(201, template),
                (Result.Conflict, null) => Conflict(),
                _ => (ActionResult<Template>) StatusCode(500)
            };
        
        [HttpPut("{templateId}"), Authorize(Roles = "admin")]
        public async Task<ActionResult<Template>> Post(Guid templateId, [FromBody] UpdateTemplate request) =>
            await _templateService.UpdateTemplateAsync(templateId, request, UserId) switch
            {
                (Result.Successful, var template) => StatusCode(201, template),
                (Result.Conflict, null) => Conflict(),
                (Result.NotFound, null) => NotFound(),
                (Result.Gone, null) => StatusCode(410),
                _ => (ActionResult<Template>) StatusCode(500)
            };
        
        [HttpDelete("{templateId}"), Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(Guid templateId, [FromBody] DeleteTemplate request) =>
            await _templateService.DeleteTemplateAsync(templateId, request, UserId) switch
            {
                Result.Successful => Ok(),
                Result.Conflict => Conflict(),
                Result.NotFound => NotFound(),
                Result.Gone => StatusCode(410),
                _ => StatusCode(500)
            };

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Template>>> Get([FromQuery] Paginate request) =>
            Ok(await _templateService.ListTemplatesAsync(request));

        [HttpGet("{templateId}")]
        public async Task<ActionResult<Template>> Get(Guid templateId) =>
            await _templateService.FindTemplateAsync(templateId) switch
            {
                (Result.Successful, var template) => Ok(template),
                (Result.NotFound, null) => NotFound(),
                (Result.Gone, null) => StatusCode(410),
                _ => (ActionResult<Template>) StatusCode(500)
            };
    }
}