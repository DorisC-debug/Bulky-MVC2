using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public IActionResult Index()
        {
            List<Company> companies = _unitOfWork.Company.GetAll().ToList();
            return View(companies);
        }

        public IActionResult Upsert(int? Id)
        {
            Company company = new Company();

            if (Id == 0 || Id == null) //create
            {

                return View(company);

            }
            else
            {
                //update
                company = _unitOfWork.Company.Get(u => u.Id == Id);
            }
            return View(company);
        }

        [HttpPost]
        public IActionResult Upsert(Company company)
        {
            if (ModelState.IsValid)
            {

                if (company.Id == 0 || company.Id == null) //create
                {

                    _unitOfWork.Company.Add(company);

                }
                else
                {
                    //update
                    _unitOfWork.Company.Update(company);
                }

                _unitOfWork.Save();
                TempData["success"] = "Company Created Successfully";
                return RedirectToAction("Index");

            }
            return View(company);


        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> companyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = companyList });
        }

        [HttpDelete]
        public IActionResult Delete(int Id)
        {
            Company? companyfromdb = _unitOfWork.Company.Get(u => u.Id == Id);
            if (companyfromdb == null)
            {
                return Json(new { success = false, Message = "Error while deleting." });
            }

            _unitOfWork.Company.Remove(companyfromdb);
            _unitOfWork.Save();
            return Json(new {success= true, Messgage= "Delete Successful" });
        }
        #endregion
    }
}
