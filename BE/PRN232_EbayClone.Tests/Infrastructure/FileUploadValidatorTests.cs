using Microsoft.AspNetCore.Http;
using PRN232_EbayClone.Infrastructure.FileStorage;
using Xunit;
using Moq;

namespace PRN232_EbayClone.Tests.Infrastructure;

public class FileUploadValidatorTests
{
    private static IFormFile CreateMockFile(string fileName, long fileSize, string contentType)
    {
        var content = new byte[fileSize];
        var stream = new MemoryStream(content);
        var file = new FormFile(stream, 0, fileSize, "file", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };
        return file;
    }

    #region ValidateFile Tests

    [Fact]
    public void ValidateFile_WithValidImageFile_ReturnsSuccess()
    {
        // Arrange
        var file = CreateMockFile("test.jpg", 1024 * 100, "image/jpeg");

        // Act
        var result = FileUploadValidator.ValidateFile(file);

        // Assert
        Assert.True(result.IsValid);
        Assert.Null(result.ErrorMessage);
    }

    [Fact]
    public void ValidateFile_WithValidPdfFile_ReturnsSuccess()
    {
        // Arrange
        var file = CreateMockFile("document.pdf", 1024 * 500, "application/pdf");

        // Act
        var result = FileUploadValidator.ValidateFile(file);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ValidateFile_WithValidDocxFile_ReturnsSuccess()
    {
        // Arrange
        var file = CreateMockFile("document.docx", 1024 * 200, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");

        // Act
        var result = FileUploadValidator.ValidateFile(file);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ValidateFile_WithNullFile_ReturnsFail()
    {
        // Act
        var result = FileUploadValidator.ValidateFile(null);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("empty or null", result.ErrorMessage);
    }

    [Fact]
    public void ValidateFile_WithEmptyFile_ReturnsFail()
    {
        // Arrange
        var file = CreateMockFile("empty.jpg", 0, "image/jpeg");

        // Act
        var result = FileUploadValidator.ValidateFile(file);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("empty", result.ErrorMessage);
    }

    [Fact]
    public void ValidateFile_WithFileSizeExceedingLimit_ReturnsFail()
    {
        // Arrange - 11MB file (exceeds 10MB limit)
        var file = CreateMockFile("large.jpg", 11 * 1024 * 1024, "image/jpeg");

        // Act
        var result = FileUploadValidator.ValidateFile(file);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("exceeds maximum", result.ErrorMessage);
    }

    [Fact]
    public void ValidateFile_WithInvalidExtension_ReturnsFail()
    {
        // Arrange
        var file = CreateMockFile("malicious.exe", 1024 * 100, "application/octet-stream");

        // Act
        var result = FileUploadValidator.ValidateFile(file);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("not allowed", result.ErrorMessage);
    }

    [Fact]
    public void ValidateFile_WithInvalidMimeType_ReturnsFail()
    {
        // Arrange
        var file = CreateMockFile("file.jpg", 1024 * 100, "application/x-msdownload");

        // Act
        var result = FileUploadValidator.ValidateFile(file);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("not allowed", result.ErrorMessage);
    }

    [Theory]
    [InlineData("image/png")]
    [InlineData("image/gif")]
    [InlineData("image/webp")]
    public void ValidateFile_WithValidImageMimeTypes_ReturnsSuccess(string mimeType)
    {
        // Arrange
        var file = CreateMockFile("test.png", 1024 * 100, mimeType);

        // Act
        var result = FileUploadValidator.ValidateFile(file);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("application/msword")]
    [InlineData("application/vnd.ms-excel")]
    [InlineData("text/plain")]
    public void ValidateFile_WithValidDocumentMimeTypes_ReturnsSuccess(string mimeType)
    {
        // Arrange
        var file = CreateMockFile("test.doc", 1024 * 100, mimeType);

        // Act
        var result = FileUploadValidator.ValidateFile(file);

        // Assert
        Assert.True(result.IsValid);
    }

    #endregion

    #region ValidateFiles Tests

    [Fact]
    public void ValidateFiles_WithValidMultipleFiles_ReturnsSuccess()
    {
        // Arrange
        var files = new List<IFormFile>
        {
            CreateMockFile("file1.jpg", 1024 * 100, "image/jpeg"),
            CreateMockFile("file2.pdf", 1024 * 500, "application/pdf"),
            CreateMockFile("file3.docx", 1024 * 200, "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
        };

        // Act
        var result = FileUploadValidator.ValidateFiles(files);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ValidateFiles_WithEmptyCollection_ReturnsFail()
    {
        // Arrange
        var files = new List<IFormFile>();

        // Act
        var result = FileUploadValidator.ValidateFiles(files);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("No files", result.ErrorMessage);
    }

    [Fact]
    public void ValidateFiles_WithNullCollection_ReturnsFail()
    {
        // Act
        var result = FileUploadValidator.ValidateFiles(null);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("No files", result.ErrorMessage);
    }

    [Fact]
    public void ValidateFiles_ExceedingMaxFileCount_ReturnsFail()
    {
        // Arrange
        var files = new List<IFormFile>();
        for (int i = 0; i < 11; i++)
        {
            files.Add(CreateMockFile($"file{i}.jpg", 1024 * 100, "image/jpeg"));
        }

        // Act
        var result = FileUploadValidator.ValidateFiles(files, maxFileCount: 10);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("Too many files", result.ErrorMessage);
    }

    [Fact]
    public void ValidateFiles_WithOneInvalidFile_ReturnsFail()
    {
        // Arrange
        var files = new List<IFormFile>
        {
            CreateMockFile("file1.jpg", 1024 * 100, "image/jpeg"),
            CreateMockFile("malicious.exe", 1024 * 100, "application/octet-stream"),
            CreateMockFile("file3.pdf", 1024 * 500, "application/pdf")
        };

        // Act
        var result = FileUploadValidator.ValidateFiles(files);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("not allowed", result.ErrorMessage);
    }

    [Fact]
    public void ValidateFiles_WithCustomMaxFileCount_ReturnsSuccess()
    {
        // Arrange
        var files = new List<IFormFile>
        {
            CreateMockFile("file1.jpg", 1024 * 100, "image/jpeg"),
            CreateMockFile("file2.jpg", 1024 * 100, "image/jpeg"),
            CreateMockFile("file3.jpg", 1024 * 100, "image/jpeg"),
            CreateMockFile("file4.jpg", 1024 * 100, "image/jpeg"),
            CreateMockFile("file5.jpg", 1024 * 100, "image/jpeg")
        };

        // Act
        var result = FileUploadValidator.ValidateFiles(files, maxFileCount: 5);

        // Assert
        Assert.True(result.IsValid);
    }

    #endregion

    #region GenerateUniqueFileName Tests

    [Fact]
    public void GenerateUniqueFileName_WithValidFileName_ReturnsUniqueFileName()
    {
        // Arrange
        var originalFileName = "document.pdf";

        // Act
        var uniqueFileName = FileUploadValidator.GenerateUniqueFileName(originalFileName);

        // Assert
        Assert.NotEqual(originalFileName, uniqueFileName);
        Assert.EndsWith(".pdf", uniqueFileName);
        Assert.Contains("document", uniqueFileName);
    }

    [Fact]
    public void GenerateUniqueFileName_GeneratesTwoDifferentNames_ForSameInput()
    {
        // Arrange
        var originalFileName = "image.jpg";

        // Act
        var uniqueFileName1 = FileUploadValidator.GenerateUniqueFileName(originalFileName);
        System.Threading.Thread.Sleep(10); // Small delay to ensure different timestamp
        var uniqueFileName2 = FileUploadValidator.GenerateUniqueFileName(originalFileName);

        // Assert
        Assert.NotEqual(uniqueFileName1, uniqueFileName2);
    }

    [Fact]
    public void GenerateUniqueFileName_PreservesExtension()
    {
        // Arrange
        var originalFileName = "myfile.docx";

        // Act
        var uniqueFileName = FileUploadValidator.GenerateUniqueFileName(originalFileName);

        // Assert
        Assert.EndsWith(".docx", uniqueFileName);
    }

    [Fact]
    public void GenerateUniqueFileName_WithFileNameWithoutExtension_AddsNoExtension()
    {
        // Arrange
        var originalFileName = "noextension";

        // Act
        var uniqueFileName = FileUploadValidator.GenerateUniqueFileName(originalFileName);

        // Assert
        Assert.NotEqual(originalFileName, uniqueFileName);
        Assert.Contains("noextension", uniqueFileName);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void ValidateFile_WithMaxAllowedFileSize_ReturnsSuccess()
    {
        // Arrange - exactly 10MB
        var file = CreateMockFile("maxsize.jpg", 10 * 1024 * 1024, "image/jpeg");

        // Act
        var result = FileUploadValidator.ValidateFile(file);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ValidateFile_WithOneByteOverLimit_ReturnsFail()
    {
        // Arrange - 10MB + 1 byte
        var file = CreateMockFile("toolarge.jpg", (10 * 1024 * 1024) + 1, "image/jpeg");

        // Act
        var result = FileUploadValidator.ValidateFile(file);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void ValidateFile_WithCaseInsensitiveExtension_ReturnsSuccess()
    {
        // Arrange
        var file = CreateMockFile("IMAGE.JPG", 1024 * 100, "image/jpeg");

        // Act
        var result = FileUploadValidator.ValidateFile(file);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ValidateFile_WithCaseInsensitiveMimeType_ReturnsSuccess()
    {
        // Arrange
        var file = CreateMockFile("test.jpg", 1024 * 100, "IMAGE/JPEG");

        // Act
        var result = FileUploadValidator.ValidateFile(file);

        // Assert
        Assert.True(result.IsValid);
    }

    #endregion

    #region Async Virus Scanning Tests

    [Fact]
    public async Task ValidateFileAsync_WithNoVirusScanService_ReturnsSuccess()
    {
        // Arrange
        var validator = new FileUploadValidator(null);
        var file = CreateMockFile("test.jpg", 1024 * 100, "image/jpeg");

        // Act
        var result = await validator.ValidateFileAsync(file);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task ValidateFileAsync_WithUnavailableVirusScanService_ReturnsSuccess()
    {
        // Arrange
        var mockVirusScanService = new Mock<IVirusScanService>();
        mockVirusScanService.Setup(x => x.IsAvailable()).Returns(false);

        var validator = new FileUploadValidator(mockVirusScanService.Object);
        var file = CreateMockFile("test.jpg", 1024 * 100, "image/jpeg");

        // Act
        var result = await validator.ValidateFileAsync(file);

        // Assert
        Assert.True(result.IsValid);
        mockVirusScanService.Verify(x => x.ScanAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ValidateFileAsync_WithCleanFile_ReturnsSuccess()
    {
        // Arrange
        var mockVirusScanService = new Mock<IVirusScanService>();
        mockVirusScanService.Setup(x => x.IsAvailable()).Returns(true);
        mockVirusScanService.Setup(x => x.ScanAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new FileUploadValidator(mockVirusScanService.Object);
        var file = CreateMockFile("test.jpg", 1024 * 100, "image/jpeg");

        // Act
        var result = await validator.ValidateFileAsync(file);

        // Assert
        Assert.True(result.IsValid);
        mockVirusScanService.Verify(x => x.ScanAsync(It.IsAny<Stream>(), "test.jpg", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ValidateFileAsync_WithInfectedFile_ReturnsFail()
    {
        // Arrange
        var mockVirusScanService = new Mock<IVirusScanService>();
        mockVirusScanService.Setup(x => x.IsAvailable()).Returns(true);
        mockVirusScanService.Setup(x => x.ScanAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new FileUploadValidator(mockVirusScanService.Object);
        var file = CreateMockFile("malware.jpg", 1024 * 100, "image/jpeg");

        // Act
        var result = await validator.ValidateFileAsync(file);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("failed virus scan", result.ErrorMessage);
    }

    [Fact]
    public async Task ValidateFileAsync_WithVirusScanException_ReturnsFail()
    {
        // Arrange
        var mockVirusScanService = new Mock<IVirusScanService>();
        mockVirusScanService.Setup(x => x.IsAvailable()).Returns(true);
        mockVirusScanService.Setup(x => x.ScanAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("ClamAV connection failed"));

        var validator = new FileUploadValidator(mockVirusScanService.Object);
        var file = CreateMockFile("test.jpg", 1024 * 100, "image/jpeg");

        // Act
        var result = await validator.ValidateFileAsync(file);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("Error scanning file", result.ErrorMessage);
    }

    [Fact]
    public async Task ValidateFilesAsync_WithMultipleCleanFiles_ReturnsSuccess()
    {
        // Arrange
        var mockVirusScanService = new Mock<IVirusScanService>();
        mockVirusScanService.Setup(x => x.IsAvailable()).Returns(true);
        mockVirusScanService.Setup(x => x.ScanAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new FileUploadValidator(mockVirusScanService.Object);
        var files = new List<IFormFile>
        {
            CreateMockFile("file1.jpg", 1024 * 100, "image/jpeg"),
            CreateMockFile("file2.pdf", 1024 * 500, "application/pdf")
        };

        // Act
        var result = await validator.ValidateFilesAsync(files);

        // Assert
        Assert.True(result.IsValid);
        mockVirusScanService.Verify(x => x.ScanAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Fact]
    public async Task ValidateFilesAsync_WithOneInfectedFile_ReturnsFail()
    {
        // Arrange
        var mockVirusScanService = new Mock<IVirusScanService>();
        mockVirusScanService.Setup(x => x.IsAvailable()).Returns(true);
        mockVirusScanService.SetupSequence(x => x.ScanAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)  // First file is clean
            .ReturnsAsync(false); // Second file is infected

        var validator = new FileUploadValidator(mockVirusScanService.Object);
        var files = new List<IFormFile>
        {
            CreateMockFile("file1.jpg", 1024 * 100, "image/jpeg"),
            CreateMockFile("malware.pdf", 1024 * 500, "application/pdf")
        };

        // Act
        var result = await validator.ValidateFilesAsync(files);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("failed virus scan", result.ErrorMessage);
    }

    #endregion
