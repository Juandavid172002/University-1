using System;
using System.Collections.Generic;
using System.Web.Mvc;
using University.BL.Models;
using University.BL.DTOs;
using University.BL.Data;
using System.Linq;
using PagedList;

namespace University.Web.Controllers
{
    public class InstructorController : Controller
    {
        
            private readonly UniversityContext context = new UniversityContext();

           
            [HttpGet]
            public ActionResult Index(int? InstructorID, int? pageSize, int? page)
            {
                
                var query = context.Instructors.ToList();

               
                var Instructor = query.Where(x => x.HireDate < DateTime.Now)
                                    .Select(x => new InstructorDTO
                                    {
                                        ID = x.ID,
                                        LastName = x.LastName,
                                        FirstMidName = x.FirstMidName,
                                       HireDate = x.HireDate
                                    }).ToList();
                #region Cursos Asociados
                if (InstructorID != null)
                {
                    

                    var departments = (from q in context.Departments
                                   join r in context.Departments on q.DepartmentID equals r.DepartmentID
                                   where q.InstructorID == InstructorID
                                   select new Department
                                   {
                                       DepartmentID = r.DepartmentID,
                                       Name = r.Name,
                                       Budget = r.Budget,
                                       StartDate = r.StartDate,

                                   }).ToList();

                ViewBag.Departments = departments;
                }

                ViewBag.Data = "Data de prueba";
                ViewBag.Message = "Mensaje de prueba";

                //ViewData["Data"] = "Data de prueba";
                //ViewData["Message"] = "Mensaje de prueba";

                #region Paginacion
                pageSize = (pageSize ?? 10);
                page = (page ?? 1);
                ViewBag.PageSize = pageSize;
                #endregion

                return View(Instructor.ToPagedList(page.Value, pageSize.Value));
            }
            #endregion

            // Instructor/Create
            [HttpGet]
            public ActionResult Create()
            {
                return View();
            }

            [HttpPost]
            public ActionResult Create(InstructorDTO instructor)
            {
                try
                {
                    if (!ModelState.IsValid)
                        return View(instructor);

                    if (instructor. HireDate> DateTime.Now)
                        throw new Exception("La fecha de matricula no puede ser mayor a la fecha actual");

                    
                    context.Instructors.Add(new Instructor
                    {
                        FirstMidName = instructor.FirstMidName,
                        LastName = instructor.LastName,
                        HireDate = instructor.HireDate
                    });
                    context.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

                return View(instructor);
            }

            [HttpGet]
            public ActionResult Edit(int id)
            {
                //var query = context.Instructor.Find(id);
                var instructor = context.Instructors.Where(x => x.ID == id)
                                              .Select(x => new InstructorDTO
                                              {
                                                  ID = x.ID,
                                                  LastName = x.LastName,
                                                  FirstMidName = x.FirstMidName,
                                                  HireDate = x.HireDate
                                              }).FirstOrDefault();

                return View(instructor);
            }

            [HttpPost]
            public ActionResult Edit(InstructorDTO instructor)
            {
                try
                {
                    if (!ModelState.IsValid)
                        return View(instructor);

                    if (instructor.HireDate > DateTime.Now)
                        throw new Exception("La fecha de matricula no puede ser mayor a la fecha actual");

                   
                    var instructorModel = context.Instructors.FirstOrDefault(x => x.ID == instructor.ID);

                //campos que se van a modificar
                //sobreescribo las propiedades del modelo de base de datos
                instructorModel.LastName = instructor.LastName;
                instructorModel.FirstMidName = instructor.FirstMidName;
                instructorModel.HireDate = instructor.HireDate;

                   
                    context.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

                return View(instructor);
            }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (!context.Departments.Any(x => x.InstructorID == id))
                {
                    var instructorModel = context.Instructors.FirstOrDefault(x => x.ID == id);
                    context.Instructors.Remove(instructorModel);
                    context.SaveChanges();
                }

                return RedirectToAction("Index");
            }
        }
    }

