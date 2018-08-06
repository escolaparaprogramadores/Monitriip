using IntegradorModel.Model;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using NewsGPS.Common.IoC;
using NewsGPS.Common.Service.CustomHeader;
using NewsGPS.Contracts.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;


namespace IntegradorRepositoryAzure
{
    public class AzureTablesRepository<TEntity> : ITablesRepository<TEntity>
        where TEntity : class, ITableEntity, new()
    {
        protected string _TableName;
        protected CloudTable _Table;
        protected static string CnnAzureStorageNGDados =
            "DefaultEndpointsProtocol=https;AccountName=quadritestetables;AccountKey=oDzsUoYVSie7iyYXw0qFb+HXinlUWGFB/jv8Y0je6d78Y8J1xWwu/3chDpj8AUZCHdZaZv1JfHRWuK1Cus2MKA==";

        public static ITablesRepository<TEntity> GetInstance()
        {
            try
            {
                return (ITablesRepository<TEntity>)IoCContainer.Resolve<ITablesRepository<TEntity>>();
            }
            catch
            {
                return null;
            }
        }

        public AzureTablesRepository(string tableName)
        {
            this._TableName = tableName;
            this.Init();
        }

        void Init()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
            CnnAzureStorageNGDados);

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            _Table = tableClient.GetTableReference(this._TableName);

