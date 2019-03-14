using Assignment.Feature.News.Models;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;

namespace Assignment.Feature.News.Repositories
{
    public interface INewsRepository
    {
        IEnumerable<Item> GetNews(Item newsRoot, string[] tagIds, int page, int limit);
        IEnumerable<Item> GetNews(Item newsRoot, string[] tagIds, int page, int limit, DateTime? pubStart, DateTime? pubEnd);
        IEnumerable<Item> GetNews(Item newsRoot, string[] tagIds, int page, int limit, DateTime? pubStart, DateTime? pubEnd, IEnumerable<Item> excludedItems);
        IEnumerable<Item> GetPopularNews(Item newsRoot, int timeFrameInDays, int limit);
        NewsItem CreateNewsItem(Item item);
    }
}