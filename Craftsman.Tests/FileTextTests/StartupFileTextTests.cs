﻿namespace Craftsman.Tests.FileTextTests
{
    using Craftsman.Builders;
    using Craftsman.Models;
    using Craftsman.Tests.Fakes;
    using FluentAssertions;
    using System;
    using System.Collections.Generic;
    using Xunit;
    using System.Linq;
    using AutoBogus;

    public class StartupFileTextTests
    {
        [Fact]
        public void GetStartupText_Devlopment_env_returns_expected_text()
        {
            var template = new ApiTemplate() { };
            var fileText = StartupBuilder.GetStartupText("Development", template);

            var expectedText = @$"namespace WebApi
{{
    using Application;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Infrastructure.Persistence;
    using Infrastructure.Shared;
    using Infrastructure.Persistence.Seeders;
    using Infrastructure.Persistence.Contexts;
    using WebApi.Extensions;
    using Serilog;

    public class StartupDevelopment
    {{
        public IConfiguration _config {{ get; }}
        public StartupDevelopment(IConfiguration configuration)
        {{
            _config = configuration;
        }}

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {{
            services.AddCorsService(""MyCorsPolicy"");
            services.AddApplicationLayer();
            services.AddPersistenceInfrastructure(_config);
            services.AddSharedInfrastructure(_config);
            services.AddControllers()
                .AddNewtonsoftJson();
            services.AddApiVersioningExtension();
            services.AddHealthChecks();

            #region Dynamic Services
            #endregion
        }}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {{
            app.UseDeveloperExceptionPage();

            #region Entity Context Region - Do Not Delete
            #endregion

            app.UseCors(""MyCorsPolicy"");

            app.UseSerilogRequestLogging();
            app.UseRouting();
            
            app.UseErrorHandlingMiddleware();
            app.UseEndpoints(endpoints =>
            {{
                endpoints.MapHealthChecks(""/api/health"");
                endpoints.MapControllers();
            }});

            #region Dynamic App
            #endregion
        }}
    }}
}}";

            fileText.Should().Be(expectedText);
        }

        [Theory]
        [InlineData("Production")]
        [InlineData("Qa")]
        [InlineData("Staging")]
        [InlineData("Local")]
        public void GetStartupText_NonDevlopment_env_returns_expected_text(string env)
        {
            var template = new ApiTemplate() { };
            var fileText = StartupBuilder.GetStartupText(env, template);

            var expectedText = @$"namespace WebApi
{{
    using Application;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Infrastructure.Persistence;
    using Infrastructure.Shared;
    using WebApi.Extensions;
    using Serilog;

    public class Startup{env}
    {{
        public IConfiguration _config {{ get; }}
        public Startup{env}(IConfiguration configuration)
        {{
            _config = configuration;
        }}

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {{
            services.AddCorsService(""MyCorsPolicy"");
            services.AddApplicationLayer();
            services.AddPersistenceInfrastructure(_config);
            services.AddSharedInfrastructure(_config);
            services.AddControllers()
                .AddNewtonsoftJson();
            services.AddApiVersioningExtension();
            services.AddHealthChecks();

            #region Dynamic Services
            #endregion
        }}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {{
            app.UseCors(""MyCorsPolicy"");

            app.UseSerilogRequestLogging();
            app.UseRouting();
            
            app.UseErrorHandlingMiddleware();
            app.UseEndpoints(endpoints =>
            {{
                endpoints.MapHealthChecks(""/api/health"");
                endpoints.MapControllers();
            }});

            #region Dynamic App
            #endregion
        }}
    }}
}}";

            fileText.Should().Be(expectedText);
        }

        [Fact]
        public void GetStartupText_Devlopment_env_with_JWT_returns_expected_text()
        {
            var template = new ApiTemplate() { };
            template.AuthSetup.AuthMethod = "JWT";
            var fileText = StartupBuilder.GetStartupText("Development", template);

            var expectedText = @$"namespace WebApi
{{
    using Application;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Infrastructure.Persistence;
    using Infrastructure.Shared;
    using Infrastructure.Persistence.Seeders;
    using Infrastructure.Persistence.Contexts;
    using WebApi.Extensions;
    using Serilog;
    using Infrastructure.Identity;
    using Infrastructure.Identity.Entities;
    using Microsoft.AspNetCore.Identity;
    using Infrastructure.Identity.Seeders;
    using WebApi.Services;
    using Application.Interfaces;

    public class StartupDevelopment
    {{
        public IConfiguration _config {{ get; }}
        public StartupDevelopment(IConfiguration configuration)
        {{
            _config = configuration;
        }}

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {{
            services.AddCorsService(""MyCorsPolicy"");
            services.AddApplicationLayer();
            services.AddIdentityInfrastructure(_config);
            services.AddPersistenceInfrastructure(_config);
            services.AddSharedInfrastructure(_config);
            services.AddControllers()
                .AddNewtonsoftJson();
            services.AddApiVersioningExtension();
            services.AddHealthChecks();
            services.AddSingleton<ICurrentUserService, CurrentUserService>();

            #region Dynamic Services
            #endregion
        }}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {{
            app.UseDeveloperExceptionPage();

            #region Entity Context Region - Do Not Delete
            #endregion

            #region Identity Context Region - Do Not Delete

            var userManager = app.ApplicationServices.GetService<UserManager<ApplicationUser>>();
            var roleManager = app.ApplicationServices.GetService<RoleManager<IdentityRole>>();
            RoleSeeder.SeedDemoRolesAsync(roleManager);

            // user seeders -- do not delete this comment
            

            #endregion

            app.UseCors(""MyCorsPolicy"");

            app.UseSerilogRequestLogging();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseErrorHandlingMiddleware();
            app.UseEndpoints(endpoints =>
            {{
                endpoints.MapHealthChecks(""/api/health"");
                endpoints.MapControllers();
            }});

            #region Dynamic App
            #endregion
        }}
    }}
}}";

            fileText.Should().Be(expectedText);
        }
    }
}
