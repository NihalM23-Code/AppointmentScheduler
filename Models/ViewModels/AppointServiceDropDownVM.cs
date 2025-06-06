using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace AppointmentScheduling.Models.ViewModels
{
    public class AppointServiceDropDownVM
    {

        //[DisplayName("Doctor Name")]
        public string DoctorId{ get; set; }

        //[DisplayName("Patient Name")]
        public string PatientId { get; set; }

        public List<SelectListItem> DoctorList { get; set; }
        public List<SelectListItem> PatientList { get; set; }

        public string Duration { get; set; }
    }
}
