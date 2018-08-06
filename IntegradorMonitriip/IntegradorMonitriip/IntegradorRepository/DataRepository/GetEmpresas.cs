using IntegradorMonitriip.Model;
using IntegradorRepository.LocalDatabase;
using IntegradorRepository.LocalDatabase.Repository.Entity;
using NewsGPS.Contracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IntegradorMonitriip.DataRepository
{
    public class GetEmpresas
    {
        DatabaseContext context;


        //PASSO SERVIÇO 04 // RECUPERA A LISTA DE EMPRESAS
        public List<Codigo_Conexao> getCodigosEmpresas()
        {
            using (var context = new DatabaseContext())
            {
                var lista = context.CodCon.AsNoTracking().ToList();

               // lista = lista.Where(x => x.IDCliente == 18568).ToList(); //18568    Águia Branca

                //]lista = lista.Where(x => x.IDCliente == 1582).ToList(); //1582    Unesul de Transporte
                //lista = lista.Where(x => x.IDCliente == 1580).ToList(); //1580    Planalto Transportes Ltda
                //lista = lista.Where(x => x.IDCliente == 3793).ToList(); //3793    Viação Riodoce Ltda
               // lista = lista.Where(x => x.IDCliente == 1581).ToList(); //1581    Viação Ouro e Prata SA
                //lista = lista.Where(x => x.IDCliente == 2343).ToList(); //2343    Santo Anjo da Guarda
                //lista = lista.Where(x => x.IDCliente == 254).ToList();  //254     Empresa União de Transporte Limitada
                //lista = lista.Where(x => x.IDCliente == 143).ToList();  //143     Empresa de Transportes Andorinha S / A
                //lista = lista.Where(x => x.IDCliente == 7625).ToList();  //7625     unica
                //lista = lista.Where(x => x.IDCliente == 259).ToList();  //259     Viação Goiânia
                //lista = lista.Where(x => x.IDCliente == 257).ToList();  //257     Viação Araguarina
                //lista = lista.Where(x => x.IDCliente == 267).ToList();  //267     ZoomSat
                // lista = lista.Where(x => x.IDCliente == 3598).ToList(); //3598    Expresso Gardênia Ltda
                //lista = lista.Where(x => x.IDCliente == 136).ToList();  //136     EMPRESA DE ÔNIBUS PASSARO MARRON
                //lista = lista.Where(x => x.IDCliente == 1).ToList();    //1       Expresso Brasileiro
                //lista = lista.Where(x => x.IDCliente == 258).ToList();  //258     Rápido Marajó Ltda
                //lista = lista.Where(x => x.IDCliente == 736).ToList();  //736     Viação União Santa Cruz Ltda.
                //lista = lista.Where(x => x.IDCliente == 728).ToList();  //728     REAL ALAGOAS DE VIACAO LTDA.
                //lista = lista.Where(x => x.IDCliente == 3778).ToList(); //3778    Asatur Turismo Ltda
                //lista = lista.Where(x => x.IDCliente == 3598).ToList(); //3598    Expresso Gardênia Ltda
                //lista = lista.Where(x => x.IDCliente == 3802).ToList(); //3802    Helios Coletivos e Cargas LTDA
                //lista = lista.Where(x => x.IDCliente == 3847).ToList(); //3847    Nordeste Transporte
                //lista = lista.Where(x => x.IDCliente == 3684).ToList(); //3684    Viação Pássaro Verde Ltda
                //lista = lista.Where(x => x.IDCliente == 3683).ToList(); //3683    Viação Umuarama Ltda
                //lista = lista.Where(x => x.IDCliente == 3802).ToList(); //3683    Viação Umuarama Ltda
               // lista = lista.Where(x => x.IDCliente == 8703).ToList(); //8703    Guerino
                //lista = lista.Where(x => x.IDCliente == 3777).ToList(); //3777 Caburai
                //lista = lista.Where(x => x.IDCliente == 7941).ToList(); //7941 Princesa do norte
                //lista = lista.Where(x => x.IDCliente == 8161).ToList(); //8161 Empresa de Ônibus Nossa Senhora da Penha S.A.
                //lista = lista.Where(x => x.IDCliente == 7935).ToList(); //7935 Lopes Sul - Lopes & Oliveira Transportes e Turismo Ltda.
                //lista = lista.Where(x => x.IDCliente == 9290).ToList(); //9290 VIAÇÃO PERNAMBUCANA TRANSPORTES E TURISMO LTDA 
                //lista = lista.Where(x => x.IDCliente == 8162).ToList(); //8162 Expresso Maringá Ltda
                //lista = lista.Where(x => x.IDCliente == 254).ToList(); //254 Empresa União de Transporte Limitada
                //lista = lista.Where(x => x.IDCliente == 7938).ToList(); //7938 Expresso União Ltda
                //lista = lista.Where(x => x.IDCliente == 11163).ToList(); //11163 Empresas Reunidas Paulista de Transportes Ltda
                //lista = lista.Where(x => x.IDCliente == 16204).ToList(); //16204 Levare Transporte
                // lista = lista.Where(x => x.IDCliente == 12488).ToList(); //12488 Viação São Bento
                //lista = lista.Where(x => x.IDCliente == 15461).ToList(); //15461 Empresa Moreira
                //lista = lista.Where(x => x.IDCliente == 15460).ToList(); //15460 Tocantins Transporte e Turismo
                //lista = lista.Where(x => x.IDCliente == 15506).ToList(); //15506 Expresso Transporte
                //lista = lista.Where(x => x.IDCliente == 16739).ToList(); //16739 Transnorte

                //context.Dispose();
                return lista;
            }
        }





































        public static string getbyId(int idCliente)
        {
            var ret = "";
            using (var context = new DatabaseContext())
            {
                var entity = context.Pessoa.Find(idCliente);
                if (entity != null)
                {
                    ret = entity.Nome;
                }
            }
            return ret;
        }

        public List<BuscaDTO> getSRVPs(int idCliente)
        {
            try
            {
                List<BuscaDTO> ret = new List<BuscaDTO>();

                using (var context = new DatabaseContext())
                {
                    var dataAmanha = DateTime.UtcNow.AddDays(1).Date;
                    var range = DateTime.UtcNow.AddDays(-2).Date;

                    var clientes = context.GradeOperacao.
                        AsNoTracking().
                        Where(x => x.IDCliente == idCliente
                        && x.DataChegadaReal == null
                        && x.DataPartidaPrevista < dataAmanha
                        && x.DataChegadaTolerancia > range
                        ).
                        Select(x => x.ID).
                        ToList();

                    if (clientes.Count > 0)
                    {
                        ret = context.GradeOperacaoOnibus.
                            AsNoTracking().
                            Where(op => clientes.Contains(op.ID)).
                            AsEnumerable().
                            Select(op => new BuscaDTO
                            {
                                SRVP = op.CodigoSRVP != null ? op.CodigoSRVP.Trim() : "",
                                DataPartida = offSetToTime(op.Ope_GradeOperacao.DataPartidaPrevista.Value)
                            }).
                            ToList();
                    }

                    return ret;
                }

            }
            catch (System.Exception ex)
            {
                return new List<BuscaDTO>();
            }
        }

        private static DateTime offSetToTime(DateTimeOffset offset)
        {
            return Convert.ToDateTime(offset.DateTime);
        }

        //public bool isZoomSat(string cnpj)
        public bool isZoomSat(int id)
        {
            using (var context = new DatabaseContext())
            {
                //var qry = context.Pessoa;
                //var empresa = qry.Where(p => p.CnpjCpf.Replace(".", "").Replace("/", "").Replace("-", "").Equals(
                //cnpj.Replace(".", "").Replace("/", "").Replace("-", "")
                //))//.Select(p => p.ID)
                //.FirstOrDefault();
                //var id = empresa.ID;

                if (id == 267)
                    return true;

                var IDMatriz = context.Pessoa_PJ.AsNoTracking().Where(x => x.ID == id).
                    Select(x => x.IDMatriz).
                    FirstOrDefault() ?? 0;

                if (IDMatriz == 267)
                    return true;

                return false;
            }
        }

        public List<IdNomeDTO> getNomesDtos()
        {
            try
            {
                var context = new DatabaseContext();
                var lista = context.CodCon.AsNoTracking().ToList();

                List<int> ids = lista.Select(x => x.IDCliente).ToList();
                var qry = context.Pessoa;

                var res = qry.AsNoTracking().Where(x => ids.Any(y => y == x.ID)).ToList();

                var ret = res.Select(x => new IdNomeDTO
                {
                    Id = x.ID,
                    Nome = x.Nome
                }).ToList();

                return ret;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Com_Empresa getMotorista(string cpf)
        {
            cpf = cpf.Replace("/", "").Replace(".", "").Replace("-", "").Trim();
            if (cpf.Length <= 11)
                cpf = Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");

            var context = new DatabaseContext();

            var ret = context.Pessoa.FirstOrDefault(u => u.PJ == false && u.CnpjCpf.Equals(cpf));

            return ret;
        }

        public Tbl_Veiculo getVeiculo(string placa)
        {
            var context = new DatabaseContext();

            var ret = context.Veiculo.Where(v => v.Ativo == true && v.Placa.Trim().ToLower().Equals(placa.Trim().ToLower())).
                FirstOrDefault();

            return ret;
        }

        public List<Codigo_Conexao> getCodigosEmpresasGM()
        {

            /*15461-- Moreira | 15460-- Tocantins Transporte e Turismo | 15506 == Expresso Transporte Tutorial*/
            using (var context = new DatabaseContext())
            {
                var lista = context.CodCon.AsNoTracking().ToList();


               // return lista.Where(x => x.IDCliente == 15506).ToList();
                return lista = lista.Where(x => x.IDCliente == 15461 || x.IDCliente == 15460 || x.IDCliente == 15506).ToList();
            }
        }
    }
}
