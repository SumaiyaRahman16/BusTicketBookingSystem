using System;
using System.Collections.Generic;
using System.Linq;
using BusTicketBookingSystem.Models;

namespace BusTicketBookingSystem.Services
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly List<Invoice> _invoices = new List<Invoice>();
        private int _idCounter = 1;

        public void AddInvoice(Invoice invoice)
        {
            string generatedId = $"INV-{_idCounter++}";

            // Use reflection to cleanly overwrite the InvoiceId field with our sequential ID
            typeof(Invoice).GetProperty("InvoiceId")?.SetValue(invoice, generatedId);

            _invoices.Add(invoice);
        }

        public Invoice GetInvoiceById(string invoiceId)
        {
            return _invoices.FirstOrDefault(i => i.InvoiceId.Equals(invoiceId, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Invoice> GetAllInvoices()
        {
            return _invoices;
        }
    }
}