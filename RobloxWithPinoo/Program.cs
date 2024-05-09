using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RobloxWithPinoo.Context;
using RobloxWithPinoo.Entity.Entities;
using RobloxWithPinoo.Middlewares;
using RobloxWithPinoo.Services.AccountService;
using RobloxWithPinoo.Services.ActivationCodeService;
using RobloxWithPinoo.Services.AdminDashboardService;
using RobloxWithPinoo.Services.AuthService;
using RobloxWithPinoo.Services.CardControlService;
using RobloxWithPinoo.Services.ContactFormService;
using RobloxWithPinoo.Services.DocArticleService;
using RobloxWithPinoo.Services.DocCategoryService;
using RobloxWithPinoo.Services.UserService;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CatalogConnection")));

builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("ecawiasqrpqrgyhwnolrudpbsrwaynbqdayndnmcehjnwqyouikpodzaqxivwkconwqbhrmxfgccbxbyljguwlxhdlcvxlutbnwjlgpfhPgqbegtbxbvwnacyqnltrby")),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };
        });


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowCors",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        });

builder.Services.AddIdentity<AppUser, AppRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<IDocCategoryService, DocCategoryService>();
builder.Services.AddScoped<IDocArticleService, DocArticleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IActivationCodeService, ActivationCodeService>();
builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();
builder.Services.AddScoped<IContactFormService, ContactFormService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowCors");

app.UseAuthentication();

app.UseMultiTenantMiddleware();

app.UseAuthorization();

app.MapControllers();

app.Run();
