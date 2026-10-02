using Grpc.Core;
using Protos.User;

namespace Backend.Grpc.User;

public class UserController : Protos.User.UserController.UserControllerBase
{
    public async override Task<UserListResponse> List(UserListRequest request, ServerCallContext context)
    {
        return new UserListResponse();
    }
}