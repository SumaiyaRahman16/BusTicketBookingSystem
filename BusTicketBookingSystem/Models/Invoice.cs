using System;

namespace BusTicketBookingSystem.Models
{
    public class Invoice
    {
 
        public string InvoiceId { get; private set; }
        public Ticket AssociatedTicket { get; private set; }
        public decimal TotalAmount { get; private set; }
        public bool IsPaid { get; private set; }

        private Invoice(Builder builder)
        {
            InvoiceId = "INV-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            AssociatedTicket = builder.Ticket;
            TotalAmount = builder.TotalAmount;
            IsPaid = builder.IsPaid;
        }


        public void MarkAsPaid()
        {
            IsPaid = true;
        }

        public override string ToString()
        {
            string status = IsPaid ? "PAID" : "UNPAID";
            return $"Invoice: {InvoiceId} | Ticket ID: {AssociatedTicket.TicketId} | Amount: BDT {TotalAmount} | Status: {status}";
        }


        public class Builder
        {
            public Ticket Ticket { get; private set; }
            public decimal TotalAmount { get; private set; }
            public bool IsPaid { get; private set; } = false; // Default state

            public Builder ForTicket(Ticket ticket)
            {
                Ticket = ticket ?? throw new ArgumentNullException(nameof(ticket));
                return this;
            }

            public Builder WithAmount(decimal amount)
            {
                if (amount <= 0) throw new ArgumentException("Amount must be positive.");
                TotalAmount = amount;
                return this;
            }

            public Builder SetPaidStatus(bool isPaid)
            {
                IsPaid = isPaid;
                return this;
            }

            public Invoice Build()
            {
                if (Ticket == null) 
                    throw new InvalidOperationException("Cannot build invoice without an associated ticket.");
                if (TotalAmount <= 0) 
                    throw new InvalidOperationException("Cannot build invoice without a valid financial amount.");

                return new Invoice(this);
            }
        }
    }
}