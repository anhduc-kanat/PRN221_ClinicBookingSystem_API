﻿using ClinicBookingSystem_BusinessObject.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClinicBookingSystem_BusinessObject.Entities;

public class BusinessService : BaseEntities
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public int? ExpectedDurationInMinute { get; set; }
    
    public ServiceType ServiceType { get; set; } = ServiceType.Examination;

    public long Price { get; set; } = 0;
    //Appointment
    public ICollection<Appointment>? Appointments { get; set; }
    //Order
    public ICollection<Order>? Orders { get; set; }
    //User
    public ICollection<User>? Users { get; set; }


}