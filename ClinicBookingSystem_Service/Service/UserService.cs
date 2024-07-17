﻿using AutoMapper;
using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Repository.IRepositories;
using ClinicBookingSystem_Service.Common.Utils;
using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Enums;
using ClinicBookingSystem_Service.Models.Request.User;
using ClinicBookingSystem_Service.Models.Response.Customer;
using ClinicBookingSystem_Service.Models.Response.User;
using System.Security.Policy;

namespace ClinicBookingSystem_Service.Service;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly HashPassword _hash;
    private readonly CheckPassword _checkPassword;
    public UserService(IUnitOfWork unitOfWork, IMapper mapper, HashPassword hash, CheckPassword checkPassword)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _hash = hash;
        _checkPassword = checkPassword;
    }

    public async Task<BaseResponse<GetMyProfileResponse>> GetMyProfile(int userId)
    {
        User user = await _unitOfWork.UserRepository.GetMyProfile(userId);
        var result = _mapper.Map<GetMyProfileResponse>(user);
        return new BaseResponse<GetMyProfileResponse>("Get my profile success", StatusCodeEnum.OK_200, result);
    }



    public async Task<BaseResponse<ChangePasswordResponse>> ChangePassword(int userId, string oldPassword, string newPassword)
    {
        User user = await _unitOfWork.UserRepository.GetMyProfile(userId);
        oldPassword = _hash.EncodePassword(oldPassword);
        if (oldPassword != user.Password)
        {
            if (user.Role.Name != "CUSTOMER")
            {
                if (!_checkPassword.ValidPassword(newPassword))
                {
                    return new BaseResponse<ChangePasswordResponse>("Old password is not correct\nPassword has at least 1 digit, 1 capital letter and 1 special character", StatusCodeEnum.BadRequest_400);
                }
            }
            return new BaseResponse<ChangePasswordResponse>("Old password is not correct", StatusCodeEnum.BadRequest_400);
        }
        if(user.Role.Name != "CUSTOMER")
        {
            if (!_checkPassword.ValidPassword(newPassword))
            {
                return new BaseResponse<ChangePasswordResponse>("Password has at least 1 digit, 1 capital letter and 1 special character", StatusCodeEnum.BadRequest_400);
            }
        }
        newPassword = _hash.EncodePassword(newPassword);
        user.Password = newPassword;
        await _unitOfWork.CustomerRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();
        return new BaseResponse<ChangePasswordResponse>("Change password succesfully", StatusCodeEnum.OK_200);

    }








































































    /*
     *  Quan trọng: KHÔNG CẦN THIẾT KHAI BÁO CÁC HÀM THÊM, TẠO, XÓA, SỬA TRONG DAO MÀ CHỈ CẦN EXTEND BASEDAO<>.
     * 
     *  Về logic xử lý thì UserService sẽ gọi các phương thức từ UserRepository thông qua IUnitOfWork
     *  và thực hiện các thao tác như thêm, sửa, xóa, lấy dữ liệu từ database
     *  và trả về kết quả cho UserController.
     *
     *  Tuy nhiên, Repository không cần gọi các hàm thêm, tạo, xóa, sửa từ DAO nữa
     *  và DAO cũng không cần code các hàm thêm, tạo, xóa, sửa, thay vào đó extend BaseDAO<> để kế
     *  thừa các hàm thêm, tạo, xóa, sửa từ BaseDAO<>: ví dụ userDAO sẽ extend BaseDAO<User>, roleDAO sẽ extend BaseDAO<Role>
     *  và chỉ cần implement các hàm riêng của DAO mà không có trong BaseDAO<>, thường là các hàm liên quan tới
     *  join nhiều bảng phức tạp, đặc biêt là các hàm xử lý nghiệp vụ.
     *
     *  Cần tham khảo thứ tự project chạy từ Controller -> Service -> Repository -> DAO
     *  khởi tạo các repository trong UnitOfWork, sử dụng IUnitOfWork trong Service để gọi các repository
     *  khi các repository thực thi thành công thì goi _unitOfWork.SaveChangesAsync() để lưu thay đổi vào database
     *
     *  Cần tham khảo cách sử dụng AutoMapper để map giữa các entities và các model Request, Response
     *  Cần tham khảo cách sử dụng BaseResponse để trả về kết quả cho Controller
     *  Cần tham khảo cách sử dụng StatusCodeEnum để trả về mã trạng thái HTTP
     *  Cần tham khảo cách sử dụng các model Request và Response để truyền dữ liệu giữa Controller và Service
     *  BaseResponse là generic class, có thể trả về dữ liệu kiểu T, có thể trả về dữ liệu kiểu IEnumerable<T>
     *  nếu dữ liệu trả về là một list.
     *
     *  Quan trọng: KHÔNG CẦN THIẾT KHAI BÁO CÁC HÀM THÊM, TẠO, XÓA, SỬA TRONG DAO MÀ CHỈ CẦN EXTEND BASEDAO<>.
     *
     *  Bên dưới là ví dụ về cách sử dụng UserService để thêm, sửa, xóa, lấy dữ liệu từ database
     *  theo 2 cách: sử dụng hàm lấy từ BaseDAO và hàm tự viết trong UserDAO => Kết quả như nhau
     *  => chỉ cần sử dụng BaseDAO để thêm, sửa, xóa, lấy dữ liệu từ database để tránh trùng lặp code
     *  
     */




    /*//--------------------------------------------------------------------------------//
    //Create User
    //Create a new user as repository
    public async Task<BaseResponse<CreateNewUserResponse>> CreateUser(CreateNewUserRequest request)
    {
        // await _unitOfWork.UserRepository.CreateUser(user);
        // await _unitOfWork.SaveChangesAsync();
        User userCreateData = _mapper.Map<User>(request);
        var user = await _unitOfWork.UserRepository.CreateUser(userCreateData);
        await _unitOfWork.SaveChangesAsync();
        var response = _mapper.Map<CreateNewUserResponse>(user);
        return new BaseResponse<CreateNewUserResponse>("Create user as repository success", StatusCodeEnum.Created_201, response);
    }
    
    //Create a new User as Base
    public async Task<BaseResponse<CreateNewUserResponse>> CreateUserFromBase(CreateNewUserRequest request)
    {
        User user = _mapper.Map<User>(request);
        await _unitOfWork.UserRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
        var response = _mapper.Map<CreateNewUserResponse>(user);
        return new BaseResponse<CreateNewUserResponse>("Create user as base success", StatusCodeEnum.Created_201, response);
    }
    //--------------------------------------------------------------------------------//
    
    
    
    //--------------------------------------------------------------------------------//
    //Get All User
    //Get All User as repository
    public async Task<BaseResponse<IEnumerable<GetAllUserResponse>>> GetAllUser()
    {
        IEnumerable<User> users = await _unitOfWork.UserRepository.GetAllUser();
        var user = _mapper.Map<IEnumerable<GetAllUserResponse>>(users);
        return new BaseResponse<IEnumerable<GetAllUserResponse>>("Get all user as repository success", StatusCodeEnum.OK_200, user);
    }
    
    //Get All User as Base
    public async Task<BaseResponse<IEnumerable<GetAllUserResponse>>> GetAllUserFromBase()
    {
        IEnumerable<User> users = await _unitOfWork.UserRepository.GetAllAsync();
        var user = _mapper.Map<IEnumerable<GetAllUserResponse>>(users);
        return new BaseResponse<IEnumerable<GetAllUserResponse>>("Get all user as base success", StatusCodeEnum.OK_200, user);
    }
    //--------------------------------------------------------------------------------//

    
    
    //--------------------------------------------------------------------------------//
    //Get User By Id
    //Get User By Id as repository
    public async Task<BaseResponse<GetUserByIdResponse>> GetUserById(int id)
    {
        User user = await _unitOfWork.UserRepository.GetUserById(id);
        var result = _mapper.Map<GetUserByIdResponse>(user);
        return new BaseResponse<GetUserByIdResponse>("Get user by id as repository success", StatusCodeEnum.OK_200, result);
    }
    
    //Get User By Id as Base
    public async Task<BaseResponse<GetUserByIdResponse>> GetUserByIdFromBase(int id)
    {
        User user = await _unitOfWork.UserRepository.GetByIdAsync(id);
        var result = _mapper.Map<GetUserByIdResponse>(user);
        return new BaseResponse<GetUserByIdResponse>("Get user by id as base success", StatusCodeEnum.OK_200, result);
    }
    //--------------------------------------------------------------------------------//
    
    
    
    //--------------------------------------------------------------------------------//
    //Delete User
    //Delete User as repository
    public async Task<BaseResponse<DeleteUserResponse>> DeleteUser(int id)
    {
       User user = await _unitOfWork.UserRepository.DeleteUser(id);
       await _unitOfWork.SaveChangesAsync();
       var result = _mapper.Map<DeleteUserResponse>(user);
       return new BaseResponse<DeleteUserResponse>("Delete user as repository success", StatusCodeEnum.OK_200, result);
    }
    
    //Delete User as Base
    public async Task<BaseResponse<DeleteUserResponse>> DeleteUserFromBase(int id)
    {
        User user = await _unitOfWork.UserRepository.GetByIdAsync(id);
        await _unitOfWork.UserRepository.DeleteAsync(user);
        await _unitOfWork.SaveChangesAsync();
        var result = _mapper.Map<DeleteUserResponse>(user);
        return new BaseResponse<DeleteUserResponse>("Delete user as base success", StatusCodeEnum.OK_200, result);
    }
    //--------------------------------------------------------------------------------//
    
    
    //--------------------------------------------------------------------------------//
    //Update User
    //Update User as repository
    public async Task<BaseResponse<UpdateUserResponse>> UpdateUser(int id, UpdateUserRequest request)
    {
        User user = await _unitOfWork.UserRepository.GetUserById(id);
        _mapper.Map(request, user);
        await _unitOfWork.UserRepository.UpdateUser(user);
        await _unitOfWork.SaveChangesAsync();
        var result = _mapper.Map<UpdateUserResponse>(user);
        return new BaseResponse<UpdateUserResponse>("Update user as repository success", StatusCodeEnum.OK_200, result);
    }
    
    //Update User as Base
    public async Task<BaseResponse<UpdateUserResponse>> UpdateUserFromBase(int id, UpdateUserRequest request)
    {
        User user = await _unitOfWork.UserRepository.GetByIdAsync(id);
        _mapper.Map(request, user);
        await _unitOfWork.UserRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();
        var result = _mapper.Map<UpdateUserResponse>(user);
        return new BaseResponse<UpdateUserResponse>("Update user as base success", StatusCodeEnum.OK_200, result);
    }
    //--------------------------------------------------------------------------------//
    */

}