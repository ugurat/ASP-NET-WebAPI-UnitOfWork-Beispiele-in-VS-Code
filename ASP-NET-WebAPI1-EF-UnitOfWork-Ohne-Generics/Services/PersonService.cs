using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersonApi.Data;
using PersonApi.Repositories;

namespace PersonApi.Services
{
    public class PersonService : IPersonService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PersonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gibt alle Personen zurück.
        /// </summary>
        public async Task<IEnumerable<Person>> GetPeopleAsync()
        {
            return await _unitOfWork.PersonRepository.GetAllAsync();
        }

        /// <summary>
        /// Sucht Personen basierend auf Kriterien.
        /// </summary>
        public async Task<IEnumerable<Person>> SearchPeopleAsync(string searchTerm)
        {
            var allPeople = await _unitOfWork.PersonRepository.GetAllAsync();

            return allPeople.Where(p =>
                (p.Vorname != null && p.Vorname.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                (p.Nachname != null && p.Nachname.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                (p.Email != null && p.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
        }

        /// <summary>
        /// Gibt eine Person anhand der ID zurück.
        /// </summary>
        public async Task<Person> GetPersonByIdAsync(int id)
        {
            return await _unitOfWork.PersonRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Fügt eine neue Person hinzu.
        /// </summary>
        public async Task AddPersonAsync(Person person)
        {
            await _unitOfWork.PersonRepository.AddAsync(person);
            await _unitOfWork.CommitAsync();
        }

        /// <summary>
        /// Aktualisiert eine bestehende Person.
        /// </summary>
        public async Task UpdatePersonAsync(Person person)
        {
            _unitOfWork.PersonRepository.Update(person);
            await _unitOfWork.CommitAsync();
        }

        /// <summary>
        /// Löscht eine Person anhand der ID.
        /// </summary>
        public async Task DeletePersonAsync(int id)
        {
            var person = await _unitOfWork.PersonRepository.GetByIdAsync(id);
            if (person != null)
            {
                _unitOfWork.PersonRepository.Delete(person);
                await _unitOfWork.CommitAsync();
            }
        }
    }
}
