using Data.Model;
using System;

namespace Data
{
    public class DataFactory : IDisposable, IDataFactory
    {
        private EventManagementEntities _mainContext;
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Obtiene el contexto de datos de la base de datos.
        /// </summary>
        /// <returns></returns>
        public EventManagementEntities GetMainContext()
        {
            return _mainContext ?? (_mainContext = new EventManagementEntities());
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (this._mainContext == null)
            {
                return;
            }

            this._mainContext.Dispose();
            this._mainContext = null;
        }
    }
}
