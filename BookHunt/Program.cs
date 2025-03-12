using ApplicationLayer.Services.Auth;
using ApplicationLayer.Services.AuthorServices;
using ApplicationLayer.Services.BooksServices;
using ApplicationLayer.Services.CategoryServices;
using ApplicationLayer.Services.RoleServices;
using ApplicationLayer.Services.UserServices;
using ApplicationLayer.UserServices;
using DomainLayer.Models;
using InfrastructureLayer;
using InfrastructureLayer.Repositories.AuthorRepository;
using InfrastructureLayer.Repositories.BookRepository;
using InfrastructureLayer.Repositories.CategoryRepository;
using InfrastructureLayer.Repositories.Commons;
using InfrastructureLayer.Repositories.RolRepository;
using InfrastructureLayer.Repositories.UserRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BookHuntDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookHuntDB"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});
builder.Services.AddAuthentication(
        options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
                ClockSkew = TimeSpan.Zero,
            };
        });

builder.Services.AddAuthorization();
builder.Services.AddScoped<ICommonProcess<User>, UsersRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUser, UsersRepository>();
builder.Services.AddScoped<ICommonProcess<Category>, CategoryRepository>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ICommonProcess<Author>, AuthorRepository>();
builder.Services.AddScoped<AuthorService>();
builder.Services.AddScoped<ICommonProcess<Role>, RoleRepository>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<IBook, BookRepository>();
builder.Services.AddScoped<BookService>();
var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<BookHuntDBContext>();
//    context.Database.Migrate();
//}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
   // app.UseSwagger();
    //app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(builder =>
{
    builder.SetIsOriginAllowed(origin => true)
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials();
});

app.MapControllers();

app.Run();
