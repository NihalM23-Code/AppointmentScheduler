namespace AppointmentScheduling.Models.ViewModels
{
    public class CommonResponseVM<T>
    {
        public int status { get; set; }
        public string message { get; set; }

        public T dataenum { get; set; }
    }
}
