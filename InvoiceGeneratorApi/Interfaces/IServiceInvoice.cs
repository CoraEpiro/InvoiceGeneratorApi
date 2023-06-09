﻿using InvoiceGeneratorApi.DTO;
using InvoiceGeneratorApi.DTO.Pagination;
using InvoiceGeneratorApi.Enums;
using InvoiceGeneratorApi.Providers;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceGeneratorApi.Interfaces;

public interface IServiceInvoice
{
    Task<InvoiceDTO> CreateInvoice(InvoiceDTO invoiceDTO);
    Task<InvoiceDTO> EditInvoice(
        int invoiceId, int? customerId, DateTimeOffset? startDate,
        DateTimeOffset? endDate, string? comment, InvoiceStatus? status);
    Task<InvoiceDTO> ChangeInvoiceStatus(int id, InvoiceStatus invoiceStatus);
    Task<InvoiceDTO> DeleteInvoice(int id);
    Task<InvoiceDTO> GetInvoice(int id);
    Task<PaginationDTO<InvoiceDTO>> GetInvoices(int page, int pageSize, string? search, OrderBy? orderBy);
    Task<byte[]> GenerateInvoicePDF(int id);
    Task<byte[]> GenerateInvoiceDocX(int id);
    Task SetUserInfo(UserInfo userInfo);
}