using System;
using System.Linq;
using System.Web.Mvc;
using University.BL.Data;
using University.BL.DTOs;
using University.BL.Models;
using PagedList;
using System.Web.UI;

namespace University.Web.Controllers
{
    public class DepartmetsController : Controller
    {
        private readonly UniversityContext context = new UniversityContext();

        [HttpGet]
        public ActionResult Index(int? departmentid, int? pageSize, int? page)
        {
            var query = context.Departments.ToList();
            var co = query.Select(x => new DepartmentDTO
            {
                DepartmentID = x.DepartmentID,
                Name = x.Name,
                Budget = x.Budget,
                StartDate = x.StartDate,
                
                
            }).ToList();

            if (departmentid != null)  
            {
                var instructors = (from q in context.Departments
                                   join s in context.Instructors on q.InstructorID equals s.ID
                                   where q.DepartmentID == departmentid
                                   select new InstructorDTO
                                   {
                                       LastName = s.LastName,
                                       FirstMidName = s.FirstMidName
                                   }).ToList();
                ViewBag.Instructores = instructors;
            }
            #region Paginacion
            pageSize = (pageSize ?? 10);
            page = (page ?? 1);
            ViewBag.pageSize = pageSize;
            #endregion
            return View(co.ToPagedList(page.Value, pageSize.Value));
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(DepartmentDTO department)
        {
            try
            {
                if (!ModelState.IsValid)

                    return View(department);


                context.Departments.Add(new Department
                {
                    DepartmentID = department.DepartmentID,
                    Name = department.Name,
                   Budget = department.Budget,
                   StartDate= department.StartDate,
                   InstructorID = department.InstructorID
                   
                });
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(department);

        }

        [HttpGet]
        public ActionResult Edit(int departmentid)
        {




            var department = context.Departments.Where(x => x.DepartmentID == departmentid)
                    .Select(x => new DepartmentDTO
                    {
                        DepartmentID = x.DepartmentID,
                        Name = x.Name,
                        Budget = x.Budget,
                        StartDate=x.StartDate,
                        InstructorID = x.InstructorID
                        
                    }).FirstOrDefault();
            return View(department);
        }

        [HttpPost]
        public ActionResult Edit(DepartmentDTO department)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(department);
                var departmentModel = context.Departments.FirstOrDefault(x => x.DepartmentID == department.DepartmentID);

                departmentModel.Name = department.Name;
                departmentModel.Budget = department.Budget;
                departmentModel.StartDate = department.StartDate;
                departmentModel.InstructorID = department.InstructorID;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(department);
        }
        [HttpGet]
        public ActionResult Delete(int departmentid)
        { 
            /*NO ES NECESARIO VALIDAR EL REGISTRO A ELIMINAR*/
            //if (!context.Departments.Any(x => x.DepartmentID == departmentid))
           //{
                var departmentModel = context.Departments.FirstOrDefault(x => x.DepartmentID == departmentid);
                context.Departments.Remove(departmentModel);
                context.SaveChanges();
          // }
            return RedirectToAction("Index");
        }
    }
}