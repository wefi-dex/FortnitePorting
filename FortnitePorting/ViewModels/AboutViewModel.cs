using FortnitePorting.Framework;

namespace FortnitePorting.ViewModels;

public class AboutViewModel : ViewModelBase
{
    public void Discord()
    {
        AppVM.Launch(Globals.DISCORD_URL);
    }

    public void KoFi()
    {
        AppVM.Launch(Globals.KOFI_URL);
    }

    public void GitHub()
    {
        AppVM.Launch(Globals.GITHUB_URL);
    }
}