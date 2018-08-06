using IntegradorRepository.LocalDatabase.Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace IntegradorRepository.DataRepository
{
    public class GradeOperacaoRepository
    {
        public DatabaseContext _Context { get; set; }
        public GradeOperacaoRepository()
        {
            _Context = new DatabaseContext();
        }
        
        public IQueryable<Ope_GradeOperacao> GetQuery()
        {
            return _Context.GradeOperacao.AsQueryable();
        }

        public Ope_GradeOperacao GetGradeOperacao(int id_grade)
        {
            return GetQuery().Where(x => x.ID == id_grade).FirstOrDefault();
        }

        public Ope_GradeOperacao GetGradeOperacaoByPartitionKey(string PartitionKey)
        {
            return GetQuery().Where(x => x.PartitionKey == PartitionKey).FirstOrDefault();
        }

        public void UpdateGrade(Ope_GradeOperacao model)
        {
            this._Context.Entry(model).State = System.Data.Entity.EntityState.Modified;
            this._Context.ChangeTracker.DetectChanges();
            this._Context.SaveChanges();
        }

        public void save(List<Ope_GradeOperacao> res/*, List<Ope_GradeOperacao> untouched*/)
        {
            var context = new DatabaseContext();
            var z = 0;
            if (res.Count() > 0)
            {
                var transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = IsolationLevel.ReadCommitted;
                transactionOptions.Timeout = TransactionManager.MaximumTimeout;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    try
                    {
                        context.Configuration.AutoDetectChangesEnabled = false;
                        //context.ChangeTracker
                        //context.Configuration.ValidateOnSaveEnabled = false;

                        int count = 0;
                        foreach (var entityToInsert in res)
                        {
                            try
                            {
                                //var original = untouched[z];
                                ++z;
                                ++count;
                                //var entity = entityToInsert as Ope_GradeOperacaoOnibus;
                                //var upd = context.GradeOperacao.Attach(entityToInsert);
                                
                                if (entityToInsert.IsAberto == false && entityToInsert.IsFechado == false
                                    && entityToInsert.IsTransbordoAberto == false && entityToInsert.IsTransbordoFechado)
                                    continue;

                                context.Entry(entityToInsert).State = System.Data.Entity.EntityState.Modified;

                                //if (context.Entry(entityToInsert).State != System.Data.Entity.EntityState.Modified)
                                //{
                                //    continue;
                                //}
                                //context.Entry(entityToInsert).State = System.Data.Entity.EntityState.Modified;

                                //context.Entry(entityToInsert.Ope_GradeOperacaoOnibus.GPS_Linha_Rota).State = System.Data.Entity.EntityState.Modified;
                                //.State = System.Data.Entity.EntityState.Modified;
                                //.Entry(entityToInsert).State = System.Data.Entity.EntityState.Modified;

                                if (count % 50 == 0)
                                {
                                    context.ChangeTracker.DetectChanges();
                                    context.SaveChanges();
                                    //context.Dispose();
                                    context = new DatabaseContext();
                                    context.Configuration.AutoDetectChangesEnabled = false;
                                    //context.Configuration.ValidateOnSaveEnabled = false;
                                }
                            }
                            catch (Exception ex)
                            {
                                try
                                {
                                    context.ChangeTracker.DetectChanges();
                                    context.SaveChanges();
                                    context = new DatabaseContext();
                                    context.Configuration.AutoDetectChangesEnabled = false;
                                    context.Entry(entityToInsert).State = System.Data.Entity.EntityState.Modified;
                                }
                                catch (Exception e)
                                {
                                }
                            }
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
        }

        public List<string> GetAllGradeOperacaoSemDataChegadaReal()
        {
            return GetQuery().Where(x => x.PartitionKey != null && x.DataChegadaReal == null).Select(x => x.PartitionKey).ToList();
        }

        public Ope_GradeOperacao GetGradeOperacao(string partitionKey)
        {
            return GetQuery().Where(x => x.PartitionKey.Equals(partitionKey)).FirstOrDefault();
        }

        public List<Ope_GradeOperacao> GetListaGradeOperacao()
        {
            DateTime data = DateTime.Now.AddDays(-3);        
            return GetQuery().Where(x => x.DataPartidaReal != null && x.DataPartidaPrevista.Value >= data && x.PartitionKey != null).ToList();
        }
    }
}
