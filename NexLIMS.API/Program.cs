<<<<<<< Updated upstream
=======
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NexLIMS.API.Controllers.middlewares;
using NextLIMS.BLL.Services.Auth;
using NextLIMS.BLL.Services.EmailService;
using NextLIMS.BLL.Services.EmployeeService;
using NextLIMS.BLL.Services.Invitation;
using NextLIMS.BLL.Services.Permissions;
using NextLIMS.BLL.Services.Roles;
using NextLIMS.BLL.Services.SignupService;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Payment;
using NextLIMS.DAL.Repositories;
using System.Text;

>>>>>>> Stashed changes
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

<<<<<<< Updated upstream
builder.Services.AddControllers();
=======
builder.Services.AddControllers(
    options => options.Filters.Add<PermissionBasedAuthFilter>()
    );
builder.Services.Configure<FawaterkSettings>(builder.Configuration.GetSection("Fawaterk"));
builder.Services.AddHttpClient();
builder.Services.Configure<FawaterkSettings>(builder.Configuration.GetSection("Fawaterk"));
builder.Services.AddHttpContextAccessor();
>>>>>>> Stashed changes
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
