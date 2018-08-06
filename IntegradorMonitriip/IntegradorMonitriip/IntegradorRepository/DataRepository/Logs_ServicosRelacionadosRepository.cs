using IntegradorRepository.LocalDatabase.Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegradorRepository.DataRepository
{
    public class Logs_ServicosRelacionadosRepository
    {

        public DatabaseContext _Context { get; set; }
        public Logs_ServicosRelacionadosRepository()
        {
            _Context = new DatabaseContext();
        }

        public IQueryable<Logs_ServicosRelacionados> GetQuery()
        {
            return _Context.Logs_ServicosRelacionados.AsQueryable();
        }

        public void Add(Logs_ServicosRelacionados model)
        {
            this._Context.Entry(model).State = System.Data.Entity.EntityState.Added;
            this._Context.ChangeTracker.DetectChanges();
            this._Context.SaveChanges();
        }
    }
}
