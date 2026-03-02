using HandlebarsDotNet;
using PRN232_EbayClone.Application.Abstractions.Shipping;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.Rendering;

namespace PRN232_EbayClone.Infrastructure.Services.ShippingProvider
{
    public sealed class PdfShippingLabelRenderer : IShippingLabelRenderer
    {
        private static readonly string TemplateRelativePath = Path.Combine("Services", "ShippingProvider", "Templates", "ShippingLabel.hbs");
        private static readonly string BrowserCachePath = Path.Combine(AppContext.BaseDirectory, ".puppeteer");
        private static readonly string[] ExecutablePathEnvironmentVariables =
        {
            "EBAYCLONE_CHROMIUM_EXECUTABLE_PATH",
            "PUPPETEER_EXECUTABLE_PATH",
            "CHROME_EXECUTABLE_PATH",
            "CHROMIUM_EXECUTABLE_PATH"
        };
        private const string DisableDownloadEnvironmentVariable = "EBAYCLONE_DISABLE_CHROMIUM_DOWNLOAD";
        private static readonly SemaphoreSlim TemplateSemaphore = new(1, 1);
        private static HandlebarsTemplate<object, object>? _compiledTemplate;
        private static readonly BrowserFetcher BrowserFetcherInstance = new(new BrowserFetcherOptions
        {
            Path = BrowserCachePath
        });

        static PdfShippingLabelRenderer()
        {
            Directory.CreateDirectory(BrowserCachePath);
        }

        private static readonly Lazy<Task<string>> ChromiumExecutableTask = new(async () =>
        {
            var installedBrowser = await BrowserFetcherInstance.DownloadAsync().ConfigureAwait(false);
            var browserType = installedBrowser.GetType();

            string? executablePath = browserType.GetProperty("ExecutablePath")?
                .GetValue(installedBrowser) as string;

            if (string.IsNullOrWhiteSpace(executablePath))
            {
                var buildId = browserType.GetProperty("BuildId")?
                    .GetValue(installedBrowser) as string;

                if (!string.IsNullOrWhiteSpace(buildId))
                {
                    executablePath = BrowserFetcherInstance.GetExecutablePath(buildId);
                }
            }

            if (string.IsNullOrWhiteSpace(executablePath))
            {
                var revision = browserType.GetProperty("BrowserRevision")?
                    .GetValue(installedBrowser) as string;

                if (!string.IsNullOrWhiteSpace(revision))
                {
                    executablePath = BrowserFetcherInstance.GetExecutablePath(revision);
                }
            }

            if (string.IsNullOrWhiteSpace(executablePath))
            {
                throw new InvalidOperationException("Unable to determine Chromium executable path after download.");
            }

            return executablePath;
        });

        public async Task<byte[]> RenderPdfAsync(ShippingLabelRenderModel model, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(model);
            ct.ThrowIfCancellationRequested();

            var template = await GetTemplateAsync(ct).ConfigureAwait(false);
            ct.ThrowIfCancellationRequested();

            var html = template(BuildTemplateContext(model));

            var pageWidthIn = model.PageWidthIn > 0 ? model.PageWidthIn : 4m;
            var pageHeightIn = model.PageHeightIn > 0 ? model.PageHeightIn : 6m;
            var (viewportWidthPx, viewportHeightPx) = ResolveViewportDimensions(pageWidthIn, pageHeightIn);

            var executablePath = await EnsureChromiumExecutableAsync(ct).ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(executablePath) || !File.Exists(executablePath))
            {
                throw new FileNotFoundException(
                    $"Chromium executable not found at '{executablePath}'. Ensure PuppeteerSharp can download Chromium.");
            }

            var launchOptions = new LaunchOptions
            {
                Headless = true,
                ExecutablePath = executablePath,
                Args = new[]
                {
                    "--no-sandbox",
                    "--disable-setuid-sandbox",
                    "--disable-dev-shm-usage",
                    "--disable-gpu",
                    "--disable-software-rasterizer",
                    "--no-zygote",
                    "--hide-scrollbars",
                    $"--window-size={viewportWidthPx},{viewportHeightPx}"
                }
            };

            await using var browser = await LaunchBrowserAsync(launchOptions, ct).ConfigureAwait(false);
            await using var page = await browser.NewPageAsync().ConfigureAwait(false);

