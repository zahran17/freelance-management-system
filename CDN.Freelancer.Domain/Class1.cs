using System.Collections.Generic;
using System.Threading.Tasks;

namespace CDN.Freelancer.Domain
{
    public class Freelancer
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsArchived { get; set; }
        public ICollection<Skillset> Skillsets { get; set; }
        public ICollection<Hobby> Hobbies { get; set; }
    }

    public class Skillset
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FreelancerId { get; set; }
        public Freelancer Freelancer { get; set; }
    }

    public class Hobby
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FreelancerId { get; set; }
        public Freelancer Freelancer { get; set; }
    }

    public interface IFreelancerRepository
    {
        Task<Freelancer> GetByIdAsync(int id);
        Task<IEnumerable<Freelancer>> GetAllAsync();
        Task<IEnumerable<Freelancer>> GetAllAsync(int pageNumber, int pageSize);
        Task<int> GetTotalCountAsync();
        Task<IEnumerable<Freelancer>> SearchAsync(string query);
        Task<IEnumerable<Freelancer>> SearchAsync(string query, int pageNumber, int pageSize);
        Task<int> GetSearchCountAsync(string query);
        Task AddAsync(Freelancer freelancer);
        Task UpdateAsync(Freelancer freelancer);
        Task DeleteAsync(int id);
        Task ArchiveAsync(int id);
        Task UnarchiveAsync(int id);
    }
}
