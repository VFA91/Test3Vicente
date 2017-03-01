using Data.Model;
using Domain.Core;

namespace Data
{
    /// <summary>
    /// Clase que sirve para mantener las transacciones de varias entidades.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDataFactory _dataFactory;
        private EventManagementEntities _mainContext;
        protected EventManagementEntities MainContext => _mainContext ?? (_mainContext = _dataFactory.GetMainContext());

        /// <summary>
        /// Constructor de la unidad de trabajo.
        /// </summary>
        /// <param name="dataFactory">Factoría que crea el contexto de datos</param>
        public UnitOfWork(IDataFactory dataFactory)
        {
            _dataFactory = dataFactory;
        }

        public void Dispose()
        {

        }

        /// <summary>
        /// Ejecuta la transacción en base de datos.
        /// </summary>
        public int Commit()
        {
            return MainContext.SaveChanges();
        }
    }
}
