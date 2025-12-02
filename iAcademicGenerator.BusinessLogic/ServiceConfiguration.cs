using iAcademicGenerator.BusinessLogic.Services;
using iAcademicGenerator.DataAccess;
using iAcademicGenerator.DataAccess.Repositories.ACA;
using iAcademicGenerator.DataAccess.Repositories.EXP;
using iAcademicGenerator.DataAccess.Repositories.UNI;
using iAcademicGenerator.DataAccess.Repositories.AUTH;
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
            services.AddScoped<CampusRepository>();
            services.AddScoped<ModalitiesRepository>();
            services.AddScoped<PeriodsRepository>();
            services.AddScoped<UsersRepository>();
            services.AddScoped<RolRepository>();
            services.AddScoped<RolesRepository>();
            services.AddScoped<SchedulesRepository>();

            
            services.AddScoped<SectionsRepository>();
            services.AddScoped<ClassroomsRepository>();
            services.AddScoped<RequestsRepository>();
            services.AddScoped<TeachersRepository>();
            services.AddScoped<AreasRepository>();
            services.AddScoped<SubjectsRepository>();
            services.AddScoped<ContactsRepository>();

            services.AddScoped<StudentsRepository>();
            services.AddScoped<AuthRepository>();
            services.AddScoped<OffersRepository>();
            

        }

        public static void BusinessLogic(this IServiceCollection services)
        {
            services.AddScoped<UNIServices>();
            services.AddScoped<ACAServices>();
            services.AddScoped<EXPServices>();
            services.AddScoped<AuthServices>();
        }



    }
}
