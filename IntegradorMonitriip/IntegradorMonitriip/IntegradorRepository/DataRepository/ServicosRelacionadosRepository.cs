using IntegradorRepository.LocalDatabase.Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace IntegradorRepository.DataRepository
{
    public class ServicosRelacionadosRepository
    {
        public DatabaseContext _Context { get; set; }
        public ServicosRelacionadosRepository()
        {
            _Context = new DatabaseContext();
        }

        public IQueryable<ServicosRelacionados> GetQuery()
        {
            return _Context.ServicosRelacionados.AsQueryable();
        }

        public bool CheckRegistro(long id, int gradeID)
        {
            var query = this.GetQuery().Where(x => x.ID == id && x.idGradeOperacao == gradeID).FirstOrDefault();

            if (query != null)
                return true;
            else
                return false;
        }

        public void Add(ServicosRelacionados model, DatabaseContext _context)
        {           

            _context.Entry(model).State = System.Data.Entity.EntityState.Added;
            _context.ChangeTracker.DetectChanges();
            _context.SaveChanges();
        }

        public void Update(ServicosRelacionados model, DatabaseContext context)
        {
            context.Entry(model).State = System.Data.Entity.EntityState.Modified;
            context.ChangeTracker.DetectChanges();
            context.SaveChanges();
        }

        public List<ServicosRelacionados> ListaServicosRelacionados(int idGradeOperacao)
        {
            return this.GetQuery().Where(x => x.idGradeOperacao == idGradeOperacao).ToList();
        }

    }
}
