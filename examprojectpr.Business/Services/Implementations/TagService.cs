using examprojectpr.Business.Services.Interfaces;
using examprojectpr.Core.Models;
using examprojectpr.Core.Repostories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace examprojectpr.Business.Services.Implementations
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }
        public async Task CreateAsync(Tag entity)
        {
            await _tagRepository.CreateAsync(entity);
            await _tagRepository.CommitAsync();
        }

        public async Task Delete(int id)
        {
            var entity = await _tagRepository.GetByIdAsync(x => x.Id == id && x.IsDeleted == false);
            if (entity is null) throw new NullReferenceException();

            _tagRepository.Delete(entity);
            await _tagRepository.CommitAsync();
        }

        public async Task<List<Tag>> GetAllAsync()
        {
            return await _tagRepository.GetAllAsync();
        }

        public async Task<Tag> GetByIdAsync(int id)
        {
            var entity = await _tagRepository.GetByIdAsync(x => x.Id == id && x.IsDeleted == false);
            if (entity is null) throw new NullReferenceException();
            return entity;
        }

        public async Task UpdateAsync(Tag entity)
        {
            var existEntity = await _tagRepository.GetByIdAsync(x => x.Id == entity.Id && x.IsDeleted == false);

            if (_tagRepository.Table.Any(x => x.Name == entity.Name && existEntity.Id != entity.Id))
                throw new NullReferenceException();

            existEntity.Name = entity.Name;
            await _tagRepository.CommitAsync();
        }
    }
}
