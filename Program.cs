using Azure.Identity;
using Microsoft.Extensions.Configuration;
using SampleWebApp.Model;

var builder = WebApplication.CreateBuilder(args);

// Configure Azure Key Vault
var keyVaultName = "kvyoutubedemowithdotnet";
if (!string.IsNullOrEmpty(keyVaultName))
{
    var keyVaultUri = new Uri($"https://kvyoutubedemowithdotnet.vault.azure.net/");
    builder.Configuration.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential());
}

// Register DAL as a service
builder.Services.AddSingleton<DAL>();

// Add services to the container
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
