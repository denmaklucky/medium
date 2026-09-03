using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FeelSpaApp.Pages;

[IgnoreAntiforgeryToken]
public class FloatButton : PageModel
{
    private static readonly List<Product> Store = Enumerable
        .Range(1, 40)
        .Select(i => new Product { Id = i, Name = $"Product {i}", State = (State)(i % 3) })
        .ToList();

    public List<Product> Products => Store;

    public void OnGet()
    {
    }

    public PartialViewResult OnPostAdd()
    {
        var id = Store.Count == 0 ? 1 : Store.Max(p => p.Id) + 1;

        Store.Add(new Product { Id = id, Name = $"Product {id}", State = State.Active });

        return Partial("_FloatButtonTable", Store);
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
