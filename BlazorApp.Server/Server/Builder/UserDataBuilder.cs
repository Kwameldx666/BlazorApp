using BlazorApp.Models.Enums;
using BlazorApp.Models;

namespace BlazorApp.Server.Builder
{
    public class UserDataBuilder : IBuilder<UserData>
    {
        private  UserData _user = new();

        public UserDataBuilder SetUsername(string username)
        {
            _user.Username = username;
            return this;
        }

        public UserDataBuilder SetEmail(string email)
        {
            _user.Email = email;
            return this;
        }

        public UserDataBuilder SetPassword(string password)
        {
            _user.Password = password;
            return this;
        }

        public UserDataBuilder SetRole(Role role)
        {
            _user.Roles = role;
            return this;
        }

        public UserDataBuilder SetPhoneNumber(string phone)
        {
            _user.PhoneNumber = phone;
            return this;
        }

        public UserData Build()
        {
            _user.FirstRegisterTime = DateTime.Now;
            var result = _user;
            Reset(); // Сбрасываем состояние строителя
            return result;
        }

        public void Reset()
        {
            _user = new UserData();
        }
    }
}
