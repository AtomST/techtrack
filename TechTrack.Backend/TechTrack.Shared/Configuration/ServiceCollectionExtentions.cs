using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTrack.Shared.Filters;

namespace TechTrack.Shared.Configuration
{
    public static class ServiceCollectionExtentions
    {
        public static IServiceCollection AddCustomControllers(this IServiceCollection services)
        {
            services.AddControllers(opt =>
            {
                opt.Filters.Add<ModelValidationFilter>();
            })
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            });

            return services;
        }
    }
}
