using CDN.Freelancer.Application;
using Moq;
using Xunit;

namespace CDN.Freelancer.Tests;

public class FreelancerServiceTests
{
    private readonly Mock<CDN.Freelancer.Domain.IFreelancerRepository> _mockRepository;
    private readonly FreelancerService _service;

    public FreelancerServiceTests()
    {
        _mockRepository = new Mock<CDN.Freelancer.Domain.IFreelancerRepository>();
        _service = new FreelancerService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFreelancer_WhenFreelancerExists()
    {
        // Arrange
        var expectedFreelancer = new CDN.Freelancer.Domain.Freelancer
        {
            Id = 1,
            Username = "john_doe",
            Email = "john@example.com",
            PhoneNumber = "1234567890",
            IsArchived = false,
            Skillsets = new List<CDN.Freelancer.Domain.Skillset> { new CDN.Freelancer.Domain.Skillset { Name = "C#" } },
            Hobbies = new List<CDN.Freelancer.Domain.Hobby> { new CDN.Freelancer.Domain.Hobby { Name = "Reading" } }
        };

        _mockRepository.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(expectedFreelancer);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedFreelancer.Id, result.Id);
        Assert.Equal(expectedFreelancer.Username, result.Username);
        Assert.Equal(expectedFreelancer.Email, result.Email);
        _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenFreelancerDoesNotExist()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((CDN.Freelancer.Domain.Freelancer)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
        _mockRepository.Verify(r => r.GetByIdAsync(999), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllFreelancers()
    {
        // Arrange
        var expectedFreelancers = new List<CDN.Freelancer.Domain.Freelancer>
        {
            new CDN.Freelancer.Domain.Freelancer { Id = 1, Username = "john_doe", Email = "john@example.com" },
            new CDN.Freelancer.Domain.Freelancer { Id = 2, Username = "jane_doe", Email = "jane@example.com" }
        };

        _mockRepository.Setup(r => r.GetAllAsync())
            .ReturnsAsync(expectedFreelancers);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, f => f.Username == "john_doe");
        Assert.Contains(result, f => f.Username == "jane_doe");
        _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnMatchingFreelancers()
    {
        // Arrange
        var searchQuery = "john";
        var expectedFreelancers = new List<CDN.Freelancer.Domain.Freelancer>
        {
            new CDN.Freelancer.Domain.Freelancer { Id = 1, Username = "john_doe", Email = "john@example.com" }
        };

        _mockRepository.Setup(r => r.SearchAsync(searchQuery))
            .ReturnsAsync(expectedFreelancers);

        // Act
        var result = await _service.SearchAsync(searchQuery);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("john_doe", result.First().Username);
        _mockRepository.Verify(r => r.SearchAsync(searchQuery), Times.Once);
    }

    [Fact]
    public async Task AddAsync_ShouldCallRepositoryAddMethod()
    {
        // Arrange
        var freelancer = new CDN.Freelancer.Domain.Freelancer
        {
            Username = "new_user",
            Email = "new@example.com",
            PhoneNumber = "1234567890",
            Skillsets = new List<CDN.Freelancer.Domain.Skillset> { new CDN.Freelancer.Domain.Skillset { Name = "C#" } },
            Hobbies = new List<CDN.Freelancer.Domain.Hobby> { new CDN.Freelancer.Domain.Hobby { Name = "Reading" } }
        };

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<CDN.Freelancer.Domain.Freelancer>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.AddAsync(freelancer);

        // Assert
        _mockRepository.Verify(r => r.AddAsync(It.Is<CDN.Freelancer.Domain.Freelancer>(f => 
            f.Username == "new_user" && 
            f.Email == "new@example.com")), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldCallRepositoryUpdateMethod()
    {
        // Arrange
        var freelancer = new CDN.Freelancer.Domain.Freelancer
        {
            Id = 1,
            Username = "updated_user",
            Email = "updated@example.com",
            PhoneNumber = "1234567890"
        };

        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<CDN.Freelancer.Domain.Freelancer>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(freelancer);

        // Assert
        _mockRepository.Verify(r => r.UpdateAsync(It.Is<CDN.Freelancer.Domain.Freelancer>(f => 
            f.Id == 1 && 
            f.Username == "updated_user")), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldCallRepositoryDeleteMethod()
    {
        // Arrange
        var freelancerId = 1;
        _mockRepository.Setup(r => r.DeleteAsync(freelancerId))
            .Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAsync(freelancerId);

        // Assert
        _mockRepository.Verify(r => r.DeleteAsync(freelancerId), Times.Once);
    }

    [Fact]
    public async Task ArchiveAsync_ShouldCallRepositoryArchiveMethod()
    {
        // Arrange
        var freelancerId = 1;
        _mockRepository.Setup(r => r.ArchiveAsync(freelancerId))
            .Returns(Task.CompletedTask);

        // Act
        await _service.ArchiveAsync(freelancerId);

        // Assert
        _mockRepository.Verify(r => r.ArchiveAsync(freelancerId), Times.Once);
    }

    [Fact]
    public async Task UnarchiveAsync_ShouldCallRepositoryUnarchiveMethod()
    {
        // Arrange
        var freelancerId = 1;
        _mockRepository.Setup(r => r.UnarchiveAsync(freelancerId))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UnarchiveAsync(freelancerId);

        // Assert
        _mockRepository.Verify(r => r.UnarchiveAsync(freelancerId), Times.Once);
    }
}

public class FreelancerDomainTests
{
    [Fact]
    public void Freelancer_ShouldHaveCorrectProperties()
    {
        // Arrange & Act
        var freelancer = new CDN.Freelancer.Domain.Freelancer
        {
            Id = 1,
            Username = "test_user",
            Email = "test@example.com",
            PhoneNumber = "1234567890",
            IsArchived = false,
            Skillsets = new List<CDN.Freelancer.Domain.Skillset>(),
            Hobbies = new List<CDN.Freelancer.Domain.Hobby>()
        };

        // Assert
        Assert.Equal(1, freelancer.Id);
        Assert.Equal("test_user", freelancer.Username);
        Assert.Equal("test@example.com", freelancer.Email);
        Assert.Equal("1234567890", freelancer.PhoneNumber);
        Assert.False(freelancer.IsArchived);
        Assert.NotNull(freelancer.Skillsets);
        Assert.NotNull(freelancer.Hobbies);
    }

    [Fact]
    public void Skillset_ShouldHaveCorrectProperties()
    {
        // Arrange & Act
        var skillset = new CDN.Freelancer.Domain.Skillset
        {
            Id = 1,
            Name = "C#",
            FreelancerId = 1
        };

        // Assert
        Assert.Equal(1, skillset.Id);
        Assert.Equal("C#", skillset.Name);
        Assert.Equal(1, skillset.FreelancerId);
    }

    [Fact]
    public void Hobby_ShouldHaveCorrectProperties()
    {
        // Arrange & Act
        var hobby = new CDN.Freelancer.Domain.Hobby
        {
            Id = 1,
            Name = "Reading",
            FreelancerId = 1
        };

        // Assert
        Assert.Equal(1, hobby.Id);
        Assert.Equal("Reading", hobby.Name);
        Assert.Equal(1, hobby.FreelancerId);
    }
}