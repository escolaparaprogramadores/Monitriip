

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Net;
using System.Data;
using System.Xml.Serialization;
using NewsGPS.Domain.Entities.Suporte;
using NewsGPS.Contracts.Enums;
using NewsGPS.Contracts.DTO.RJ;
using System.Data.Entity.Spatial;

namespace NewsGPS.Logic.Integracao.RJ
{
    public class ImportadorServico : ImportadorRJ
    {
        #region Propriedades

        protected override EEntidade Entidade
        {
            get { return EEntidade.GradeOperacao; }
        }

        protected override EEntidade EntidadeExterna
        {
            get { return EEntidade.Carroceria; }
        }

        #endregion Propriedades

        #region Construtores

        private ImportadorServico(int idCliente)
            : base(idCliente)
        {

        }

        #endregion Construtores

        #region Factory

        public static ImportadorServico Create(int idCliente)
        {
            var ret = new ImportadorServico(idCliente);
            return ret;
        }

        #endregion Factory
        public override void ProcessarUnitario(object item)
        {

        }
        public override void Processar()
        {
            DateTime data = DateTime.UtcNow;
            int dias = 1;

            if (data.Hour == 0) // meia noite faz de 3 dias
            {
                dias = 3;
            }
            else if (data.Hour == 12) // meio dia faz de 2 dias
            {
                dias = 2;
            }

            for (int i = 0; i < dias; i++)
            {
                XmlDocument Servicos;
                DateTime dataExecucao = DateTime.UtcNow;
                try
                {
                    Servicos = BaixaServicos(data.AddDays(i));
                }
                catch
                {
                    Servicos = null;
                }

                try
                {
                    if (Servicos != null)
                    {
                        var xmlReader = new XmlNodeReader(Servicos);


                        servicoes servicosRJ = null;


                        XmlSerializer serializer = new XmlSerializer(typeof(servicoes));
                        servicosRJ = (servicoes)serializer.Deserialize(xmlReader);



                        var servicos = servicosRJ.servicos.Select(row => new ServicoDTO
                        {
                            Data = row.data,
                            HoraSaida = row.horarioSaida,
                            Origem = row.origem,
                            Destino = row.destino,
                            Linha = row.linha,
                            NumServico = row.numServico,
                            //Status = row.status,
                            //Seccoes = row.Seccao.Select(ser => new SeccaoDTO
                            //{
                            //    Ordem = ser.ordem,
                            //    Hora = ser.hora,
                            //    Localidade = ser.localidade,
                            //    Prefixo = ser.prefixo,
                            //}).ToList()
                        }).ToList();


                        this.ImportarServicos(servicos);
                    }
                    if (i == 0)
                    {
                        //SalvarDataUltimaImportacao(this.IDIntegracao, dataExecucao);
                        this.DataUltimaIntegracao = data;
                    }
                }
                catch (Exception e)
                { }
            }
        }

        protected void ImportarServicos(List<ServicoDTO> listaEntidadeExterna)
        {
            //listaEntidadeExterna.ForEach(x => ImportarServico(x));
        }


        PontoReferencia ponto;

        //    public void ImportarServico(ServicoDTO Servico)
        //    {
        //        try
        //        {
        //            _Linha linha = new RJ._Linha();
        //            int tamHora = Servico.HoraSaida.ToString().Count();
        //            string hora = Servico.HoraSaida.ToString();
        //            for (int i = tamHora; i < 4; i++)
        //            {
        //                hora = "0" + hora;
        //            }
        //            string sql = string.Empty;

        //            DateTime data = new DateTime(
        //Convert.ToInt32("20" + Servico.Data.ToString().Substring(0, 2)), //ano
        //Convert.ToInt32(Servico.Data.ToString().Substring(2, 2)), //mes
        //Convert.ToInt32(Servico.Data.ToString().Substring(4)), //dia
        //Convert.ToInt32(hora.Substring(0, 2)), //hora
        //Convert.ToInt32(hora.Substring(2, 2)), //min
        //0);

