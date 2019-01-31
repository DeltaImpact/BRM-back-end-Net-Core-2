using BRM.BL.PermissionsService;
using BRM.BL.RolesService;
using BRM.BL.UsersPermissionsService;
using BRM.BL.UsersRolesService;
using BRM.BL.UserService;
using BRM.DAO.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BRM.Extensions
{
    public static class ConfigureContainerExtensions
    {
        public static void AddRepository(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static void AddTransientServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IUserService, UserService>();
            serviceCollection.AddTransient<IRolesService, RolesService>();
            serviceCollection.AddTransient<IPermissionsService, PermissionsService>();
            serviceCollection.AddTransient<IUsersPermissionsService, UsersPermissionsService>();
            serviceCollection.AddTransient<IUsersRolesService, UsersRolesService>();
        }
    }
}
