using System.ComponentModel.DataAnnotations;

namespace NewsAggregator.Models
{
    public class SavedArticles
    {
        
        public int SavedId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ArticleId { get; set; }
        public UserArticles Articles { get; set; }
    }
}
