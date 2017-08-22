using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TextScoreDashboard.Models;

namespace TextScoreDashboard.Controllers
{
    public class DocumentsController : Controller
    {
        private TextScoresDBContext _context;

        public DocumentsController(TextScoresDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.DocumentTextScore.ToList());
        }

    }
}
