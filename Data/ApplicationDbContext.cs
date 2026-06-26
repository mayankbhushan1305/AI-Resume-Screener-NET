using Microsoft.EntityFrameworkCore;
using ResumeScreeningAPI.Models;

namespace ResumeScreeningAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Candidate> Candidates => Set<Candidate>();

    public DbSet<Resume> Resumes => Set<Resume>();
}