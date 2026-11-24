using LibraryManagement.Api.Data;
using LibraryManagement.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<LibraryDbContext>(
    option => option.UseSqlite(
    builder.Configuration.GetConnectionString("LibraryDb")
    ));
builder.Services.AddScoped<IBookService, BookService>();//ek HTTP request ke scope mein service ka instance manage hota hai. Agli request par alag scope hota hai.
builder.Services.AddScoped<IMemberService, MemberService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
