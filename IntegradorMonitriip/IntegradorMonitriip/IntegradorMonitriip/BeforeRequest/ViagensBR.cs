using antt.gov.br.monitriip.v1._0;
using IntegradorModel.Model;
using IntegradorModel.Model.XmlModel;
using IntegradorMonitriip.Model;
using IntegradorRepository.DataRepository;
using IntegradorRepository.LocalDatabase.Repository.Entity;
using IntegradorRepositoryAzure;
using IntegradorRequestWeb.RequestWeb;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Xml;

namespace IntegradorMonitriip.BeforeRequest
{
    public class ViagensBR : ViagensRW
    {
        public static void EnviaANTT(ViagemModel item, string tipoLog, string QueueName, string prefixo)
        {

            switch (QueueName)
            {
                case "iniciofimviagemregular":
                    if (tipoLog.Equals("inicioFimViagemRegular"))
                    {
                        EnviaLogInicioFimViagemRegular(item);
                    }
                    else if (tipoLog.Equals("inicioFimViagemFretado"))
                    {
                        EnviaLogInicioFimViagemFretado(item);
                    }

                    break;
                case "jornadamotorista":
                    EnviaLogJornadaMotorista(item);
                    break;
                case "detectorparada":
                    EnviaLogDetectorParada(item);
                    break;
                case "velocidadetempolocalizacao":
                    EnviaLogVelocidadeTempoLocalizacao(item);
                    break;
                case "leitorbilheteembarque":
                    EnviaLogLeitorBilheteEmbarque(item, prefixo);
                    break;
                default:
                    break;
            }

        }
        public static string converterJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
        }

