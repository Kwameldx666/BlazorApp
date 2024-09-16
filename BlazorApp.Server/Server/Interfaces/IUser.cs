using BlazorApp.Models;
using BlazorApp.Models.Enums;
using BlazorApp.Models.Response;

namespace BlazorApp.Interfaces
{
    public interface IUser
    {
        // Retrieves all users
        IEnumerable<UserData> GetAllUsers();

        // Asynchronously retrieves a user by their identifier
        Task<UserResponse> GetOneUserByIdAsync(Guid Id);

        // Adds a new user
        Task<UserResponse> Register(RegisterData user);

        // Asynchronously updates an existing user
        Task<UserResponse> UpdateUser(UserData UserNew);

        // Deletes a user by their identifier
        UserResponse DeleteUser(Guid Id);

        // Finds users by their name
        IEnumerable<UserData> FindUsersByName(string name);

        // Authenticates a user using their username and password
        Task<UserResponse> AuthenticateUser(LoginModel data);

        // Retrieves roles assigned to a specific user
        Role GetUserRole(Guid Id);

        // Changes the password of a user identified by their email
        UserResponse ChangeUserPassword(string email);

        // Checks if a user exists by their identifier
        UserResponse IsUserExists(Guid Id);

        // Assigns a role to a user
        UserResponse AssignRoleToUser(Guid userId, Role role);

        // Deactivates a user by their identifier
        UserResponse DeactivateUser(Guid Id);

        // Retrieves user data based on login information
        UserResponse GetUserData(LoginModel loginData);

        // Asynchronously retrieves user data by cookie value
        Task<UserData> GetUserByCookie(string value);

        // Logs out the user and clears session information
        ResponseMain UserLogout();

        // Asynchronously updates the address for a user
        Task<UserResponse> UpdateAddress(UserData delivery, DeliveryAddress data);

        // Asynchronously retrieves a delivery address by user identifier
        Task<DeliveryResponse> GetOneAddressByUserIdAsync(Guid userId);
    }
}
