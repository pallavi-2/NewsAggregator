namespace NewsAggregator.Dtos
{
    public class ArticleResponse
    {
        
        public string ArticleId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishedAt { get; set; }
        public string Url { get; set; }
        public string Author { get; set; }
    }
}
