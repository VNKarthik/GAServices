using System.Configuration;
using System.Net;
using GAServices.Repositories;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


#region
builder.Services.AddCors(options =>
{
    options.AddPolicy("CustomPolicy", x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});
#endregion

builder.Services.AddTransient<IFibreRepository, FibreRepository>();
builder.Services.AddTransient<IPartyRepository, PartyRepository>();
builder.Services.AddTransient<ICommonRepository, CommonRepository>();
builder.Services.AddTransient<IMasterRepository, MasterRepository>();
builder.Services.AddTransient<IConversionRepository, ConversionRepository>();
builder.Services.AddTransient<IYarnOrderRepository, YarnOrderRepository>();

builder.Services.AddControllers();
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

app.UseExceptionHandler(
    options =>
    {
        options.Run(
            async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "text/html";
                var ex = context.Features.Get<IExceptionHandlerFeature>();
                if (ex != null)
                {
                    await context.Response.WriteAsync(ex.Error.Message);
                }
            }
        );
    });

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
