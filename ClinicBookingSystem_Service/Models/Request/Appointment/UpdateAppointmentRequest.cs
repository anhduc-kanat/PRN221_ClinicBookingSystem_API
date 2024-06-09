﻿namespace ClinicBookingSystem_Service.Models.Request.Appointment;

public class UpdateAppointmentRequest
{
    public DateTime? Date { get; set; }
    public bool? IsPeriod { get; set; } = false;
    public int? ReexamUnit { get; set; }
    public int? ReexamNumber { get; set; }
    public bool? IsTreatment { get; set; } = false;
    public int? ServiceId { get; set; }
    public int? SlotId { get; set; }
    public int? DentistId { get; set; }
    public int? PatientId { get; set; }
}