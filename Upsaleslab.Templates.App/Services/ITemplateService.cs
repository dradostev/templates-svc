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
        Task<(Result, Template?)> CreateTemplateAsync(CreateTemplate request, Guid userId);
        Task<(Result, Template?)> UpdateTemplateAsync(Guid templateId, UpdateTemplate request, Guid userId);
        Task<Result> DeleteTemplateAsync(Guid templateId, DeleteTemplate request, Guid userId);
        Task<(Result, Template?)> FindTemplateAsync(Guid templateId);
        Task<IEnumerable<Template>> ListTemplatesAsync(Paginate request, string category = null);
    }
}