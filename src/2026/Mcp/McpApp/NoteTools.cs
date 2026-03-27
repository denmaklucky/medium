using System.ComponentModel;
using ModelContextProtocol.Server;

namespace McpApp;

[McpServerToolType]
public sealed class NoteTools(NoteRepository repository)
{
    [McpServerTool(Name = "save_note")]
    [Description("Save a note. Use this to persist important context, findings, or information for later retrieval.")]
    public string SaveNote([Description("The full content or context to store in the note")] string content)
    {
        var id = repository.SaveNote(content);

        return $"Note saved successfully with ID {id}.";
    }

    [McpServerTool(Name = "retrieve_notes")]
    [Description("List all saved notes.")]
    public string ListNotes()
    {
        var notes = repository.GetAllNotes();

        if (!notes.Any())
        {
            return "No notes found.";
        }

        var lines = notes.Select(n => $"[{n.Id}] (created: {n.Created})\n{n.Content}");

        return string.Join("\n\n---\n\n", lines);
    }
}
