using AppointmentScheduling.Models.ViewModels;

namespace AppointmentScheduling.Service
{
    public interface IAppointmentService
    {
        public List<DoctorVM> GetAllDoctors();
        public List<PatientVM> GetAllPatients();

        public string GetUserid();
        public string GetRole();
        public int AddUpdate(AppointmentVM model);

        public List<AppointmentVM> DoctorsEventById(string doctorId);
        public List<AppointmentVM> PatientsEventById(string patientId);

        public AppointmentVM GetById(int Id);
        public int Delete(int id);
        public int Confirm(int id);
    }
}
