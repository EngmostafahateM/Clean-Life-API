using Clean_Life_API.ApplicationDbContext;
using Clean_Life_API.Models;
using Clean_Life_API.Repo.repointerface;
using Microsoft.EntityFrameworkCore;

namespace Clean_Life_API.Repo.reboclass
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        // دالة للتحقق من وجود المستخدم
        public async Task<bool> UserExistsAsync(string email, string username)
        {
            return await _context.Users.AnyAsync(u => u.Email == email || u.Username == username);
        }

        // دالة لتسجيل مستخدم جديد
        public async Task<user> RegisterUserAsync(user user, string password)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password); // Hashing كلمة المرور
            user.PasswordHash = hashedPassword;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        // دالة للتحقق من كلمة المرور وتسجيل الدخول
        public async Task<user> AuthenticateAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null; // كلمة المرور خاطئة أو المستخدم غير موجود
            }
            return user; // إرجاع المستخدم إذا كانت كلمة المرور صحيحة
        }

        // دالة لتحديث كلمة المرور
        public async Task<bool> UpdatePasswordAsync(string email, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<user> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<user> UpdateUserAsync(user user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
