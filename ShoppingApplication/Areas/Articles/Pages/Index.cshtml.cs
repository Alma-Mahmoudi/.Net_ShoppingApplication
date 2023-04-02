using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShoppingApplication.Data;
using ShoppingApplication.Models;

namespace ShoppingApplication.Areas.Articles.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ShoppingApplication.Data.ApplicationDbContext _context;

        public IndexModel(ShoppingApplication.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Article> Article { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Articles != null)
            {
                Article = await _context.Articles.ToListAsync();
            }
        }
    }
}
