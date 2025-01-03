﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data;
using NewsAggregator.Dtos;
using NewsAggregator.Interface;
using NewsAggregator.Models;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace NewsAggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ArticlesController : ControllerBase
    {
        private readonly INewsService _newsService;
        private readonly ApplicationDbContext _context;

        public ArticlesController(INewsService newsService, ApplicationDbContext context)
        {
            _newsService = newsService;
            _context = context;

        }


        //Populate according to the interest of the user
        [HttpGet]
        public async Task<ActionResult<List<ArticleResponse>>> NewsArticles()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            List<ArticleResponse> ArticleResponse = new List<ArticleResponse>();

            //Get the interests of the user
            var interestName = await _context.Interests.FindAsync(int.Parse(userId));

            // If no interest send all the available articles
                var interests = interestName?.InterestName;
            if (string.IsNullOrEmpty(interests))
            {
                var result = await _newsService.GetNewsArticles("");
                if (result != null && result.Any()) return Ok(result);
            }
            else
            {
                //The interest are saved with spaces in the database
                string[] AllInterest = interests.Split(' ');

                //Get the articles for each interest from api
                foreach (string interest in AllInterest)
                {
                    var result = await _newsService.GetNewsArticles(interest);
                    if (result != null && result.Any())
                    {
                        ArticleResponse.AddRange(result);

                    }
                }
            }
            
            return ArticleResponse;
        }


        //Save the articles 
        [HttpPost("save")]
        public async Task<IActionResult> SaveArticles([FromBody] string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            
            //get all the articles from the api
            var result = await _newsService.GetNewsArticles("");

            Console.WriteLine(id);
            foreach(var article in result)
            {
                //Console.WriteLine(article);
                //check if the url matches
                if (article.ArticleId == id)
                {
                   //check if the article is already saved in the articles table
                   var articleSaved = await _context.UserArticles.FirstOrDefaultAsync(x=>x.ArticleId == id);
                    //Console.WriteLine(article.ArticleId);
                    //if not create a new article and save it
                    if(articleSaved == null)
                    {
                        
                        articleSaved = new UserArticles
                        {
                            ArticleId = id,
                            Title = article.Title,
                            Description = article.Description,
                            Url = article.Url,
                            PublishedAt = article.PublishedAt,
                            Author = article.Author,
                        };
                        await _context.UserArticles.AddAsync(articleSaved);
                    }

                    //if the article is already present associate the article id and user id in the saved articles table
                    string articleId = articleSaved.ArticleId;
                    var connectArticleAndUser = new SavedArticles
                    {
                        ArticleId = articleId,
                        UserId = int.Parse(userId),
                    };
                    await _context.SavedArticles.AddAsync(connectArticleAndUser);
                }
            }
            await _context.SaveChangesAsync();
            return Ok("Article saved");
        }


        //Get the saved articles
        [HttpGet("getSavedArticles")]
        public async Task<ActionResult<List<ArticleResponse>>> GetSavedArticles()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            List<ArticleResponse> result = new List<ArticleResponse>();

            //Get all the article id associated with the user
            List<string> articleId = await _context.SavedArticles
            .Where(x => x.UserId == int.Parse(userId))
            .Select(x => x.ArticleId)
            .ToListAsync();

            if (articleId == null)
            {
                return new List<ArticleResponse>(); 
            }

            //Get the articles from the UserArticles table for each articleId associated with the user
            var articles = await _context.UserArticles
            .Where(a => articleId.Contains(a.ArticleId))
            .ToListAsync();
            return Ok(articles);
        }
    }
}
