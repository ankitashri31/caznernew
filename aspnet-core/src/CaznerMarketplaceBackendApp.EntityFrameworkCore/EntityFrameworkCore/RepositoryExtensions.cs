using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FimApp.EntityFrameworkCore
{
    public static class RepositoryExtensions
    {
        public static Task BulkInsertAsync<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository, IEnumerable<TEntity> list)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            //throw new NotImplementedException();

            return repository.GetDbContext().AddRangeAsync(list);
        }

        public static void BulkUpdate<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository, IEnumerable<TEntity> list)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            //throw new NotImplementedException();

             repository.GetDbContext().UpdateRange(list);
        }
        public static void BulkDelete<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository, IEnumerable<TEntity> list)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            //throw new NotImplementedException();

            repository.GetDbContext().RemoveRange(list);
        }
    }
}
