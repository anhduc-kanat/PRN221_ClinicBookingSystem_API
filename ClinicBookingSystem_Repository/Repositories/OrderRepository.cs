using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_DataAccessObject;
using ClinicBookingSystem_Repository.BaseRepositories;
using ClinicBookingSystem_Repository.IRepositories;

namespace ClinicBookingSystem_Repository.Repositories;

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    private readonly OrderDAO _orderDAO;
    public OrderRepository(OrderDAO orderDAO) : base(orderDAO)
    {
        _orderDAO = orderDAO;
    }
}