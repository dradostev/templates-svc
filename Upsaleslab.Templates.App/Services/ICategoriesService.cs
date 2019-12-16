using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Upsaleslab.Templates.App.Models;
using Upsaleslab.Templates.App.Requests;

namespace Upsaleslab.Templates.App.Services
{
    public interface ICategoriesService
    {
        Task<(Result, Category?)> CreateCategoryAsync(CreateCategory request);
        Task<(Result, Category?)> UpdateCategoryAsync(Guid catId, UpdateCategory request);
        Task<Result> DeleteCategoryAsync(Guid catId, DeleteCategory request);
        Task<IEnumerable<Category>> ListCategoriesAsync();
    }
}