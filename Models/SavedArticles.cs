using System.ComponentModel.DataAnnotations;

namespace NewsAggregator.Models
{
    public class SavedArticles
    {
        [Key]
        public int SavedId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ArticleId { get; set; }
        public Articles Articles { get; set; }
    }
}
