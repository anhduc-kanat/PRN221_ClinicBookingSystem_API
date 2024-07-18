using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_BusinessObject.Enums;
using ClinicBookingSystem_Service.Models.Response.Appointment;
using ClinicBookingSystem_Service.Models.Response.AppointmentService;

namespace ClinicBookingSystem_Service.Models.Response.Transaction;

public class GetTransactionResponse
{
    public int Id { get; set; }
    public TransactionStatus Status { get; set; }
    public long Amount { get; set; }
    public string BankCode { get; set; }
    public string BankTranNo { get; set; }
    public string CardType { get; set; }
    public DateTime PayDate { get; set; }
    public string TransactionNo { get; set; }
    public string TransactionStatus { get; set; }
    public TransactionType Type { get; set; }

    //Appointment
    /* public GetAppointmentResponse Appointment { get; set; }*/
    public GetAppointmentOfTransactionResponse Appointment { get; set; }

}