using Microsoft.Extensions.DependencyInjection;

namespace HappyWarehouse.Domain.CQRS;

public class Dispatcher(IServiceProvider serviceProvider)
{
    public async Task<TResult> SendCommandAsync<TCommand, TResult>(TCommand command)
        where TCommand : ICommand<TResult>
    {
        var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResult>>();
        return await handler.HandleAsync(command);
    }

    public async Task<TResult> SendQueryAsync<TQuery, TResult>(TQuery query)
        where TQuery : IQuery<TResult>
    {
        var handler = serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
        return await handler.HandleAsync(query);
    }
}