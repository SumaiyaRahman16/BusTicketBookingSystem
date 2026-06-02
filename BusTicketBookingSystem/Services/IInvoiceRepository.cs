using System.Collections.Generic;
using BusTicketBookingSystem.Models;

namespace BusTicketBookingSystem.Services
{
    public interface IInvoiceRepository
    {
         void AddInvoice(Invoice invoice);
         Invoice GetInvoiceById(string invoiceId);
        IEnumerable<Invoice> GetAllInvoices();
    }
}