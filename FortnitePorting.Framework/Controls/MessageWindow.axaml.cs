using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using FortnitePorting.Framework.Services;

namespace FortnitePorting.Framework.Controls;

public partial class MessageWindow : Window
{
    public static MessageWindow? ActiveWindow;
    public bool UseFallbackBackground { get; set; } = Environment.OSVersion.Platform != PlatformID.Win32NT || (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Build < 22000);
    private event EventHandler OnClosedWindow;

    public MessageWindow(string caption, string text, Window? owner = null, Action<object?, EventArgs>? onClosed = null)
    {
        InitializeComponent();
        DataContext = this;

        Title = caption;
        CaptionTextBlock.Text = caption;
        InfoTextBlock.Text = text;
        Owner = owner;
        if (onClosed is not null) OnClosedWindow += (sender, args) => onClosed(sender, args);
    }

    public static void Show(string caption, string text, Window? owner = null, Action<object?, EventArgs>? onClosed = null)
    {
        if (ActiveWindow is not null) return;
        TaskService.RunDispatcher(() =>
        {
            ActiveWindow = new MessageWindow(caption, text, owner, onClosed);
            ActiveWindow.Show();
        });
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        BeginMoveDrag(e);
    }

    private void OnCloseClicked(object? sender, RoutedEventArgs e)
    {
        ActiveWindow = null;
        Close();
    }

    private void OnContinueClicked(object? sender, RoutedEventArgs e)
    {
        ActiveWindow = null;
        Close();
        OnClosedWindow?.Invoke(this, EventArgs.Empty);
    }
}