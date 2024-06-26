using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using prod_server.Classes;
using prod_server.database;
using prod_server.Services;
using prod_server.Services.DB;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// DB Connection
// TODO: Move the connection string to the ENV Variables.


var cnString = new NpgsqlConnectionStringBuilder("Host=ruby.db.elephantsql.com;Database=sjqfdnjb;Username=sjqfdnjb;Password=MqrtzEbZ6mS2IlstUdQdpeo1iQdUdktK");
builder.Services.AddDbContext<Context>(options =>
{
    var connectionStringBuilder = new NpgsqlConnectionStringBuilder(cnString.ConnectionString)
    {
        Pooling = true,
        MaxPoolSize = 5,
        MinPoolSize = 1,
        ConnectionIdleLifetime = 300, // Set the maximum time a connection can be idle in the pool (in seconds),
    };

    options.UseNpgsql(connectionStringBuilder.ConnectionString, (o) => o.EnableRetryOnFailure());
});

//Setup Authentication
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer("Accounts", options =>
{
    options.RequireHttpsMetadata = false; // Only in DEV! Remove for prod.
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
       
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("33d7b68dc5eab0934f001fc5801bd234a255aa0e7314cb43619e5604001132ece77b4a73f0fd8c67d89e43662d2d968d2b18bd20c8dbc78ac5512ccf41f381cb")),
        ValidateIssuer = false,
        ValidateLifetime = true,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,

    };
});


builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Configure JsonSerializerSettings here
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
}); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Create 
builder.Services.AddRateLimiter(RateLimiterOptions =>
{
    RateLimiterOptions.AddFixedWindowLimiter("quoteslimit", options =>
    {
        options.PermitLimit = 1;
        options.Window= TimeSpan.FromSeconds(5);
        options.QueueLimit = 0;
    });
});

builder.Services.AddAuthorization();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IQuoteService, QuoteService>();
builder.Services.AddScoped<INotificationsService, NotificationsService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProviderService, ProviderService>();
builder.Services.AddScoped<IDocumentsService, DocumentsService>();
builder.Services.AddTransient<IUtilitiesService, UtilitiesService>();


builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
} else
{
    app.UseHttpsRedirection();
}

app.UseRateLimiter();

// SETUP CORS

app.UseCors(builder =>
{
    //builder.WithOrigins("http://localhost:3000")
    //        .WithOrigins("http://localhost:5231")
    //       .AllowAnyMethod()
    //       .AllowAnyHeader()
    //       .AllowCredentials();

    //.AllowAnyOrigin()

    builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
            //.AllowCredentials();
});

app.UsePathBase(new PathString("/api"));// Adding /api prefix to all endpoints.

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
