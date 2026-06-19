using Microsoft.EntityFrameworkCore;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Models;

public class InvitationRepository 
{
    private readonly ApplicationDbContext _db;

    public InvitationRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _db.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<Role?> GetRoleByIdAsync(int roleId)
    {
        return await _db.Roles.FindAsync(roleId);
    }

    public async Task AddUserAsync(User user)
    {
        await _db.Users.AddAsync(user);
    }

    public async Task AddPasswordResetAsync(PasswordReset passwordReset)
    {
        await _db.passwordResets.AddAsync(passwordReset);
    }
    public async Task<Role> getRoleWithItsPermissions(int roleId)
    {
        return await _db.Roles.Where(e=>e.Id==roleId).Include(e => e.RolePermissions).ThenInclude(e => e.Permission).FirstOrDefaultAsync();
    } 

    public async Task addRoleAsync(Role role)
    {
        await _db.Roles.AddAsync(role);
    }

    public async Task AddRolePermissionsAsync(List<RolePermission> rolePermission)
    {
        await _db.RolePermissions.AddRangeAsync(rolePermission);
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}