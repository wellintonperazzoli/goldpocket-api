using GoldPocket.Data;
using Microsoft.EntityFrameworkCore;
using GoldPocket.Models.DB;
using GoldPocket.Services.ModelServices;
using GoldPocket.Services;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddControllers();


services.AddDbContext<GoldPocketContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DockerString"))
);

services.AddDefaultIdentity<appUser>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;

    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.SignIn.RequireConfirmedAccount = false;
}).AddEntityFrameworkStores<GoldPocketContext>();

services.AddTransient<UserService>();
services.AddTransient<CategoryService>();
services.AddTransient<ItemService>();
services.AddTransient<LocationService>();
services.AddTransient<ExpenseService>();
services.AddTransient<ExpenseItemService>();
services.AddTransient<AuthService>();
services.AddTransient<ChartService>();

services.AddCors();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    var license = new OpenApiLicense
    {
        Name = "Wellinton Perazzoli",
        Url = null
    };
    var contact = new OpenApiContact
    {
        Name = "Wellinton Perazzoli",
        Email = "wellctba@gmail.com",
        Url = null
    };

    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "GoldPocket API v1",
            Description = "",
            Contact = contact,
            //License = license,
            Version = "v1"
        }
    );

    //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //var xmlPath = $"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xmlFile)}";
    //c.IncludeXmlComments(xmlPath);

    #region Swagger Bearer Authentication 
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                    });
    #endregion

});

#region Configure Bearer Authentication
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
#endregion


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
