using ChessApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;

namespace ChessApi {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddCors();

            services.AddControllers();
            //services.Add(new ServiceDescriptor(typeof(ChessContext), new ChessContext(Configuration.GetConnectionString("DefaultConnection"))));
            //services.AddDbContext<ChessContext>(options => options.UseMySQL(Configuration.GetConnectionString("DefaultConnection")));

            //services.AddDbContext<ChessContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //     "DefaultConnection": "Data Source=localhost;Initial Catalog=parallel-chess;Integrated Security=True"

            //services.AddDbContext<ChessContext>(options => options.My)

            services.AddDbContext<ChessContext>(options => options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(options => {
                options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                options.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
