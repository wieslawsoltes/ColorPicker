using Avalonia.ReactiveUI;
using Avalonia.Web.Blazor;

namespace ThemeEditor.Web;

public partial class App
{
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        WebAppBuilder.Configure<ThemeEditor.App>()
            .UseReactiveUI()
            .SetupWithSingleViewLifetime();
    }
}
