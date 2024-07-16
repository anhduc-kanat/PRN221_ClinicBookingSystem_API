﻿using System.ComponentModel.DataAnnotations;
using ClinicBookingSystem_BusinessObject.Enums;

namespace ClinicBookingSystem_BusinessObject.Entities;

public class Appointment : BaseEntities
{

    public DateTime Date { get; set; }
    public bool? IsApproved { get; set; }
    public AppointmentStatus Status { get; set; }
    public string? Description { get; set; }
    public string? FeedBack {get; set; }
    public string? UserTreatmentName { get; set; }
    public string? UserAccountName { get; set; }
    public string? StaffAccountName { get; set; }
    public bool? IsReExam { get; set; }
    public bool? IsClinicalExamPaid { get; set; }
    public bool? IsFullyPaid { get; set; }
    //User
    public ICollection<User>? Users { get; set; }
    public int? UserAccountId { get; set; }
    public int? UserTreatmentId { get; set; }
    public int? StaffAccountId { get; set; }
    //AppointmentService
    public ICollection<AppointmentBusinessService>? AppointmentBusinessServices { get; set; }
    //Slot
    public Slot? Slot { get; set; }
    //Trnasaction
    public IEnumerable<Transaction>? Transactions { get; set; }
    //Result
    public Result? Result { get; set; }
}