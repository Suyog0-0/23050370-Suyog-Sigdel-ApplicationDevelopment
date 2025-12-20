using _23050370_Suyog_Sigdel.Data;

namespace _23050370_Suyog_Sigdel.Services;

public partial class JournalService
{
    private readonly AppDbContext _db;

    public JournalService(AppDbContext db)
    {
        _db = db;
    }
}
