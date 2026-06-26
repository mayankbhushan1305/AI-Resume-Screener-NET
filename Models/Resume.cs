namespace ResumeScreeningAPI.Models;

public class Resume
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string RawText { get; set; } = string.Empty;
    
    // Foreign Key linking back to the Candidate
    public int CandidateId { get; set; }
}