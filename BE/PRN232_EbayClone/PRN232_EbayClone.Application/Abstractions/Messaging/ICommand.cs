using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>;
