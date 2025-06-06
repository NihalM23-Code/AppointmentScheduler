using AppointmentScheduling.Models;
using AppointmentScheduling.Models.ViewModels;

namespace AppointmentScheduling.Data
{
    public interface IAppointmentRepository
    {
        public List<DoctorVM> GetDoctorsList();
        public List<PatientVM> GetPatientsList();
        public void AddAppointment(Appointment model);
        public List<AppointmentVM> DoctorsEventById(string doctorId);
        public List<AppointmentVM> PatientsEventById(string patientId);
        public AppointmentVM GetById(int Id);
        public int Delete(int id);
        public int Confirm(int id);

    }
}
