using BackSide2.DAO.Repository;
using BRM.BL.PermissionsService;
using BRM.BL.RolesService;
using BRM.BL.UsersPermissionsService;
using BRM.BL.UserService;
using BRM.DAO.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BackSide2.Extensions
{
    public static class ConfigureContainerExtensions
    {
        public static void AddRepository(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static void AddScopedServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUserService, UserService>();
            serviceCollection.AddScoped<IRolesService, RolesService>();
            serviceCollection.AddScoped<IPermissionsService, PermissionsService>();
            serviceCollection.AddScoped<IUsersPermissionsService, UsersPermissionsService>();
            serviceCollection.AddScoped<IUsersRolesService, UsersRolesService>();
        }
    }
}
