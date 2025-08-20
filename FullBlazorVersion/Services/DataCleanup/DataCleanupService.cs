using System.Data;
using DiceRolls.Models;
using Microsoft.EntityFrameworkCore;

namespace DiceRolls.Services.DataCleanup;

public class DataCleanupService : IHostedService, IDisposable
{
    private Timer? _timer;
    private readonly DataContext _context;

    public DataCleanupService(IConfiguration configuration)
    {
        var dbPath = configuration.GetValue<string>("Database:Path");
        _context = new DataContext(configuration, new DbContextOptionsBuilder<DataContext>().UseSqlite($"Data Source={dbPath}").Options);
    }

    private void DoCleanup(object? state)
    {
        var sessionIds = _context.Throw
            .GroupBy(x => x.SessionId)
            .Where(x => x.Max(y => y.Date) < DateTime.UtcNow.AddDays(-8))
            .Select(x => x.Key)
            .ToList();
        
        using var transaction = _context.Database.BeginTransaction(IsolationLevel.RepeatableRead);
        
        var throws = _context.Throw
            .Where(x => sessionIds.Contains(x.SessionId))
            .ToList();
        
        _context.Throw.RemoveRange(throws);
        _context.SaveChanges();
        
        transaction.Commit();
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoCleanup, null, TimeSpan.FromHours(1), TimeSpan.FromDays(1));
        await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        await Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}   