using IdentityServerAuthenticationService;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityServer()

    .AddInMemoryClients(IdentityConfig.Clients())
    .AddInMemoryIdentityResources(IdentityConfig.IdentityResources)
    .AddInMemoryApiResources(IdentityConfig.ApiResources)
    .AddInMemoryApiScopes(IdentityConfig.ApiScopes)
    //.AddTestUsers(IdentityConfig.GetTestUsers())
        .AddDeveloperSigningCredential();

var app = builder.Build();
app.MapGet("/", () => "Hello World!");
app.UseRouting();
app.UseIdentityServer();

app.Run();
