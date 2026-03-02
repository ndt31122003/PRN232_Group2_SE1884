using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Abstractions.Messaging;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;
