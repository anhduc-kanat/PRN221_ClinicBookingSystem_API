﻿using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Repository.IBaseRepository;

namespace ClinicBookingSystem_Repository.IRepositories;

public interface IUserProfileRepository : IBaseRepository<UserProfile>
{
       Task<IEnumerable<UserProfile>> GetUserProfilesByUser(string phone);
    

}