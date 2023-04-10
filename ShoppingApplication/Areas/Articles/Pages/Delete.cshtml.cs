using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShoppingApplication.Data;
using ShoppingApplication.Models;
using ShoppingApplication.Areas;
using ShoppingApplication.Services;

namespace ShoppingApplication.Areas.Articles.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext ctx;
        private ImageService imageService;

        public DeleteModel(ApplicationDbContext ctx,ImageService imageService)
        {
            this.ctx = ctx;
            this.imageService = imageService;
        }

        [BindProperty]
        public Article Article { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, bool? hasErrorMessage = false)
        {
            if (id == null || ctx.Articles == null)
            {
                return NotFound();
            }

            var article = await ctx.Articles
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (article == null)
            {
                return NotFound();
            }
            else 
            {
                Article = article;
            }

            if (hasErrorMessage.GetValueOrDefault())
            {
                ErrorMessage = $"Une erreur est survenue lors la tentative de suppression de {Article.Name} ({Article.Id})";
            }


            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || ctx.Articles == null)
            {
                return NotFound();
            }

            var articleToDelete = await ctx.Articles
                .Include(f => f.Image)
                .FirstOrDefaultAsync(f=>f.Id == id);

            if (articleToDelete == null)
            {
                return NotFound();
            }
         

            try
            {
                imageService.DeleteUploadedFile(articleToDelete.Image);
                ctx.Articles.Remove(articleToDelete);
                await ctx.SaveChangesAsync();

                return RedirectToPage("./Index");
            }
            catch (Exception)
            {
                return RedirectToAction("./Delete",new {id,hasErrorMessage =true});
            }

            /*var article = await ctx.Articles.FindAsync(id);

            if (article != null)
            {
                Article = article;
                ctx.Articles.Remove(Article);
                await ctx.SaveChangesAsync();
            }

            return RedirectToPage("./Index");*/
        }
    }
}
