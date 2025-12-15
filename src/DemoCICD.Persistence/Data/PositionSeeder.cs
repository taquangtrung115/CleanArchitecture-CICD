using DemoCICD.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace DemoCICD.Persistence.Data;

public static class PositionSeeder
{
    public static async Task SeedAdditionalPositionsAsync(ApplicationDbContext context)
    {
        // Check how many positions already exist
        var existingCount = await context.Positions.AnyAsync();
        
        if (existingCount)
            return; // Already have enough data

        var additionalPositions = new List<Position>
        {
            new Position
            {
                Id = Guid.NewGuid(),
                Name = "Junior Developer",
                Description = "Entry level software developer",
                Code = "JR_DEV",
                Level = 5,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            },
            new Position
            {
                Id = Guid.NewGuid(),
                Name = "Lead Developer",
                Description = "Team lead for development projects",
                Code = "LEAD_DEV",
                Level = 3,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            },
            new Position
            {
                Id = Guid.NewGuid(),
                Name = "DevOps Engineer",
                Description = "Infrastructure and deployment specialist",
                Code = "DEVOPS",
                Level = 4,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            },
            new Position
            {
                Id = Guid.NewGuid(),
                Name = "QA Engineer",
                Description = "Quality assurance and testing specialist",
                Code = "QA_ENG",
                Level = 4,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            },
            new Position
            {
                Id = Guid.NewGuid(),
                Name = "Business Analyst",
                Description = "Business requirements and process analyst",
                Code = "BA",
                Level = 4,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            },
            new Position
            {
                Id = Guid.NewGuid(),
                Name = "Product Owner",
                Description = "Product development and roadmap owner",
                Code = "PO",
                Level = 3,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            },
            new Position
            {
                Id = Guid.NewGuid(),
                Name = "Scrum Master",
                Description = "Agile process facilitator and team coach",
                Code = "SM",
                Level = 4,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            },
            new Position
            {
                Id = Guid.NewGuid(),
                Name = "UI/UX Designer",
                Description = "User interface and experience designer",
                Code = "UIUX",
                Level = 4,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            }
        };

        context.Positions.AddRange(additionalPositions);
        await context.SaveChangesAsync();
    }
}
