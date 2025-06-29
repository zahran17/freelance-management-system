using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CDN.Freelancer.Infrastructure;

public class FreelancerDbContext : DbContext
{
    public FreelancerDbContext(DbContextOptions<FreelancerDbContext> options) : base(options) { }

    public DbSet<CDN.Freelancer.Domain.Freelancer> Freelancers { get; set; }
    public DbSet<CDN.Freelancer.Domain.Skillset> Skillsets { get; set; }
    public DbSet<CDN.Freelancer.Domain.Hobby> Hobbies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CDN.Freelancer.Domain.Freelancer>()
            .HasMany(f => f.Skillsets)
            .WithOne(s => s.Freelancer)
            .HasForeignKey(s => s.FreelancerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CDN.Freelancer.Domain.Freelancer>()
            .HasMany(f => f.Hobbies)
            .WithOne(h => h.Freelancer)
            .HasForeignKey(h => h.FreelancerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class FreelancerRepository : CDN.Freelancer.Domain.IFreelancerRepository
{
    private readonly FreelancerDbContext _context;
    public FreelancerRepository(FreelancerDbContext context)
    {
        _context = context;
    }

    public async Task<CDN.Freelancer.Domain.Freelancer> GetByIdAsync(int id)
    {
        return await _context.Freelancers
            .Include(f => f.Skillsets)
            .Include(f => f.Hobbies)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<IEnumerable<CDN.Freelancer.Domain.Freelancer>> GetAllAsync()
    {
        return await _context.Freelancers
            .Include(f => f.Skillsets)
            .Include(f => f.Hobbies)
            .ToListAsync();
    }

    public async Task<IEnumerable<CDN.Freelancer.Domain.Freelancer>> GetAllAsync(int pageNumber, int pageSize)
    {
        return await _context.Freelancers
            .Include(f => f.Skillsets)
            .Include(f => f.Hobbies)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Freelancers.CountAsync();
    }

    public async Task<IEnumerable<CDN.Freelancer.Domain.Freelancer>> SearchAsync(string query)
    {
        return await _context.Freelancers
            .Include(f => f.Skillsets)
            .Include(f => f.Hobbies)
            .Where(f => f.Username.Contains(query) || f.Email.Contains(query))
            .ToListAsync();
    }

    public async Task<IEnumerable<CDN.Freelancer.Domain.Freelancer>> SearchAsync(string query, int pageNumber, int pageSize)
    {
        return await _context.Freelancers
            .Include(f => f.Skillsets)
            .Include(f => f.Hobbies)
            .Where(f => f.Username.Contains(query) || f.Email.Contains(query))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetSearchCountAsync(string query)
    {
        return await _context.Freelancers
            .Where(f => f.Username.Contains(query) || f.Email.Contains(query))
            .CountAsync();
    }

    public async Task AddAsync(CDN.Freelancer.Domain.Freelancer freelancer)
    {
        await _context.Freelancers.AddAsync(freelancer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CDN.Freelancer.Domain.Freelancer freelancer)
    {
        _context.Freelancers.Update(freelancer);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var freelancer = await _context.Freelancers.FindAsync(id);
        if (freelancer != null)
        {
            _context.Freelancers.Remove(freelancer);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ArchiveAsync(int id)
    {
        var freelancer = await _context.Freelancers.FindAsync(id);
        if (freelancer != null)
        {
            freelancer.IsArchived = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task UnarchiveAsync(int id)
    {
        var freelancer = await _context.Freelancers.FindAsync(id);
        if (freelancer != null)
        {
            freelancer.IsArchived = false;
            await _context.SaveChangesAsync();
        }
    }
}

// Repository interfaces and implementations can be added here or in separate files for larger projects.

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<FreelancerDbContext>(options =>
            options.UseSqlite(connectionString));
        services.AddScoped<CDN.Freelancer.Domain.IFreelancerRepository, FreelancerRepository>();
        return services;
    }
}
