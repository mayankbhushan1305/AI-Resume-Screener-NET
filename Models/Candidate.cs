namespace ResumeScreeningAPI.Models;

public class Candidate
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Skills { get; set; } = string.Empty;

    public int ExperienceYears { get; set; }

    public string ResumeFileName { get; set; } = string.Empty;

    public List<Resume> Resumes { get; set; } = new();
}