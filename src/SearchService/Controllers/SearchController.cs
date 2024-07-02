using Microsoft.AspNetCore.Mvc;

using MongoDB.Entities;

using SearchService.Models;
using SearchService.RequestHelpers;

namespace SearchService;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Item>>> SearchItems([FromQuery] SearchParams searchParams)
    {
        var query = DB.PagedSearch<Item, Item>()
            .PageNumber(searchParams.PageNumber)
            .PageSize(searchParams.PageSize);

        if (!string.IsNullOrEmpty(searchParams.SearchTerm))
        {
            query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
        }

        query = searchParams.OrderBy switch
        {
            "make" => query.Sort(x => x.Ascending(a => a.Make)),
            "new" => query.Sort(x => x.Descending(a => a.CreatedAt)),
            "model" => query.Sort(x => x.Ascending(a => a.Model)),
            "year" => query.Sort(x => x.Ascending(a => a.Year)),
            _ => query.Sort(x => x.Ascending(a => a.AuctionEnd))  // Default Sorting
        };

        query = searchParams.FilterBy switch
        {
            "finished" => query.Match(x => x.AuctionEnd < DateTime.UtcNow),
            "endingSoon" => query.Match(x => x.AuctionEnd < DateTime.UtcNow.AddHours(8) && x.AuctionEnd > DateTime.UtcNow),
            "seller" => query.Match(x => x.Seller == searchParams.Seller),
            "winner" => query.Match(x => x.Winner == searchParams.Winner),
            _ => query.Match(x => x.AuctionEnd > DateTime.UtcNow)  // Default Filter
        };

        if (!string.IsNullOrEmpty(searchParams.Seller))
        {
            query.Match(x => x.Seller == searchParams.Seller);
        }

        if (!string.IsNullOrEmpty(searchParams.Winner))
        {
            query.Match(x => x.Winner == searchParams.Winner);
        }

        var result = await query.ExecuteAsync();

        return Ok(new
        {
            results = result.Results,
            pageCount = result.PageCount,
            totalCount = result.TotalCount
        });
    }
}
