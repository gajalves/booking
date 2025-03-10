﻿using Microsoft.EntityFrameworkCore;

namespace BooKing.Generics.Infra.Implementations;
public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
{
    private readonly TContext _dbContext;
    public UnitOfWork(TContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CommitAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public void Commit()
    {
        _dbContext.SaveChanges();
    }
}
