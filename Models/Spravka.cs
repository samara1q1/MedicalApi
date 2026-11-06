namespace MedicalApi.Models
{
    public class Spravka
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        // Новое поле: связь с пациентом
        public int PatientId { get; set; }
    }
}