        //            //Verifico se o ponto de destino está cadastrado
        //            //if (trecho.destino == grade.Rota.)
        //            PontoReferenciaRepository repPR = new PontoReferenciaRepository();
        //            ponto = repPR.Find(new Contracts.Filter.PontoReferenciaFilter() { IdClienteContexto = this.IDCliente, Descricao = Servico.Origem }).FirstOrDefault();
        //            if (ponto != null)
        //            {
        //                fuso = FusoHorarioRepository.BuscaFusoHorario(ponto.PontoCentral, data);
        //                //seccao.FusoChegadaPrevista = fuso.FusoHorario.Value;

        //                // data = data.ConverterTimeZoneClienteParaUTC();
        //                GradeOperacao_Srvp Srvp = new GradeOperacao_Srvp();
        //                GradeOperacao_Srvp_Servicos SrvpServico = new GradeOperacao_Srvp_Servicos();
        //                GradeOperacaoSrvpRepository rep = new GradeOperacaoSrvpRepository();
        //                var srvps = rep.Find(new Contracts.Filter.GradeOperacaoSrvpFilter()
        //                {
        //                    IDCliente = this.IDCliente,
        //                    Partida = data,
        //                    Origem = Servico.Origem,
        //                    Destino = Servico.Destino,



        //                });


        //                if (srvps.Count() == 0)
        //                {
        //                    #region Não encontrou o srvp
        //                    Srvp.IDCliente = this.IDCliente;
        //                    Srvp.DataReferencia = data.Date;

        //                    Srvp.Origem = Servico.Origem;
        //                    Srvp.Destino = Servico.Destino;



        //                    #region Linha
        //                    var Linhax = (from e in EFContext.GetDBSetAdmin<Linha>()
        //                                  join lr in EFContext.GetDBSetAdmin<LinhaRota>() on e.ID equals lr.IDLinha
        //                                  join r in EFContext.GetDBSetAdmin<Rota>() on lr.IDRota equals r.ID
        //                                  where e.Numero == Servico.Linha &&
        //                                                      e.IDCliente == this.Cliente.ID &&
        //                                                      (r.PontoDestino.Descricao.Contains(Servico.Destino) && r.PontoOrigem.Descricao.Contains(Servico.Origem))
        //                                  select new _Linha
        //                                  {
        //                                      IDLinhaRota = lr.ID,
        //                                      IDLinha = e.ID,
        //                                      ToleranciaAnterior = e.ToleranciaAnterior,
        //                                      ToleranciaPosterior = e.ToleranciaPosterior,

        //                                      Rotas = new _Rotas
        //                                      {
        //                                          //r.Comprimento,
        //                                          TempoViagem = lr.TempoViagem,
        //                                          IDRota = r.ID,
        //                                          IDTipoRota = lr.IDTipoRota,
        //                                          IDPontoOrigem = r.IDPontoOrigem,
        //                                          IDPontoDestino = r.IDPontoDestino,
        //                                          PontoCentralOrigem = r.PontoOrigem.PontoCentral,
        //                                          PontoCentralDestino = r.PontoDestino.PontoCentral
        //                                      }
        //                                  }
        //                                   );

        //                    var Linha = Linhax.ToList();
        //                    if (Linha.Count > 0)
        //                    {
        //                        linha = Linha.First();
        //                        Srvp.IDLinha = linha.IDLinha;

        //                        var rota = linha.Rotas;
        //                        Srvp.IDRota = rota.IDRota;
        //                        Srvp.IDSentido = rota.IDTipoRota;

        //                        Srvp.IDOrigem = rota.IDPontoOrigem;
        //                        Srvp.IDDestino = rota.IDPontoDestino;
        //                    }
        //                    #endregion

        //                    //Partida
        //                    Srvp.Partida = data.ConverterDateTimeToUTC((short)fuso.FusoHorario);

