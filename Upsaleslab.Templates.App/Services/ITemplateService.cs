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
        Task<(Result, Template?)> CreateAsync(CreateTemplate request, Guid userId);
        Task<(Result, Template?)> UpdateAsync(Guid templateId, UpdateTemplate request, Guid userId);
        Task<Result> DeleteAsync(Guid templateId, DeleteTemplate request, Guid userId);
        Task<(Result, Template?)> FindAsync(Guid templateId);
        Task<IEnumerable<Meta>> ListAsync(Paginate request);
    }
}