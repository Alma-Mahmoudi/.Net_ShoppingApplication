using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShoppingApplication.Data;
using ShoppingApplication.Models;
using ShoppingApplication.Data;

namespace ShoppingApplication.Areas.Articles.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext ctx;

        public IndexModel(ApplicationDbContext ctx)
        {
            this.ctx = ctx;
        }

        public IList<Article> Article { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (ctx.Articles != null)
            {
                Article = await ctx.Articles.Include(f=>f.Image).ToListAsync();
            }
        }
    }
}
