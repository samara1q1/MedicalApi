using Microsoft.AspNetCore.Mvc;
using MedicalApi.Models;

namespace MedicalApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        // Добавили тестовые данные, чтобы при старте в базе сразу были пациенты
        public static List<Patient> _patients = new List<Patient>
        {
            new Patient { Id = 1, Name = "Азамат", BirthDate = new DateTime(2004, 5, 12), Phone = "555-01-01-09" },
            new Patient { Id = 2, Name = "Айгерим", BirthDate = new DateTime(2005, 8, 20), Phone = "555-02-02-09" }
        };

        [HttpGet]
        public List<Patient> GetAll()
        {
            return _patients;
        }

        [HttpGet("{id}")]
        public ActionResult<Patient> GetById(int id)
        {
            Patient? patient = _patients.FirstOrDefault(p => p.Id == id);

            if (patient == null)
            {
                return NotFound();
            }

            return patient;
        }

        // НОВЫЙ МЕТОД (Пункт 4 из методички): Получить все справки конкретного пациента
        [HttpGet("{id}/spravki")]
        public ActionResult<List<Spravka>> GetSpravki(int id)
        {
            Patient? patient = _patients.FirstOrDefault(p => p.Id == id);

            if (patient == null)
            {
                return NotFound(new { message = "Пациент не найден" }); // Ошибка 404, если пациента нет
            }

            // Ищем справки, привязанные к этому пациенту (предполагаем, что в SpravkiController есть список Spravki или Types)
            var patientSpravki = SpravkiController.Types // или твой список справок
                .Where(s => s.PatientId == id)
                .ToList();

            return Ok(patientSpravki);
        }

        [HttpPost]
        public ActionResult<Patient> Create(Patient patient)
        {
            patient.Id = _patients.Count > 0 ? _patients.Max(p => p.Id) + 1 : 1;
            _patients.Add(patient);

            return CreatedAtAction(
                nameof(GetById),
                new { id = patient.Id },
                patient);
        }

        [HttpPut("{id}")]
        public ActionResult<Patient> Update(int id, Patient patient)
        {
            Patient? existingPatient = _patients.FirstOrDefault(p => p.Id == id);

            if (existingPatient == null)
            {
                return NotFound();
            }

            existingPatient.Name = patient.Name;
            existingPatient.BirthDate = patient.BirthDate;
            existingPatient.Phone = patient.Phone;

            return Ok(existingPatient);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Patient? patient = _patients.FirstOrDefault(p => p.Id == id);

            if (patient == null)
            {
                return NotFound();
            }

            _patients.Remove(patient);

            return NoContent();
        }
    }
}