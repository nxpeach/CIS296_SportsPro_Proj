using SportsPro.Data.Repositories;
using SportsPro.Models;

namespace SportsPro.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<Product> Products { get; }
        IRepository<Technician> Technicians { get; }
        IRepository<Country> Countries { get; }
        IRepository<Customer> Customers { get; }
        IRepository<Incident> Incidents { get; }
        IRepository<Registration> Registrations { get; }
        void Save();
    }
}
