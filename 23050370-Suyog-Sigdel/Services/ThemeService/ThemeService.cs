using Microsoft.Maui.Controls;

namespace _23050370_Suyog_Sigdel.Services;

public class ThemeService
{
    public bool IsDark { get; private set; }
    public string CurrentColorTheme { get; private set; } = "blue"; // default color theme

    public bool IsDarkMode => IsDark;

    public event Action? OnThemeChanged;

    public ThemeService()
    {
        IsDark = Application.Current?.RequestedTheme == AppTheme.Dark;
    }

    // Toggle between light and dark mode
    public void ToggleTheme()
    {
        IsDark = !IsDark;

        Application.Current!.UserAppTheme =
            IsDark ? AppTheme.Dark : AppTheme.Light;

        OnThemeChanged?.Invoke();
    }

    // Set color theme (blue, soft-gray, dark-purple)
    public void SetColorTheme(string colorTheme)
    {
        CurrentColorTheme = colorTheme.ToLower();
        OnThemeChanged?.Invoke();
    }

    // Get the CSS class for current theme combination
    public string GetThemeClass()
    {
        return $"{(IsDark ? "dark" : "light")} theme-{CurrentColorTheme}";
    }
}