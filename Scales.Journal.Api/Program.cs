using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Scales.Journal.Api.Middleware;
using Scales.Journal.Core.Configuration;
using Scales.Journal.Domain;
using Scales.Journal.Domain.Configuration;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Default");
var migrationsAssembly = typeof(Program).Assembly.GetName().Name;
// Add services to the container.
builder.Services.AddDbContext<JournalDbContext>(options =>
    {
        options.UseNpgsql(connectionString, npgsqlOptionsAction =>
        {
            npgsqlOptionsAction.MigrationsAssembly(migrationsAssembly);
        });
    });

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
builder.Services.AddCors(options =>
    options.AddPolicy("ClientPolicy",
    policy => policy.WithOrigins(allowedOrigins)
    .WithExposedHeaders("X-Pagination")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()));

var authority = builder.Configuration.GetSection("Authority").Value;
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

    })
    .AddJwtBearer(options =>
    {
        options.Authority = authority;
        options.RequireHttpsMetadata = false;
        options.Audience = "journalAPI";
    });

builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddAutoMapper(typeof(JournalMapperConfiguration));
builder.Services.AddRedisConfiguration(builder.Configuration);
builder.Services.AddRabbitMqConfiguration(builder.Configuration);

builder.Services.AddControllers();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandlerMiddleware();
// Configure the HTTP request pipeline.
app.UseForwardedHeaders();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("ClientPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

await app.PopulateAsync();


app.Run();
