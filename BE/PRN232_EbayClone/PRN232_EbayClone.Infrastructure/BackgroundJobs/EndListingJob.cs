using PRN232_EbayClone.Application.Abstractions.Data;
using Quartz;

namespace PRN232_EbayClone.Infrastructure.BackgroundJobs;

public sealed class EndListingJob : IJob
{
    private readonly IListingRepository _listingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EndListingJob(
        IUnitOfWork unitOfWork,
        IListingRepository listingRepository)
    {
        _unitOfWork = unitOfWork;
        _listingRepository = listingRepository;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var listings = await _listingRepository.GetListingsToEndAsync(
            DateTime.UtcNow,
            cancellationToken: context.CancellationToken);

        if(listings.Count == 0)
            return;

        listings.ForEach(listing =>
        {
            listing.End();
        });

        await _unitOfWork.SaveChangesAsync(context.CancellationToken);
    }
}
