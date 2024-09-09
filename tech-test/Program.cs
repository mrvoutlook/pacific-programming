using Microsoft.EntityFrameworkCore;
using tech_test.Data;
using tech_test.Responses;
using tech_test.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<DataDbContext>(options
    => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAvatarService, AvatarService>();

builder.Services.AddHttpClient<IMyJsonServerApi, MyJsonServerApi>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["MyJsonServerApi:BaseUrl"]!);
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

// Map a default route to serve index.html
app.MapGet("/", async context =>
{
    context.Response.ContentType = "text/html";
    await context.Response.SendFileAsync(Path.Combine("wwwroot", "index.html"));
});


// optional MVC attribute: [FromQuery(Name = "userIdentifier")] string? userIdentifier
app.MapGet("/avatar", async (IAvatarService avatarService, string? userIdentifier) =>
{
    var url = await avatarService.GetImageUrl(userIdentifier);

    //return url is not null ? Results.Ok(new AvatarResponse { Url = url }) : Results.NotFound();
    return Results.Ok(new AvatarResponse { Url = url });
});

app.Run();
