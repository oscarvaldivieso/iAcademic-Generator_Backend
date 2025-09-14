using iAcademicGenerator.DataAccess;
using iAcademicGenerator.DataAccess.Repositories;
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
            iAcademicGenerator.DataAccess.iAcademicGeneratorContext.BuildConnectionString(connectionString);

            services.AddScoped<CareersRepository>();

            iAcademicGeneratorContext.BuildConnectionString(connectionString);

        }

        public static void BusinessLogic(this IServiceCollection services)
        {
        }



    }
}
