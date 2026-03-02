using System;

namespace PRN232_EbayClone.Application.Orders.Dtos;

public sealed record PreviewShippingLabelResult(
    string FileName,
    string ContentType,
    string FileBase64);
