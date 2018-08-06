using IntegradorRepository.LocalDatabase.Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace IntegradorRepository.DataRepository
{
    public class GradeOperacaoSeccaoRepository
    {
        public DatabaseContext _Context { get; set; }
        public GradeOperacaoSeccaoRepository()
        {
            _Context = new DatabaseContext();
        }

        public IQueryable<Ope_GradeOperacaoSeccao> GetQuery()
        {
            return _Context.GradeOperacaoSeccao.AsQueryable();
        }

        public List<Ope_GradeOperacaoSeccao> GetGradeOperacaoSeccaoMotoristaNull()
        {


            //var query = (from sec in GetQuery()
            //             join ope in _Context.GradeOperacao
            //             on sec.IDGradeOperacao equals ope.ID select new { sec, ope }).Where(x => (x.sec.IDMotorista == null && x.sec.IDVeiculo != null) || (x.sec.IDMotorista == null && x.sec.IDVeiculo == null) && x.ope.DataPartidaReal != null).ToList();

            var query = (from sec in GetQuery()
                         join ope in _Context.GradeOperacao
                         on sec.IDGradeOperacao equals ope.ID
                         select new { sec, ope }).Where(x => x.ope.DataPartidaReal != null).ToList();

            var list = new List<Ope_GradeOperacaoSeccao>();

            foreach (var item in query)
            {
                if(item.ope.PartitionKey != null)
                {
                    var model = new Ope_GradeOperacaoSeccao();
                    model = item.sec;
                    model.partitionKey = item.ope.PartitionKey;
                    list.Add(model);
                }
               
            }

            return list;
        }

        public void UpdateSeccao(Ope_GradeOperacaoSeccao model)
        {
            this._Context.Database.ExecuteSqlCommand("update ope_gradeoperacaoSeccao set IDMotorista = "+ model.IDMotorista+" where IDGradeOperacao = " + model.IDGradeOperacao);
            //this._Context.Entry(model).State = System.Data.Entity.EntityState.Modified;
            //this._Context.ChangeTracker.DetectChanges();
           
        }

        public Ope_GradeOperacaoSeccao GetSeccaoByIdGradeOperacao(int id)
        {
            return GetQuery().Where(x => x.IDGradeOperacao == id).FirstOrDefault();
        }

        public void CreateSeccao(Ope_GradeOperacaoSeccao model)
        {
            var context = new DatabaseContext();
         
                var transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = IsolationLevel.ReadCommitted;
                transactionOptions.Timeout = TransactionManager.MaximumTimeout;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    try
                    {
                        context.Configuration.AutoDetectChangesEnabled = false;                                 
                       
                        try
                        {
                           
                            context.Entry(model).State = System.Data.Entity.EntityState.Added;
                            context.ChangeTracker.DetectChanges();
                            context.SaveChanges();                            
                            context = new DatabaseContext();
                            context.Configuration.AutoDetectChangesEnabled = false;                              
                            
                        }
                        catch (Exception ex)
                        {
                            
                        }
                        
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        if (context != null)
                            context.Dispose();
                    }

                    scope.Complete();
                }
            }
            //this._Context.Database.ExecuteSqlCommand("update ope_gradeoperacaoSeccao set IDMotorista = " + model.IDMotorista + " where IDGradeOperacao = " + model.IDGradeOperacao);

            //this._Context.Entry(model).State = System.Data.Entity.EntityState.Modified;
            //this._Context.ChangeTracker.DetectChanges();

        
    }
}