            _Table.CreateIfNotExists();
        }

        public IQueryable<TEntity> GetQuery()
        {
            var ret = this.GetQuery(this.GetTableQuery());
            return ret;
        }

        public IQueryable<TEntity> GetQuery(TableQuery<TEntity> tableQuery)
        {
            var query = from e in tableQuery
                        select e;

            return query;
        }

        public TableQuery<TEntity> GetTableQuery()
        {
            var ret = _Table.CreateQuery<TEntity>();

            return ret;
        }

        public List<TEntity> ExecuteTableQuery(string tableQueryFilter)
        {
            var filter = this.GetTableQuery().Where(tableQueryFilter);
            var ret = _Table.ExecuteQuery(filter).ToList();
            return ret;
        }

        public IQueryable<TEntity> GetQueryByPartitionKey(string partitionKey)
        {
            var query = this.GetQuery();
            query = query.Where(x => x.PartitionKey == partitionKey);
            return query;
        }

        public IQueryable<TEntity> GetQueryByPartitionKeyAndRowKey(string partitionKey, string rowKey)
        {
            var query = this.GetQueryByPartitionKey(partitionKey);
            query = query.Where(x => x.RowKey == rowKey);
            return query;
        }

        public TEntity Get(string partitionKey, string rowKey)
        {
            var query = GetQueryByPartitionKeyAndRowKey(partitionKey, rowKey);

            return query.FirstOrDefault();
        }


        public TEntity Add(TEntity entity)
        {
            var insert = TableOperation.Insert(entity);
            _Table.Execute(insert);
            //var ret = this.Get(entity.PartitionKey, entity.RowKey);
            //return ret;

            return entity;
        }

        //public bool MultipleUpdate(List<TEntity> entitys)
        //{
        //try
        //{
        //    var list = entitys.OrderBy(x => x.PartitionKey);
        //    TableBatchOperation batchOperation = new TableBatchOperation();
        //    var count = 0;
        //    var lastPk = list.First().PartitionKey;
        //    foreach (var item in list)
        //    {
        //        count++;
        //        if (count == 100 || (!lastPk.Equals(item.PartitionKey)))
        //        {
        //            count = 0;
        //            _Table.ExecuteBatch(batchOperation);
        //            batchOperation = new TableBatchOperation();
        //        }
        //        lastPk = item.PartitionKey;
        //        batchOperation.u(item);
        //    }
        //    if (count > 0)
        //    {
        //        count = 0;
        //        _Table.ExecuteBatch(batchOperation);
        //    }

        //    return true;
        //}
        //catch (System.Exception ex)
        //{
        //    return false;
        //}
        //}

        public bool MultiplesAdd(List<TEntity> entities)
        {
            try
            {
                TableBatchOperation batchOperation = new TableBatchOperation();
                var count = 0;
                foreach (var item in entities)
                {

                    count++;

                    //var res = this.getAllEntities(this.GetQueryByPartitionKey(item.PartitionKey).ToList());//Get(item.PartitionKey, item.RowKey);

                    //foreach (var venda in res)
                    //{
                    //    venda.
                    //}

                    //if (res != null)
                    //{
                    //    var update = TableOperation.Merge(res);
                    //    _Table.Execute(update);
                    //}
                    //else
                    //{

                    if (count == 100)
                    {
                        count = 0;
                        _Table.ExecuteBatch(batchOperation);
                        batchOperation = new TableBatchOperation();
                    }
                    batchOperation.Insert(item);
                    //}
                }
                if (count > 0)
                {
                    count = 0;
                    _Table.ExecuteBatch(batchOperation);
                }

                return true;
            }
            catch (System.Exception ex)
            {
                foreach (var item in entities)
                {
                    try
                    {
                        
                        VendasRepository rep = new VendasRepository();

                        var modelExiste = rep.GetQuery().Where(x => x.PartitionKey == item.PartitionKey && x.RowKey == item.RowKey).FirstOrDefault();
                        if (modelExiste != null && (string.IsNullOrEmpty(modelExiste.retornoANTT)))
                            this.Add(item);
                    }
                    catch (System.Exception e)
                    {
                    }
                }
                return true;
            }
        }


        public bool MultiplesAddServices(List<TEntity> entities)
        {
            try
            {
                TableBatchOperation batchOperation = new TableBatchOperation();
                var count = 0;
                foreach (var item in entities)
                {

                    count++;

                    if (count == 100)
                    {
                        count = 0;
                        _Table.ExecuteBatch(batchOperation);
                        batchOperation = new TableBatchOperation();
                    }
                    batchOperation.Insert(item);
                    
                }
                if (count > 0)
                {
                    count = 0;
                    _Table.ExecuteBatch(batchOperation);
                }

                return true;
            }
            catch (System.Exception ex)
            {
                foreach (var item in entities)
                {
                    try
                    {
                        IntegracaoServicosRepository rep = new IntegracaoServicosRepository();

                        var modelExiste = rep.GetQuery().Where(x => x.PartitionKey == item.PartitionKey && x.RowKey == item.RowKey).FirstOrDefault();
                        if (modelExiste == null)
                            this.Add(item);
                    }
                    catch (System.Exception e)
                    {
                    }
                }
                return true;
            }
        }


        public List<TEntity> getAllEntities(List<TEntity> entities)
        {
            var list = new List<TEntity>();
            var listByPk = this.GetQueryByPartitionKey(entities.FirstOrDefault().PartitionKey).ToList();
            var selectedEntities = listByPk.Where(x => entities.Any(y => y.RowKey == x.RowKey)).ToList();

            foreach (var item in selectedEntities)
            {
                if (item != null)
                {
                    var obj = (object)item;
                    var venda = (IntegradorModel.Model.VendasModel)obj;

                    if (string.IsNullOrEmpty(venda.retornoANTT))
                    {
                        var entity = entities.Where(x => x.RowKey == item.RowKey).FirstOrDefault();
                        entity.ETag = item.ETag;
                        list.Add(entity);
                    }
                }
            }
            return list;
        }

        public List<TEntity> getAllEntitiesIntegracaoServicos(List<TEntity> entities)
        {
            var list = new List<TEntity>();
            var listByPk = this.GetQueryByPartitionKey(entities.FirstOrDefault().PartitionKey).ToList();
            var selectedEntities = listByPk.Where(x => entities.Any(y => y.RowKey == x.RowKey)).ToList();

            //foreach (var item in selectedEntities)
            //{
            //    if (item != null)
            //    {
            //        var obj = (object)item;
            //        var venda = (IntegradorModel.Model.IntegracaoServicos)obj;

            //        //if (string.IsNullOrEmpty(venda.))
            //        //{
            //        //    var entity = entities.Where(x => x.RowKey == item.RowKey).FirstOrDefault();
            //        //    entity.ETag = item.ETag;
            //        //    list.Add(entity);
            //        //}
            //    }
            //}
            return list;
        }

        public List<TEntity> getAllEntitiesExiste(List<TEntity> entities)
        {
            var list = new List<TEntity>();
            var listByPk = this.GetQueryByPartitionKey(entities.FirstOrDefault().PartitionKey).ToList();
            var selectedEntities = listByPk.Where(x => entities.Any(y => y.RowKey == x.RowKey)).ToList();

            foreach (var item in selectedEntities)
            {
                if (item != null)
                {
                    var obj = (object)item;
                    var venda = (IntegradorModel.Model.VendasModel)obj;

                    //if (string.IsNullOrEmpty(venda.retornoANTT))
                    // {
                    var entity = entities.Where(x => x.RowKey == item.RowKey).FirstOrDefault();
                    entity.ETag = item.ETag;
                    list.Add(entity);
                    //}
                }
            }
            return list;
        }
    

        public List<IntegracaoServicos> Mapper(List<TEntity> entities)
        {
            try { AutoMapper.Mapper.Initialize(cfg => cfg.CreateMap<List<TEntity>, List<IntegracaoServicos>>()); } catch (Exception ex) { }

            return AutoMapper.Mapper.Map<List<TEntity>, List<IntegracaoServicos>>(entities);
        }

        public bool MultiplesMerge(List<TEntity> entities, ref List<TEntity> embarques)
        {
            try
            {
                var list = getAllEntities(entities);
                var listaJaExiste = getAllEntitiesExiste(entities);

                TableBatchOperation batchOperation = new TableBatchOperation();
                var count = 0;
                embarques.AddRange(listaJaExiste);
                foreach (var item in list)
                {
                    count++;
                    if (count == 100)
                    {
                        count = 0;
                        _Table.ExecuteBatch(batchOperation);
                        batchOperation = new TableBatchOperation();
                    }
                    batchOperation.Merge(item);
                }
                if (count > 0)
                {
                    count = 0;
                    _Table.ExecuteBatch(batchOperation);
                }


                return true;
            }
            catch (System.Exception ex)
            {
                foreach (var item in entities)
                {
                    try
                    {
                        this.Update(item);
                    }
                    catch (System.Exception e)
                    {
                    }
                }
                return true;
            }
        }

        public TEntity Update(TEntity entity)
        {
            var update = TableOperation.Replace(entity);
            _Table.Execute(update);
            return entity;
        }

        public void Delete(string partitionKey, string rowKey)
        {
            var entity = this.Get(partitionKey, rowKey);
            this.Delete(partitionKey, rowKey);
        }

        public void Delete(TEntity entity)
        {
            if (entity.ETag == null)
            {
                entity = Get(entity.PartitionKey, entity.RowKey);
            }

            if (entity == null)
                return;

            var delete = TableOperation.Delete(entity);
            _Table.Execute(delete);
        }

        public int IdUsuario
        {
            get { return ClientContext.IdUsuario; }
        }

        public int IdContratante
        {
            get { return ClientContext.IdContratante; }
        }
    }
}
