using CDN.Freelancer.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CDN.Freelancer.Application
{
    public class FreelancerService
    {
        private readonly IFreelancerRepository _repository;
        public FreelancerService(IFreelancerRepository repository)
        {
            _repository = repository;
        }

        public Task<CDN.Freelancer.Domain.Freelancer> GetByIdAsync(int id) => _repository.GetByIdAsync(id);
        public Task<IEnumerable<CDN.Freelancer.Domain.Freelancer>> GetAllAsync() => _repository.GetAllAsync();
        public Task<IEnumerable<CDN.Freelancer.Domain.Freelancer>> SearchAsync(string query) => _repository.SearchAsync(query);
        public Task AddAsync(CDN.Freelancer.Domain.Freelancer freelancer) => _repository.AddAsync(freelancer);
        public Task UpdateAsync(CDN.Freelancer.Domain.Freelancer freelancer) => _repository.UpdateAsync(freelancer);
        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
        public Task ArchiveAsync(int id) => _repository.ArchiveAsync(id);
        public Task UnarchiveAsync(int id) => _repository.UnarchiveAsync(id);

        public async Task<PaginatedResult<CDN.Freelancer.Domain.Freelancer>> GetAllPaginatedAsync(int pageNumber, int pageSize)
        {
            var items = await _repository.GetAllAsync(pageNumber, pageSize);
            var totalCount = await _repository.GetTotalCountAsync();
            return new PaginatedResult<CDN.Freelancer.Domain.Freelancer>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<PaginatedResult<CDN.Freelancer.Domain.Freelancer>> SearchPaginatedAsync(string query, int pageNumber, int pageSize)
        {
            var items = await _repository.SearchAsync(query, pageNumber, pageSize);
            var totalCount = await _repository.GetSearchCountAsync(query);
            return new PaginatedResult<CDN.Freelancer.Domain.Freelancer>(items, totalCount, pageNumber, pageSize);
        }
    }
} 