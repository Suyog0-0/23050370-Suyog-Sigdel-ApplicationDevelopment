namespace _23050370_Suyog_Sigdel;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new MainPage()) { Title = "23050370-Suyog-Sigdel" };
    }
}