        public static inicioFimViagemRegular SendAntt(ref resultadoOperacao resultadoOperacao, ViagemModel item)
        {

            GradeOperacaoRepository ope_rep = new GradeOperacaoRepository();
            var grade = new Ope_GradeOperacao();

            if (item.id_gradeoperacao > 0)
                grade = ope_rep.GetGradeOperacao(item.id_gradeoperacao);


            inicioFimViagemRegular x = new inicioFimViagemRegular();


            x.cnpjEmpresaTransporte = item.cnpjEmpresa.Trim();
            x.tipoRegistroViagem = item.codigoTipoRegistroViagem.ToString().Trim();

            if (grade != null)
                x.codigoTipoViagem = string.IsNullOrEmpty(grade.tipoViagem) ? "00" : grade.tipoViagem;
            else
                x.codigoTipoViagem = "00";

            x.codigoSentidoLinha = (item.codigoSentidoLinha.ToString()).Trim();
            x.idLog = (item.codigoTipoLogID.ToString());
            x.dataProgramadaViagem = item.dataProgramada;
            x.horaProgramadaViagem = item.horaProgramada;
            x.dataHoraEvento = item.dataHoraEvento.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
            x.imei = item.IMEI;
            x.latitude = item.latitude;
            x.longitude = item.longitude;
            x.pdop = item.pdop.ToString();
            x.placaVeiculo = item.placaVeiculo;
            x.identificacaoLinha = item.identificacaoLinha;

            resultadoOperacao ret = new resultadoOperacao();

            ret = EnviaANTTWebInicioFimViagemRegular(x, item.IDCliente);

            if (ret != null)
            {
                item.idTransacao = ret.idTransacao;
                item.isErro = ret.erros != null ? true : false;
                item.Erros = converterJson(ret);
            }

            resultadoOperacao = ret;

            item.dataEnvioAntt = DateTime.UtcNow;

            ViagensRepository rep = new ViagensRepository();
            item.RowKey = GetRowKey(item.dataHoraEvento, item.placaVeiculo, item.codigoTipoLogID);
            rep.UpdateLogViagem(item);

            ServicosRelacionadosRepository repServRel = new ServicosRelacionadosRepository();
            var listaServicosRelacionados = repServRel.ListaServicosRelacionados(item.id_gradeoperacao);

            Logs_ServicosRelacionadosRepository repRelacionados = new Logs_ServicosRelacionadosRepository();
            foreach (var serv in listaServicosRelacionados)
            {
              
                var dataHoraViagem = new DateTime(int.Parse(item.dataProgramada.Substring(0, 4)), int.Parse(item.dataProgramada.Substring(4, 2)), int.Parse(item.dataProgramada.Substring(6, 2)), int.Parse(item.horaProgramada.Substring(0, 2)), int.Parse(item.horaProgramada.Substring(2, 2)), int.Parse(item.horaProgramada.Substring(4, 2)));


                Logs_ServicosRelacionados model = new Logs_ServicosRelacionados();
                model.ID_ServicoRelacionado = serv.ID;
                model.IDCliente = grade.IDCliente;
                model.IMEI = grade.IMEI;
                model.codigoTipoLogID = item.codigoTipoLogID;
                model.placaVeiculo = item.placaVeiculo;
                model.cnpjEmpresa = item.cnpjEmpresa;
                model.latitude = item.latitude;
                model.longitude = item.longitude;
                model.pdop = item.pdop;
                model.dataHoraEvento = item.dataHoraEvento;
                model.dataHoraViagem = dataHoraViagem;
                model.dataEnvioAntt = DateTime.Now;
                model.codigoRetorno = item.codigoRetorno;
                model.mensagem = item.mensagem;
                model.identificacaoLinha = serv.prefixoLinha;
                model.dataProgramada = item.dataProgramada;
                model.horaProgramada = item.horaProgramada;
                model.codigoTipoRegistroViagem = item.codigoTipoRegistroViagem;
                model.codigoSentidoLinha = item.codigoTipoRegistroViagem;
                model.autorizacaoViagem = item.autorizacaoViagem;
                model.tipoViagem = grade.tipoViagem;

                inicioFimViagemRegular y = new inicioFimViagemRegular();

                y.cnpjEmpresaTransporte = model.cnpjEmpresa.Trim();
                y.tipoRegistroViagem = model.codigoTipoRegistroViagem.ToString().Trim();

                if (grade != null)
                    y.codigoTipoViagem = string.IsNullOrEmpty(model.tipoViagem) ? "00" : model.tipoViagem;
                else
                    y.codigoTipoViagem = "00";

                y.codigoSentidoLinha = (model.codigoSentidoLinha.ToString()).Trim();
                y.idLog = (model.codigoTipoLogID.ToString());
                y.dataProgramadaViagem = model.dataProgramada;
                y.horaProgramadaViagem = model.horaProgramada;
                y.dataHoraEvento = model.dataHoraEvento.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
                y.imei = model.IMEI.Trim();
                y.latitude = model.latitude;
                y.longitude = model.longitude;
                y.pdop = model.pdop.ToString();
                y.placaVeiculo = model.placaVeiculo;
                y.identificacaoLinha = model.identificacaoLinha;

                resultadoOperacao retRelacionados = new resultadoOperacao();

                retRelacionados = EnviaANTTWebInicioFimViagemRegular(y, item.IDCliente);

                if (retRelacionados != null)
                {
                    model.idTransacao = retRelacionados.idTransacao;
                    model.isErro = retRelacionados.erros != null ? true : false;
                    model.Erros = converterJson(retRelacionados);
                }

               
                repRelacionados.Add(model);
            }
        
            return x;
        }

