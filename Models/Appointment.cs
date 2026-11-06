namespace MedicalApi.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int PatientId { get; set; } // Ссылка на пациента
        public string DoctorName { get; set; } // Имя врача
        public DateTime AppointmentDate { get; set; } // Дата и время приема
        public string Status { get; set; } = "Запланирован"; // Статус
    }
}