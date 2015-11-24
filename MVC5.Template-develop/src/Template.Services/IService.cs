using System;

namespace Template.Services
{
    public interface IService : IDisposable
    {
        String CurrentAccountId { get; set; }
    }
}