using iAcademicGenerator.BusinessLogic.Services;
using iAcademicGenerator.DataAccess;
using iAcademicGenerator.DataAccess.Repositories.UNI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAcademicGenerator.BusinessLogic
{
    public static class ServiceConfiguration
    {
        public static void DataAccess(this IServiceCollection services, string connectionString)
        {
            // Configura la cadena de conexión del context
            iAcademicGeneratorContext.BuildConnectionString(connectionString);

            // Repositorios
            services.AddScoped<CareersRepository>();

        }

        public static void BusinessLogic(this IServiceCollection services)
        {
            services.AddScoped<UNIServices>();
        }



    }
}
