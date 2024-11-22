
//Suandan itibaren ben Türkce yazacagim. Sen sadece Almanca cevap vereceksin. Code icinde de sadece Almanca yazacaksin. Baslayalim mi?

//Bir ASP.NET MVC projesi yapiyoruz. VS Code kullanacagiz. Entity Framework (EF) in Kombination mit den Designmustern Generic Repository und Unit of Work, um den Datenzugriff modular und verwaltbar zu gestalten. 

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonApi.Data;
using PersonApi.Repositories;

namespace PersonApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PersonController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gibt alle Personen zurück.
        /// </summary>
        /// <returns>Liste von Personen</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPeople()
        {
            var people = await _unitOfWork.PersonRepository.GetAllAsync();
            return Ok(people);
        }

        /// <summary>
        /// Gibt eine bestimmte Person anhand ihrer ID zurück.
        /// </summary>
        /// <param name="id">ID der Person</param>
        /// <returns>Person oder NotFound</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            var person = await _unitOfWork.PersonRepository.GetByIdAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        /// <summary>
        /// Sucht Personen anhand eines Suchbegriffs.
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Person>>> SearchPeople([FromQuery] string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest("Suchbegriff darf nicht leer sein.");
            }

            var people = await _unitOfWork.PersonRepository.SearchAsync(searchTerm);
            return Ok(people);
        }

        /// <summary>
        /// Erstellt eine neue Person.
        /// </summary>
        /// <param name="person">Zu erstellende Person</param>
        /// <returns>Erstellte Person</returns>
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            await _unitOfWork.PersonRepository.AddAsync(person);
            await _unitOfWork.CommitAsync();

            return CreatedAtAction(nameof(GetPerson), new { id = person.PersonId }, person);
        }

        /// <summary>
        /// Aktualisiert eine bestehende Person.
        /// </summary>
        /// <param name="id">ID der zu aktualisierenden Person</param>
        /// <param name="person">Aktualisierte Personendaten</param>
        /// <returns>NoContent oder BadRequest/NotFound</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(int id, Person person)
        {
            if (id != person.PersonId)
            {
                return BadRequest();
            }

            _unitOfWork.PersonRepository.Update(person);

            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _unitOfWork.PersonRepository.GetByIdAsync(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Löscht eine Person anhand ihrer ID.
        /// </summary>
        /// <param name="id">ID der zu löschenden Person</param>
        /// <returns>NoContent oder NotFound</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var person = await _unitOfWork.PersonRepository.GetByIdAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _unitOfWork.PersonRepository.Delete(person);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }
    }
}

