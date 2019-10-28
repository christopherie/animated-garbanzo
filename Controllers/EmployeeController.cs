using MVCDemo.Models;
using MVCDemo.Repository.Classes;
using System;
using System.Web.Mvc;

namespace MVCDemo.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            GenericRepository<Employee> employeeRepository = new GenericRepository<Employee>("Employees");

            return View(employeeRepository.GetAll());
        }

        // GET: Employee/Details/5
        public ActionResult Details(Guid id)
        {
            GenericRepository<Employee> employeeRepository = new GenericRepository<Employee>("Employees");

            return View(employeeRepository.GetById(id));
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Guid guid = Guid.NewGuid();
                    GenericRepository<Employee> employeeRepository = new GenericRepository<Employee>("Employees");
                    Employee employee = new Employee
                    {
                        ID = guid,
                        FirstName = collection["FirstName"],
                        LastName = collection["LastName"],
                        Gender = collection["Gender"],
                        Salary = Convert.ToInt32(collection["Salary"])
                    };

                    employeeRepository.Insert(employee);
                    TempData["message"] = "Employee successfully created";
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(Guid id)
        {
            GenericRepository<Employee> employeeRepository = new GenericRepository<Employee>("Employees");

            return View(employeeRepository.GetById(id));
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(Guid id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                GenericRepository<Employee> employeeRepository = new GenericRepository<Employee>("Employees");

                Employee employee = new Employee
                {
                    ID = new Guid(collection["ID"]),
                    FirstName = collection["FirstName"],
                    LastName = collection["LastName"],
                    Gender = collection["Gender"],
                    Salary = Convert.ToInt32(collection["Salary"])
                };

                employeeRepository.Update(employee);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(Guid id)
        {
            GenericRepository<Employee> employeeRepository = new GenericRepository<Employee>("Employees");

            return View(employeeRepository.GetById(id));
        }

        // POST: Employee/Delete/5
        [HttpPost]
        public ActionResult Delete(Guid id, FormCollection collection)
        {
            GenericRepository<Employee> employeeRepository = new GenericRepository<Employee>("Employees");


            try
            {
                if (employeeRepository.Delete(id))
                {
                    TempData["message"] = "Employee successfully deleted";
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }
    }
}
