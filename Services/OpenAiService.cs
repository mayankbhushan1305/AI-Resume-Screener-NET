namespace ResumeScreeningAPI.Services;

public class OpenAiService
{
    public OpenAiService(IConfiguration configuration)
    {
        // Prevents initialization crash since we aren't using a real key
    }

    public async Task<string> ScreenResumeAsync(string resumeText, string jobDescription)
    {
        // Delays for 2 seconds to simulate a live AI computation window
        await Task.Delay(2000);

        return "Score: 92/100\n\n" +
               "Reason: The candidate demonstrates exceptional alignment with core engineering metrics. Practical experience with target framework components matches desired infrastructure deliverables cleanly.\n\n" +
               "Key Highlights:\n" +
               "- Strong baseline proficiency in language requirements\n" +
               "- Front-end architecture stack implementation details match parameters\n" +
               "- Technical dataset tracking aligns cleanly with database requirements";
    }
}