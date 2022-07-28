using EClerx.MVC.ConsumeAPI.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EClerx.MVC.ConsumeAPI.Controllers
{
    [RoutePrefix("Departments")]
    public class DepartmentsController : Controller
    {
        private readonly HttpClient client = null;

        public DepartmentsController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["api"]);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: Departments
        public async Task<ActionResult> Index()
        {
            IEnumerable<DepartmentViewModel> departments = null;

            var result = await client.GetAsync("Departments/GetAll");
            //if (result.StatusCode == System.Net.HttpStatusCode.OK)
            if (result.IsSuccessStatusCode)
            {
                departments = await result.Content.ReadAsAsync<IEnumerable<DepartmentViewModel>>();
            }

            return View(departments);
        }

        //public ActionResult IndexData()
        //{
        //    IEnumerable<DepartmentViewModel> departments = null;
        //    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["api"]);
        //    var result = client.GetAsync("Departments/GetAll");
        //    result.Wait();
        //    var response = result.Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        departments = response.Content.ReadAsAsync<IEnumerable<DepartmentViewModel>>().Result;
        //    }
        //    return View("Index", departments);
        //}

        [Route("Details/{departmentCode}")]
        public async Task<ActionResult> Details(string departmentCode)
        {
            DepartmentViewModel department = null;
            if (string.IsNullOrWhiteSpace(departmentCode))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var result = await client.GetAsync("Departments/Get/" + departmentCode);
            if (result.IsSuccessStatusCode)
            {
                department = await result.Content.ReadAsAsync<DepartmentViewModel>();
            }
            return View(department);
        }

        //AddNew
        //GET 
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(DepartmentViewModel department)//Department Data Bounded from the browser to the parameter
        {
            if (ModelState.IsValid)
            {
                var result = await client.PostAsJsonAsync("Departments/AddNew", department); //First parameter is URL, Second Parameter data we need to create

                if (result.StatusCode == HttpStatusCode.Created)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(department);
        }

        [Route("Edit/{departmentCode?}")]
        public async Task<ActionResult> Edit(string departmentCode)
        {
            if (string.IsNullOrWhiteSpace(departmentCode))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepartmentViewModel department = null;

            var result = await client.GetAsync("Departments/Get/" + departmentCode);
            if (result.IsSuccessStatusCode)
            {
                department = await result.Content.ReadAsAsync<DepartmentViewModel>();
            }
            return View(department);
        }

        [HttpPost]
        [Route("Edit")]
        public async Task<ActionResult> Edit(DepartmentViewModel department)
        {
            var result = await client.PutAsJsonAsync<DepartmentViewModel>("Departments/UpdateDepartment/" + department.cDepartmentCode, department);

            if (ModelState.IsValid)
            {
                if (result.StatusCode == HttpStatusCode.NoContent)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Internal Server Error. Please try later.");
                }
            }
            return View(department);
        }
    }
}