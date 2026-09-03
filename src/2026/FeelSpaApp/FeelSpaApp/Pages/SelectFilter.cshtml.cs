using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FeelSpaApp.Pages;

public class SelectFilter : PageModel
{
    private static readonly List<Product> Store =
    [
        new() { Id = 1, Name = "Product 1", State = State.Active },
        new() { Id = 2, Name = "Product 2", State = State.Active },
        new() { Id = 3, Name = "Product 3", State = State.Archived },
        new() { Id = 4, Name = "Product 4", State = State.Archived },
        new() { Id = 5, Name = "Product 5", State = State.Deleted },
        new() { Id = 6, Name = "Product 6", State = State.Deleted }
    ];

    public List<Product> Products => Store;

    public void OnGet()
    {
    }

    public PartialViewResult OnGetFilter(State? state)
    {
        var products = state is null
            ? Store
            : Store.Where(p => p.State == state).ToList();

        return Partial("_SelectFilterTable", products);
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
