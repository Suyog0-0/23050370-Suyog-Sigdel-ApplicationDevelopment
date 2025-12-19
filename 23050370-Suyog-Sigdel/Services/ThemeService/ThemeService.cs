using Microsoft.Maui.Controls;

namespace _23050370_Suyog_Sigdel.Services;

public class ThemeService
{
    public bool IsDark { get; private set; }

    public bool IsDarkMode => IsDark; // for Razor pages

    public event Action? OnThemeChanged;

    public ThemeService()
    {
        IsDark = Application.Current?.RequestedTheme == AppTheme.Dark;
    }

    public void ToggleTheme()
    {
        IsDark = !IsDark;

        Application.Current!.UserAppTheme =
            IsDark ? AppTheme.Dark : AppTheme.Light;

        OnThemeChanged?.Invoke();
    }
}