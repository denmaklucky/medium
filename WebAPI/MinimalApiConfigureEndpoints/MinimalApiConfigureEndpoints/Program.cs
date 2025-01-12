using MinimalApiConfigureEndpoints;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Configure endpoint in Program
app.MapGet("/notes", () => TypedResults.Ok(NotesProvider.GetRandomNotes()));

// Configure specific GetNotes endpoint by extensions
//app.MapGetNotesEndpoint();

// Configure all notes endpoints by extensions
//app.MapNotesEndpoints();

// Configure all endpoint in current assembly by IEndpointConfigurator
//app.MapEndpoints();

app.Run();