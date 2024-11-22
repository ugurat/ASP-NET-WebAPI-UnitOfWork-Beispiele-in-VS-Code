using System;
using System.Threading.Tasks;

namespace PersonApi.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Ermöglicht den Zugriff auf das PersonRepository.
        /// </summary>
        IPersonRepository PersonRepository { get; }

        /// <summary>
        /// Speichert Änderungen.
        /// </summary>
        void Commit();

        /// <summary>
        /// Speichert Änderungen asynchron.
        /// </summary>
        Task CommitAsync();
    }
}
