namespace _23050370_Suyog_Sigdel.Helpers;

public static class MoodHelper
{
    // List of available moods with Font Awesome icons
    public static readonly List<(string Name, string Icon, string Color)> AvailableMoods = new()
    {
        ("Happy", "fa-smile", "#f39c12"),
        ("Sad", "fa-frown", "#3498db"),
        ("Excited", "fa-star", "#e74c3c"),
        ("Calm", "fa-spa", "#16a085"),
        ("Anxious", "fa-exclamation-triangle", "#e67e22"),
        ("Angry", "fa-angry", "#c0392b"),
        ("Tired", "fa-bed", "#95a5a6"),
        ("Energetic", "fa-bolt", "#f1c40f"),
        ("Grateful", "fa-heart", "#e91e63"),
        ("Stressed", "fa-dizzy", "#d35400"),
        ("Peaceful", "fa-dove", "#27ae60"),
        ("Confused", "fa-question-circle", "#7f8c8d"),
        ("Motivated", "fa-fire", "#e74c3c"),
        ("Bored", "fa-meh", "#95a5a6"),
        ("Hopeful", "fa-seedling", "#2ecc71"),
        ("Lonely", "fa-user", "#34495e"),
        ("Loved", "fa-heart", "#e91e63"),
        ("Proud", "fa-trophy", "#f39c12"),
        ("Frustrated", "fa-hand-rock", "#c0392b"),
        ("Content", "fa-smile-beam", "#27ae60")
    };

    // Get mood icon by name
    public static string GetMoodIcon(string moodName)
    {
        var mood = AvailableMoods.FirstOrDefault(m => m.Name == moodName);
        return mood.Icon ?? "fa-smile";
    }

    // Get mood color by name
    public static string GetMoodColor(string moodName)
    {
        var mood = AvailableMoods.FirstOrDefault(m => m.Name == moodName);
        return mood.Color ?? "#3498db";
    }

    // Get mood display text (Icon + Name)
    public static (string Icon, string Name, string Color) GetMoodDisplay(string moodName)
    {
        if (string.IsNullOrEmpty(moodName))
            return ("fa-smile", "", "#3498db");
            
        var mood = AvailableMoods.FirstOrDefault(m => m.Name == moodName);
        return mood.Name != null ? (mood.Icon, mood.Name, mood.Color) : ("fa-smile", moodName, "#3498db");
    }
}