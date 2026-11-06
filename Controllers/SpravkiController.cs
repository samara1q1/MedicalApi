using Microsoft.AspNetCore.Mvc;
using MedicalApi.Models;

namespace MedicalApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpravkiController : ControllerBase
    {
        // Список видов справок (или выписанных справок)
        public static List<Spravka> Types = new List<Spravka>
        {
            new Spravka { Id = 1, Name = "Справка 083", Price = 100, PatientId = 1 },
            new Spravka { Id = 2, Name = "Санитарная книжка", Price = 750, PatientId = 2 },
        };

        // GET: api/spravki
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(Types);
        }

        // GET: api/spravki/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = Types.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                return NotFound(new { message = "Справка не найдена" });
            }
            return Ok(item);
        }

        // === ВСТАВЛЯЕМ СЮДА МЕТОД СТАТИСТИКИ (Пункт 7) ===
        [HttpGet("statistics")]
        public IActionResult GetStatistics()
        {
            int totalCount = Types.Count;
            decimal totalPrice = Types.Sum(s => s.Price); // Считаем общую сумму всех справок

            return Ok(new
            {
                Message = "Статистика клиники успешно сформирована",
                TotalSpravkiCount = totalCount,
                TotalRevenue = totalPrice, // Общая сумма
                ServerTime = DateTime.Now
            });
        }
    }
}