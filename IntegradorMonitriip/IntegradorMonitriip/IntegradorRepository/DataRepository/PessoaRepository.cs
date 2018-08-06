using IntegradorRepository.LocalDatabase.Repository.Entity;
using NewsGPS.Domain.Entities.Suporte;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegradorRepository.DataRepository
{
    public class PessoaRepository
    {
        public DatabaseContext _Context { get; set; }
        public PessoaRepository()
        {
            _Context = new DatabaseContext();
        }

        public IQueryable<Com_Empresa> GetQuery()
        {
            return _Context.Pessoa.AsQueryable();
        }

        public int GetIDPessoaByCPF(string cpf)
        {
            return GetQuery().Where(x => x.CnpjCpf.Replace("-", "").Replace(".", "").Trim().Equals(cpf.Trim())).Select(x=> x.ID).FirstOrDefault();
        }
    }
}
