using AppointmentScheduling.Data;
using AppointmentScheduling.Models;
using AppointmentScheduling.Models.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;

namespace AppointmentScheduling.Service
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repository;
        private readonly IHttpContextAccessor _contextAccessor;
        public AppointmentService(IAppointmentRepository repository,IHttpContextAccessor contextAccessor) 
        {
            _repository = repository;
            _contextAccessor = contextAccessor;
        }


        public string GetUserid()
        {
            string userid = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userid;
        }
        public string GetRole()
        {
            string Role = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            return Role;
        }
        public List<DoctorVM> GetAllDoctors()
        {
            List<DoctorVM> doctors = _repository.GetDoctorsList();
            if (doctors == null|| !doctors.Any())
            {
                throw new InvalidOperationException("No doctors found in the system.");
            }
            else
            {
                return doctors;
              
            }
        }

        public List<PatientVM> GetAllPatients()
        {
            List<PatientVM> patients = _repository.GetPatientsList();
            if (patients == null || !patients.Any())
            {
                throw new InvalidOperationException("No patient found in the system.");
            }
            else
            {
                return patients;

            }
            
        }

        public int AddUpdate(AppointmentVM model)
        {
            var startDate = DateTime.Parse(model.StartDate);
            var endDate = DateTime.Parse(model.StartDate).AddMinutes(Convert.ToDouble(model.Duration));
            if(model!=null&&model.Id>0)
            {
                return 1;
            }
            else
            {
                Appointment appointment = new Appointment
                {
                 
                    Title = model.Title,
                    Description = model.Description,
                    StartDate=startDate,
                    EndDate=endDate,
                    Duration = model.Duration,
                    DoctorId=model.DoctorId,
                    PatientId=model.PatientId,
                    AdminId= GetUserid(),
                    IsDoctorApproved=false

                };
                _repository.AddAppointment(appointment);
                return 2;
            }
        }
        
        public List<AppointmentVM> DoctorsEventById(string doctorId)
        {
            return _repository.DoctorsEventById( doctorId);
        }

        public List<AppointmentVM> PatientsEventById(string patientId)
        {
            return _repository.PatientsEventById(patientId);
        }

        public AppointmentVM GetById(int Id)
        {
            return _repository.GetById(Id);
        }

        public int Delete(int id)
        {
            return _repository.Delete(id);
        }

        public int Confirm(int id)
        {
           return _repository.Confirm(id);
        }
    }
}
