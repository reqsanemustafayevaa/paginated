using examprojectpr.Business.Services.Implementations;
using examprojectpr.Business.Services.Interfaces;
using examprojectpr.Core.Repostories.Interfaces;
using examprojectpr.Data.DAL;
using examprojectpr.Data.Repostories.Implementations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IGenreRepository,GenreRepository>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IBookTagsRepository, BookTagsRepository>();
builder.Services.AddScoped<IBookImagesRepository, BookImaagesRepository>();

builder.Services.AddScoped<IBookRepository, BookRepository>();

builder.Services.AddDbContext<AppDbcontext>(opt =>
{
    opt.UseSqlServer("server=DESKTOP-4T5RTRO;database=examm;Trusted_Connection=True");
});
var app = builder.Build();
app.UseStaticFiles();

app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=index}"
    );


app.Run();