        public static void EnviaLogInicioFimViagemRegular(ViagemModel item)
        {
            try
            {

                GradeOperacaoRepository ope_rep = new GradeOperacaoRepository();
                var grade = new Ope_GradeOperacao();

                inicioFimViagemRegular x = new inicioFimViagemRegular();        
                resultadoOperacao ret = new resultadoOperacao();               
                ViagensRepository rep = new ViagensRepository();                         

                x = SendAntt(ref ret, item);

                if(item.id_gradeoperacao > 0)
                   grade = ope_rep.GetGradeOperacao(item.id_gradeoperacao);

                try
                {

                    if (grade != null && grade.ID > 0)
                    {

                        if (item.codigoTipoRegistroViagem == 0)
                            grade.IsFechado = true;
                        else if (item.codigoTipoRegistroViagem == 1)
                            grade.IsAberto = true;
                        else if (item.codigoTipoRegistroViagem == 3 && item.isTransbordo == true)
                            grade.IsTransbordoAberto = true;
                        else if (item.codigoTipoRegistroViagem == 2 && item.isTransbordo == true)
                            grade.IsTransbordoFechado = true;

                        ope_rep.UpdateGrade(grade);
                    }
                    else
                    {

                        var model = rep.getLogInicioViagem(item.PartitionKey);

                        if (model != null)
                        {

                            grade = ope_rep.GetGradeOperacaoByPartitionKey(item.PartitionKey);

                            if (grade != null)
                            {

                                if (item.codigoTipoRegistroViagem == 0)
                                    grade.IsFechado = true;
                                else if (item.codigoTipoRegistroViagem == 1)
                                    grade.IsAberto = true;
                                else if (item.codigoTipoRegistroViagem == 3 && item.isTransbordo == true)
                                    grade.IsTransbordoAberto = true;
                                else if (item.codigoTipoRegistroViagem == 2 && item.isTransbordo == true)
                                    grade.IsTransbordoFechado = true;

                                ope_rep.UpdateGrade(grade);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {

                }

            }
            catch (Exception ex)
            {
                erro[] erro = new erro[] {
                    new erro() {
                    descricao = "Mensagem: " + ex.Message + " Erro: " + ex.ToString()
                    }};
                var _dto = new resultadoOperacao() { erros = erro };

                item.Erros = converterJson(_dto);
                item.dataEnvioAntt = DateTime.UtcNow;

                ViagensRepository rep = new ViagensRepository();
                item.RowKey = GetRowKey(item.dataHoraEvento, item.placaVeiculo, item.codigoTipoLogID);
                rep.UpdateLogViagem(item);

                try
                {

                    GradeOperacaoRepository ope_rep = new GradeOperacaoRepository();
                    var grade = ope_rep.GetGradeOperacao(item.id_gradeoperacao);

                    if (grade != null)
                    {
                        if (item.codigoTipoRegistroViagem == 0)
                            grade.IsFechado = false;
                        else if (item.codigoTipoRegistroViagem == 1)
                            grade.IsAberto = false;
                        else if (item.codigoTipoRegistroViagem == 3 && item.isTransbordo == true)
                            grade.IsTransbordoAberto = false;
                        else if (item.codigoTipoRegistroViagem == 2 && item.isTransbordo == true)
                            grade.IsTransbordoFechado = false;

                        ope_rep.UpdateGrade(grade);
                    }

                }
                catch (Exception e)
                {

                }

            }
        }

        public static void EnviaLogInicioFimViagemFretado(ViagemModel item)
        {
            try
            {
                inicioFimViagemFretado x = new inicioFimViagemFretado();
                x.cnpjEmpresaTransporte = item.cnpjEmpresa.Trim();
                x.tipoRegistroViagem = (item.codigoTipoRegistroViagem.ToString().Trim());
                x.autorizacaoViagem = item.autorizacaoViagem.Replace("-", "").Trim();
                x.sentidoLinha = (item.codigoSentidoLinha.ToString()).Trim();
                x.idLog = ("0" + item.codigoTipoLogID.ToString()).Trim();
                x.dataHoraEvento = Convert.ToDateTime(item.dataHoraEvento).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
                x.imei = item.IMEI.Trim();
                x.latitude = item.latitude.ToString();
                x.longitude = item.longitude.ToString();
                x.pdop = item.pdop.ToString();
                x.placaVeiculo = item.placaVeiculo.Trim();

                resultadoOperacao ret = new resultadoOperacao();

                ret = EnviaANTTWebInicioFimViagemFretado(x, item.IDCliente);

                if (ret != null)
                {
                    item.idTransacao = ret.idTransacao;
                    item.isErro = ret.erros != null ? true : false;
                    item.Erros = converterJson(ret);
                }

                item.dataEnvioAntt = DateTime.UtcNow;

                ViagensRepository rep = new ViagensRepository();
                item.RowKey = GetRowKey(item.dataHoraEvento, item.placaVeiculo, item.codigoTipoLogID);
                rep.UpdateLogViagem(item);

                try
                {

                    GradeOperacaoFretamentoRepository ope_rep = new GradeOperacaoFretamentoRepository();
                    var grade = ope_rep.GetGradeOperacao(item.id_gradeoperacao);

                    if (grade != null)
                    {
                        if (item.codigoTipoRegistroViagem == 0)
                            grade.IsFechado = true;
                        else if (item.codigoTipoRegistroViagem == 1)
                            grade.IsAberto = true;
                        else if (item.codigoTipoRegistroViagem == 3 && item.isTransbordo == true)
                            grade.IsTransbordoAberto = true;
                        else if (item.codigoTipoRegistroViagem == 2 && item.isTransbordo == true)
                            grade.IsTransbordoFechado = true;

                        ope_rep.UpdateGrade(grade);
                    }

                }
                catch (Exception e)
                {

                }

            }
            catch (Exception ex)
            {
                erro[] erro = new erro[] {
                    new erro() {
                    descricao = "Mensagem: " + ex.Message + " Erro: " + ex.ToString()
                    }};
                var _dto = new resultadoOperacao() { erros = erro };

                item.Erros = converterJson(_dto);
                item.dataEnvioAntt = DateTime.UtcNow;

                ViagensRepository rep = new ViagensRepository();
                item.RowKey = GetRowKey(item.dataHoraEvento, item.placaVeiculo, item.codigoTipoLogID);
                rep.UpdateLogViagem(item);

                try
                {

                    GradeOperacaoFretamentoRepository ope_rep = new GradeOperacaoFretamentoRepository();
                    var grade = ope_rep.GetGradeOperacao(item.id_gradeoperacao);

                    if (grade != null)
                    {
                        if (item.codigoTipoRegistroViagem == 0)
                            grade.IsFechado = false;
                        else if (item.codigoTipoRegistroViagem == 1)
                            grade.IsAberto = false;
                        else if (item.codigoTipoRegistroViagem == 3 && item.isTransbordo == true)
                            grade.IsTransbordoAberto = false;
                        else if (item.codigoTipoRegistroViagem == 2 && item.isTransbordo == true)
                            grade.IsTransbordoFechado = false;

                        ope_rep.UpdateGrade(grade);
                    }

                }
                catch (Exception e)
                {

                }

            }
        }

        public static void EnviaLogJornadaMotorista(ViagemModel item)
        {
            try
            {

                jornadaTrabalhoMotorista x = new jornadaTrabalhoMotorista();

                x.cnpjEmpresaTransporte = item.cnpjEmpresa.Trim();
                x.tipoRegistroEvento = item.codigoTipoRegistroEvento.ToString().Trim();
                x.idLog = item.codigoTipoLogID.ToString();
                x.dataHoraEvento = item.dataHoraEvento.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
                x.imei = item.IMEI;
                x.latitude = item.latitude;
                x.longitude = item.longitude;
                x.pdop = item.pdop.ToString();
                x.placaVeiculo = item.placaVeiculo;
                x.cpfMotorista = item.cpfMotorista.Trim();

                resultadoOperacao ret = new resultadoOperacao();

                ret = EnviaJornadaMotorista(x, item.IDCliente);

                if (ret != null)
                {
                    item.idTransacao = ret.idTransacao;
                    item.isErro = ret.erros != null ? true : false;
                    item.Erros = converterJson(ret);
                }

                item.dataEnvioAntt = DateTime.UtcNow;

                ViagensRepository rep = new ViagensRepository();
                item.RowKey = GetRowKey(item.dataHoraEvento, item.placaVeiculo, item.codigoTipoLogID);
                rep.UpdateLogViagem(item);

            }
            catch (Exception ex)
            {
                erro[] erro = new erro[] {
                    new erro() {
                    descricao = "Mensagem: " + ex.Message + " Erro: " + ex.ToString()
                    }};
                var _dto = new resultadoOperacao() { erros = erro };

                item.Erros = converterJson(_dto);
                item.dataEnvioAntt = DateTime.UtcNow;

                ViagensRepository rep = new ViagensRepository();
                item.RowKey = GetRowKey(item.dataHoraEvento, item.placaVeiculo, item.codigoTipoLogID);
                rep.UpdateLogViagem(item);

            }
        }

        public static void EnviaLogDetectorParada(ViagemModel item)
        {
            try
            {

                detectorParada x = new detectorParada();
                x.cnpjEmpresaTransporte = item.cnpjEmpresa.Trim();
                x.codigoMotivoParada = (item.codigoMotivoParada.ToString());
                x.idLog = (item.codigoTipoLogID.ToString());
                x.dataHoraEvento = item.dataHoraEvento.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
                x.imei = item.IMEI;
                x.latitude = item.latitude;
                x.longitude = item.longitude;
                x.pdop = item.pdop.ToString();
                x.placaVeiculo = item.placaVeiculo;

                resultadoOperacao ret = new resultadoOperacao();

                ret = EnviaDetectorParada(x, item.IDCliente);

                if (ret != null)
                {
                    item.idTransacao = ret.idTransacao;
                    item.isErro = ret.erros != null ? true : false;
                    item.Erros = converterJson(ret);
                }

                item.dataEnvioAntt = DateTime.UtcNow;

                ViagensRepository rep = new ViagensRepository();
                item.RowKey = GetRowKey(item.dataHoraEvento, item.placaVeiculo, item.codigoTipoLogID);
                rep.UpdateLogViagem(item);

            }
            catch (Exception ex)
            {
                erro[] erro = new erro[] {
                    new erro() {
                    descricao = "Mensagem: " + ex.Message + " Erro: " + ex.ToString()
                    }};
                var _dto = new resultadoOperacao() { erros = erro };

                item.Erros = converterJson(_dto);
                item.dataEnvioAntt = DateTime.UtcNow;

                ViagensRepository rep = new ViagensRepository();
                item.RowKey = GetRowKey(item.dataHoraEvento, item.placaVeiculo, item.codigoTipoLogID);
                rep.UpdateLogViagem(item);

            }
        }

        public static void EnviaLogVelocidadeTempoLocalizacao(ViagemModel item)
        {
            try
            {

                velocidadeTempoLocalizacao x = new velocidadeTempoLocalizacao();

                x.cnpjEmpresaTransporte = item.cnpjEmpresa;
                x.situacaoIgnicaoMotor = (item.codigoSituacaoIgnicaoMotor.ToString());
                x.situacaoPortaVeiculo = (item.codigoSituacaoPortaVeiculo.ToString());
                x.distanciaPercorrida = Convert.ToInt32(item.distanciaPercorrida / 1000).ToString();
                x.velocidadeAtual = (item.velocidadeAtual.ToString());
                x.idLog = (item.codigoTipoLogID.ToString());
                x.dataHoraEvento = item.dataHoraEvento.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
                x.imei = item.IMEI;
                x.latitude = item.latitude;
                x.longitude = item.longitude;
                x.pdop = item.pdop.ToString();
                x.placaVeiculo = item.placaVeiculo;

                resultadoOperacao ret = new resultadoOperacao();

                ret = EnviaVelocidadeTempoLocalizacao(x, item.IDCliente);

                if (ret != null)
                {
                    item.idTransacao = ret.idTransacao;
                    item.isErro = ret.erros != null ? true : false;
                    item.Erros = converterJson(ret);
                }
                else
                {
                    item.isErro = true;
                    item.Erros = "Timeout de envio para ANTT.";
                }

                item.dataEnvioAntt = DateTime.UtcNow;

                ViagensRepository rep = new ViagensRepository();
                item.RowKey = GetRowKey(item.dataHoraEvento, item.placaVeiculo, item.codigoTipoLogID);
                rep.UpdateLogViagem(item);

            }
            catch (Exception ex)
            {
                erro[] erro = new erro[] {
                    new erro() {
                    descricao = "Mensagem: " + ex.Message + " Erro: " + ex.ToString()
                    }};
                var _dto = new resultadoOperacao() { erros = erro };

                item.Erros = converterJson(_dto);
                item.dataEnvioAntt = DateTime.UtcNow;

                ViagensRepository rep = new ViagensRepository();
                item.RowKey = GetRowKey(item.dataHoraEvento, item.placaVeiculo, item.codigoTipoLogID);
                rep.UpdateLogViagem(item);
            }
        }

        public static void EnviaLogLeitorBilheteEmbarque(ViagemModel item, string prefixo)
        {
            try
            {

                bilheteEmbarque x = new bilheteEmbarque();

                if (item.NumeroBilheteEmbarque != null)
                {


                    x.bilhetes = item.NumeroBilheteEmbarque.ToArray();
                    x.idLog = (item.codigoTipoLogID.ToString());
                    x.dataHoraEvento = item.dataHoraEvento.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
                    x.imei = item.IMEI;
                    x.latitude = item.latitude;
                    x.longitude = item.longitude;
                    x.pdop = item.pdop.ToString();
                    x.placaVeiculo = item.placaVeiculo;
                    x.cnpjEmpresaTransporte = item.cnpjEmpresa;

                    resultadoOperacao ret = new resultadoOperacao();
                    var json = "";

                    if (!string.IsNullOrEmpty(prefixo) && prefixo.Length > 7)
                    {
                        ret = EnviaBilheteEmbarque(x, item.IDCliente);
                    }
                    else
                    {
                        ret = null;
                        json = "Prefixo inválido. O prefixo deve conter um tamanho de 8 caracteres, sem traços, pontos ou espaços.";
                    }

                    if (ret != null)
                    {

                        item.idTransacao = ret.idTransacao;
                        item.isErro = ret.erros != null ? true : false;
                        item.Erros = converterJson(ret);
                    }
                    else
                    {
                        item.Erros = json;
                        item.isErro = true;
                    }

                    item.dataEnvioAntt = DateTime.UtcNow;

                    ViagensRepository rep = new ViagensRepository();
                    item.RowKey = GetRowKey(item.dataHoraEvento, item.placaVeiculo, item.codigoTipoLogID);
                    rep.UpdateLogViagem(item);
                }

            }
            catch (Exception ex)
            {
                erro[] erro = new erro[] {
                    new erro() {
                    descricao = "Mensagem: " + ex.Message + " Erro: " + ex.ToString()
                    }};
                var _dto = new resultadoOperacao() { erros = erro };

                item.Erros = converterJson(_dto);
                item.dataEnvioAntt = DateTime.UtcNow;

                ViagensRepository rep = new ViagensRepository();
                item.RowKey = GetRowKey(item.dataHoraEvento, item.placaVeiculo, item.codigoTipoLogID);
                rep.UpdateLogViagem(item);

            }
        }

        public static string GetRowKey(DateTime dataEvento, String PlacaVeiculo, int CodigoTipoLogID)
        {
            string rowPattern = "{0}V{1}T{2}";
            var ret = String.Format(rowPattern
                    , DateTime.MaxValue.Subtract(dataEvento).Ticks.ToString().PadLeft(20, '0')
                    , PlacaVeiculo.ToString().PadLeft(10, '0')
                    , CodigoTipoLogID.ToString().PadLeft(2, '0')

                    );
            return ret;
        }
    }
}
