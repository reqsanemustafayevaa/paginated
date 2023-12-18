using examprojectpr.Core.Models;
using examprojectpr.Core.Repostories.Interfaces;
using examprojectpr.Data.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace examprojectpr.Data.Repostories.Implementations
{
    public class GenericRepostory<TEntity> : IGenericRepostory<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly AppDbcontext _context;
        public GenericRepostory(AppDbcontext context)
        {
            _context = context;
        }

        public DbSet<TEntity> Table => _context.Set<TEntity>();

        public async Task<int> CommitAsync( )
        {
            return await _context.SaveChangesAsync();  
        }

        public async Task CreateAsync(TEntity entity)
        {
            await Table.AddAsync(entity);
        }

        public  void Delete(TEntity entity)
        {
            Table.Remove(entity);
        }

       

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity,bool>>?expression=null,params string[]includes)
        {
            var query=Table.AsQueryable();  //_context.books(author,genre,booktag vs)
            GetQuery(includes);
            return expression is not null
                       ? await query.Where(expression).ToListAsync()
                       : await query.ToListAsync();


        }

       
        public async Task<TEntity> GetByIdAsync(Expression<Func<TEntity, bool>>? expression = null, params string[]? includes)
        {
            var query = Table.AsQueryable();
            GetQuery(includes);
            
            return expression is not null
                ?await query.Where(expression).FirstOrDefaultAsync()
                :await query.FirstOrDefaultAsync();

        }
        public IQueryable<TEntity> GetQuery(string[]? includes)
        {
            var query = Table.AsQueryable();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
                
            }
            return query;
        }

    }
}
