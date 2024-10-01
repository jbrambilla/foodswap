using Carter;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAppServices();
builder.SetupSerilogMSSQl();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAppMiddleware(app.Environment);
app.MapCarter();

app.Run();
