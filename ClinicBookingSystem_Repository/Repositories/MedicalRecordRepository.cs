using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_DataAccessObject;
using ClinicBookingSystem_Repository.BaseRepositories;
using ClinicBookingSystem_Repository.IRepositories;

namespace ClinicBookingSystem_Repository.Repositories;

public class MedicalRecordRepository : BaseRepository<MedicalRecord>, IMedicalRecordRepository
{
    private readonly MedicalRecordDAO _medicalRecordDAO;
    public MedicalRecordRepository(MedicalRecordDAO medicalRecordDAO) : base(medicalRecordDAO)
    {
        _medicalRecordDAO = medicalRecordDAO;
    }
}