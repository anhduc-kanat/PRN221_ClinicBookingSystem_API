using ClinicBookingSystem_BusinessObject.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBookingSystem_BusinessObject.Entities
{
    public class Transaction : BaseEntities
    {
        /*
        public string Name { get; set; }
        */
        public TransactionStatus Status { get; set; }
        /*
        public string? Description { get; set; }
        */
        public bool? IsPay { get; set; }
        /*public string? PaymentLink {get; set; }*/
        
        public long? Amount { get; set; }
        public string? BankCode { get; set; }
        public string? BankTranNo { get; set; }
        public string? CardType { get; set; }
        public string? OrderInfo { get; set; }
        public DateTime? PayDate { get; set; }
        public string? ResponseCode { get; set; }
        public string? TransactionNo { get; set; }
        public string? TransactionStatus { get; set; }
        public string? OrderId { get; set; }
        public TransactionType Type { get; set; }
        
        //Appointment
        public Appointment? Appointment { get; set; }
        //Billing
        public Billing? Billing { get; set; }
    }
}