using Microsoft.AspNetCore.Mvc;
using MedicalApi.Models;

namespace MedicalApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        // Статический список с тестовыми данными для старта (пункт 7: тестовые данные)
        public static List<Appointment> Appointments = new List<Appointment>
        {
            new Appointment { Id = 1, PatientId = 1, DoctorName = "Иванов И.И.", AppointmentDate = DateTime.Now.AddDays(1) },
            new Appointment { Id = 2, PatientId = 2, DoctorName = "Петрова А.С.", AppointmentDate = DateTime.Now.AddDays(2) }
        };

        // GET: api/appointments (с поддержкой поиска/фильтрации по patientId, пункт 7)
        [HttpGet]
        public IActionResult GetAll([FromQuery] int? patientId)
        {
            var query = Appointments.AsQueryable();

            if (patientId.HasValue)
            {
                query = query.Where(a => a.PatientId == patientId.Value);
            }

            return Ok(query.ToList());
        }

        // GET: api/appointments/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var appointment = Appointments.FirstOrDefault(a => a.Id == id);
            if (appointment == null)
            {
                return NotFound(new { message = "Запись на приём не найдена" });
            }
            return Ok(appointment);
        }

        // POST: api/appointments
        [HttpPost]
        public IActionResult Create([FromBody] Appointment newAppointment)
        {
            // Проверка: существует ли такой пациент в системе
            var patientExists = PatientsController._patients.Any(p => p.Id == newAppointment.PatientId);
            if (!patientExists)
            {
                return BadRequest(new { message = "Нельзя записать на приём несуществующего пациента (ошибка 400)" });
            }

            if (string.IsNullOrWhiteSpace(newAppointment.DoctorName))
            {
                return BadRequest(new { message = "Имя врача обязательно для заполнения" });
            }

            newAppointment.Id = Appointments.Count > 0 ? Appointments.Max(a => a.Id) + 1 : 1;
            Appointments.Add(newAppointment);

            return CreatedAtAction(nameof(GetById), new { id = newAppointment.Id }, newAppointment);
        }

        // PUT: api/appointments/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Appointment updatedAppointment)
        {
            var appointment = Appointments.FirstOrDefault(a => a.Id == id);
            if (appointment == null)
            {
                return NotFound(new { message = "Запись не найдена" });
            }

            appointment.DoctorName = updatedAppointment.DoctorName;
            appointment.AppointmentDate = updatedAppointment.AppointmentDate;
            appointment.Status = updatedAppointment.Status;

            return Ok(appointment);
        }

        // DELETE: api/appointments/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var appointment = Appointments.FirstOrDefault(a => a.Id == id);
            if (appointment == null)
            {
                return NotFound(new { message = "Запись не найдена" });
            }

            Appointments.Remove(appointment);
            return NoContent();
        }
    }
}