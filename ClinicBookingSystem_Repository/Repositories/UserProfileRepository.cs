﻿using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_DataAccessObject;
using ClinicBookingSystem_Repository.BaseRepositories;
using ClinicBookingSystem_Repository.IRepositories;

namespace ClinicBookingSystem_Repository.Repositories;

public class UserProfileRepository : BaseRepository<UserProfile>, IUserProfileRepository
{
    private readonly UserProfileDAO _userProfileDAO;
    public UserProfileRepository(UserProfileDAO userProfileDAO) : base(userProfileDAO)
    {
        _userProfileDAO = userProfileDAO;
    }

    public async Task<IEnumerable<UserProfile>> GetUserProfilesByUser(string phone)
    {
        return await _userProfileDAO.GetUserProfilesByUser(phone);
    }
    public async Task<IEnumerable<UserProfile>> GetUserProfileById(int userId)
    {
        return await _userProfileDAO.GetUserProfileById(userId);
    }

    public async Task<IEnumerable<UserProfile>> GetUserProfileByUserAccountId(int userId)
    {
        return await _userProfileDAO.GetUserProfileByUserAccountId(userId);
    }
}