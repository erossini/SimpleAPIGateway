using SimpleAPIGateway;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

Router router = new Router("routes.json");

app.Run(async (context) =>
{
    var content = await router.RouteRequest(context.Request);
    await context.Response.WriteAsync(await content.Content.ReadAsStringAsync());
});

app.Run();
