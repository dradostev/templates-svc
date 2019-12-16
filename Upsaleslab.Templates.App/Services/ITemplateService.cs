using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Upsaleslab.Projects.App.Models;
using Upsaleslab.Templates.App.Models;
using Upsaleslab.Templates.App.Requests;

namespace Upsaleslab.Templates.App.Services
{
    public interface ITemplateService
    {
        Task<(Result, Template?)> CreateTemplateAsync(CreateTemplate request);
        Task<(Result, Template?)> UpdateTemplateAsync(Guid templateId, UpdateTemplate request);
        Task<Result> DeleteTemplateAsync(Guid templateId, DeleteTemplate request);
        Task<(Result, Template?)> FindTemplateAsync(Guid templateId);
        Task<IEnumerable<Template>> ListTemplatesAsync(Paginate request, string category = null);
    }
}