        //                    Srvp.FusoPartida = fuso.FusoHorario.Value;
        //                    SrvpServico.Linha = Servico.Linha;
        //                    SrvpServico.Servico = Servico.NumServico;
        //                    Srvp.Servicos.Add(SrvpServico);
        //                    #endregion Não encontrou o srvp

        //                }
        //                else
        //                {
        //                    #region encontrou SRVP
        //                    Srvp = srvps.FirstOrDefault();
        //                    if (Srvp.IDLinha == null || Srvp.IDRota == null || Srvp.IDSentido == null || Srvp.IDOrigem == null || Srvp.IDDestino == null)
        //                    {
        //                        #region Linha
        //                        var Linhax = (from e in EFContext.GetDBSetAdmin<Linha>()
        //                                      join lr in EFContext.GetDBSetAdmin<LinhaRota>() on e.ID equals lr.IDLinha
        //                                      join r in EFContext.GetDBSetAdmin<Rota>() on lr.IDRota equals r.ID
        //                                      where e.Numero == Servico.Linha &&
        //                                                          e.IDCliente == this.Cliente.ID &&
        //                                                          (r.PontoDestino.Descricao.Contains(Servico.Destino) && r.PontoOrigem.Descricao.Contains(Servico.Origem))
        //                                      select new _Linha
        //                                      {
        //                                          IDLinhaRota = lr.ID,
        //                                          IDLinha = e.ID,
        //                                          ToleranciaAnterior = e.ToleranciaAnterior,
        //                                          ToleranciaPosterior = e.ToleranciaPosterior,
        //                                          Rotas = new _Rotas
        //                                          {
        //                                              //r.Comprimento,
        //                                              TempoViagem = lr.TempoViagem,
        //                                              IDRota = r.ID,
        //                                              IDTipoRota = lr.IDTipoRota,
        //                                              IDPontoOrigem = r.IDPontoOrigem,
        //                                              IDPontoDestino = r.IDPontoDestino

        //                                          }
        //                                      }
        //                                       );

        //                        var Linha = Linhax.ToList();
        //                        if (Linha.Count > 0)
        //                        {
        //                            linha = Linha.First();
        //                            Srvp.IDLinha = linha.IDLinha;

        //                            var rota = linha.Rotas;
        //                            Srvp.IDRota = rota.IDRota;
        //                            Srvp.IDSentido = rota.IDTipoRota;
        //                            Srvp.IDOrigem = rota.IDPontoOrigem;
        //                            Srvp.IDDestino = rota.IDPontoDestino;

        //                        }
        //                        #endregion
        //                    }


        //                    var SrvpServicos = (Srvp.Servicos.Where(x => x.Linha.Trim() == Servico.Linha.Trim() && x.Servico.Trim() == Servico.NumServico.Trim()));

        //                    if (SrvpServicos.Count() == 0)
        //                    {
        //                        SrvpServico.Linha = Servico.Linha.Trim();
        //                        SrvpServico.Servico = Servico.NumServico.Trim();
        //                        Srvp.Servicos.Add(SrvpServico);
        //                    }
        //                    #endregion encontrou SRVP
        //                }

        //                // Gero a grade de operação 
        //                if (Srvp.IDGradeOperacao == null)
        //                {
        //                    #region Grade de Operacao
        //                    GradeOperacaoOnibusLogic goo = new GradeOperacaoOnibusLogic();
        //                    var entidade = goo.Find(new Contracts.Filter.GradeOperacaoOnibusFilter()
        //                    {
        //                        CodigoSRVP = Servico.NumServico,
        //                        DataPartidaPrevista = data.ConverterDateTimeToUTC((short)fuso.FusoHorario),
        //                        DestinoDescricao = Servico.Destino,
        //                        OrigemDescricao = Servico.Destino,
        //                        NumeroLinha = Servico.Linha,
        //                        IdClienteContexto = this.Cliente.ID
        //                    });

