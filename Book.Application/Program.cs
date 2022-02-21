using AutoMapper;
using Book.Application;
using Book.Application.Domain;
using Book.Application.Domain.ChangeHistory;
using Book.Application.Infrastructure.Ado.ChangeHistory;
using Book.Application.Infrastructure.Sql;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlDependencies(builder.Configuration.GetConnectionString("Book"));

builder.Services.AddScoped<IContextChangeHandler>(options =>
{
    var context = options.GetRequiredService<Context>();
    var sqlContextChangeHandler = new ContextChangeHandler(context);
    context.ChangeTracker.StateChanged += sqlContextChangeHandler.OnSaveFinished;
    context.SavingChanges += sqlContextChangeHandler.OnSaving;
    return sqlContextChangeHandler;
});

builder.Services.AddScoped<IHistoryHandler>(options =>
{
    IContextChangeHandler contextChangeHandler = options.GetRequiredService<IContextChangeHandler>();
    var historyHandler = new BookHistoryHandler(builder.Configuration.GetConnectionString("Book"));
    contextChangeHandler.HistoryChanged += historyHandler.OnChangeExtracted;
    return historyHandler;

});

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(options =>
{
    options.AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin();
});
app.UseAuthorization();

app.MapControllers();

app.Run();
