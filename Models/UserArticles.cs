using System.ComponentModel.DataAnnotations;

namespace NewsAggregator.Models
{
    public class UserArticles
    {
        [Key]
        public int ArticleId { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }   
        public DateTime PublishedAt { get; set; }
    }
}
