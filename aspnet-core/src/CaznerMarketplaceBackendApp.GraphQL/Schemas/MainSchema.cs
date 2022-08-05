using Abp.Dependency;
using GraphQL;
using GraphQL.Types;
using CaznerMarketplaceBackendApp.Queries.Container;

namespace CaznerMarketplaceBackendApp.Schemas
{
    public class MainSchema : Schema, ITransientDependency
    {
        public MainSchema(IDependencyResolver resolver) :
            base(resolver)
        {
            Query = resolver.Resolve<QueryContainer>();
        }
    }
}