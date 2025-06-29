using FluentValidation;

namespace CDN.Freelancer.Application;

public class CreateFreelancerDtoValidator : AbstractValidator<CreateFreelancerDto>
{
    public CreateFreelancerDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required")
            .Length(3, 50).WithMessage("Username must be between 3 and 50 characters")
            .Matches("^[a-zA-Z0-9_]+$").WithMessage("Username can only contain letters, numbers, and underscores");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be a valid email address")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Phone number must be a valid international format");

        RuleFor(x => x.Skillsets)
            .NotNull().WithMessage("Skillsets cannot be null")
            .Must(skillsets => skillsets.Count <= 10).WithMessage("Cannot have more than 10 skillsets");

        RuleForEach(x => x.Skillsets)
            .SetValidator(new SkillsetDtoValidator());

        RuleFor(x => x.Hobbies)
            .NotNull().WithMessage("Hobbies cannot be null")
            .Must(hobbies => hobbies.Count <= 10).WithMessage("Cannot have more than 10 hobbies");

        RuleForEach(x => x.Hobbies)
            .SetValidator(new HobbyDtoValidator());
    }
}

public class UpdateFreelancerDtoValidator : AbstractValidator<UpdateFreelancerDto>
{
    public UpdateFreelancerDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required")
            .Length(3, 50).WithMessage("Username must be between 3 and 50 characters")
            .Matches("^[a-zA-Z0-9_]+$").WithMessage("Username can only contain letters, numbers, and underscores");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be a valid email address")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Phone number must be a valid international format");

        RuleFor(x => x.Skillsets)
            .NotNull().WithMessage("Skillsets cannot be null")
            .Must(skillsets => skillsets.Count <= 10).WithMessage("Cannot have more than 10 skillsets");

        RuleForEach(x => x.Skillsets)
            .SetValidator(new SkillsetDtoValidator());

        RuleFor(x => x.Hobbies)
            .NotNull().WithMessage("Hobbies cannot be null")
            .Must(hobbies => hobbies.Count <= 10).WithMessage("Cannot have more than 10 hobbies");

        RuleForEach(x => x.Hobbies)
            .SetValidator(new HobbyDtoValidator());
    }
}

public class SkillsetDtoValidator : AbstractValidator<SkillsetDto>
{
    public SkillsetDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Skillset name is required")
            .Length(1, 50).WithMessage("Skillset name must be between 1 and 50 characters")
            .Matches("^[a-zA-Z0-9\\s\\-_\\.]+$").WithMessage("Skillset name can only contain letters, numbers, spaces, hyphens, underscores, and dots");
    }
}

public class HobbyDtoValidator : AbstractValidator<HobbyDto>
{
    public HobbyDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Hobby name is required")
            .Length(1, 50).WithMessage("Hobby name must be between 1 and 50 characters")
            .Matches("^[a-zA-Z0-9\\s\\-_\\.]+$").WithMessage("Hobby name can only contain letters, numbers, spaces, hyphens, underscores, and dots");
    }
} 