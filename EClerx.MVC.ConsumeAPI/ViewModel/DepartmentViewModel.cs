using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EClerx.MVC.ConsumeAPI.ViewModel
{
    public class DepartmentViewModel
    {
        [Required]
        [Display(Name ="Department Code")]
        public string cDepartmentCode { get; set; }

        [Required]
        [Display(Name = "Department Name")]
        public string vDepartmentName { get; set; }

        [Required]
        [Display(Name = "Department Head")]
        public string vDepartmentHead { get; set; }

        [Required]
        [Display(Name = "Location")]
        public string vLocation { get; set; }
    }
}