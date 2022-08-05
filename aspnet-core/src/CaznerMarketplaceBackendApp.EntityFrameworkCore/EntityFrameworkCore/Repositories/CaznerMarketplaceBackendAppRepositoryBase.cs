using Abp.Domain.Entities;
using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Repositories;
using EFCore.BulkExtensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaznerMarketplaceBackendApp.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// Base class for custom repositories of the application.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key type of the entity</typeparam>
    public abstract class CaznerMarketplaceBackendAppRepositoryBase<TEntity, TPrimaryKey> : EfCoreRepositoryBase<CaznerMarketplaceBackendAppDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {

        protected CaznerMarketplaceBackendAppRepositoryBase(IDbContextProvider<CaznerMarketplaceBackendAppDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add your common methods for all repositories
        public async Task BulkInsertAsync(List<TEntity> entities)
        {
            await Context.BulkInsertAsync(entities);
        }
        public async Task BulkUpdate(List<TEntity> entities)
        {
            //Context.BulkUpdate(entities);
            await Context.BulkUpdateAsync(entities);
        }
        public async Task BulkDelete(List<TEntity> entities)
        {
            await Context.BulkDeleteAsync(entities);
        }
    }

    /// <summary>
    /// Base class for custom repositories of the application.
    /// This is a shortcut of <see cref="CaznerMarketplaceBackendAppRepositoryBase{TEntity,TPrimaryKey}"/> for <see cref="int"/> primary key.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public abstract class CaznerMarketplaceBackendAppRepositoryBase<TEntity> : CaznerMarketplaceBackendAppRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected CaznerMarketplaceBackendAppRepositoryBase(IDbContextProvider<CaznerMarketplaceBackendAppDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)!!!
    }
}
