using AppointmentScheduling.Models.ViewModels;
using AppointmentScheduling.Service;
using AppointmentScheduling.Utility;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduling.Controllers.API
{
    [Route("api/Appointment")]
    [ApiController]
    public class AppointmentApiController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly string loginUserId;
        private readonly string role;
        public AppointmentApiController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
            loginUserId = _appointmentService.GetUserid();
            role = _appointmentService.GetRole();
        }
        [HttpPost]
        [Route("SaveCalendarData")]
        public IActionResult SaveCalendarData([FromBody] AppointmentVM data)
        {
            CommonResponseVM<int> commonResponse = new CommonResponseVM<int>();
            try
            {
                commonResponse.status = _appointmentService.AddUpdate(data);
                if (commonResponse.status == 1)
                {
                    commonResponse.message = Helper.appointmentUpdated;
                }
                else if (commonResponse.status == 2)
                {
                    commonResponse.message = Helper.appointmentAdded;
                }
            }
            catch (Exception ex)
            {
                commonResponse.message = ex.Message;
                commonResponse.status = Helper.failure_code;
            }

            return Ok(commonResponse);
        }

        [HttpGet]
        [Route("GetCalendarData")]
        public IActionResult GetCalendarData(string doctorId)
        {
            CommonResponseVM<List<AppointmentVM>> commonResponse = new CommonResponseVM<List<AppointmentVM>>();
            try
            {
                if (role == Helper.Patient)
                {
                    commonResponse.dataenum = _appointmentService.PatientsEventById(loginUserId);
                    commonResponse.status = Helper.success_code;
                }
                else if (role == Helper.Doctor)
                {
                    commonResponse.dataenum = _appointmentService.DoctorsEventById(loginUserId);
                    commonResponse.status = Helper.success_code;
                }
                else
                {
                    commonResponse.dataenum = _appointmentService.DoctorsEventById(doctorId);
                    commonResponse.status = Helper.success_code;
                }
            }
            catch (Exception ex)
            {
                commonResponse.message = ex.Message;
                commonResponse.status = Helper.failure_code;
            }
            return Ok(commonResponse);
        }
        [HttpGet]
        [Route("GetCalendardataById/{id}")]
        public IActionResult GetCalendardataById(int id)
        {
            CommonResponseVM<AppointmentVM> commonResponse = new CommonResponseVM<AppointmentVM>();
            try
            {
                commonResponse.dataenum = _appointmentService.GetById(id);
                commonResponse.status = Helper.success_code;
            }
            catch (Exception ex) 
            {
                commonResponse.message = ex.Message;
                commonResponse.status = Helper.failure_code;
            }
            return Ok(commonResponse);
        }

        [HttpGet]
        [Route("DeleteAppointment/{id}")]
        public IActionResult DeleteAppointment(int id)
        {
            CommonResponseVM<int> commonResponse = new CommonResponseVM<int>();
            try
            {
                commonResponse.status = _appointmentService.Delete(id);
                commonResponse.message = commonResponse.status == 1 ? Helper.appointmentDeleted : Helper.somethingWentWrong;
            }
            catch (Exception ex)
            {
                commonResponse.message = ex.Message;
                commonResponse.status = Helper.failure_code;
            }
            return Ok(commonResponse);
        }

        [HttpGet]
        [Route("ConfirmAppointment/{id}")]
        public IActionResult ConfirmAppointment(int id)
        {
            CommonResponseVM<int> commonResponse = new CommonResponseVM<int>();

            try
            {               
                commonResponse.status = _appointmentService.Confirm(id);
                commonResponse.message = commonResponse.status == 0 ? Helper.somethingWentWrong : Helper.meetingConfirm;
            }
            catch (Exception ex)
            {
                commonResponse.message = ex.Message;
                commonResponse.status = Helper.failure_code;
            }
            return Ok(commonResponse);
        }
    }
}
