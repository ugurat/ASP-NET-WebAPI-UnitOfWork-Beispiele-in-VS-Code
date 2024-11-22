
using Microsoft.EntityFrameworkCore;  // Sicherstellen, dass dies eingefügt ist.
using PersonApi.Data;  // Sicherstellen, dass dies eingefügt ist.
using PersonApi.Repositories;
using PersonApi.Services;  // Sicherstellen, dass dies eingefügt ist.

var builder = WebApplication.CreateBuilder(args);


// EINTRAGEN - CORS-Konfiguration hinzufügen
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
                // erlaubte Domains:
                .AllowAnyOrigin() // Erlaubt alle Quellen (Domains). // Dies bedeutet, dass auf die API von jeder Domain aus zugegriffen werden kann.
                //.WithOrigins("https://www.ornek1.com", "https://www.ornek2.com") // Erlaubt nur die angegebenen Domains

                // erlaubte Methoden:
                .AllowAnyMethod() // Erlaubt alle HTTP-Methoden (z. B. GET, POST, PUT, DELETE). // Dies bedeutet, dass alle HTTP-Anfragen an die API akzeptiert werden.
                //.WithMethods("GET", "POST") // Erlaubt nur die Methoden GET und POST

                // erlaubte Header:
                .AllowAnyHeader() // Erlaubt alle HTTP-Anfrage-Header. // Diese Einstellung ermöglicht es, jeder Anfrage an die API beliebige Header hinzuzufügen.
                //.WithHeaders("Content-Type", "Authorization"); // Erlaubt nur die angegebenen Header
                ;

    });
});



// EINTRAGEN: Die Verbindungszeichenfolge aus appsettings.json hinzufügen und DbContext einfügen
builder.Services.AddDbContext<PersonDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// EINTRAGEN: Add services to the container.


// Repositories und Services registrieren
builder.Services.AddScoped<IPersonRepository, PersonRepository>(); // PersonRepository
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // UnitOfWork
builder.Services.AddScoped<IPersonService, PersonService>(); // PersonService


// Beispiel für die Service-Registrierung
//builder.Services.AddScoped<IServiceGeneric<Person, PersonDto>, ServiceGeneric<Person, PersonDto>>();



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// EINTRAGEN - CORS aktivieren
app.UseCors("AllowAll"); // Erlaubt alle Domains, Methoden und Header
//app.UseCors("AllowSpecificMethods"); // Wenn GET, POST, ... angegeben sind.
//app.UseCors("AllowSpecificOrigins"); // Erlaubt nur bestimmte Domains
//app.UseCors("AllowSpecificMethodsAndHeaders"); // Erlaubt bestimmte Domains, bestimmte Header und bestimmte HTTP-Methoden



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// EINTRAGEN - Haupt-URL auf die Swagger-Oberfläche umleiten
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger/index.html");
        return;
    }
    await next();
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
