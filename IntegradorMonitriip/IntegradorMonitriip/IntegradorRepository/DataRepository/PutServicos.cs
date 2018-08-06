using IntegradorModel.Model;
using IntegradorRepository.LocalDatabase;
using IntegradorRepository.LocalDatabase.Repository.Entity;
using IntegradorRepositoryAzure;
using NewsGPS.Contracts.DTO;
using NewsGPS.Contracts.DTO.RJ;
using NewsGPS.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace IntegradorMonitriip.DataRepository
{
    public class PutServicos
    {
        //TODO
        //identificar os pontos de referencia
        //identificar as linhas
        //insertar as grades
        private static ServicoRepository repository;


        static bool validaDuplicidade(ServicoDTO item)
        {
            try
            {
                if (ultimo == null)
                {
                    ultimo = item;
                    return false;
                }
                if (string.IsNullOrEmpty(item.Data)
                 || string.IsNullOrEmpty(item.Destino)
                 || string.IsNullOrEmpty(item.HoraSaida)
                 || string.IsNullOrEmpty(item.Linha)
                 || string.IsNullOrEmpty(item.NumServico)
                 || string.IsNullOrEmpty(item.Origem)
                 )
                {
                    return false;
                }
                else if (item.Data.Trim().Equals(ultimo.Data.Trim())
                    && item.Destino.Trim().Equals(ultimo.Destino.Trim())
                    && item.HoraSaida.Trim().Equals(ultimo.HoraSaida.Trim())
                    && item.Linha.Trim().Equals(ultimo.Linha.Trim())
                    && item.NumServico.Trim().Equals(ultimo.NumServico.Trim())
                    && item.Origem.Trim().Equals(ultimo.Origem.Trim())
                    )
                {
                    return true;
                }

                ultimo = item;
                return false;
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        //static bool validaDuplicidadeLista(string SRVP, int Cliente)
        //{
        //    var ret = false;
        //    var tam = srvps.Count();
        //    for (int i = 0; i < tam; i++)
        //    {
        //        if (srvps[i].Trim().Equals(SRVP.Trim())
        //            && idCliente[i] == Cliente)
        //        {
        //            ret = true;
        //            break;
        //        }
        //    }

        //    return ret;
        //}

        //public static List<string> srvps = new List<string>();
        //public static List<int> idCliente = new List<int>();
        public static ServicoDTO ultimo;

        public class countErros
        {
            public int rotasCount { get; set; }
            public int linhasCount { get; set; }

            public string Prefixo { get; set; }
            public int IDOrigem { get; set; }
            public int IDDestino { get; set; }
            public string SRVP { get; set; }

        }
     

        public void salvarGrades(List<ServicoDTO> grades, int IDCliente)
        {
            int origemVazio = 0, destinoVazio = 0, origemDestinovazios = 0, linhaNula = 0,
                rotaNula = 0, semPrefixo = 0, entidadeNula = 0, idNaoIdentificado = 0;
            List<ServicoDTO> OrigemDestino = new List<ServicoDTO>();
            List<int> lstLinha = new List<int>();
            List<ServicoDTO> SemPrefixo = new List<ServicoDTO>();
            List<ServicoDTO> RotaNaoAssociada = new List<ServicoDTO>();
            List<Ope_GradeOperacaoOnibus> origem = new List<Ope_GradeOperacaoOnibus>();
            List<Ope_GradeOperacaoOnibus> destino = new List<Ope_GradeOperacaoOnibus>();
            List<countErros> listaErros = new List<countErros>();
            List<Ope_GradeOperacaoOnibus> lista = new List<Ope_GradeOperacaoOnibus>();
            List<IntegracaoServicos> listaServicosIntegrados = new List<IntegracaoServicos>();
            List<ServicosRelacionados> listaServicosRelacionados = new List<ServicosRelacionados>();

            repository = new ServicoRepository();
            using (var context = repository.dbContext)
            {
                var linhas = repository.getLineByClient(IDCliente, context);
                if (linhas == null || linhas.Count < 1)
                    return;

                var prefixos = linhas.Select(x => x.Prefixo != null ? x.Prefixo.Replace("-", "").Replace(" ", "") : "").ToList();

                foreach (var grade in grades)
                {

                    var modelServico = GetModelServicosIntegrados(grade, IDCliente, false, "");

                    try
                    {
                        if (validaDuplicidade(grade))
                            continue;

                        int IDOrigem = 0;
                        int IDDestino = 0;

                        repository.GetInfoPontoReferencia(grade.Origem, grade.Destino, IDCliente, ref IDOrigem, ref IDDestino, context);

                        if (IDOrigem == 0)
                            origemVazio++;
                        if (IDDestino == 0)
                            destinoVazio++;
                        if (IDOrigem == 0 || IDDestino == 0)
                        {
                            OrigemDestino.Add(grade);
                            origemDestinovazios++;
                 
                            modelServico.OrigemQuadri = Convert.ToString(IDOrigem);
                            modelServico.DestinoQuadri = Convert.ToString(IDDestino);
                            modelServico.Status = true;
                            modelServico.StatusErro = "A origem ou destino não são equivalentes com a base de dados";
                            listaServicosIntegrados.Add(modelServico);
                            continue;
                        }
                        else
                        {
                            modelServico.OrigemQuadri = Convert.ToString(IDOrigem);
                            modelServico.DestinoQuadri = Convert.ToString(IDDestino);
                        }

                        var prefixoLinha = grade.Linha.Replace("-", "").Replace(" ", "");
                        var prefixoLinhaLista = prefixos.FirstOrDefault(p => p.Replace("-", "").Replace(" ", "").Equals(prefixoLinha));

                        if (string.IsNullOrEmpty(prefixoLinhaLista))
                        {
                            SemPrefixo.Add(grade);
                            semPrefixo++;
                            modelServico.Status = true;
                            modelServico.StatusErro = "Não foi identificado o prefixo da linha na base de dados";
                            listaServicosIntegrados.Add(modelServico);
                            continue;
                        }

                        var idLinha = linhas.
                            Where(l => l.Prefixo.Replace("-", "").Replace(" ", "").Equals(prefixoLinhaLista)).
                            Select(p => p.IDLinha).
                            FirstOrDefault();

                        if (idLinha == 0)
                        {
                            idNaoIdentificado++;
                            modelServico.Status = true;
                            modelServico.StatusErro = "Linha não identificada na base de dados";
                            listaServicosIntegrados.Add(modelServico);
                            continue;
                        }


                        GPS_Linha_Rota Linha = new GPS_Linha_Rota();
                        repository.GetInfoLinha(idLinha, ref Linha, IDOrigem, IDDestino, linhas, context /*,ref listaErros, ref linhaNula, ref rotaNula*/);

                        if (Linha == null)
                        {
                            lstLinha.Add(idLinha);
                            RotaNaoAssociada.Add(grade);
                            rotaNula++;
                            modelServico.Status = true;
                            modelServico.StatusErro = "Não foi possível localizar linhas com a origem e destino recebida.";
                            listaServicosIntegrados.Add(modelServico);
                            continue;
                        }
                        //continue;

                        listaServicosIntegrados.Add(modelServico);

                        var entity = convertToEntity(grade, Linha, IDCliente);

                    
                        if (entity == null)
                            entidadeNula++;

                        if (entity != null)
                        {

                            if (grade.ServicosRelacionado != null && grade.ServicosRelacionado.Count > 0)
                            {
                                entity.ListaServicosRelacionados = new List<ServicosRelacionados>();

                                foreach (var item in grade.ServicosRelacionado)
                                {
                                    if (item.linha != null)
                                    {
                                        var m = new ServicosRelacionados()
                                        {
                                            //idGradeOperacao = (int)grade.Id,
                                            linha = item.linha.ToString(),
                                            numServico = item.numServico,
                                            origem = item.origem,
                                            destino = item.destino,
                                            codOrigem = item.codOrigem,
                                            codDestino = item.codDestino,
                                            assentos = item.assentos,
                                            piso = item.piso,
                                            prefixoLinha = item.prefixoLinha,
                                            GradeOperacao = entity.Ope_GradeOperacao
                                        };

                                        
                                        entity.ListaServicosRelacionados.Add(m);
                                    }
                                }
                            }
                           
                            lista.Add(entity);                            
                        }
                    }
                    catch (Exception ex)
                    {
                        listaServicosIntegrados.Add(modelServico);
                    }
                }                

                try
                {
                   
                    IntegracaoServicosRepository repServicosIntegrados = new IntegracaoServicosRepository();
                    repServicosIntegrados.SalvarServicosIntegrados(listaServicosIntegrados);
                }
                catch (Exception ex)
                {

                }

                ultimo = null;
                if (lista.Count > 0)
                {
                    repository.saveGrades(lista, IDCliente, context);
                }

            }
        }
        // FIM SALVA SERVIÇOS NA GRADE

        private IntegracaoServicos GetModelServicosIntegrados(ServicoDTO servicoDTO, int idCliente, bool status, string statusErro)
        {

            IntegracaoServicos model = new IntegracaoServicos();

            try
            {
                var dataHora = formataDataHora(servicoDTO.Data, servicoDTO.HoraSaida);
                model = new IntegracaoServicos(idCliente, servicoDTO.NumServico, Convert.ToDateTime(dataHora));
               // if (idCliente == 1581 && servicoDTO.NumServico.Equals("139701")  && servicoDTO.Data.Equals("180806"))
               // {
                model.IDCliente = idCliente;
                model.Data = servicoDTO.Data;
                model.HoraSaida = servicoDTO.HoraSaida;
                model.NumServico = servicoDTO.NumServico;
                model.OrigemRJ = servicoDTO.Origem;
                model.DestinoRJ = servicoDTO.Destino;
                model.LinhaRJ = servicoDTO.Linha;
                model.Motorista = servicoDTO.Motorista;
                model.Veiculo = servicoDTO.Veiculo;
                model.Status = status;
                model.StatusErro = statusErro;
                   

               // }
            }
            catch (Exception ex)
            {

            }

            return model;
        }

        private static int ObterTurno(string hora)
        {

            try
            {

                var hour = Convert.ToInt32(hora.Replace(":", "").Substring(2));

                if (hour > 4 && hour < 12)
                    return (int)ETurno.MANHA;

                if (hour >= 12 && hour < 19)
                    return (int)ETurno.TARDE;

            }
            catch (Exception ex)
            {

            }


            return 23208;
        }

        private static string formatarData(string data)
        {
            var _data = "20" + data.Substring(0, 2) + "-"
                + data.Substring(2, 2) + "-" + data.Substring(4);

            return _data;
        }
        private static string formataDataHoraNovoFormato(string data, string hora)
        {
            var _hora = hora.Substring(0, 2) + ":" +
            hora.Substring(2, 2) + ":" +
            hora.Substring(4, 2);

            var _data = data.Substring(0, 4) + "-"
                + data.Substring(4, 2) + "-" + data.Substring(6) + " "
                + _hora;

            return _data;
        }

        private static string formataDataHora(string data, string hora)
        {
            var _hora = "";
            var _data = "";
            int countHora = hora.Count(f => f == ':');
            int countData = hora.Count(f => f == '-');

            if (hora.Length == 5 && countHora == 1)
            {
                _hora = hora;

            }
            else
            {
                _hora = (hora.Length < 4) ?
                            "0" + hora.Substring(0, 1) + ":" +
                            hora.Substring(1) + ":00"
                                    : hora.Substring(0, 2) + ":" +
                                   hora.Substring(2) + ":00";
            }

            if (data.Length == 10 && countData == 2)
            {
                _data = data;
            }
            else
            {
                _data = "20" + data.Substring(0, 2) + "-"
                                + data.Substring(2, 2) + "-" + data.Substring(4) + " "
                                + _hora;
            }

            return _data;
        }

        //public interface IPontoReferencia : IIdNome
        //{
        //    int ID { get; }

        //    System.Data.Entity.Spatial.DbGeography GEO { get; }
        //    System.Data.Entity.Spatial.DbGeography PontoCentral { get; }

        //}
        private Ope_GradeOperacaoSeccao SeccaoFactory(Ope_GradeOperacaoOnibus grade)
        {
            var ret = new DatabaseContext().GradeOperacaoSeccao.Create();
            ret.IDGradeOperacao = grade.ID;
            ret.DataCriacao = DateTime.Now;

            if (grade.Ope_GradeOperacao.IDVeiculo.HasValue)
                ret.IDVeiculo = grade.Ope_GradeOperacao.IDVeiculo;

            return ret;
        }
        //private void GerarSeccoes(Ope_GradeOperacaoOnibus grade)
        //{
        //    if (grade.GPS_Linha_Rota == null)
        //        return;
        //    //grade.Seccoes
        //    //if (grade.Seccoes == null)
        //    grade.Ope_GradeOperacaoSeccao1 = new List<Ope_GradeOperacaoSeccao>();

        //    var rota = grade.GPS_Linha_Rota.GPS_Rota;

        //    //Daí pra frente, vamos sempre criar uma nova secção com o ponto inicial sendo o final da ultima e o ponto final sendo o ponto do loop...

        //    var seccao = SeccaoFactory(grade);
        //    //seccao.IDPontoOrigem = ultimaSeccao.IDPontoDestino;
        //    //seccao.DataPartidaPrevista = ultimaSeccao.DataChegadaPrevista;
        //    //seccao.IDPontoDestino = ponto.Ponto.ID;
        //    //seccao.DataChegadaPrevista = ((DateTimeOffset)seccao.DataPartidaPrevista).AddMinutes(tempoTrecho);
        //    grade.Ope_GradeOperacaoSeccao1.Add(seccao);

        //}

        void buscaMotorista(string cpf, ref Ope_GradeOperacaoOnibus onibus)
        {


            var logic = new GetEmpresas();

            var motorista = new Com_Empresa();

            try
            {
                motorista = logic.getMotorista(cpf);
            }
            catch (Exception ex)
            {

            }

            /*var seccao*/
            onibus.Ope_GradeOperacaoSeccao = new DatabaseContext().GradeOperacaoSeccao.Create();
            onibus.Ope_GradeOperacaoSeccao.IDGradeOperacao = onibus.ID;
            onibus.Ope_GradeOperacaoSeccao.IDPontoOrigem = onibus.GPS_Linha_Rota.GPS_Linha.GPS_PontoReferencia1.ID;//origem
            onibus.Ope_GradeOperacaoSeccao.IDPontoDestino = onibus.GPS_Linha_Rota.GPS_Linha.GPS_PontoReferencia.ID;
            onibus.Ope_GradeOperacaoSeccao.IDUsuarioCriacao = 103;

            if (motorista != null && motorista.ID > 0)
            {
                onibus.Ope_GradeOperacaoSeccao.IDMotorista = motorista.ID;
            }

            onibus.Ope_GradeOperacaoSeccao.isCancelado = false;
            onibus.Ope_GradeOperacaoSeccao.IDVeiculo = onibus.Ope_GradeOperacao.IDVeiculo;


            //onibus.Ope_GradeOperacaoSeccao = seccao;


        }

        void buscaVeiculo(string placa, ref Ope_GradeOperacaoOnibus onibus)
        {
            var logic = new GetEmpresas();
            var veiculo = logic.getVeiculo(placa);

            if (veiculo != null)
                onibus.Ope_GradeOperacao.IDVeiculo = veiculo.ID;
        }
        public Ope_GradeOperacaoOnibus convertToEntity(ServicoDTO dto, GPS_Linha_Rota Linha, int IDCliente)
        {
            try
            {
                Ope_GradeOperacao entity = new Ope_GradeOperacao();
                Ope_GradeOperacaoOnibus onibus = new Ope_GradeOperacaoOnibus();


                onibus.IdLinhaRota = Linha.ID;
                onibus.CodigoSRVP = dto.NumServico.Trim();
                onibus.IDTurno = ObterTurno(dto.HoraSaida);

                var dataHora = formataDataHora(dto.Data, dto.HoraSaida);

                entity.DataReferencia = Convert.ToDateTime(formatarData(dto.Data));
                entity.IDTipoGrade = (int)ETipoGradeOperacao.ONIBUS;
                entity.IDCliente = IDCliente;
                entity.IDUsuarioCriacao = 103; //Monit
                entity.ToleranciaAnterior = 30;
                entity.ToleranciaPosterior = 180;
                entity.DataPartidaPrevista = new DateTimeOffset(Convert.ToDateTime(dataHora), new TimeSpan(0, 0, 0));
                entity.DataChegadaPrevista = entity.DataPartidaPrevista.Value.AddHours(1);
                entity.DataCriacao = DateTime.UtcNow;
                entity.isCancelado = false;
                entity.IDContratante = IDCliente;
                

                if (Linha != null && Linha.GPS_Linha != null)
                {
                    entity.ToleranciaAnterior = Linha.GPS_Linha.ToleranciaAnterior;
                    entity.ToleranciaPosterior = Linha.GPS_Linha.ToleranciaPosterior;
                    entity.DataPartidaTolerancia = entity.DataPartidaPrevista.Value.AddMinutes(Linha.GPS_Linha.ToleranciaAnterior);
                    entity.DataChegadaTolerancia = entity.DataChegadaPrevista.Value.AddMinutes(Linha.GPS_Linha.ToleranciaPosterior);
                }
                else
                {
                    entity.DataPartidaTolerancia = entity.DataPartidaPrevista.Value.AddMinutes(30);
                    entity.DataChegadaTolerancia = entity.DataChegadaPrevista.Value.AddMinutes(180);
                }

                if (dto.DataChegadaPrevista != null)
                    entity.DataChegadaPrevista = new DateTimeOffset(Convert.ToDateTime(dto.DataChegadaPrevista), new TimeSpan(0, 0, 0));
                entity.Observacao = "Criada Automaticamente Pelo Sistema";

                /*Acrescentado depois da implementação do serviço buscaServicoDetalhado2*/
                entity.assentos = dto.assentos;
                entity.plataforma = dto.plataforma;
                entity.tipoViagem = dto.tipoViagem;
                entity.piso = dto.piso;
                


                onibus.Ope_GradeOperacao = entity;
                onibus.GPS_Linha_Rota = Linha;

            
                if (!string.IsNullOrEmpty(dto.Veiculo))
                {
                    buscaVeiculo(dto.Veiculo, ref onibus);
                }

                //if (!string.IsNullOrEmpty(dto.Motorista))
                //{
                /*Inicia uma Seccão*/
                buscaMotorista(dto.Motorista, ref onibus);
                //}

                return onibus;
            }
            catch (Exception ex)
            {
                string sSource;
                string sLog;
                string sEvent;
                sSource = "Integrador de dados - Empresa " + IDCliente;
                sLog = "Application";
                sEvent = "Erro ao converter elementos: " + ex.Message + " /n  Inner:" + ex.InnerException + " /n StackTrace" + ex.StackTrace;

                if (!EventLog.SourceExists(sSource))
                    EventLog.CreateEventSource(sSource, sLog);
                EventLog.WriteEntry(sSource, sEvent);
                EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 777);

                return null;
            }
        }
    }
}
