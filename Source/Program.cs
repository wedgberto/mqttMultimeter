using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.ReactiveUI;

namespace mqttMultimeter;

static class Program
{
    public static void Main(string[] args)
    {
        try
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
        catch(Exception ex)
        {

       }
    }

    // Do not remove this method! It is required for the Designer.
    static AppBuilder BuildAvaloniaApp()
    {
        var appBuilder = AppBuilder.Configure<App>().UseReactiveUI().UsePlatformDetect().WithInterFont();
        
        if (Debugger.IsAttached)
        {
            appBuilder = appBuilder.LogToTrace(Avalonia.Logging.LogEventLevel.Debug, Avalonia.Logging.LogArea.Binding, Avalonia.Logging.LogArea.Visual);
        }

        return appBuilder;
    }
}