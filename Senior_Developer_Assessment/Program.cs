using Microsoft.EntityFrameworkCore;
using Senior_Developer_Assessment.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();



app.Run();
