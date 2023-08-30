using Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(option=>{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddCors(option=>{
    option.AddPolicy("CorsPolicy",policy=>{
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000");
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection(); for http redirections 

app.UseCors("CorsPolicy");
app.UseAuthorization();

app.MapControllers();


//Execute the migration automatiquelly
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try{
    var Context = services.GetRequiredService<DataContext>();
    await Context.Database.MigrateAsync();
    await Seed.SeedData(Context);
}
catch(Exception exception){
    var Logger = services.GetRequiredService<ILogger<Program>>();
    Logger.LogError(exception,"An Error Occured during Migration "); 
}
//Run Application
app.Run();
