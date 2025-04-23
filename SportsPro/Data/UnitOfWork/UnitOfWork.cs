using SportsPro.Data.Repositories;
using SportsPro.Models;

namespace SportsPro.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private SportsProContext context;
        private IRepository<Product>? products;
        private IRepository<Technician>? technicians;
        private IRepository<Country>? countries;
        private IRepository<Customer>? customers;
        private IRepository<Incident>? incidents;
        private IRepository<Registration>? registrations;

        public UnitOfWork(SportsProContext ctx) => context = ctx;

        public IRepository<Product> Products => products ??= new Repository<Product>(context);
        public IRepository<Technician> Technicians => technicians ??= new Repository<Technician>(context);
        public IRepository<Country> Countries => countries ??= new Repository<Country>(context);
        public IRepository<Customer> Customers => customers ??= new Repository<Customer>(context);
        public IRepository<Incident> Incidents => incidents ??= new Repository<Incident>(context);
        public IRepository<Registration> Registrations => registrations ??= new Repository<Registration>(context);

        public void Save() => context.SaveChanges();
    }
}