        //                    //Se nao encontrou grade de operação para este servico, verifica se tem este horario para esta linha
        //                    if (entidade.Result.Count() == 0)
        //                    {
        //                        entidade = goo.Find(new Contracts.Filter.GradeOperacaoOnibusFilter()
        //                        {
        //                            DataPartidaPrevista = data.ConverterDateTimeToUTC((short)fuso.FusoHorario),
        //                            DestinoDescricao = Servico.Destino,
        //                            OrigemDescricao = Servico.Origem,
        //                            NumeroLinha = Servico.Linha,
        //                            IdClienteContexto = this.Cliente.ID
        //                        });
        //                    }
        //                    // var entidade = RJRepository.g.Get(idEntidadeExterna);

        //                    if (entidade != null && linha.Rotas != null)
        //                    {


        //                        GradeOperacaoOnibusDTO dto = new GradeOperacaoOnibusDTO();
        //                        if (entidade.Result.Count() > 0)
        //                        {
        //                            dto = entidade.Result.FirstOrDefault();

        //                        }
        //                        else
        //                        {
        //                            dto.Id = 0;
        //                            dto.DataPartidaPrevista = data.ConverterDateTimeToUTC((short)fuso.FusoHorario);
        //                            dto.FusoPartidaPrevista = fuso.FusoHorario.Value;
        //                            dto.DataReferencia = data.Date;
        //                        }


        //                        dto.CodigoSRVP = string.Empty;
        //                        foreach (var lsrvp in Srvp.Servicos)
        //                        {
        //                            dto.CodigoSRVP = lsrvp.Servico.Trim() + "/";
        //                        }
        //                        dto.CodigoSRVP = dto.CodigoSRVP.Substring(0, dto.CodigoSRVP.Length - 1).Trim();

        //                        var ret = this.SalvarGradeOperacao(dto, Servico, linha);
        //                        Srvp.IDGradeOperacao = ret.Id;

        //                    }
        //                    #endregion Grade de Operacao
        //                }
        //                else
        //                {
        //                    #region Encontrou grade
        //                    GradeOperacaoOnibusLogic goo = new GradeOperacaoOnibusLogic();
        //                    var entidade = goo.Find(new Contracts.Filter.GradeOperacaoOnibusFilter()
        //                    {
        //                        Id = Srvp.IDGradeOperacao

        //                    });
        //                    GradeOperacaoOnibusDTO dto = new GradeOperacaoOnibusDTO();
        //                    if (entidade.Result.Count() > 0)
        //                    {
        //                        dto = entidade.Result.FirstOrDefault();

        //                    }

        //                    string _CodigoSRVP = string.Empty;
        //                    foreach (var lsrvp in Srvp.Servicos)
        //                    {
        //                        _CodigoSRVP = lsrvp.Servico.Trim() + "/";
        //                    }

        //                    _CodigoSRVP = _CodigoSRVP.Substring(0, _CodigoSRVP.Length - 1).Trim();

        //                    if (dto.CodigoSRVP.Trim() != _CodigoSRVP)
        //                    {
        //                        dto.CodigoSRVP = _CodigoSRVP;
        //                        goo.SaveOrUpdate(dto);
        //                    }
        //                    #endregion Encontrou grade
        //                }



        //                //Salva no banco
        //                if (Srvp.ID == 0)
        //                {
        //                    rep.Add(Srvp);
        //                    #region Baixar Passagens dos serviços novos

        //                    //foreach (var x in Srvp.Servicos)
        //                    //{
        //                    //    try
        //                    //    {
        //                    //        ImportadorRJ ret = ImportadorPassagem.Create(this.IDCliente, false, x.Servico);
        //                    //        ret.Processar();
        //                    //    }
        //                    //    catch (Exception e)
        //                    //    { }
        //                    //}
        //                    #endregion Baixar Passagens dos serviços novos
        //                }
        //                else
        //                    rep.Update(Srvp);


        //                rep.SaveChanges();
        //            }

        //        }
        //        catch (Exception e)
        //        { }
        //    }

