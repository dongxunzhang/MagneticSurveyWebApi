using Newtonsoft.Json;

namespace MagneticSurvey.Data
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Pwd { get; set; }

        [JsonIgnore]
        public List<UserQuestion> UserQuestions { get; } = new();
    }
}
