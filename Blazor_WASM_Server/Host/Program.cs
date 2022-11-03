using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using System.Net.Http;
using System;
using System.Linq;
using Blazor_WASM_Server.Host;
using Blazor_WASM_Server.Shared;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
//Add services to the container.
//***This provides authentication for the Web Api
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

//***This provides authentication for the Blazor Server Pages
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddInMemoryTokenCaches();

builder.Services.AddControllersWithViews()
    .AddMicrosoftIdentityUI();

builder.Services.AddAuthorization(options =>
{
    //By default, all incoming requests will be authorized according to the default policy
    //options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddRazorPages();

builder.Services.AddServerSideBlazor()
.AddMicrosoftIdentityConsentHandler(); ;

builder.Services.AddHttpClient();
builder.Services.AddScoped<IHostHttpClient, HostHttpClient>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI();

    //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Glossary v1"));
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

//***Use MapWhen to handle all endpoints that don't use our chosen prefix (/svrWASM).  ie. this handles routing to WASM
//***Choose an arbitrary prefix to identify server pages (eg. svrWASM)
//***Use Blazor Server if path starts with our prefix
app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/svrWeb"), svrWeb =>
{
    svrWeb.UseRouting();
    svrWeb.UseStaticFiles();
    svrWeb.UseAuthentication();
    svrWeb.UseAuthorization();
    svrWeb.UseEndpoints(endpoints =>
    {
        //endpoints.MapControllers();
        endpoints.MapRazorPages();
        endpoints.MapBlazorHub();
        //***_Host_svrHost.cshtml and _Layout_svrHost.cshtml are just copied from a standard Blazor Server project and renamed
        endpoints.MapFallbackToPage("/svrWeb/{*path:nonfile}", "/_Host_svrWeb");
    });
});

app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/svrHost"), svrHost =>
{
    svrHost.UseRouting();
    svrHost.UseStaticFiles();
    svrHost.UseAuthentication();
    svrHost.UseAuthorization();
    svrHost.UseEndpoints(endpoints =>
    {
        //endpoints.MapControllers();
        endpoints.MapRazorPages();
        endpoints.MapBlazorHub();
        //***_Host_svrHost.cshtml and _Layout_svrHost.cshtml are just copied from a standard Blazor Server project and renamed
        endpoints.MapFallbackToPage("/svrHost/{*path:nonfile}", "/_Host_svrHost");
    });
});

app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/svrWASM"), svrWASM =>
{
    svrWASM.UseRouting();
    svrWASM.UseStaticFiles();
    svrWASM.UseAuthentication();
    svrWASM.UseAuthorization();
    svrWASM.UseEndpoints(endpoints =>
    {
        //endpoints.MapControllers();
        endpoints.MapRazorPages();
        endpoints.MapBlazorHub();
        //***_Host_svrHost.cshtml and _Layout_svrHost.cshtml are just copied from a standard Blazor Server project and renamed
        endpoints.MapFallbackToPage("/svrWASM/{*path:nonfile}", "/_Host_svrWASM");
    });
});

//***We can  add another map for the server side api
app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/api")
    , svrHost =>
    {
        svrHost.UseRouting();;
        svrHost.UseAuthentication();
        svrHost.UseAuthorization();
        svrHost.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    });


//default is to use WASM
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

//***indexWASM.cshtml is a standard index.html from wwwroot in a standard WASM project 
//***delete the index.html file from the client project
app.MapFallbackToPage("/indexWASM");

app.Run();