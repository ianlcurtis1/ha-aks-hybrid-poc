//This is NON PRODUCTION CODE
namespace PoC.HaAKSHybrid.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("poc.ha-aks-hybrid.api starting...");

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddTransient<IQueue, DiskQueue>(); // InMemTestQueue>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //get settings from local settings file
            builder.Services.Configure<Config>(
               builder.Configuration.GetSection("Config"));
            //https://techcommunity.microsoft.com/t5/azure-arc-blog/ga-azure-key-vault-secrets-provider-extension-for-arc-enabled/ba-p/3389231


            //no cors for dev https://learn.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-7.0&preserve-view=true
            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("NoCORSPolicy", builder =>
            //    {
            //        builder
            //        .AllowAnyOrigin()
            //        .AllowAnyHeader()
            //        .AllowAnyMethod()
            //        .SetIsOriginAllowedToAllowWildcardSubdomains();
            //    });
            //});


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //don't redirect to https in k8s
            //app.UseHttpsRedirection();


            //no cors for dev
            //app.UseCors("NoCORSPolicy");


            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}