
namespace SUS.MvcFramework
{
    public interface IServiceCollection
    {
        // .Add<IUsersService, UsersService>
        void Add<TSource, TDestination>();

        object CreateInstance(Type type);
    }
}
