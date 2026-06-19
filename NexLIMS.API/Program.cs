using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NexLIMS.API.Controllers.middlewares;
using NextLIMS.BLL.Settings;
using NextLIMS.BLL.Services.Auth;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NextLIMS.BLL.Services.ClientPortal;
using NextLIMS.BLL.Services.ClientService;
using NextLIMS.BLL.Services.Department;
using NextLIMS.BLL.Services.EmailService;
using NextLIMS.BLL.Services.EmployeeService;
using NextLIMS.BLL.Services.Invitation;
using NextLIMS.BLL.Services.PasswordReset;
using NextLIMS.BLL.Services.Permissions;
using NextLIMS.BLL.Services.prep.DetectionService;
using NextLIMS.BLL.Services.prep.EnumerationService;
using NextLIMS.BLL.Services.prep.LabAnalysisService;
using NextLIMS.BLL.Services.Roles;
using NextLIMS.BLL.Services.SampleServic;
using NextLIMS.BLL.Services.SignupService;
using NextLIMS.BLL.Services.Tests;
using NextLIMS.DAL;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.DataSeed;
using NextLIMS.DAL.Data.Payment;
using NextLIMS.DAL.Repositories;
using NextLIMS.DAL.Repository.ClientPortal;
using NextLIMS.DAL.Repository.ClientRepo;
using NextLIMS.DAL.Repository;
using NextLIMS.DAL.Repository.Department;
using NextLIMS.DAL.Repository.SampleRepo;
using NextLIMS.DAL.Repository.TenantRepo;
using NextLIMS.DAL.Repository.Test;
using NextLIMS.DAL.Repository.SampleRepo.LabRepo;
using NextLIMS.DAL.Repository.SampleRepo.SampleWorkflowRepository;
using NextLIMS.DAL.Repository.Test;
using System.Text;
using NextLIMS.BLL.Services.PasswordReset;
using NextLIMS.DAL.Repository.DepartmentDirector;
using NextLIMS.BLL.Services.DepartmentDiractor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(
    options => options.Filters.Add<PermissionBasedAuthFilter>()
    );
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.Configure<FawaterkSettings>(builder.Configuration.GetSection("Fawaterk"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// ///Database Configuration ///Start//
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default"));
});

// Twilio Configurations and Client Portal Settings
builder.Services.Configure<TwilioSettings>(
    builder.Configuration.GetSection("Twilio"));

builder.Services.Configure<ClientPortalSettings>(
    builder.Configuration.GetSection("ClientPortal"));

builder.Services.Configure<ClientOtpSettings>(
    builder.Configuration.GetSection("ClientOtp"));

////////
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<InvitationService>();
builder.Services.AddScoped<UserAuthenticationService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<PermissionService>();
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<EmployeeRepository>();
builder.Services.AddScoped<InvitationRepository>();
builder.Services.AddScoped<PasswordResetRepository>();
builder.Services.AddScoped<PasswordResetService>();
builder.Services.AddScoped<RoleRepository>();
builder.Services.AddScoped<PermissionRepository>();
builder.Services.AddScoped<ITestRepository, TestRepository>();
builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IJwtAuthenticationService, JwtAuthenticationService>();
builder.Services.AddScoped<ITenantRepository, TenantRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientPortalRepository, ClientPortalRepository>();
builder.Services.AddScoped<IClientPortalService, ClientPortalService>();
builder.Services.AddScoped<IWhatsAppService, TwilioWhatsAppService>();
if (builder.Environment.IsDevelopment()) {
    builder.Services.Replace(
        ServiceDescriptor.Scoped<IWhatsAppService, DevelopmentWhatsAppService>()); 
}
builder.Services.AddScoped<IClientPortalInvitationService, ClientPortalInvitationService>();
builder.Services.AddScoped<IClientOtpService, ClientOtpService>();

builder.Services.AddScoped<DepartmentDirectorRepository>();
builder.Services.AddScoped<DepartmentDirectorService>();
//
builder.Services.AddScoped<SampleRepository>();
builder.Services.AddScoped<SampleService>();
////////////////
//sayed/////////////
builder.Services.AddScoped<ISignupService, SignupService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IEnumerationService, EnumerationService>();
builder.Services.AddScoped<IDetectionService, DetectionService>();
builder.Services.AddScoped<ISampleWorkflowRepository, SampleWorkflowRepository>();
builder.Services.AddScoped<ILabAnalysisRepository, LabAnalysisRepository>();
builder.Services.AddScoped<ILabAnalysisService, LabAnalysisService>();
////////////////
///cors

builder.Services.AddCors(options =>
{
    options.AddPolicy("OpenCorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


///


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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        "ClientOnly",
        policy =>
        {
            policy.RequireAuthenticatedUser();

            policy.RequireClaim(
                "ActorType",
                "Client");

            policy.RequireClaim("ClientId");
            policy.RequireClaim("TenantId");
            policy.RequireClaim("TenantSlug");
        });
});

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode =
        StatusCodes.Status429TooManyRequests;

    options.AddPolicy(
        "ClientOtp",
        httpContext =>
        {
            var remoteIp =
                httpContext.Connection
                    .RemoteIpAddress?
                    .ToString() ?? "unknown";

            var slug =
                httpContext.Request
                    .RouteValues["slug"]?
                    .ToString() ?? "unknown";

            var partitionKey =
                $"{remoteIp}:{slug}";

            return RateLimitPartition
                .GetFixedWindowLimiter(
                    partitionKey,
                    _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 10,
                        Window =
                            TimeSpan.FromMinutes(10),
                        QueueLimit = 0,
                        QueueProcessingOrder =
                            QueueProcessingOrder
                                .OldestFirst,
                        AutoReplenishment = true
                    });
        });

    options.OnRejected =
        async (context, cancellationToken) =>
        {
            context.HttpContext.Response.StatusCode =
                StatusCodes.Status429TooManyRequests;

            await context.HttpContext.Response
                .WriteAsJsonAsync(
                    new
                    {
                        message =
                            "Too many OTP requests. " +
                            "Please try again later."
                    },
                    cancellationToken);
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
app.UseRouting();
app.UseCors("OpenCorsPolicy");

app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();