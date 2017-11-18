using IyunExtension.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IyunExtension.Controllers
{
    [RoutePrefix("api")]
    public class IyunController : ApiController
    {
        static readonly string[] CategoryLayers = new[] {
            "Tanakh",
            "Mishnah",
            "Tanaitic",
            "Talmud",
            "Halakhah",
            "Kabbalah",
            "Midrash",
            "Musar",
            "Chasidut",
            "Philosophy"
        };

        [HttpGet]
        [Route("links/{anchor}")]
        public IEnumerable<object> GetLinks(string anchor)
        {
            using (var db = new IyunDbContext())
            {
                var results = SearchLevels(db, anchor, 3);

                return results
                    .Where(x => x.Level > 0)
                    .OrderByDescending(x => x.Rank.Rank)
                    .Take(10)
                    .Select(x => new
                    {
                        category = x.Link.Citation_1 == x.Rank.Ref
                            ? x.Link.Category_1
                            : x.Link.Category_2,

                        sourceHeRef = x.Link.Citation_1 == x.Rank.Ref
                            ? x.Link.He_Citation_1
                            : x.Link.He_Citation_2,
                        sourceRef = x.Rank.Ref,

                        @ref = x.Rank.Ref,
                        anchorRef = x.Link.Citation_1 == x.Rank.Ref
                            ? x.Link.Citation_2
                            : x.Link.Citation_1,
                        rank = x.Rank.Rank
                    });
            }
        }

        List<SearchResult> SearchLevels(IyunDbContext db, string rootAnchor, int levels)
        {
            var results = new List<SearchResult>();

            var level = new[] { rootAnchor };
            for (int i = 0; i < levels; i++)
            {
                var levelResult = SearchLevel(db, level, i);
                var newResults = levelResult.Where(x => !results.Any(r => r.Rank.Ref == x.Rank.Ref))
                    .Where(x => IsHigherLevelResult(x, i == 0));

                level = newResults.OrderByDescending(x => x.Rank.Rank)
                    .Take(10)
                    .Select(x => x.Rank.Ref)
                    .ToArray();

                results.AddRange(newResults);
            }

            return results;
        }

        List<SearchResult> SearchLevel(IyunDbContext db, string[] anchors, int level)
        {
            var query = from l in db.SefariaLinks
                        join r1 in db.UniqueLinks on l.Citation_1 equals r1.Ref
                        join r2 in db.UniqueLinks on l.Citation_2 equals r2.Ref
                        let r1t = anchors.Contains(l.Citation_1)
                        let r2t = anchors.Contains(l.Citation_2)
                        where r1t || r2t
                        select new SearchResult
                        {
                            Link = l,
                            Rank = r1t ? r2 : r1,
                            Level = level
                        };

            var list = query.ToList();
            return list;
        }

        bool IsHigherLevelResult(SearchResult result, bool allowSameLevel)
        {
            var sourceCategory = result.Rank.Ref == result.Link.Citation_1
                ? result.Link.Category_2
                : result.Link.Category_1;

            var targetCategory = result.Rank.Ref == result.Link.Citation_1
                ? result.Link.Category_1
                : result.Link.Category_2;

            var sourceLevel = CategoryLayers.Contains(sourceCategory)
                ? Array.IndexOf(CategoryLayers, sourceCategory)
                : int.MaxValue;

            var targetLevel = CategoryLayers.Contains(targetCategory)
                ? Array.IndexOf(CategoryLayers, targetCategory)
                : int.MaxValue;

            return allowSameLevel
                ? (targetLevel > sourceLevel)
                : (targetLevel >= sourceLevel);
        }
    }
}
