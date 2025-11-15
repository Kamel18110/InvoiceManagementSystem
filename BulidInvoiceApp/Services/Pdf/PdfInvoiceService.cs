using BulidInvoiceApp.Models;
using BulidInvoiceApp.Services.Pdf;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace BulidInvoiceApp.Services.PDF
{
   
    public class PdfInvoiceService : IPdfInvoiceService
    {
        public async Task<byte[]> GenerateInvoicePdf(Invoice invoice, List<InvoiceProductInfo> products, CompanyInfo companyInfo)
        {
            
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);
                    page.DefaultTextStyle(x => x.FontSize(11));

                   
                    page.Header().Element(c => ComposeHeader(c, companyInfo));

                    
                    page.Content().Element(c => ComposeContent(c, invoice, products));

                    
                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Page ");
                        text.CurrentPageNumber();
                        text.Span(" of ");
                        text.TotalPages();
                    });
                });
            });

            return await Task.FromResult(document.GeneratePdf());
        }

        private void ComposeHeader(IContainer container, CompanyInfo companyInfo)
        {
            container.Column(column =>
            {
                
                column.Item().Row(row =>
                {
                   
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text(companyInfo.Name)
                            .FontSize(20)
                            .Bold()
                            .FontColor(Colors.Blue.Darken3);

                        if (!string.IsNullOrEmpty(companyInfo.Address))
                        {
                            col.Item().PaddingTop(5).Text($"Address: {companyInfo.Address}")
                                .FontSize(10)
                                .FontColor(Colors.Grey.Darken2);
                        }

                        if (!string.IsNullOrEmpty(companyInfo.Phone))
                        {
                            col.Item().Text($"Phone: {companyInfo.Phone}")
                                .FontSize(10)
                                .FontColor(Colors.Grey.Darken2);
                        }

                        if (!string.IsNullOrEmpty(companyInfo.Email))
                        {
                            col.Item().Text($"Email: {companyInfo.Email}")
                                .FontSize(10)
                                .FontColor(Colors.Grey.Darken2);
                        }
                    });

                   
                    if (companyInfo.Logo != null && companyInfo.Logo.Length > 0)
                    {
                        row.ConstantItem(120).Height(80).Image(companyInfo.Logo);
                    }
                });

               
                column.Item().PaddingTop(15).PaddingBottom(15)
                    .BorderBottom(2)
                    .BorderColor(Colors.Blue.Darken3);
            });
        }

        private void ComposeContent(IContainer container, Invoice invoice, List<InvoiceProductInfo> products)
        {
            container.Column(column =>
            {
                
                column.Item().PaddingBottom(10).Row(row =>
                {
                    row.RelativeItem().Text("INVOICE")
                        .FontSize(24)
                        .Bold()
                        .FontColor(Colors.Blue.Darken3);

                    row.RelativeItem().AlignRight().Column(col =>
                    {
                        col.Item().Text($"Invoice #: {invoice.Id.ToString().Substring(0, 8)}")
                            .FontSize(10)
                            .Bold();
                        col.Item().Text($"Date: {invoice.Date:yyyy-MM-dd}")
                            .FontSize(10);
                    });
                });

                
                column.Item().PaddingTop(10).PaddingBottom(20)
                    .Background(Colors.Grey.Lighten3)
                    .Padding(15)
                    .Column(col =>
                    {
                        col.Item().Text("Customer Information")
                            .FontSize(12)
                            .Bold()
                            .FontColor(Colors.Blue.Darken3);

                        col.Item().PaddingTop(8).Row(r =>
                        {
                            r.RelativeItem().Text($"Name: {invoice.NameCustomer}").FontSize(10);
                            r.RelativeItem().Text($"Phone: {invoice.PhoneNumber}").FontSize(10);
                        });

                        if (!string.IsNullOrEmpty(invoice.AddressCustomer))
                        {
                            col.Item().PaddingTop(3).Text($"Address: {invoice.AddressCustomer}")
                                .FontSize(10);
                        }
                    });

               
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(40);
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(1.5f); 
                        columns.RelativeColumn(2); 
                    });

                   
                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("#").Bold();
                        header.Cell().Element(CellStyle).Text("Product").Bold();
                        header.Cell().Element(CellStyle).Text("Price").Bold();
                        header.Cell().Element(CellStyle).Text("Qty").Bold();
                        header.Cell().Element(CellStyle).Text("Total").Bold();

                        static IContainer CellStyle(IContainer c)
                        {
                            return c
                                .Background(Colors.Blue.Darken3)
                                .Padding(8)
                                .AlignCenter()
                                .AlignMiddle();
                        }
                    });

                    
                    int index = 1;
                    foreach (var product in products)
                    {
                        var isEven = index % 2 == 0;
                        var bgColor = isEven ? Colors.Grey.Lighten4 : Colors.White;

                        table.Cell().Element(c => ProductCellStyle(c, bgColor)).Text(index.ToString());
                        table.Cell().Element(c => ProductCellStyle(c, bgColor)).Text(product.ProductName);
                        table.Cell().Element(c => ProductCellStyle(c, bgColor)).Text($"دج{product.Price:N2}");
                        table.Cell().Element(c => ProductCellStyle(c, bgColor)).Text(product.Quantity.ToString());
                        table.Cell().Element(c => ProductCellStyle(c, bgColor)).Text($"${(product.Price * product.Quantity):N2}");

                        index++;
                    }

                    static IContainer ProductCellStyle(IContainer c, string bgColor)
                    {
                        return c
                            .Background(bgColor)
                            .BorderBottom(1)
                            .BorderColor(Colors.Grey.Lighten2)
                            .Padding(8)
                            .AlignCenter()
                            .AlignMiddle();
                    }
                });

                
                column.Item().PaddingTop(20).AlignRight().Column(col =>
                {
                    var subtotal = products.Sum(p => p.Price * p.Quantity);
                    var taxAmount = subtotal * invoice.Tax / 100;
                    var discountAmount = subtotal * invoice.Discount / 100;
                    var total = subtotal + taxAmount - discountAmount;

                    col.Item().Row(r =>
                    {
                        r.ConstantItem(150).Text("Subtotal:").Bold();
                        r.ConstantItem(100).AlignRight().Text($"دج{subtotal:N2}");
                    });

                    col.Item().PaddingTop(5).Row(r =>
                    {
                        r.ConstantItem(150).Text($"Tax ({invoice.Tax}%):").Bold();
                        r.ConstantItem(100).AlignRight().Text($"دج{taxAmount:N2}");
                    });

                    col.Item().PaddingTop(5).Row(r =>
                    {
                        r.ConstantItem(150).Text($"Discount ({invoice.Discount}%):").Bold();
                        r.ConstantItem(100).AlignRight().Text($"-دج{discountAmount:N2}").FontColor(Colors.Red.Medium);
                    });

                    col.Item().PaddingTop(10)
                        .BorderTop(2)
                        .BorderColor(Colors.Blue.Darken3)
                        .PaddingTop(10)
                        .Row(r =>
                        {
                            r.ConstantItem(150).Text("TOTAL:").FontSize(14).Bold().FontColor(Colors.Blue.Darken3);
                            r.ConstantItem(100).AlignRight().Text($"دج{total:N2}").FontSize(14).Bold().FontColor(Colors.Blue.Darken3);
                        });
                });

               
                column.Item().PaddingTop(30).AlignCenter()
                    .Text("Thank you for your business!")
                    .FontSize(12)
                    .Italic()
                    .FontColor(Colors.Grey.Darken1);
            });
        }
    }
}