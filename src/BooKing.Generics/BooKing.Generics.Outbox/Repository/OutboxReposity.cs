using BooKing.Generics.Infra;
using BooKing.Generics.Outbox.Context;
using BooKing.Generics.Outbox.Entities;
using Microsoft.EntityFrameworkCore;

namespace BooKing.Generics.Outbox.Repository;
public class OutboxReposity : IOutboxReposity
{
    private readonly OutboxContext _context;
    private readonly IUnitOfWork<OutboxContext> _unitOfWork;

    public OutboxReposity(OutboxContext context, IUnitOfWork<OutboxContext> unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task AddAsync(OutboxIntegrationEvents outboxEvent)
    {
        await _context.Set<OutboxIntegrationEvents>().AddAsync(outboxEvent);

        await _unitOfWork.Commit();
    }

    public async Task<List<OutboxIntegrationEvents>> ReadAsync()
    {
        return await _context.Set<OutboxIntegrationEvents>().Where(e => e.ProcessedAt == null).ToListAsync();
    }
}
