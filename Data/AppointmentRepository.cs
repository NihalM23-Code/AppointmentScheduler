using AppointmentScheduling.Models;
using AppointmentScheduling.Models.ViewModels;
using AppointmentScheduling.Utility;
using Microsoft.AspNetCore.Identity;

namespace AppointmentScheduling.Data
{
    public class AppointmentRepository:IAppointmentRepository
    {
        private readonly ApplicationDbContext _context;
        public AppointmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<DoctorVM> GetDoctorsList()
        {
            var doctorsList = (from user in _context.Users
                               join userrole in _context.UserRoles on user.Id equals userrole.UserId
                               join roles in _context.Roles.Where(x=>x.Name=="Doctor") on userrole.RoleId equals roles.Id
                               select new DoctorVM
                               {
                                   Id=user.Id,
                                   Name=user.Name
                               }
                             ).ToList();
            return doctorsList;
        }

        public List<PatientVM> GetPatientsList()
        {
            var patientList = (from user in _context.Users
                               join userrole in _context.UserRoles on user.Id equals userrole.UserId
                               join roles in _context.Roles.Where(x => x.Name == Helper.Patient) on userrole.RoleId equals roles.Id
                               select new PatientVM
                               {
                                   Id = user.Id,
                                   Name = user.Name
                               }
                             ).ToList();
            return patientList;
        }

        public  void AddAppointment(Appointment model)
        {
            _context.Appointments.Add(model);
            _context.SaveChanges();
        }
        

        public List<AppointmentVM> DoctorsEventById(string doctorId)
        {
            var DoctorEventlist = (from appointment in _context.Appointments where appointment.DoctorId == doctorId
                                   select new AppointmentVM
                                   {
                                       Id = appointment.Id,
                                       Title = appointment.Title,
                                       Description = appointment.Description,
                                       StartDate = appointment.StartDate.ToString(),
                                       EndDate=appointment.EndDate.ToString(),
                                       Duration=appointment.Duration,
                                       IsDoctorApproved=appointment.IsDoctorApproved

                                   }
                                 ).ToList();
            return DoctorEventlist;
        }

        public List<AppointmentVM> PatientsEventById(string patientId)
        {
            var PatientEventlist = (from appointment in _context.Appointments
                                   where appointment.PatientId == patientId 
                                   select new AppointmentVM
                                   {
                                       Id = appointment.Id,
                                       Title = appointment.Title,
                                       Description = appointment.Description,
                                       StartDate = appointment.StartDate.ToString(),
                                       EndDate = appointment.EndDate.ToString(),
                                       Duration = appointment.Duration,
                                       IsDoctorApproved = appointment.IsDoctorApproved

                                   }
                                 ).ToList();
            return PatientEventlist;
        }

        public AppointmentVM GetById(int Id)
        {
            var Eventlist = (from appointment in _context.Appointments
                             where appointment.Id == Id
                             select new AppointmentVM
                             {
                                 Id = appointment.Id,
                                 Title = appointment.Title,
                                 Description = appointment.Description,
                                 StartDate = appointment.StartDate.ToString(),
                                 EndDate = appointment.EndDate.ToString(),
                                 Duration = appointment.Duration,
                                 IsDoctorApproved = appointment.IsDoctorApproved,
                                 PatientId = appointment.PatientId,
                                 DoctorId = appointment.DoctorId,
                                 PatientName = _context.Users.Where(x => x.Id == appointment.PatientId).Select(x => x.Name).FirstOrDefault(),
                                 DoctorName = _context.Users.Where(x => x.Id == appointment.DoctorId).Select(x => x.Name).FirstOrDefault(),

                             }
                                ).SingleOrDefault();
            return Eventlist;
        }

        public int Delete(int id)
        {
            var appointment = _context.Appointments.FirstOrDefault(x => x.Id == id);
            if(appointment != null)
            {
                _context.Appointments.Remove(appointment);
               return _context.SaveChanges();
            }
            return 0;
        }

        public int Confirm(int id)
        {
            var appointment = _context.Appointments.FirstOrDefault(x => x.Id == id);
            if (appointment != null)
            {
                appointment.IsDoctorApproved = true;
                return _context.SaveChanges();
            }
            return 0;
        }
    }
}
