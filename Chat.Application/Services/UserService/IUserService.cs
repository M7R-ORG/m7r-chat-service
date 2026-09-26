using Chat.Application.Services.Common;
using Chat.Application.Services.UserService.Models;

namespace Chat.Application.Services.UserService;

public interface IUserService : IBaseService
{
    Task<UserServiceRegistrationResponse> RegistrationAsync(UserServiceRegistrationRequest request);
    Task<UserServiceConfirmationResponse> ConfirmationAsync(UserServiceConfirmationRequest request);
    Task<UserServiceUpdateResponse> UpdateAsync(UserServiceUpdateRequest request);
    Task<UserServiceUsersResponse> UsersAsync(UserServiceUsersRequest request);
    Task<UserServiceUserResponse> UserAsync(UserServiceUserRequest request);
    Task<UserServiceCreateUserResponse> CreateUserAsync(UserServiceCreateUserRequest request);
    Task<UserServiceUpdateUserResponse> UpdateUserAsync(UserServiceUpdateUserRequest request);
    Task<UserServiceRemoveUserResponse> RemoveUserAsync(UserServiceRemoveUserRequest request);
    Task<UserServiceBlockUserResponse> BlockUserAsync(UserServiceBlockUserRequest request);
    Task<UserServiceUnblockUserResponse> UnblockUserAsync(UserServiceUnblockUserRequest request);
}
