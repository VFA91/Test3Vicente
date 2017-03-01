using System;

namespace Domain.Core
{
    public interface IUnitOfWork : IDisposable
    {
        int Commit();
    }
}
