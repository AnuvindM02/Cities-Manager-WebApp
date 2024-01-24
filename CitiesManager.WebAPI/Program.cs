using CitiesManager.Core.Entities;
using CitiesManager.Infrastructure.DatabaseContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ProducesAttribute("application/json"));//Only returns app/json in all responses globally for all action methods
    options.Filters.Add(new ConsumesAttribute("application/json"));//Only accepts app/json in all requests globally for all action methods
}).AddXmlSerializerFormatters();//Xml serializer enabled for getcities action method in cities controller v2

builder.Services.AddApiVersioning(config =>
{
    config.ApiVersionReader = new UrlSegmentApiVersionReader();// Version specified in Url
    //config.ApiVersionReader = new QueryStringApiVersionReader(); //Version specified as query string "api-version"
    //config.ApiVersionReader = new HeaderApiVersionReader("asp-version"); //Version specified in request header
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

//Swagger
builder.Services.AddEndpointsApiExplorer();//Generates description for all end points
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml"));

    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "Cities Web API", Version = "1.0" });
    options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "Cities Web API", Version = "2.0" });//Versions to reflect in swagger

});//Generates OpenAPI specification

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";//v1
    options.SubstituteApiVersionInUrl = true;
});

//Configuring CORS
builder.Services.AddCors(options =>
{

    //Custom policy for clients specified in "AllowedOrigins" in appsettings.json
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>())//Client domain port number from appsettings.json
        .WithHeaders("Authorization","origin","accept","content-type")//What request headers to accept from client
        .WithMethods("GET","POST","PUT","DELETE");//What request methods to accept from client
    });


    //Custom policy for clients specified in "AllowedOrigins2" in appsettings.json
    //This should be applied in the controller/action method explicitly (check v1 cities controller for reference)
    //If controller hasn't mentioned any [EnablCors] attribute, default policy works
    options.AddPolicy("4100Client",policyBuilder =>
    {
        policyBuilder.WithOrigins(builder.Configuration.GetSection("AllowedOrigins2").Get<string[]>())//Client domain port number from appsettings.json
        .WithHeaders("Authorization", "origin", "accept")//What request headers to accept from client
        .WithMethods("GET");//What request methods to accept from client
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHsts();
app.UseHttpsRedirection();

app.UseSwagger();//creates endpoint for swagger.json
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json","1.0");
    options.SwaggerEndpoint("/swagger/v2/swagger.json","2.0");
});//creates a swagger UI for testing API endpoints/action methods

app.UseRouting();
app.UseCors();// Includes response header "Access-Control-Allow-Origin"
app.UseAuthorization();

app.MapControllers();

app.Run();