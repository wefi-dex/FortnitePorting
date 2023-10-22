using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using FortnitePorting.Services.Endpoints.Models;

namespace FortnitePorting.Controls.Home;

public partial class FeaturedArtItem : UserControl
{
    public string Artist { get; set; }
    public string ImageURL { get; set; }
    public string SocialsURL { get; set; }
    
    public FeaturedArtItem(FeaturedResponse featured)
    {
        InitializeComponent();

        Artist = featured.Artist;
        ImageURL = featured.ImageURL;
        SocialsURL = featured.SocialsURL;
    }

    private void OnSocialsButtonClicked(object? sender, RoutedEventArgs e)
    {
        AppVM.Launch(SocialsURL);
    }
}