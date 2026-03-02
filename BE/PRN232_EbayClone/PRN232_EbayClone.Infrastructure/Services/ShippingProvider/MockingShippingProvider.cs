using PRN232_EbayClone.Application.Abstractions.Shipping;
using PRN232_EbayClone.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_EbayClone.Infrastructure.Services.ShippingProvider
{
    public class MockingShippingProvider : IShippingProvider
    {
        public MockingShippingProvider() { }

        public Task<CreateLabelResponse> CreateShippingLabelAsync(
            string fromName, 
            string fromStreet1, 
            string fromStreet2, 
            string fromCity, 
            string fromState, 
            string fromPostalCode, 
            string fromCountry, 
            string toName, 
            string toStreet1, 
            string toStreet2, 
            string toCity, 
            string toState, 
            string toPostalCode, 
            string toCountry, 
            decimal weightOz, 
            decimal lengthIn, 
            decimal widthIn,
            decimal heightIn, 
            Money insuranceAmount)
        {
            var cost = Money.Create(10.00m, "VNĐ").Value;
            var mokingResponse = new CreateLabelResponse
            {
                LabelDocumentId = Guid.NewGuid().ToString(),
                LabelUrl = "https://mockshippingprovider.com/labels/" + Guid.NewGuid().ToString() + ".pdf",
                TrackingNumber = "MOCKTRACK" + new Random().Next(1000000, 9999999).ToString(),
                Cost = cost,
                Insurance = insuranceAmount,
                PackageType = "MockBox",
                WeightOz = weightOz,
                LengthIn = lengthIn,
                WidthIn = widthIn,
                HeightIn = heightIn
            };
            return Task.FromResult(mokingResponse);
        }
    }
}
