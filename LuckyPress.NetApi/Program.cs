using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using LuckyPress.NetApi;
using LuckyPress.NetData;
using LuckyPress.NetService.Repositories;
using LuckyPress.NetService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.WebEncoders;
using Microsoft.IdentityModel.Tokens;
using NpgsqlDataProtection;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContextFactory<PressContext>(opt =>
        opt.UseSqlite("Data Source=Data.db"));
    builder.Services.AddDataProtection()
        .PersistKeysToFileSystem(new DirectoryInfo("./keys"));
}
else
{
    var sql = Environment.GetEnvironmentVariable("SQL", EnvironmentVariableTarget.Process);
    if (string.IsNullOrEmpty(sql))
    {
        builder.Services.AddDbContextFactory<PressContext>(opt =>
            opt.UseSqlite("Data Source=Data.db"));
        builder.Services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo("./keys"));
    }
    else
    {
        builder.Services.AddDbContext<PressContext>(options =>
            options.UseNpgsql(sql));

        builder.Services.AddDataProtection()
            .PersistKeysToPostgres(sql, true);
    }
}

builder.Services.AddControllers();
builder.Services.AddOpenApi(opt => { opt.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); });

builder.Services.AddAuthentication(options => { options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; })
    .AddJwtBearer(options =>
    {
        var jwtKey = "";
        if (builder.Environment.IsDevelopment())
        {
            jwtKey = builder.Configuration["Jwt:SecretKey"] ?? "";
        }
        else
        {
            Environment.GetEnvironmentVariable("SecretKey", EnvironmentVariableTarget.Process);
        }

        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false, //是否验证Issuer
            ValidateAudience = false, //是否验证Audience
            ValidateIssuerSigningKey = true, //是否验证SecurityKey

            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)), //SecurityKey
            ValidateLifetime = true, //是否验证失效时间
            ClockSkew = TimeSpan.FromSeconds(30), //过期时间容错值，解决服务器端时间不同步问题（秒）
            RequireExpirationTime = true,
        };
    });

// 跨域 设置
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin() //允许任何来源的主机访问
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// 日志 注册
var logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.SQLite(
        sqliteDbPath: Environment.CurrentDirectory + "/logs/log.db",
        tableName: "Logs")
    .CreateLogger();

builder.Logging
    .ClearProviders()
    .AddConsole()
    .AddDebug()
    .SetMinimumLevel(LogLevel.Information)
    .AddSerilog(logger);

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEMailRepository, EMailRepository>();

builder.Services.AddScoped<ILoginService,LoginService>();

builder.Services.AddScoped<IJwtHelper, JwtHelper>();
builder.Services.AddScoped<TokenActionFilter>();

builder.Services.Configure<WebEncoderOptions>(options =>
    options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapOpenApi();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<PressContext>();

    var pending = context.Database.GetPendingMigrations();
    var enumerable = pending as string[] ?? pending.ToArray();

    if (enumerable.Length != 0)
    {
        try
        {
            await context.Database.MigrateAsync();
            Console.WriteLine("Migrations applied successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Migration error: " + ex);
            throw; // 让异常冒泡，方便定位问题
        }
    }
    else
    {
        Console.WriteLine("No pending migrations.");
    }

    await context.SaveChangesAsync();
    await context.DisposeAsync();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapScalarApiReference();
app.MapControllers();

app.Run();