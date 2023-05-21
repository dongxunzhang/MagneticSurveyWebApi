using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace MagneticSurvey.Data
{
    public class UserQuestion
    {
        public int UserEntitysId { get; set; }
        public int QuestionEntitysId { get; set; }
        [JsonIgnore]
        public UserEntity UserEntity { get; set; } = null!;
        public QuestionEntity QuestionEntity { get; set; } = null!;
        public bool IsRead { get; set; }
    }
}
