namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///
/// </summary>
public static class UserConfigurations
{
    /// <summary>
    /// Configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseServices(this WebApplication app)
    {
        if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
            app.UserSwaggerDocument();

        app.UseHttpsRedirection();
        app.UseHsts();
        app.UseAuthorization();
        app.UseStaticFiles();
        app.UseRouting();
        app.MapControllers();
        //app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        return app;
    }
}