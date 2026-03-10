using PRN232_EbayClone.Application.Abstractions.Data;
using Quartz;

namespace PRN232_EbayClone.Infrastructure.BackgroundJobs;

public sealed class ActivateListingJob : IJob
{
    private readonly IListingRepository _listingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ActivateListingJob(
        IListingRepository listingRepository,
        IUnitOfWork unitOfWork)
    {
        _listingRepository = listingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var listings = await _listingRepository.GetListingsToActivateAsync(
            DateTime.UtcNow,
            cancellationToken: context.CancellationToken);

        if (listings.Count == 0)
            return;

        listings.ForEach(listing =>
        {
            listing.Activate();
        });

        await _unitOfWork.SaveChangesAsync(context.CancellationToken);
    }
}
