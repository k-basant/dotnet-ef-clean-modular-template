using ProjName.API.Bootstrapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddAppServices(builder.Configuration);
builder.Services.AddInfraServices(builder);


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(o => o.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
