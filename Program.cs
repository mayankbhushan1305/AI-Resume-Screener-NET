using Microsoft.EntityFrameworkCore;
using ResumeScreeningAPI.Data;
using ResumeScreeningAPI.Models;
using ResumeScreeningAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Database Connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Services
builder.Services.AddScoped<ResumeService>();
builder.Services.AddScoped<OpenAiService>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Frontend Static Files (HTML/JS)
app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Resume Upload and Text Extraction Endpoint
app.MapPost("/api/resumes/upload", async (IFormFile file, int candidateId, ResumeService resumeService, ApplicationDbContext db) =>
{
    if (file == null || file.Length == 0)
        return Results.BadRequest("No file uploaded.");

    if (!file.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
        return Results.BadRequest("Only PDF files are supported.");

    using var stream = file.OpenReadStream();
    string extractedText = resumeService.ExtractTextFromPdf(stream);

    var resume = new Resume
    {
        FileName = file.FileName,
        FilePath = "uploads/" + file.FileName,
        RawText = extractedText,
        CandidateId = candidateId
    };

    db.Resumes.Add(resume);
    await db.SaveChangesAsync();

    return Results.Ok(new { Message = "Resume processed successfully", ResumeId = resume.Id, RawText = extractedText });
}).DisableAntiforgery();

// AI Screening Endpoint
app.MapPost("/api/resumes/screen", async (int resumeId, string jobDescription, OpenAiService openAiService, ApplicationDbContext db) =>
{
    var resume = await db.Resumes.FindAsync(resumeId);
    if (resume == null) return Results.NotFound("Resume not found");

    string aiResult = await openAiService.ScreenResumeAsync(resume.RawText, jobDescription);

    return Results.Ok(new { Result = aiResult });
});

// Get All Candidates from Database
app.MapGet("/api/candidates", async (ApplicationDbContext db) =>
{
    var candidates = await db.Candidates.ToListAsync();
    return Results.Ok(candidates);
});

// Add Candidate
app.MapPost("/api/candidates", async (ApplicationDbContext db, Candidate candidate) =>
{
    db.Candidates.Add(candidate);
    await db.SaveChangesAsync();

    return Results.Created($"/api/candidates/{candidate.Id}", candidate);
});

app.Run();