using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using clothing_be.Data;
using clothing_be.Services.Media;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//swagger
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

//cors
builder.Services.AddCors(options => 
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

//DB
var connectionString = builder.Configuration.GetConnectionString("cnn");
builder.Services.AddDbContext<MyDbContextApplication>(options =>
    options.UseNpgsql(connectionString)
    .LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging()
);

//S3

var awsAccessKey = builder.Configuration["AWS:AccessKey"];
var awsSecretKey = builder.Configuration["AWS:SecretKey"];
var awsRegion = builder.Configuration["AWS:Region"];
var credentials = new BasicAWSCredentials(awsAccessKey, awsSecretKey);
var s3Config = new AmazonS3Config
{
    RegionEndpoint = RegionEndpoint.GetBySystemName(awsRegion)
};

builder.Services.AddSingleton<IAmazonS3>(new AmazonS3Client(credentials, s3Config));
builder.Services.AddScoped<IImageUploadService, S3ImageUploadService>();

//
//var jwtSetting = builder.Configuration.GetSection("jwt");
//var key = jwtSetting["SecretKey"];
//builder.Services.AddAuthentication(
//   options =>
//   {
//       options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//       options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//   }
//).AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = jwtSetting["Issuer"],
//        ValidAudience = jwtSetting["Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
//    };
//});


//builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
//builder.Services.AddAWSService<IAmazonS3>();
//builder.Services.AddScoped<IImageUploadService, S3ImageUploadService>();

//Timeout
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
    options.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(30);
});

//policy.WithOrigins("https://yourdomain.com")
//      .AllowAnyHeader()
//      .AllowAnyMethod();

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.UseAuthorization();
//
//app.UseHsts();
app.UseMiddleware<ExceptionMiddleware>();
//
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
