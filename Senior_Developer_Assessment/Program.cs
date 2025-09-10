using Microsoft.EntityFrameworkCore;
using Senior_Developer_Assessment.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.Run();
