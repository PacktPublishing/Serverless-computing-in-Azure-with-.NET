using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TextScoreDashboard.Models;

namespace TextScoreDashboard.Controllers
{
    public class TweetsController : Controller
    {
        private TextScoresDBContext _context;

        public TweetsController(TextScoresDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
			return View(_context.TweetTextScore.ToList());
        }

    }
}
