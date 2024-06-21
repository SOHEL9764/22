using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var keyVaultName = builder.Configuration["KeyVault:Name"];
if (!string.IsNullOrEmpty(keyVaultName))
{
    var keyVaultUri = new Uri($"https://{keyVaultName}.vault.azure.net/");
    builder.Configuration.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential());

    var client = new SecretClient(keyVaultUri, new DefaultAzureCredential());
    var secret = client.GetSecret(builder.Configuration["KeyVault:SecretName"]).Value;
    
    builder.Services.AddDbContext<SampleDatabaseContext>(options =>
    {
        options.UseSqlServer(secret.Value);
    });
}

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<SampleDatabaseContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
}

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
