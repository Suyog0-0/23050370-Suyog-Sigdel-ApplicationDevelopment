namespace _23050370_Suyog_Sigdel.Services;

public class LockService
{
    private const string AppPassword = "1234"; // Password

    public bool IsLocked { get; private set; } = true;

    public bool TryUnlock(string password)
    {
        if (password == AppPassword)
        {
            IsLocked = false;
            return true;
        }

        return false;
    }

    public void Lock()
    {
        IsLocked = true;
    }
}