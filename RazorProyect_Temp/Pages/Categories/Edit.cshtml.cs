using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorProyect_Temp.Data;
using RazorProyect_Temp.Models;

namespace RazorProyect_Temp.Pages.Categories
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public Category Category { get; set; }
        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet(int Id)
        {
            if(Id != 0 && Id != null)
            {
                Category = _db.Categories.Find(Id);
            }
        }

        public IActionResult OnPost()
        { 

            if (ModelState.IsValid)
            {
                _db.Categories.Update(Category);
                _db.SaveChanges();
                TempData["success"] = "Category was updated successfully";
                return RedirectToPage("Index");

            }
            return Page();
        }
    }
}
