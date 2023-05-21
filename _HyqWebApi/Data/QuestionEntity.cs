using Newtonsoft.Json;

namespace MagneticSurvey.Data
{
    public class QuestionEntity
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public string? Content { get; set; }
        [JsonIgnore]
        public List<UserQuestion> UserQuestions { get; } = new();
    }
}
