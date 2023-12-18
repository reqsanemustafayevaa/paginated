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
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }
        public async Task CreateAsync(Author entity)
        {
            //if (_authorRepository.Table.Any(x => x.FullName == entity.FullName))
            //{
            //    throw new NullReferenceException();
            //}

            await _authorRepository.CreateAsync(entity);
            await _authorRepository.CommitAsync();
        }

        public async Task Delete(int id)
        {
            var existauthor=await _authorRepository.GetByIdAsync(x => x.Id == id && x.IsDeleted == false);
            if (existauthor == null)
            {
                throw new NullReferenceException();
            }
             _authorRepository.Delete(existauthor);
            await _authorRepository.CommitAsync();
        }

        public async Task<List<Author>> GetAllAsync()
        {
            return await _authorRepository.GetAllAsync();
        }

        public async Task<Author> GetByIdAsync(int id)
        {
            var existauthor = await _authorRepository.GetByIdAsync(x => x.Id == id && x.IsDeleted == false);
            if (existauthor == null)
            {
                throw new NullReferenceException();
            }
            return existauthor;
        }

        public async Task UpdateAsync(Author entity)
        {
            var existauthor = await _authorRepository.GetByIdAsync(x => x.Id == entity.Id && x.IsDeleted == false);
            if (_authorRepository.Table.Any(x => x.FullName == entity.FullName&&existauthor.Id!= entity.Id))
            {
                throw new NullReferenceException();
            }
            existauthor.FullName = entity.FullName;
            await _authorRepository.CommitAsync();

        }
    }
}
