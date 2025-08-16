using System.Data;
using DiceRolls.Models;
using Microsoft.EntityFrameworkCore;

namespace DiceRolls.Repositories.ThrowRepository;

public class ThrowRepository : IThrowRepository
{
    private readonly DataContext _context;
    
    public ThrowRepository(DataContext context)
    {
        _context = context;
    }
    
    public async Task AddThrow(Throw input)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead);
        
        await _context.Throw.AddAsync(input);
        await _context.SaveChangesAsync();
        
        await transaction.CommitAsync();
    }
}