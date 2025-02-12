using Clean_Life_API.Models;

namespace Clean_Life_API.Repo.repointerface
{
    public interface IUserRepository
    {
        Task<bool> UserExistsAsync(string email, string username); // التحقق من وجود المستخدم
        Task<user> RegisterUserAsync(user user, string password); // تسجيل مستخدم جديد
        Task<user> AuthenticateAsync(string username, string password); // التحقق من كلمة المرور للمستخدم
        Task<bool> UpdatePasswordAsync(string email, string newPassword); // تحديث كلمة المرور
        Task<user> GetUserByIdAsync(int userId);
        Task<user> UpdateUserAsync(user user);

    }
}
