using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using PRN232_EbayClone.Application.Abstractions.Shipping;
using PRN232_EbayClone.Infrastructure.Services.ShippingProvider;
using Xunit;

namespace PRN232_EbayClone.Tests.Infrastructure.Services.ShippingProvider
{
    public sealed class PdfShippingLabelRendererTests
    {
        [Fact]
        public async Task RenderPdfAsync_ReturnsValidPdfBytes()
        {
            // Arrange
            var renderer = new PdfShippingLabelRenderer();
            var model = new ShippingLabelRenderModel
            {
                OrderNumber = "ORD-12345678",
                RecipientName = "John Doe",
                RecipientAddress = "123 Main St\nSpringfield, IL 62704",
                SenderName = "Jane Seller",
                SenderAddress = "456 Market Ave\nNew York, NY 10001",
                CarrierName = "USPS",
                TrackingNumber = "9400111899223857481234",
                BarcodeValue = "420494409101128882300556460234",
                ShipDate = new DateTime(2025, 10, 27)
            };

            // Act
            var pdfBytes = await renderer.RenderPdfAsync(model, CancellationToken.None);

            // 👇 Export to a temp file to ease manual verification without locking a shared path.
            var tempFilePath = Path.Combine(Path.GetTempPath(), $"label-test-{Guid.NewGuid():N}.pdf");
            await File.WriteAllBytesAsync(tempFilePath, pdfBytes);
            Console.WriteLine($"PDF exported to: {tempFilePath}");

            // Assert
            pdfBytes.Should().NotBeNull();
            pdfBytes.Should().NotBeEmpty();

            var header = Encoding.ASCII.GetString(pdfBytes, 0, Math.Min(pdfBytes.Length, 4));
            header.Should().Be("%PDF");

            var tailLength = Math.Min(1024, pdfBytes.Length);
            var eofSegment = Encoding.ASCII.GetString(pdfBytes, pdfBytes.Length - tailLength, tailLength);
            eofSegment.Should().Contain("%%EOF");
        }
    }
}
