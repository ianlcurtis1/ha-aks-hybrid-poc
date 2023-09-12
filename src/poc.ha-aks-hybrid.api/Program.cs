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


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //don't redirect to https in k8s
            //app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}