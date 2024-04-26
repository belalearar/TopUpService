using Microsoft.AspNetCore.ResponseCompression;
using Serilog;
using System.IO.Compression;
using System.Text.Json.Serialization;
using TopUpService.API;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();
builder.Host.ConfigureServices((context, services) =>
{
    Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(context.Configuration).CreateLogger();
    services.AddResponseCompression(options =>
    {
        options.EnableForHttps = true;
        options.Providers.Add<BrotliCompressionProvider>();
        options.Providers.Add<GzipCompressionProvider>();
    });

    services.Configure<BrotliCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
    services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.ConfigureHttpJsonOptions(opts =>
    {
        opts.SerializerOptions.IncludeFields = true;
        opts.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseHttpsRedirection();
app.MapApiEndpoints();

app.UseHttpsRedirection();
app.Run();