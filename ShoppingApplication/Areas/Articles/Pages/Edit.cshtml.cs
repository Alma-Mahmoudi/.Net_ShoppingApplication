using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingApplication.Data;
using ShoppingApplication.Models;
using ShoppingApplication.Data;
using ShoppingApplication.Services;

namespace ShoppingApplication.Areas.Articles.Pages
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext ctx;
        private readonly ImageService imageService;

        public EditModel(ApplicationDbContext ctx,ImageService imageService)
        {
            this.ctx = ctx;
            this.imageService = imageService;
        }

        [BindProperty]
        public Article Article { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || ctx.Articles == null)
            {
                return NotFound();
            }

            var article =  await ctx.Articles
                .Include(f => f.Image)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (article == null)
                return NotFound();
            
            Article = article;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var articleToUpdate = await ctx.Articles
                .Include(f => f.Image)
                .FirstOrDefaultAsync(f => f.Id == id);

            if(Article == null)
            {
                return NotFound();
            }

            var uploadedImage = Article.Image;
            if(null != uploadedImage)
            {
                uploadedImage = await imageService.UploadAsync(uploadedImage);

                if (articleToUpdate.Image != null)
                {
                    imageService.DeleteUploadedFile(articleToUpdate.Image);
                    articleToUpdate.Image.Name = uploadedImage.Name;
                    articleToUpdate.Image.Path = uploadedImage.Path;
                }
                else
                    articleToUpdate.Image = uploadedImage;
            }
            if (await TryUpdateModelAsync(articleToUpdate, "Article", f => f.Name, f => f.Description, f => f.Price))
            {
                await ctx.SaveChangesAsync();

                return RedirectToPage("./Index");
            }

            return Page();
        }

        private bool ArticleExists(int id)
        {
          return (ctx.Articles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
