using AppointmentScheduling.Models;
using AppointmentScheduling.Models.ViewModels;
using AppointmentScheduling.Service;
using AppointmentScheduling.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppointmentScheduling.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService) 
        {
            _appointmentService = appointmentService;

        }
        //[Authorize(Roles =Helper.Admin)]
        public IActionResult Index()
        {
            //ViewBag.doctorlist = _appointmentService.GetAllDoctors();
            //ViewBag.patientlist=_appointmentService.GetAllPatients();
            //ViewBag.Duration = Helper.GetTimeDropDown();
            
           var doctors= _appointmentService.GetAllDoctors();
           var patients= _appointmentService.GetAllPatients();
            var model = new AppointServiceDropDownVM
            {
                DoctorList = doctors.Select(d => new SelectListItem
                {
                    Text = d.Name,
                    Value = d.Id.ToString()
                }).ToList(),
                PatientList = patients.Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                }).ToList()

            };
            return View(model);
        }
    }
}
