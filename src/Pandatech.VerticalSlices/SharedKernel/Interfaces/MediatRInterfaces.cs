using MediatR;

namespace Pandatech.VerticalSlices.SharedKernel.Interfaces;

public interface ICommand<out TResponse> : IRequest<TResponse>;

public interface ICommand : IRequest;

public interface IQuery<out TResponse> : IRequest<TResponse>;

public interface IQuery : IRequest;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
   where TCommand : ICommand<TResponse>;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand>
   where TCommand : ICommand;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
   where TQuery : IQuery<TResponse>;

public interface IQueryHandler<in TQuery> : IRequestHandler<TQuery>
   where TQuery : IQuery;
