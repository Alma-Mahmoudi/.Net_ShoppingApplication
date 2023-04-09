using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingApplication.Models;
using ShoppingApplication.Data;
using ShoppingApplication.Services;

namespace ShoppingApplication.Areas.Articles.Pages
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext ctx;
        private readonly ImageService imageService;

        public CreateModel(ApplicationDbContext ctx,ImageService imageService)
        {
           this.ctx = ctx;
            this.imageService = imageService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Article Article { get; set; } = new();
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var emptyArticle = new Article();

            if(null != Article.Image)
            {
                emptyArticle.Image = await imageService.UploadAsync(Article.Image);
            }

            if(await TryUpdateModelAsync(emptyArticle,"Article",f=>f.Name,f=>f.Description,f=> f.Price))
            {

                ctx.Articles.Add(emptyArticle);
                await ctx.SaveChangesAsync();

                return RedirectToPage("./Index");
            }
            return Page();
        }
  
    }
}
