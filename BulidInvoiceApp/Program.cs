using Blazored.LocalStorage;
using BulidInvoiceApp.Components;
using BulidInvoiceApp.Services.Admin;
using BulidInvoiceApp.Services.Invoices;
using BulidInvoiceApp.Services.Pdf;
using BulidInvoiceApp.Services.PDF;
using BulidInvoiceApp.Services.Products;
using BulidInvoiceApp.Storages;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddLocalization();

builder.Services.AddScoped<DatabaseHelper>();



builder.Services.AddScoped<IProductServices, ProductServices>();

builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IadminSrevices, AdminServices>();




builder.Services.AddScoped<IPdfInvoiceService, PdfInvoiceService>();
builder.Services.AddApplicationInsightsTelemetry();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
