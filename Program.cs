using Carter;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAppServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAppMiddleware(app.Environment);
app.MapCarter();

app.Run();
