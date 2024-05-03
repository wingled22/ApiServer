using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using olappApi.Entities;

var builder = WebApplication.CreateBuilder(args); 

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddDbContext<OlappContext>(options => options.UseSqlite("Data Source= Olapp.db"));

// builder.Services.AddDbContext<OlappContext>(options =>
//     options.UseMySql("server=localhost;database=sampleapp;user=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.27-mariadb")));

builder.Services.AddControllers().AddJsonOptions(
    options => {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    }
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors();


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
