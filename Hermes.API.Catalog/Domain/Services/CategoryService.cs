using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Hermes.API.Catalog.Domain.Constants;
using Hermes.API.Catalog.Domain.Entities;
using Hermes.API.Catalog.Domain.Models;
using Hermes.API.Catalog.Domain.Repositories;
using Hermes.API.Catalog.Domain.Requests;
using Hermes.API.Catalog.Domain.Responses;
using Hermes.API.Catalog.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Hermes.API.Catalog.Domain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CategoryDto> Create(CategoryDto categoryDto)
        {
            var isCategoryNameDuplicate = await IsCategoryNameDuplicate(categoryDto.Name);
            if (isCategoryNameDuplicate)
            {
                throw new InvalidOperationException(string.Format(ExceptionMessageConstants.CategoryNameDuplicate,
                    categoryDto.Name));
            }

            var category = _mapper.Map<Category>(categoryDto);
            category.Id = 0;
            category.Slug = category.Name.GenerateSlug();
            await _categoryRepository.Insert(category);

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> Update(CategoryDto categoryDto)
        {
            var isCategoryNameDuplicate = await IsCategoryNameDuplicate(categoryDto.Name, categoryDto.Id);
            if (isCategoryNameDuplicate)
            {
                throw new InvalidOperationException(string.Format(ExceptionMessageConstants.CategoryNameDuplicate,
                    categoryDto.Name));
            }

            var category = await _categoryRepository.Get(c => c.Id == categoryDto.Id && !c.IsDeleted);
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            category.Slug = category.Name.GenerateSlug();
            category.Description = categoryDto.Description;
            category.ImageUrl = categoryDto.ImageUrl;
            _categoryRepository.Update(category);

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task Delete(long id)
        {
            var category = await _categoryRepository.Get(c => c.Id == id);
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            category.IsDeleted = true;
            _categoryRepository.Update(category);
        }

        public async Task<CategoryDto> Get(long id)
        {
            var category = await _categoryRepository.Get(c => c.Id == id && !c.IsDeleted);
            return category == null ? null : _mapper.Map<CategoryDto>(category);
        }

        public async Task<PagedResponse<CategoryDto>> GetAll(SearchCategoryRequest searchCategoryRequest)
        {
            var query = _categoryRepository
                .GetQueryable()
                .Where(q => !q.IsDeleted);

            if (!string.IsNullOrWhiteSpace(searchCategoryRequest.Query))
            {
                query = query.Where(q =>
                    q.Name.Contains(searchCategoryRequest.Query) ||
                    q.Description.Contains(searchCategoryRequest.Query)
                );
            }

            var totalCount = await query.CountAsync();
            var categories = await query
                .Skip(searchCategoryRequest.SkipCount)
                .Take(searchCategoryRequest.PageSize)
                .ToListAsync();

            return new PagedResponse<CategoryDto>
            {
                Items = _mapper.Map<List<CategoryDto>>(categories),
                TotalCount = totalCount
            };
        }


        private async Task<bool> IsCategoryNameDuplicate(string name, long? id = null)
        {
            var isCategoryExistsWithName = await _categoryRepository.Any(c =>
                c.Name.Equals(name) &&
                (!id.HasValue || c.Id != id.Value)
            );
            return isCategoryExistsWithName;
        }
    }
}