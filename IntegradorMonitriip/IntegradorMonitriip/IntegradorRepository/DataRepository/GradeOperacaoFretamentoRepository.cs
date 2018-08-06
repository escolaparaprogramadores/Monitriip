using IntegradorRepository.LocalDatabase.Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace IntegradorRepository.DataRepository
{
    public class GradeOperacaoFretamentoRepository
    {
        public DatabaseContext _Context { get; set; }
        public GradeOperacaoFretamentoRepository()
        {
            _Context = new DatabaseContext();
        }

        public IQueryable<Ope_GradeOperacaoFretamento> GetQuery()
        {
            return _Context.GradeOperacaoFretamento.AsQueryable();
        }

        public Ope_GradeOperacaoFretamento GetGradeOperacao(int id_grade)
        {
            return GetQuery().Where(x => x.ID == id_grade).FirstOrDefault();
        }

        public void UpdateGrade(Ope_GradeOperacaoFretamento model)
        {
            this._Context.Entry(model).State = System.Data.Entity.EntityState.Modified;
            this._Context.ChangeTracker.DetectChanges();
            this._Context.SaveChanges();
        }
       
    }
}
