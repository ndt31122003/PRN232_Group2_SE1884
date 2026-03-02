using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
