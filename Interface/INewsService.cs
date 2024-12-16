using NewsAggregator.Dtos;

namespace NewsAggregator.Interface
{
    public interface INewsService
    {
        public Task<List<ArticleResponse>> GetNewsArticles(string category);
    }
}