        //public GradeOperacaoOnibusDTO SalvarGradeOperacao(GradeOperacaoOnibusDTO dto, ServicoDTO Servico, _Linha linha)
        //{

        //    int? Id_Linha = null;
        //    int? Id_Rota = null;

        //    try
        //    {


        //        GradeOperacaoOnibusLogic logic = new GradeOperacaoOnibusLogic();
        //        //Não existe grade salva
        //        if (dto.Id == 0)
        //        {
        //            #region inclusao Grade
        //            int tamHora = Servico.HoraSaida.ToString().Count();
        //            string hora = Servico.HoraSaida.ToString();
        //            for (int i = tamHora; i < 4; i++)
        //            {
        //                hora = "0" + hora;
        //            }
        //            string sql = string.Empty;

        //            //DateTime dataPartida = new DateTime(
        //            //        Convert.ToInt32("20" + Servico.Data.ToString().Substring(0, 2)), //ano
        //            //        Convert.ToInt32(Servico.Data.ToString().Substring(2, 2)), //mes
        //            //        Convert.ToInt32(Servico.Data.ToString().Substring(4)), //dia
        //            //        Convert.ToInt32(hora.Substring(0, 2)), //hora
        //            //        Convert.ToInt32(hora.Substring(2, 2)), //min
        //            //        0);

        //            //dto.DataPartidaPrevista = dataPartida;

        //            PontoReferenciaRepository repPR = new PontoReferenciaRepository();
        //            //ponto = repPR.Find(new Contracts.Filter.PontoReferenciaFilter() { IdClienteContexto = this.IDCliente, Descricao = Servico.Origem }).FirstOrDefault();
        //            //fuso = FusoHorarioRepository.BuscaFusoHorario(ponto.PontoCentral, dataPartida);
        //            //dto.FusoPartidaPrevista = fuso.FusoHorario.Value;

        //            dto.Cliente = this.Cliente.ToIdNomeDTO();
        //            dto.Cobrador = new IdNomeDTO();
        //            dto.Motorista = new IdNomeDTO();
        //            dto.DataCriacao = DateTime.UtcNow.SetUtcKind();
        //            dto.Veiculo = new IdNomeDTO();

        //            #region Encontra Linha


        //            Id_Linha = linha.IDLinha;


        //            var rota = linha.Rotas;
        //            Id_Rota = rota.IDRota;
        //            #endregion Encontra Linha
        //            if (Id_Linha == null || Id_Rota == null || linha.IDLinhaRota == null)
        //                return null;

        //            DateTime dataChegada;
        //            if (rota.TempoViagem.HasValue)
        //            {
        //                dataChegada =dto.DataPartidaPrevista.Value.AddMinutes((int)rota.TempoViagem);
        //            }
        //            else
        //            {
        //                //Por nao receber a tempo de viagem, defini tempo de 10 hrs
        //                dataChegada = dto.DataPartidaPrevista.Value.AddMinutes(600);
        //            }



        //            if (dto.Linha == null)
        //                dto.Linha = new IdNomeDTO() { Id = Id_Linha };
        //            else
        //                dto.Linha.Id = Id_Linha;

        //            if (dto.Rota == null)
        //                dto.Rota = new RotaMapaDTO() { Id = Id_Rota };
        //            else
        //                dto.Rota.Id = Id_Rota;

        //            if (dto.LinhaRota == null)
        //                dto.LinhaRota = new IdNomeDTO() { Id = linha.IDLinhaRota };
        //            else
        //                dto.LinhaRota.Id = linha.IDLinhaRota;

        //            dto.ToleranciaAnterior = linha.ToleranciaAnterior;
        //            dto.ToleranciaPosterior = linha.ToleranciaPosterior;
        //            //dto.DataReferencia = dataPartida.Date;
        //            //dto.DataPartidaPrevista = dataPartida;
        //            dto.DataChegadaPrevista = dataChegada;
        //            dto.HoraPartidaPrevista = dto.DataPartidaPrevista;
        //            dto.HoraChegadaPrevista = dataChegada;
        //            dto.TipoJornada = new IdNomeDTO() { Id = (int)ETipoJornada.IMPORTACAO };
        //            dto.UsuarioCriacao = new IdNomeDTO() { Id = 2 };

