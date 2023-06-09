﻿using DocumentFormat.OpenXml.Office.CustomUI;
using InvoiceGeneratorApi.PdfRelated;
using InvoiceGeneratorApi.PdfRelated.Models;
using NuGet.Protocol;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

public class InvoiceDocument : IDocument
{
    public InvoiceModel Model { get; }

    public InvoiceDocument(InvoiceModel model)
    {
        Model = model;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(50);

            page.Header().Element(ComposeHeader);
            page.Content().Element(ComposeContent);


            page.Footer().AlignCenter().Text(x =>
            {
                x.CurrentPageNumber();
                x.Span(" / ");
                x.TotalPages();
            });
        });
    }
    void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().Text($"Invoice #{Model.InvoiceNumber}")
                    .FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

                column.Item().Text(text =>
                {
                    text.Span("Issue date: ").SemiBold();
                    text.Span($"{Model.InvoiceDto.CreatedAt:d}");
                });

                column.Item().Text(text =>
                {
                    text.Span("Due date: ").SemiBold();
                    text.Span($"{Model.DueDate:d}");
                });
            });

            row.ConstantItem(100).Height(50).Placeholder();
        });
    }

    void ComposeContent(IContainer container)
    {
        container
            .Column(column =>
        {
            column.Spacing(5);

            column.Item().Row(row =>
            {
                row.RelativeItem().Component(new AddressComponent("From", Model.SellerAddress));
                row.ConstantItem(50);
                row.RelativeItem().Component(new AddressComponent("For", Model.CustomerAddress));
            });

            column.Item().Element(ComposeTable);

            var totalPrice = Model.InvoiceDto.TotalSum;
            column.Item().AlignRight().Text($"Grand total: {totalPrice}$").FontSize(14);

            if (!string.IsNullOrWhiteSpace(Model.InvoiceDto.Comment))
                column.Item().PaddingTop(25).Element(ComposeComments);
        });
    }

    void ComposeTable(IContainer container)
    {
        var headerStyle = TextStyle.Default.SemiBold();

        container.Table(table =>
        {
            // step 1
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(25);
                columns.RelativeColumn(3);
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
            });

            // step 2
            table.Header(header =>
            {
                header.Cell().Text("#");
                header.Cell().Text("Product").Style(headerStyle);
                header.Cell().AlignRight().Text("Unit price").Style(headerStyle);
                header.Cell().AlignRight().Text("Quantity").Style(headerStyle);
                header.Cell().AlignRight().Text("Total").Style(headerStyle);

                static IContainer CellStyle(IContainer container)
                {
                    return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                }
            });

            // step 3
            foreach (var row in Model.InvoiceDto.Rows)
            {
                var index = Model.InvoiceDto.Rows.ToList().IndexOf(row) + 1;

                table.Cell().Element(CellStyle).Text(index);
                table.Cell().Element(CellStyle).Text(row.Service);
                table.Cell().Element(CellStyle).AlignRight().Text($"{row.Amount}$");
                table.Cell().Element(CellStyle).AlignRight().Text($"{row.Quantity}$");
                table.Cell().Element(CellStyle).AlignRight().Text($"{row.Sum}$");

                static IContainer CellStyle(IContainer container) =>
                    container.BorderBottom(1)
                    .BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
            }
        });
    }

    void ComposeComments(IContainer container)
    {
        container.ShowEntire().Background(Colors.Grey.Lighten3).Padding(10).Column(column =>
        {
            column.Spacing(5);
            column.Item().Text("Comments").FontSize(14).SemiBold();
            column.Item().Text(Model.InvoiceDto.Comment);
        });
    }
}