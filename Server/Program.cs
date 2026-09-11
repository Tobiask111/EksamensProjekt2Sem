/* using Server.Repositories.Proj.CreateProjectsFolder;
using Server.Repositories.Proj.HourCalculator; */
using Server.Repositories.User;
using Server.Repositories.HourRepositories;
using Server.Repositories.MaterialRepositories;
using Server.Repositories.ProjectRepositories;
using Server.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);


builder.Services.AddScoped<IProjectRepository, ProjectRepositorySQL>();
builder.Services.AddScoped<IHourRepository, HourRepositorySQL>();
builder.Services.AddScoped<IMaterialRepository, MaterialRepositorySQL>();

builder.Services.AddScoped<ProjectCalculationsService>();

builder.Services.AddSingleton<ICreateUserRepo, CreateUserRepoSQL>();
/* builder.Services.AddSingleton<ICreateProjectRepo, CreateProjectRepo>();

builder.Services.AddSingleton<IProjectHourCalcRepo, ProjectHourCalcRepositorySQL>(); */

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("policy",
        policy =>
        {
            policy.AllowAnyOrigin();
            policy.AllowAnyMethod();
            policy.AllowAnyHeader();
        });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("policy");

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();