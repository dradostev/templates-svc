using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Upsaleslab.Templates.App.Models;
using Upsaleslab.Templates.App.Requests;

namespace Upsaleslab.Templates.App.Services
{
    public interface ITagService
    {
        Task<(Result, Tag?)> CreateAsync(CreateTag request, Guid userId);
        Task<(Result, Tag?)> UpdateAsync(Guid catId, UpdateTag request, Guid userId);
        Task<Result> DeleteAsync(Guid catId, DeleteTag request, Guid userId);
        Task<IEnumerable<Tag>> ListAsync();
    }
}