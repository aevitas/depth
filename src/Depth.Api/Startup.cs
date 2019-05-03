using System.Net.Http;
using Depth.Client.MovieDb;
using Depth.Client.MovieDb.Abstractions;
using Depth.Client.YouTube;
using Depth.Client.YouTube.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Depth.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MovieDbOptions>(Configuration.GetSection("MovieDb"));
            services.Configure<YouTubeOptions>(Configuration.GetSection("YouTube"));

            services.AddHttpContextAccessor();
            services.AddSingleton<HttpClient>();
            
            services.AddScoped<IMovieSearchProvider, MovieDbClient>();
            services.AddScoped<IMovieDetailProvider, MovieDbClient>();
            services.AddScoped<IVideoSearchProvider, YouTubeClient>();
            services.AddScoped<IMovieTrailerProvider, YouTubeClient>();

            services.AddMemoryCache();

            services.AddRouting(opts =>
            {
                opts.LowercaseQueryStrings = true;
                opts.LowercaseUrls = true;
            });

            services.AddCors(o => o.AddPolicy("PermissivePolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseMvc();
        }
    }
}
