namespace CDN.Freelancer.Application;

public class CreateFreelancerDto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public bool IsArchived { get; set; }
    public List<SkillsetDto> Skillsets { get; set; } = new();
    public List<HobbyDto> Hobbies { get; set; } = new();
}

public class UpdateFreelancerDto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public bool IsArchived { get; set; }
    public List<SkillsetDto> Skillsets { get; set; } = new();
    public List<HobbyDto> Hobbies { get; set; } = new();
}

public class SkillsetDto
{
    public string Name { get; set; } = string.Empty;
}

public class HobbyDto
{
    public string Name { get; set; } = string.Empty;
} 