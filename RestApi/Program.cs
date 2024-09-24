public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Agregar servicios al contenedor
        ConfigureServices(builder.Services);

        var app = builder.Build();

        // Configurar el middleware de la aplicación
        Configure(app);

        // Ejecutar la aplicación
        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // Registrar servicios necesarios
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddControllers();
    }

    private static void Configure(WebApplication app)
    {
        // Configuración de Swagger si el entorno es de desarrollo
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Habilitar redirección HTTPS
        app.UseHttpsRedirection();

        // Mapear controladores
        app.MapControllers();
    }
}
