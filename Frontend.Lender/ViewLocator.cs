using System;
using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using FrontendShared.ViewModels;

namespace FrontendLender;

/// <summary>
///     Given a view model, returns the corresponding view if possible.
/// </summary>
[RequiresUnreferencedCode(
    "Default implementation of ViewLocator involves reflection which may be trimmed away.",
    Url = "https://docs.avaloniaui.net/docs/concepts/view-locator")]
public class ViewLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        if (data is null) return null;

        var name = data.GetType().FullName!.Replace("ViewModel", "View");

        var localAssembly = data.GetType().Assembly.GetName().Name;
        var type = Type.GetType($"{name}, {localAssembly}");

        // TODO: ViewLocator to shared assembly
        if (type == null)
        {
            const string sharedAssembly = "FrontendShared";
            type = Type.GetType($"{name}, {sharedAssembly}");
        }

        if (type != null) return (Control)Activator.CreateInstance(type)!;

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data)
    {
        return data != null && data.GetType().Name.EndsWith("ViewModel");
    }
}