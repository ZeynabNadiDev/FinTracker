using Identity.Composition;

var builder = WebApplication.CreateBuilder(args);

// Add Identity module configuration and infrastructure
builder.Services.AddIdentityModule(builder.Configuration);

builder.Services.AddControllers()
    .AddApplicationPart(typeof(Identity.Presentation.ModuleReference).Assembly);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
