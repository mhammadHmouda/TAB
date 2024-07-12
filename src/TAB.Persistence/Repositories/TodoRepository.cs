﻿using TAB.Application.Core.Abstractions;
using TAB.Domain.Features.Todos;

namespace TAB.Persistence.Repositories;

public class TodoRepository : BaseRepository<Todo>, ITodoRepository
{
    public TodoRepository(IDbContext dbContext)
        : base(dbContext) { }
}
