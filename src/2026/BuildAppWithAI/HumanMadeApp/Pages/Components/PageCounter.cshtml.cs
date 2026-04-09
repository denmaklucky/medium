using Hydro;

namespace HumanMadeApp.Pages.Components;

public class PageCounter : HydroComponent
{
    public int Count { get; set; }

    public void Add()
    {
        Count++;
    }
}
