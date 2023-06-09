﻿using InvoiceGeneratorApi.DTO;
using InvoiceGeneratorApi.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceGeneratorApi.Models;

public class Invoice
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    [NotMapped]
    public InvoiceRowDTO[] Rows { get; set; }
    public decimal TotalSum { get; set; }
    public string? Comment { get; set; }
    public InvoiceStatus Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public DateTimeOffset DeletedAt { get; set; }
}