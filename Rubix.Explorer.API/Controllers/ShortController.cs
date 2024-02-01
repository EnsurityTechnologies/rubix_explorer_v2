using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Rubix.Explorer.API.Controllers
{
    public class ShortController : Controller
    {
        private readonly UrlShortener _urlShortener;

        public ShortController(UrlShortener urlShortener) {
            _urlShortener = urlShortener;
        }

        [HttpGet]
        public  async Task<IActionResult> Index(string url)
        {
            var expandedUrl=await _urlShortener.ExpandUrl(url);
            return Redirect(expandedUrl);
        }
    }
}
