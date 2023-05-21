using MagneticSurvey.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connection1 = "Data Source = survey.db";
builder.Services.AddDbContext<SurveyDbContext>(options => options.UseSqlite(connection1));

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
      builder =>
      {
          builder
            .AllowAnyMethod()
           .SetIsOriginAllowed(_ => true).SetIsOriginAllowed(_ => true)
           .AllowAnyHeader()
           .AllowCredentials();
      });
});

var app = builder.Build();

using (var serviceScope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope())
{
    var pdb = serviceScope?.ServiceProvider.GetRequiredService<SurveyDbContext>();
    pdb?.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.UseCors("CorsPolicy");

app.Run();
