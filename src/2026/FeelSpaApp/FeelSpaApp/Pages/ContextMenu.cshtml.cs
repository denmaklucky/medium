using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FeelSpaApp.Pages;

[IgnoreAntiforgeryToken]
public class ContextMenu : PageModel
{
    private static readonly List<Product> Store =
    [
        new() { Id = 1, Name = "Product 1" },
        new() { Id = 2, Name = "Product 2" },
        new() { Id = 3, Name = "Product 3" },
        new() { Id = 4, Name = "Product 4" },
        new() { Id = 5, Name = "Product 5" },
        new() { Id = 6, Name = "Product 6" }
    ];

    public List<Product> Products => Store;

    public void OnGet()
    {
    }

    public PartialViewResult OnDelete(int id)
    {
        SetState(id, State.Deleted);

        return Partial("_ContextMenuTable", Store);
    }

    public PartialViewResult OnPostArchive(int id)
    {
        SetState(id, State.Archived);

        return Partial("_ContextMenuTable", Store);
    }

    private static void SetState(int id, State state)
    {
        var product = Store.FirstOrDefault(p => p.Id == id);

        if (product is not null)
        {
            product.State = state;
        }
    }

    public sealed class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public State State { get; set; }
    }

    public enum State
    {
        Active,
        Archived,
        Deleted
    }
}
