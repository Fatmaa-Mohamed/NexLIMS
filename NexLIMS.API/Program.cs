using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NexLIMS.API.Controllers.middlewares;
using NextLIMS.BLL.Services.Auth;
using NextLIMS.BLL.Services.Department;
using NextLIMS.BLL.Services.EmailService;
using NextLIMS.BLL.Services.EmployeeService;
using NextLIMS.BLL.Services.Invitation;
using NextLIMS.BLL.Services.Permissions;
using NextLIMS.BLL.Services.Roles;
using NextLIMS.BLL.Services.Tests;
using NextLIMS.DAL;
using NextLIMS.BLL.Services.SampleServic;
using NextLIMS.BLL.Services.SignupService;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.DataSeed;
using NextLIMS.DAL.Repositories;
using NextLIMS.DAL.Repository.Test;
using NextLIMS.DAL.Repository.Department;
using NextLIMS.DAL.Repository.SampleRepo;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(
    options => options.Filters.Add<PermissionBasedAuthFilter>()
    );
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// ///Database Configuration ///Start//
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default"));
});
/////Radwan///
builder.Services.AddScoped< IEmailService,EmailService>();
builder.Services.AddScoped<InvitationService>();
builder.Services.AddScoped<UserAuthenticationService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<PermissionService>();
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped< UserRepository>();
builder.Services.AddScoped< EmployeeRepository>();
builder.Services.AddScoped< InvitationRepository>();
builder.Services.AddScoped<PasswordResetRepository>();
builder.Services.AddScoped<RoleRepository>();
builder.Services.AddScoped< PermissionRepository>();
builder.Services.AddScoped<ITestRepository, TestRepository>();
builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
//
builder.Services.AddScoped< SampleRepository>();
builder.Services.AddScoped<SampleService>();
////////////////
//sayed/////////////
builder.Services.AddScoped<ISignupService,SignupService>();

////////////////
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            //ValidIssuer = builder.Configuration["Jwt:Issuer"],
            //ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });

///End//
var app = builder.Build();

// seed data
using (var scope = app.Services.CreateScope())
{
   var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

   await DataSeeder.SeedAsync(context);
}

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
