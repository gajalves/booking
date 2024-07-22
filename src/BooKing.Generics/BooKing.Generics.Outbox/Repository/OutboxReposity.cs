using BooKing.Generics.Infra;
using BooKing.Generics.Outbox.Configurations;
using BooKing.Generics.Outbox.Context;
using BooKing.Generics.Outbox.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;

namespace BooKing.Generics.Outbox.Repository;
public class OutboxReposity : IOutboxReposity
{
    private readonly OutboxContext _context;
    private readonly IUnitOfWork<OutboxContext> _unitOfWork;
    private readonly OutboxOptions _outboxOptions;

    public OutboxReposity(OutboxContext context, 
                          IUnitOfWork<OutboxContext> unitOfWork, 
                          IOptions<OutboxOptions> outboxOptions)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _outboxOptions = outboxOptions.Value;
    }

    public async Task AddAsync(OutboxIntegrationEvents outboxEvent)
    {
        await _context.Set<OutboxIntegrationEvents>().AddAsync(outboxEvent);

        await Commit();
    }

    public async Task Commit()
    {
        await _unitOfWork.CommitAsync();
    }

    public async Task<OutboxIntegrationEvents> GetByEventIdAsync(Guid id)
    {
        return await _context.Set<OutboxIntegrationEvents>()
                        .FirstAsync(e => e.Id == id);
    }

    public async Task<List<OutboxIntegrationEvents>> ReadAsync()
    {
        return await _context.Set<OutboxIntegrationEvents>()
                             .Where(e => e.ProcessedAt == null)
                             .OrderBy(e => e.CreatedAt)
                             .Take(_outboxOptions.BatchSize)
                             .ToListAsync();
    }
}
