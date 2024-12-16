using System.ComponentModel.DataAnnotations;

namespace NewsAggregator.Models
{
    public class Interests
    {
        [Key]
        public int InterestId { get; set; }
        public string InterestName { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
