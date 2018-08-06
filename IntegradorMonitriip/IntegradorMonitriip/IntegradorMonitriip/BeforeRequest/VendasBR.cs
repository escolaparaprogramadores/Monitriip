using antt.gov.br.monitriip.v1._0;
using IntegradorModel.Model;
using IntegradorModel.Model.XmlModel;
using IntegradorMonitriip.Model;
using IntegradorRequestWeb.RequestWeb;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Xml;

namespace IntegradorMonitriip.BeforeRequest
{
    public class VendasBR : VendasRW
    {
        public static XmlDocument ProcessarPassagens(string url, string codConexao, string letraConexao, string servico, DateTime data, bool isVendas, int IDCliente)
        {
            try
            {
                return BaixaPassagem( IDCliente, data, isVendas, servico, url, codConexao, letraConexao);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static XmlDocument ProcessarPassagensItamarati(string url)
        {
            try
            {

                XmlDocument dados = BuscarDadosPassagensItamarati(url, 1, 7937);

                return dados;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static XmlDocument BaixaPassagem(int IDCliente, DateTime quando, bool isVendas = false, string servico = "", string url = "", string codEmpresaConexao = "", string letraEmpresaConexao = "")
        {

            string requestUrl;
            /*Caso Empresas GM*/
            if (IDCliente == 15461 || IDCliente == 15460 || IDCliente == 15506)
            {
                requestUrl = string.Format("http://200.143.16.42:8084/gm/integrador/I01WCFWEBQUADRISYSTEM/CTL/WS_buscaVendas?codigo1={0}&codigo2={1}&dataBase={2}&horaBase={3}",
                     codEmpresaConexao, letraEmpresaConexao,
                     quando.ToString("yyMMdd"),
                     quando.ToString("HHmm"));
            }
            else
            {
                if (isVendas)
                    /*Caso Empresa Guerino*/
                    if (isVendas && IDCliente == 8703)
                    {
                        requestUrl = string.Format("{0}WSMonitriipRJ_Homolog/busca/buscaVendas/{1}/{2}/{3}/{4}",
                            url, codEmpresaConexao, letraEmpresaConexao,
                            quando.ToString("yyMMdd"),
                            quando.ToString("HHmm"));
                    }
                    else
                    {
                    requestUrl = string.Format("{0}WSMonitriipRJ/busca/buscaVendas/{1}/{2}/{3}/{4}",
                            url, codEmpresaConexao, letraEmpresaConexao,
                            quando.ToString("yyMMdd"),
                            quando.ToString("HHmm"));
                    }
                else
      
                    requestUrl = string.Format("{0}WSMonitriipRJ/busca/buscaBilhetes/{1}/{3}/{2}/{4}",
                        url, codEmpresaConexao, letraEmpresaConexao,
                        quando.ToString("yyMMdd"), servico);
            }

            //requestUrl = "http://qslinuxrj.cloudapp.net:9991/WSMonitriipRJ/busca/buscaBilhetes/18/170215/A/4614";
            XmlDocument dados = BuscarDadosPassagens(requestUrl, 1, IDCliente);
            //XmlDocument dados = BuscarDadosPassagens(requestUrl, 1);
            
            Parameters.DataUltimaIntegracao = quando.AddMinutes(-1);

            return dados;
        }
        private static string formatHora(string input)
        {
            var hora = "";
            if (input.Length < 6)
            {
                hora = (input.Length < 4) ?
                                        "0" + input.Substring(0, 1) + ":" +
                                        input.Substring(1) + ":00"
                                                : input.Substring(0, 2) + ":" +
                                                input.Substring(2) + ":00";
            }
            else
            {
                hora = input.Substring(0, 2) + ":" +
                                input.Substring(2, 2) + ":" +
                                input.Substring(4);
            }
            //var hora = (input.Length < 4) ?
            //            "0" + input.Substring(0, 1) + ":" +
            //            input.Substring(1) + ":00"
            //                    : input.Substring(0, 2) + ":" +
            //                    input.Substring(2) + ":00";

            return hora;
        }

        private static string formatData(string input)
        {
            var data = "";
            if (input.Length < 8)
            {
                data = "20" + input.Substring(0, 2) + "-"
                                + input.Substring(2, 2) + "-" + input.Substring(4);
            }
            else
            {
                data = input.Substring(0, 4) + "-"
                    + input.Substring(4, 2) + "-" + input.Substring(6);
            }
            //var data = "20" + input.Substring(0, 2) + "-"
            //    + input.Substring(2, 2) + "-" + input.Substring(4);
            return data;
        }
        //public static List<ResultAnttDTO> EnviaANTT(ref List<VendasModel> passagemRJ)
        public static void EnviaANTT(ref List<VendasModel> passagemRJ)
        {
            //List<ResultAnttDTO> ret = new List<ResultAnttDTO>();
            foreach (var item in passagemRJ)
            {
                try
                {
                    informacoesPassageiro pass = new informacoesPassageiro();
                    if (item.docPassageiro.Equals(item.cpfPassageiro))
                    {
                        pass.celularPassageiro = "";
                        pass.cpfPassageiro = "";
                    } else
                    {
                        pass.celularPassageiro = string.IsNullOrEmpty(item.celularPassageiro) ? "" : item.celularPassageiro.PadLeft(12, '0');
                        pass.cpfPassageiro = string.IsNullOrEmpty(item.cpfPassageiro) ? "" : item.cpfPassageiro.PadLeft(11, '0');
                        
                    }
                    pass.documentoIdentificacaoPassageiro = item.docPassageiro;
                    pass.nomePassageiro = item.nomePassageiro;

                    vendaPassagem x = new vendaPassagem();
                    x.aliquotaICMS = item.aliquotaICMS;
                    x.cnpjEmpresa = item.cnpj.Replace(".","").Replace("/", "").Replace("-", "");
                    x.codigoBilheteEmbarque = item.identificadorBilhete;
                    //x.codigoCategoriaTransporte = "01";
                    x.codigoMotivoDesconto = item.motivoDesconto;
                    x.codigoCategoriaTransporte = item.categoria;
                    x.codigoTipoServico = item.tipoServico.Equals("00") ? "01" : item.tipoServico.PadLeft(2, '0');
                    x.codigoTipoViagem = item.tipoViagem;
                    //x.dataEmissaoBilhete = Convert.ToDateTime(dataEmissaoBilhete + " 00:00:00.000").ToString("yyyyMMdd");
                    //x.horaEmissaoBilhete = Convert.ToDateTime("2016-01-01 " + horaEmissaoBilhete + ".000").ToString("HHmmss");
                    x.dataEmissaoBilhete = Convert.ToDateTime(formatData(item.dataEmissao)).ToString("yyyyMMdd");
                    x.horaEmissaoBilhete = Convert.ToDateTime("2016-01-01 " + formatHora(item.horaEmissao) + ".000").ToString("HHmmss");
                    x.dataViagem = Convert.ToDateTime(formatData(item.dataViagem)).ToString("yyyyMMdd");
                    x.horaViagem = Convert.ToDateTime("2016-01-01 " + formatHora(item.horaViagem) + ".000").ToString("HHmmss");
                    x.idLog = item.idLog;
                    x.identificacaoLinha = item.linha.Replace("-","");
                    x.informacoesPassageiro = pass;                   

                    x.numeroBilheteEmbarque = string.IsNullOrEmpty(item.numBilheteEmbarque) ? item.numBilheteEmbarque : item.numBilheteEmbarque.PadLeft(6, '0');
                    x.numeroSerieEquipamentoFiscal = item.numSerie;
                    x.numeroPoltrona = item.poltrona;                    
                    x.percentualDesconto = item.perDesconto;
                    //2016-12-22
                    x.plataformaEmbarque = item.plataformaEmbarque; 
                    x.valorPedagio = item.valorPedagio;
                    x.valorTarifa = item.tarifa;
                    x.valorTaxaEmbarque = item.taxaEmbarque;
                    x.valorTotal = item.valorTotal;
                    //x.idPontoDestinoViagem = item.locDestino;
                    //x.idPontoOrigemViagem = item.locOrigem;
                    x.idPontoDestinoViagem = item.destino;
                    x.idPontoOrigemViagem = item.origem;
                    resultadoOperacao result = new resultadoOperacao();
                    if (item.status != null && !item.status.Trim().Equals("0"))
                    {
                        result = EnviaANTTWebCancelados(item, item.idCliente);
                        if (!string.IsNullOrEmpty(item.numeroNovoBilheteEmbarque))
                        {
                            x.numeroBilheteEmbarque = item.numeroNovoBilheteEmbarque;
                            result = EnviaANTTWeb(x, item.idCliente);
                        }
                    } else
                    {
                        result = EnviaANTTWeb(x, item.idCliente);
                    }

                    //else
                    //{
                    //    result = EnviaANTTWebCancelados(item);
                    //}
                    
                    item.retornoANTT = converterJson(result);
                    item.dataEnvioAntt = DateTime.UtcNow;
                    //var dto = new ResultAnttDTO()
                    //{
                    //    result = result,
                    //    rowKey = GetRowKey(item.numBilheteEmbarque, item.numSerie),
                    //    dataEnvioAntt = DateTime.UtcNow
                    //};
                    //ret.Add(dto);

                }
                catch (Exception ex)
                {
                    var logic = new NewsGPS.Logic.AnttLogLogic();
                    logic.saveError(ex.Message, ex.InnerException.ToString(), "RK:" + GetRowKey(item.numBilheteEmbarque, item.numSerie) + "   StackTrace:" + ex.StackTrace, DateTime.Now.ToString(), 99);

                    erro[] erro = new erro[] {
                    new erro() {
                    descricao = "Mensagem: " + ex.Message + " Erro: " + ex.ToString()
                    }};
                    var _dto =  new resultadoOperacao() { erros = erro };

                    item.retornoANTT = converterJson(_dto);
                    item.dataEnvioAntt = DateTime.UtcNow;
                    //var dto = new ResultAnttDTO()
                    //{
                    //    result = _dto,
                    //    rowKey = GetRowKey(item.numBilheteEmbarque, item.numSerie),
                    //    dataEnvioAntt = DateTime.UtcNow
                    //};
                    //ret.Add(dto);
                    continue;
                }
            }
            //return ret;

        }

        public static string converterJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
        }

        public static string GetRowKey(string numBilheteSistema, string numSerie)
        {
            string param1 = numBilheteSistema.PadLeft(6, '0');
            string param2 = numSerie.PadLeft(6, '0');
            string rowPattern = "B{0}S{1}";
            var ret = String.Format(rowPattern
                    , param1.Substring(param1.Length - 6)
                    , param2.Substring(param2.Length - 6)
                    );
            return ret;
        }
    }
}
