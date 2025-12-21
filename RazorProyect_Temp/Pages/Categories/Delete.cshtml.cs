using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorProyect_Temp.Data;
using RazorProyect_Temp.Models;

namespace RazorProyect_Temp.Pages.Categories
{
    [BindProperties]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public Category Category { get; set; }
        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
            
        }
        public void OnGet(int Id)
        {

            if (Id != 0 && Id != null)
            {
                Category = _db.Categories.Find(Id);
            }
        }

        public IActionResult OnPost(int id)
        {
            Category obj = _db.Categories.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            if (id == 0 || id == null)
            {
                return NotFound();
            }

            _db.Categories.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Category was deleted successfully";
            return RedirectToPage("Index");


        }
    }
}