            await page.SetViewportAsync(new ViewPortOptions { Width = viewportWidthPx, Height = viewportHeightPx }).ConfigureAwait(false);
            await page.SetContentAsync(html, new NavigationOptions { WaitUntil = new[] { WaitUntilNavigation.Networkidle0 } }).ConfigureAwait(false);
            await page.EmulateMediaTypeAsync(MediaType.Print).ConfigureAwait(false);

            try
            {
                var pdfBytes = await page.PdfDataAsync(new PdfOptions
                {
                    Width = FormattableString.Invariant($"{pageWidthIn}in"),
                    Height = FormattableString.Invariant($"{pageHeightIn}in"),
                    PrintBackground = true,
                    MarginOptions = new MarginOptions
                    {
                        Top = "0in",
                        Bottom = "0in",
                        Left = "0in",
                        Right = "0in"
                    }
                }).ConfigureAwait(false);

                return pdfBytes;
            }
            catch (TargetClosedException ex)
            {
                throw new InvalidOperationException(
                    "Chromium closed unexpectedly while generating the PDF. Ensure required system libraries (fonts, libX11, libXcomposite, libnss3, libatk, libgbm, etc.) are installed in the container, and that no security policy kills the process.",
                    ex);
            }
        }

        public static async Task<string> EnsureChromiumExecutableAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var envExecutablePath = TryResolveExecutablePathFromEnvironment();
            if (!string.IsNullOrWhiteSpace(envExecutablePath))
            {
                cancellationToken.ThrowIfCancellationRequested();
                return envExecutablePath;
            }

            if (IsChromiumDownloadDisabled())
            {
                throw new InvalidOperationException(
                    $"Chromium executable path was not provided via environment variables ({string.Join(", ", ExecutablePathEnvironmentVariables)}) and automatic downloads are disabled by setting {DisableDownloadEnvironmentVariable}=true. Provide a valid executable path or allow Chromium downloads.");
            }

