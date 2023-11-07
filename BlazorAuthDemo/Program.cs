using BlazorAuthDemo.Data;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using System.Collections.Generic;

namespace BlazorAuthDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string[] initialScopes = builder
                .Configuration.GetSection("MicrosoftGraph:Scopes")?
                .Get<List<string>>()
                .ToArray();

            builder.Services.AddSingleton<WeatherForecastService>();

            builder.Services
                .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(
                    builder.Configuration.GetSection("AzureAd"))
                .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
                .AddDownstreamApi(
                    "MicrosoftGraph",
                    builder.Configuration.GetSection("MicrosoftGraph"))
                .AddInMemoryTokenCaches();

            builder.Services
                .AddControllersWithViews()
                .AddMicrosoftIdentityUI();

            builder.Services
                .AddRazorPages(options =>
                    options.RootDirectory = "/Views/Pages");

            builder.Services
                .AddServerSideBlazor()
                .AddMicrosoftIdentityConsentHandler();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRewriter(new RewriteOptions().Add(
                context =>
                {
                    if (context.HttpContext.Request.Path ==
                            "/MicrosoftIdentity/Account/SignedOut")
                        context.HttpContext.Response.Redirect("/");
                }));

            app.MapControllers();
            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");
            app.Run();
        }
    }
}