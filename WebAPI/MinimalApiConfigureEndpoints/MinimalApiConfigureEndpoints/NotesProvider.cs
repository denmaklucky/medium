using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;

namespace MinimalApiConfigureEndpoints;

internal static class NotesProvider
{
    public static IEnumerable<Note> GetRandomNotes(int count = 100) =>
        Enumerable.Range(1, count).Select(_ => new Note(Guid.CreateVersion7(), RandomizerFactory.GetRandomizer(new FieldOptionsTextWords()).Generate()!));
}