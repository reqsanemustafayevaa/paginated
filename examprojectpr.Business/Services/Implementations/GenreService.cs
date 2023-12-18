using examprojectpr.Business.Services.Interfaces;
using examprojectpr.Core.Models;
using examprojectpr.Core.Repostories.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace examprojectpr.Business.Services.Implementations
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genrerepository;
        public GenreService(IGenreRepository genreRepository)
        {
            _genrerepository = genreRepository;
        }

       
        public async Task CreateAsync(Genre entity)
        {
            if (_genrerepository.Table.Any(x => x.Name == entity.Name ))
            {
                throw new NullReferenceException();
            }
            await _genrerepository.CreateAsync(entity);
            await _genrerepository.CommitAsync();
        }

        public async Task Delete(int id)
        {
            var entity=await _genrerepository.GetByIdAsync(x=>x.Id==id&&x.IsDeleted==false);
            if (entity == null)
            {
                throw new NullReferenceException();
            }
             _genrerepository.Delete(entity);
            await _genrerepository.CommitAsync();
        }

        public async Task<List<Genre>> GetAllAsync()
        {
            return await _genrerepository.GetAllAsync();
        }

        public async Task<Genre> GetByIdAsync(int id)
        {
            var entity =await _genrerepository.GetByIdAsync(x => x.Id == id && x.IsDeleted == false);
            if(entity == null)
            {
                throw new NullReferenceException();
            }
            return entity;
        }

        public async Task UpdateAsync(Genre genre)
        {
            var existentity=await _genrerepository.GetByIdAsync(x => x.Id == genre.Id && x.IsDeleted == false);
            if (_genrerepository.Table.Any(x => x.Name == genre.Name && existentity.Id!=genre.Id))
            {
                throw new NullReferenceException();
            }
            existentity.Name = genre.Name;
            await _genrerepository.CommitAsync();
            
        }
    }
}
