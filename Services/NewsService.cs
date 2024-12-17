using NewsAggregator.Dtos;
using NewsAggregator.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using static System.Net.WebRequestMethods;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;
using NewsAggregator.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NewsAggregator.Services
{
    public class NewsService : INewsService
    {
        private readonly string _baseUrl, _apiKey;
        static HttpClient client;

        public NewsService(IConfiguration config)
        {
            _baseUrl = config["NewsApi: Url"];
            _apiKey = config["NewsApi:ApiKey"];
            client = new HttpClient();
            client.BaseAddress = new Uri("https://newsapi.org/v2/");
            client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(
            //    new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<ArticleResponse>> GetNewsArticles(string category = "all")
        {

            //var url = $"https://newsapi.org/v2/everything?q=finance&apiKey{_apiKey}";
            var url = $"https://newsapi.org/v2/everything?q={category}&apiKey={_apiKey}";
            
            List<ArticleResponse> articles = new List<ArticleResponse>();
            HttpResponseMessage response = await client.GetAsync(url);
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                // Parse the JSON response
                var root = JsonConvert.DeserializeObject<RootResponse>(json);
                if (root?.Articles != null)
                {
                    articles = root.Articles.Select(article => new ArticleResponse
                    {
                        Title = article.Title,
                        Description = article.Description,
                        PublishedAt = article.PublishedAt,
                        Url = article.Url,
                        Author = article.Author
                    }).ToList();
                }

            }

            return articles;


        }
    }

}