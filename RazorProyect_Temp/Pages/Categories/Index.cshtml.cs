using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorProyect_Temp.Data;
using RazorProyect_Temp.Models;

namespace RazorProyect_Temp.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public List<Category> CategoryList { get; set; }
        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
            
        }
        public void OnGet()
        {
            CategoryList = _db.Categories.ToList();
        }
    }
}
