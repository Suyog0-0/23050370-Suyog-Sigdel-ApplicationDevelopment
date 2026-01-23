using _23050370_Suyog_Sigdel.Data;
using _23050370_Suyog_Sigdel.Models;
using Microsoft.EntityFrameworkCore;

namespace _23050370_Suyog_Sigdel.Services;

public class SecurityService
{
    private readonly AppDbContext _db;
    private SecurityModel? _cachedSettings;
    private bool _hasCachedSettings = false;
    
    public bool IsAuthenticated { get; private set; } = false;
    public event Action? OnAuthenticationChanged;

    public SecurityService(AppDbContext db)
    {
        _db = db;
    }

    // Get security settings with caching
    public async Task<SecurityModel?> GetSecuritySettingsAsync()
    {
        try
        {
            if (!_hasCachedSettings)
            {
                _cachedSettings = await _db.SecuritySettings.FirstOrDefaultAsync();
                _hasCachedSettings = true;
            }
            return _cachedSettings;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Security Get Error: " + ex.Message);
            return null;
        }
    }

    // Check if PIN is enabled (with caching)
    public async Task<bool> IsPinEnabledAsync()
    {
        var settings = await GetSecuritySettingsAsync();
        return settings?.IsEnabled ?? false;
    }

    // Set or update PIN
    public async Task<bool> SetPinAsync(string pin)
    {
        try
        {
            var settings = await _db.SecuritySettings.FirstOrDefaultAsync();
            
            if (settings == null)
            {
                // Create new security settings
                settings = new SecurityModel
                {
                    Pin = pin,
                    IsEnabled = true
                };
                await _db.SecuritySettings.AddAsync(settings);
            }
            else
            {
                // Update existing settings
                settings.Pin = pin;
                settings.IsEnabled = true;
                _db.SecuritySettings.Update(settings);
            }

            await _db.SaveChangesAsync();
            
            // Update cache
            _cachedSettings = settings;
            _hasCachedSettings = true;
            
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Set PIN Error: " + ex.Message);
            return false;
        }
    }

    // Verify PIN
    public async Task<bool> VerifyPinAsync(string pin)
    {
        try
        {
            var settings = await GetSecuritySettingsAsync();
            
            if (settings == null || !settings.IsEnabled)
            {
                IsAuthenticated = true;
                OnAuthenticationChanged?.Invoke();
                return true;
            }

            bool isValid = settings.Pin == pin;
            
            if (isValid)
            {
                IsAuthenticated = true;
                OnAuthenticationChanged?.Invoke();
            }

            return isValid;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Verify PIN Error: " + ex.Message);
            return false;
        }
    }

    // Disable PIN protection
    public async Task<bool> DisablePinAsync()
    {
        try
        {
            var settings = await _db.SecuritySettings.FirstOrDefaultAsync();
            
            if (settings != null)
            {
                settings.IsEnabled = false;
                _db.SecuritySettings.Update(settings);
                await _db.SaveChangesAsync();
                
                // Update cache
                _cachedSettings = settings;
                _hasCachedSettings = true;
            }

            IsAuthenticated = true;
            OnAuthenticationChanged?.Invoke();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Disable PIN Error: " + ex.Message);
            return false;
        }
    }

    // Logout (lock the app)
    public void Logout()
    {
        IsAuthenticated = false;
        OnAuthenticationChanged?.Invoke();
    }
    
    // Clear cache (call this when needed)
    /*public void ClearCache()
    {
        _cachedSettings = null;
        _hasCachedSettings = false;
    }*/
}
