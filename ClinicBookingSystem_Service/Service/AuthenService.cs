using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Repository.IRepositories;
using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Enums;
using ClinicBookingSystem_Service.Models.Response.Authen;
using ClinicBookingSystem_Service.Models.Utils;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using ClinicBookingSystem_Service.Models.Request.Authen;
using System.Security.Cryptography;
using System.Data;
using Azure;
namespace ClinicBookingSystem_Service.Service
{
    public class AuthenService : IAuthenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly HashPassword _hashPassword;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;


        public AuthenService( 
            IUnitOfWork unitOfWork, HashPassword hashPassword, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _hashPassword = hashPassword;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<BaseResponse<LoginResponse>> LoginWithJwtTokenAsync(LoginRequest request)
        {
            try
            {
                request.Password = _hashPassword.EncodePassword(request.Password);
                User user = await _unitOfWork.UserRepository.GetUserByPhone(request.PhoneNumber);
                if( user!=null &&  user.Password == request.Password)
                {
                    var response = await GetJwtTokenAsync(user);
                    return new BaseResponse<LoginResponse>("Succesfully", StatusCodeEnum.OK_200, response);

                }
                return new BaseResponse<LoginResponse>("User not found", StatusCodeEnum.BadRequest_400);
            }
            catch (Exception ex)
            {
                return new BaseResponse<LoginResponse>("Authen Service, Login: " + ex.Message, StatusCodeEnum.BadRequest_400);
            }
        }


        public async Task<LoginResponse> GetJwtTokenAsync(User user)
        {
            Role role = await _unitOfWork.RoleRepository.GetByIdAsync(user.Role.Id);
            DateTime timestamp = DateTime.UtcNow;
            string timestampString = timestamp.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
            var authClaims = new List<System.Security.Claims.Claim>
                    {
                        new System.Security.Claims.Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
                        new System.Security.Claims.Claim(ClaimTypes.Role, role.Name),
                        new System.Security.Claims.Claim("Timestamp", timestampString),
                        new System.Security.Claims.Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                    };
            var jwtToken = GetToken(authClaims);
            var refreshToken = GenerateRefreshToken();
            _ = int.TryParse(_configuration["JWT:RefreshTokenValidity"], out int refreshTokenValidity);
            DateTime expiredRefreshToken = DateTime.UtcNow.AddHours(refreshTokenValidity);


            string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            DateTime expired = jwtToken.ValidTo;
            Token tokenData = new Token
            {
                AccessToken = token,
                Expired = expired,
                RefreshToken = refreshToken,
                ExpiredRefreshToken = expiredRefreshToken,
                TimeStamps = timestamp,
                User = user
            };
            await _unitOfWork.TokenRepository.AddAsync(tokenData);
            await _unitOfWork.SaveChangesAsync();

            var response = _mapper.Map<LoginResponse>(tokenData);
            return response;
        }


        public async Task<BaseResponse<LoginResponse>> RenewJwtTokenAsync(RenewTokenRequest request)
        {
            var accessToken = request.AccessToken;
            var refreshToken = request.RefreshToken;
            var principal = GetClaimsPrincipal(accessToken);
            var phoneNumberClaim = principal.FindFirst(ClaimTypes.MobilePhone);
            var timestamp = principal.FindFirst("Timestamp");
            if (phoneNumberClaim == null)
            {
                return new BaseResponse<LoginResponse>("Some errors appear, please login again", StatusCodeEnum.BadRequest_400);

            }
            var user = await _unitOfWork.UserRepository.GetUserByPhone(phoneNumberClaim.Value.ToString());
            var refresh = await _unitOfWork.TokenRepository.GetTokenByTimestamp(timestamp.ToString());
            if (refreshToken != refresh.RefreshToken && refresh.ExpiredRefreshToken < DateTime.UtcNow)
            {
                return new BaseResponse<LoginResponse>("Some errors appear, please login again", StatusCodeEnum.BadRequest_400);
            }
            var response = await GetJwtTokenAsync(user);
            return new BaseResponse<LoginResponse>("User not found", StatusCodeEnum.OK_200, response);
        }

        private JwtSecurityToken GetToken(List<System.Security.Claims.Claim> authClaims) 
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            return token;
        }

        private ClaimsPrincipal GetClaimsPrincipal(string acessToken)
        {
            var tokenValidiationParameter = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"])),
                ValidateLifetime = false,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(acessToken,tokenValidiationParameter,out SecurityToken securityToken);
            return principal;
        }

        private  string GenerateRefreshToken()
        {
            var randomNumber = new Byte[64];
            var range = RandomNumberGenerator.Create();
            range.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber); 
        }

    }
    
}