            var executablePath = await ChromiumExecutableTask.Value.ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
            return executablePath;
        }

        private static async Task<IBrowser> LaunchBrowserAsync(LaunchOptions launchOptions, CancellationToken ct)
        {
            try
            {
                return await Puppeteer.LaunchAsync(launchOptions).ConfigureAwait(false);
            }
            catch (PuppeteerSharp.ProcessException processException)
            {
                var builder = new StringBuilder()
                    .Append("Failed to launch Chromium using PuppeteerSharp. Confirm that the target host allows process execution, the executable is compatible with the current OS/architecture, and all required native dependencies (fontconfig, libX11, libnss3, libatk, libgbm, etc.) are installed.")
                    .AppendLine()
                    .Append("Executable path: ")
                    .Append(string.IsNullOrWhiteSpace(launchOptions.ExecutablePath) ? "(not provided)" : launchOptions.ExecutablePath)
                    .AppendLine()
                    .Append("You can also set one of the environment variables [")
                    .Append(string.Join(", ", ExecutablePathEnvironmentVariables))
                    .Append("] to point to a pre-installed Chromium/Chrome binary or set ")
                    .Append(DisableDownloadEnvironmentVariable)
                    .Append("=false to allow automatic downloads.");

                throw new InvalidOperationException(builder.ToString(), processException);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "Failed to launch headless browser for PDF rendering. Verify that the hosting environment allows Chromium to run.",
                    ex);
            }
        }

        private static async Task<HandlebarsTemplate<object, object>> GetTemplateAsync(CancellationToken ct)
        {
            if (_compiledTemplate is not null)
            {
                return _compiledTemplate;
            }

            await TemplateSemaphore.WaitAsync(ct).ConfigureAwait(false);
            try
            {
                if (_compiledTemplate is null)
                {
                    var templatePath = Path.Combine(AppContext.BaseDirectory, TemplateRelativePath);
                    if (!File.Exists(templatePath))
                    {
                        throw new FileNotFoundException($"Shipping label template not found at '{templatePath}'.");
                    }

                    var templateContent = await File.ReadAllTextAsync(templatePath, ct).ConfigureAwait(false);
                    _compiledTemplate = Handlebars.Compile(templateContent);
                }
            }
            finally
            {
                TemplateSemaphore.Release();
            }

            return _compiledTemplate;
        }

        private static object BuildTemplateContext(ShippingLabelRenderModel model)
        {
            var senderAddress = model.SenderAddress ?? string.Empty;
            var recipientAddress = model.RecipientAddress ?? string.Empty;

            var serviceName = FormatServiceName(model.ServiceName);
            var packageType = FormatPackageType(model.PackageType);
            var shipDateFormatted = FormatShipDate(model.ShipDate);
            var packageWeight = FormatWeight(model.WeightOz);
            var packageDimensions = FormatDimensions(model.LengthIn, model.WidthIn, model.HeightIn);
            var postage = FormatCurrency(model.Cost, model.CostCurrency);
            var insurance = FormatCurrency(model.InsuranceAmount, model.InsuranceCurrency);
            var carrierBadge = BuildCarrierBadge(model.CarrierName, serviceName, packageType);

            return new
            {
                CarrierName = FormatCarrierName(model.CarrierName),
                ServiceName = serviceName,
                PackageType = packageType,
                CarrierBadge = carrierBadge,
                ShipDateFormatted = shipDateFormatted,
                OriginZip = ExtractZipCode(senderAddress),
                OrderNumber = model.OrderNumber ?? string.Empty,
                TrackingNumber = model.TrackingNumber ?? string.Empty,
                PackageWeight = packageWeight,
                PackageDimensions = packageDimensions,
                PostageDisplay = postage,
                InsuranceDisplay = insurance,
                RecipientName = model.RecipientName ?? string.Empty,
                RecipientAddressLines = SplitAddressLines(recipientAddress),
                SenderName = model.SenderName ?? string.Empty,
                SenderAddressLines = SplitAddressLines(senderAddress),
                BarcodeBase64 = GenerateCode128BarcodeBase64(model.BarcodeValue),
                FormattedBarcode = FormatBarcodeNumber(model.BarcodeValue),
                QrCodeBase64 = GenerateQrCodeBase64(model.TrackingNumber)
            };
        }

        private static (int WidthPx, int HeightPx) ResolveViewportDimensions(decimal pageWidthIn, decimal pageHeightIn)
        {
            const int dpi = 96;
            var widthPx = Math.Max(1, (int)Math.Round(pageWidthIn * dpi, MidpointRounding.AwayFromZero));
            var heightPx = Math.Max(1, (int)Math.Round(pageHeightIn * dpi, MidpointRounding.AwayFromZero));
            return (widthPx, heightPx);
        }

        private static string FormatCarrierName(string? carrier)
        {
            return string.IsNullOrWhiteSpace(carrier)
                ? "Carrier"
                : carrier.Trim();
        }

        private static string FormatShipDate(DateTime shipDate)
        {
            if (shipDate == default)
            {
                return "—";
            }

            return shipDate.ToString("MMM dd, yyyy", CultureInfo.InvariantCulture);
        }

        private static string FormatServiceName(string? serviceName)
        {
            if (string.IsNullOrWhiteSpace(serviceName))
            {
                return string.Empty;
            }

            if (serviceName.Any(char.IsWhiteSpace))
            {
                return serviceName.Trim();
            }

            var normalized = serviceName.Replace('_', ' ').Replace('-', ' ').Trim();
            return normalized.ToUpperInvariant();
        }

        private static string FormatPackageType(string? packageType)
        {
            if (string.IsNullOrWhiteSpace(packageType))
            {
                return string.Empty;
            }

            var normalized = packageType.Replace('_', ' ').Replace('-', ' ').Trim();
            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(normalized.ToLowerInvariant());
        }

        private static string FormatWeight(decimal weightOz)
        {
            if (weightOz <= 0)
            {
                return "—";
            }

            const decimal ouncesPerPound = 16m;
            var pounds = Math.Floor(weightOz / ouncesPerPound);
            var remainingOunces = weightOz - (pounds * ouncesPerPound);

            if (pounds > 0 && remainingOunces > 0)
            {
                return string.Format(CultureInfo.InvariantCulture, "{0:0} lb {1:0.#} oz", pounds, remainingOunces);
            }

            if (pounds > 0)
            {
                return string.Format(CultureInfo.InvariantCulture, "{0:0} lb", pounds);
            }

            return string.Format(CultureInfo.InvariantCulture, "{0:0.#} oz", remainingOunces);
        }

        private static string FormatDimensions(decimal lengthIn, decimal widthIn, decimal heightIn)
        {
            if (lengthIn <= 0 || widthIn <= 0 || heightIn <= 0)
            {
                return "—";
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                "{0} × {1} × {2} in",
                FormatNumber(lengthIn),
                FormatNumber(widthIn),
                FormatNumber(heightIn));
        }

        private static string FormatCurrency(decimal amount, string? currencyCode)
        {
            if (amount <= 0)
            {
                return "—";
            }

            var code = string.IsNullOrWhiteSpace(currencyCode)
                ? "USD"
                : currencyCode.Trim().ToUpperInvariant();

            return string.Format(CultureInfo.InvariantCulture, "{0} {1:0.00}", code, amount);
        }

        private static string FormatNumber(decimal value)
        {
            return value.ToString("0.##", CultureInfo.InvariantCulture);
        }

        private static string BuildCarrierBadge(string? carrier, string serviceName, string packageType)
        {
            var segments = new List<string>(3);

            if (!string.IsNullOrWhiteSpace(carrier))
            {
                segments.Add(carrier.Trim());
            }

            if (!string.IsNullOrWhiteSpace(serviceName))
            {
                segments.Add(serviceName.Trim());
            }

            if (!string.IsNullOrWhiteSpace(packageType))
            {
                segments.Add(packageType.Trim());
            }

            return segments.Count == 0 ? "Shipping Label" : string.Join(" · ", segments);
        }

        private static string GenerateQrCodeBase64(string? content)
        {
            var writer = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = 200,
                    Width = 200,
                    Margin = 1
                }
            };

            var pixelData = writer.Write(content ?? string.Empty);
            return EncodePixelDataToBase64(pixelData);
        }

        private static string GenerateCode128BarcodeBase64(string? content)
        {
            var writer = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions
                {
                    Height = 80,
                    Width = 480,
                    Margin = 0,
                    PureBarcode = true
                }
            };

            var barcodeValue = string.IsNullOrWhiteSpace(content) ? "000000000000" : content;
            var pixelData = writer.Write(barcodeValue);
            return EncodePixelDataToBase64(pixelData);
        }

        private static string EncodePixelDataToBase64(PixelData pixelData)
        {
            using var bitmap = new SKBitmap(pixelData.Width, pixelData.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
            var ptr = bitmap.GetPixels();
            Marshal.Copy(pixelData.Pixels, 0, ptr, pixelData.Pixels.Length);

            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            return Convert.ToBase64String(data.ToArray());
        }

        private static IReadOnlyList<string> SplitAddressLines(string? address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                return Array.Empty<string>();
            }

            return address
                .Replace("\r\n", "\n", StringComparison.Ordinal)
                .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .ToArray();
        }

        private static string ExtractZipCode(string address)
        {
            var match = System.Text.RegularExpressions.Regex.Match(address, @"\b\d{5}(-\d{4})?\b");
            return match.Success ? match.Value : "00000";
        }

        private static string FormatBarcodeNumber(string? barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode))
            {
                return string.Empty;
            }

            var digits = new string(barcode.Where(char.IsDigit).ToArray());

            if (digits.Length < 20)
            {
                return barcode;
            }

            var formatted = new StringBuilder();
            formatted.Append(digits.Substring(0, 3)).Append(' ');
            formatted.Append(digits.Substring(3, 5)).Append(' ');

            for (var i = 8; i < digits.Length; i += 4)
            {
                var length = Math.Min(4, digits.Length - i);
                formatted.Append(digits.Substring(i, length));
                if (i + length < digits.Length)
                {
                    formatted.Append(' ');
                }
            }

            return formatted.ToString();
        }

        private static string? TryResolveExecutablePathFromEnvironment()
        {
            foreach (var variable in ExecutablePathEnvironmentVariables)
            {
                var rawValue = Environment.GetEnvironmentVariable(variable);
                if (string.IsNullOrWhiteSpace(rawValue))
                {
                    continue;
                }

                var expandedValue = Environment.ExpandEnvironmentVariables(rawValue).Trim().Trim('"');
                if (!string.IsNullOrWhiteSpace(expandedValue) && File.Exists(expandedValue))
                {
                    return Path.GetFullPath(expandedValue);
                }
            }

            return null;
        }

        private static bool IsChromiumDownloadDisabled()
        {
            var rawValue = Environment.GetEnvironmentVariable(DisableDownloadEnvironmentVariable);
            if (string.IsNullOrWhiteSpace(rawValue))
            {
                return false;
            }

            return bool.TryParse(rawValue, out var parsed) && parsed;
        }
    }
}