        //            ponto = repPR.Find(new Contracts.Filter.PontoReferenciaFilter() { IdClienteContexto = this.IDCliente, Descricao = Servico.Destino }).FirstOrDefault();
        //            fuso = FusoHorarioRepository.BuscaFusoHorario(ponto.PontoCentral, dataChegada);
        //            dto.FusoChegadaPrevista = fuso.FusoHorario.Value;


        //            if (dto.TipoGradeOperecacao == null)
        //                dto.TipoGradeOperecacao = new IdNomeDTO() { Id = (int)ETipoOperacao.RODOVIARIO };
        //            else
        //                dto.TipoGradeOperecacao.Id = (int)ETipoOperacao.RODOVIARIO;

        //            var retdto = logic.InsertGradeOperacaoRJ(dto);
        //            dto = retdto.Result;
        //            #endregion inclusao Grade
        //        }
        //        else
        //        {
        //            var retdto = logic.InsertGradeOperacaoRJ(dto);
        //            dto = retdto.Result;
        //        }



        //    }
        //    catch (Exception ex)
        //    {
        //        NewsGPS.Logic.BusinessRules.Log.PublicarErro(ex);
        //        LogarErro(ex, String.Format("Erro na integração {1}. Cliente: {2} Partida: {3}  Servico: {0}", Servico.NumServico, this.EntidadeExterna.ToString(), this.IDCliente, Servico.Data));

        //    }
        //    //} //end transaction.. commit or rollback.. 

        //    return dto;

        //}

        private XmlDocument BaixaServicos(DateTime quando)
        {
            try
            {
                string[] detalhe = this.SistemaCliente.userToken.Split(':');

                string requestUrl = string.Format("{0}WSMonitriipRJ/busca/buscaServico/{1}/{3}/{2}", this.SistemaCliente.URL, detalhe[0], detalhe[1], quando.ToString("yyMMdd"));


                XmlDocument dados = BuscarDados(requestUrl);

                return dados;
            }
            catch
            {
                throw;
            }


        }

        //Chama o webservice
        private XmlDocument BuscarDados(string requestUrl)
        {
            NewsGPS.Logic.BusinessRules.Log.PublicarTrace(string.Format("{0}", requestUrl), "RJ");
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                request.Timeout = 250000;

                String username = "newgps";
                String password = "rjnewgpsrj";
                String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
                request.Headers.Add("Authorization", "Basic " + encoded);

                //request.Credentials = new NetworkCredential(username, password);
                //CookieContainer myContainer = new CookieContainer();
                //request.CookieContainer = myContainer;
                //request.PreAuthenticate = true;

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());
                this.DataUltimaIntegracao = DateTime.UtcNow.AddMinutes(-1);
                return xmlDoc;

            }
            catch (Exception e)
            {
                NewsGPS.Logic.BusinessRules.Log.PublicarTrace(string.Format("{0} - {1}", requestUrl, e.Message), "RJ");
                throw;

            }
        }
    }

    public class _Linha
    {

        public int IDLinha { get; set; }
        public int? IDLinhaRota { get; set; }
        public short ToleranciaAnterior { get; set; }
        public short ToleranciaPosterior { get; set; }

        public _Rotas Rotas { get; set; }


    }

    public class _Rotas
    {

        public int? TempoViagem { get; set; }
        public int IDRota { get; set; }
        public int IDTipoRota { get; set; }
        public int IDPontoOrigem { get; set; }
        public int IDPontoDestino { get; set; }
        public DbGeography PontoCentralOrigem { get; set; }

        public DbGeography PontoCentralDestino { get; set; }
    }
}
