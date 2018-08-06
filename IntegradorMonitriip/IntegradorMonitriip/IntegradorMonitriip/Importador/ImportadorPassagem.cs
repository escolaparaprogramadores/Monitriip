

using System;
using System.Collections.Generic;
using System.Xml;
using System.Net;
using System.Xml.Serialization;
using System.Globalization;
using NewsGPS.Contracts.Enums;
using IntegradorModel.Model.XmlModel;

namespace NewsGPS.Logic.Integracao.RJ
{
    public class ImportadorPassagem : ImportadorRJ
    {
        #region Propriedades

        protected override EEntidade Entidade
        {
            get { return EEntidade.Cliente; }
        }

        protected override EEntidade EntidadeExterna
        {
            get { return EEntidade.Chip; }
        }

        public bool isVendas
        {
            get; set;
        }
        public string servico
        {
            get; set;
        }

        #endregion Propriedades

        #region Construtores

        private ImportadorPassagem(int idCliente, bool IsVendas, string Servico)
            : base(idCliente)
        {
            this.isVendas = IsVendas;
            this.servico = Servico;
        }

        #endregion Construtores

        #region Factory

        public static ImportadorPassagem Create(int idCliente, bool isVendas, string servico)
        {
            var ret = new ImportadorPassagem(idCliente, isVendas, servico);
            return ret;
        }

        #endregion Factory
        public override void ProcessarUnitario(object item)
        {

        }
        public override void Processar()
        {
            XmlDocument Passagens;
            DateTime data = DateTime.UtcNow;
            try
            {
                Passagens = BaixaPassagem(data, isVendas, servico);
            }
            catch
            {
                throw;
            }

            try
            {
                if (Passagens != null)
                {
                    var xmlReader = new XmlNodeReader(Passagens);

                    if (isVendas)
                    {
                        vendas passagemRJ = null;


                        XmlSerializer serializer = new XmlSerializer(typeof(vendas));
                        passagemRJ = (vendas)serializer.Deserialize(xmlReader);




                         this.ImportarPassagens(passagemRJ.passagens);
                    }
                    else
                    {
                        bilhetes passagemRJ = null;


                        XmlSerializer serializer = new XmlSerializer(typeof(bilhetes));
                        passagemRJ = (bilhetes)serializer.Deserialize(xmlReader);




                        this.ImportarPassagens(passagemRJ.passagens);
                    }
                    
                }
               // SalvarDataUltimaImportacao(this.IDIntegracao, data);
                this.DataUltimaIntegracao = data;
            }
            catch
            { }
        }

        protected void ImportarPassagens(List<IntegradorModel.Model.XmlModel.PassagemXML> listaEntidadeExterna)
        {
            //listaEntidadeExterna.ForEach(x => ImportarPassagem(x));
        }

        CultureInfo ci = new CultureInfo("en-US");
//        public void ImportarPassagem(PassagemXML xml)
//        {
//            if (xml.horaEmissao == null)
//                return;
//            int tamHora;
//            string hora;
//            DateTime dataEmissao;

//            if (isVendas)
//            {
//                tamHora = xml.horaEmissao.ToString().Count();
//                hora = xml.horaEmissao.ToString();
//                for (int i = tamHora; i < 4; i++)
//                {
//                    hora = "0" + hora;
//                }


//                dataEmissao = new DateTime(
//   Convert.ToInt32("20" + xml.dataEmissao.ToString().Substring(0, 2)), //ano
//   Convert.ToInt32(xml.dataEmissao.ToString().Substring(2, 2)), //mes
//   Convert.ToInt32(xml.dataEmissao.ToString().Substring(4)), //dia
//   Convert.ToInt32(hora.Substring(0, 2)), //hora
//   Convert.ToInt32(hora.Substring(2, 2)), //min
//   0);
//            }
//            else
//            {
//                tamHora = xml.horaVenda.ToString().Count();
//                hora = xml.horaVenda.ToString();
//                for (int i = tamHora; i < 4; i++)
//                {
//                    hora = "0" + hora;
//                }


//                dataEmissao = new DateTime(
//   Convert.ToInt32("20" + xml.dataVenda.ToString().Substring(0, 2)), //ano
//   Convert.ToInt32(xml.dataVenda.ToString().Substring(2, 2)), //mes
//   Convert.ToInt32(xml.dataVenda.ToString().Substring(4)), //dia
//   Convert.ToInt32(hora.Substring(0, 2)), //hora
//   Convert.ToInt32(hora.Substring(2, 2)), //min
//   0);
//            }

//            tamHora = xml.horaViagem.ToString().Count();
//            hora = xml.horaViagem.ToString();
//            for (int i = tamHora; i < 4; i++)
//            {
//                hora = "0" + hora;
//            }


//            DateTime dataViagem = new DateTime(
//Convert.ToInt32("20" + xml.dataViagem.ToString().Substring(0, 2)), //ano
//Convert.ToInt32(xml.dataViagem.ToString().Substring(2, 2)), //mes
//Convert.ToInt32(xml.dataViagem.ToString().Substring(4)), //dia
//Convert.ToInt32(hora.Substring(0, 2)), //hora
//Convert.ToInt32(hora.Substring(2, 2)), //min
//0);
//            PassagemRepository rep = new PassagemRepository();


//            var _passagens = rep.ObterPassagens(new PassagemFilter()
//            {
//                dataViagem = dataViagem,
//                IdCliente = this.IDCliente,
//                numBilheteEmbarque = (isVendas ? xml.numBilheteEmbarque : xml.identificadorBilhete),
//                numServico = (isVendas ? xml.numServico : servico),
//                celularPassageiro = xml.celularPassageiro,
//                CodigoDesconto = xml.motivoDesconto,
//                destino = (isVendas ? xml.destino : xml.locDestino),
//                origem = (isVendas ? xml.origem : xml.locOrigem),
//                linha = xml.linha,
//                numSerie = xml.numSerie,
//                perDesconto = decimal.Parse(xml.perDesconto, ci),
//                ValorTarifa = decimal.Parse(xml.tarifa, ci)
//            });

           
//            if (_passagens.Count() == 0)
//            {
//                Passagem dto = new Passagem(this.IDCliente, (isVendas ? xml.numServico : servico), dataViagem, xml.numSerie, xml.numBilheteEmbarque,
//                xml.linha, dataViagem, xml.motivoDesconto, decimal.Parse(xml.tarifa, ci), decimal.Parse(xml.perDesconto, ci),
//                xml.celularPassageiro, (isVendas ? xml.origem : xml.locOrigem), (isVendas ? xml.destino : xml.locDestino));

//                dto.aliquotaICMS = xml.aliquotaICMS;
//                dto.categoria = xml.categoria;
//                dto.celularPassageiro = xml.celularPassageiro;
//                dto.cpfPassageiro = xml.cpfPassageiro;
//                dto.cnpj = xml.cnpj;
//                dto.linha = xml.linha;
//                dto.dataEmissao = dataEmissao;
//                dto.dataViagem = dataViagem;
//                dto.destino = (isVendas ? xml.destino : xml.locDestino);
//                dto.docPassageiro = xml.docPassageiro;
//                dto.idLog = xml.idLog;
//                dto.identificadorBilhete = (isVendas ? xml.identificadorBilhete : xml.numBilheteSistema);
//                dto.motivoDesconto = xml.motivoDesconto;
//                dto.nomePassageiro = xml.nomePassageiro;
//                dto.numBilheteEmbarque = xml.numBilheteEmbarque;
//                dto.numBilheteImpresso = xml.numBilheteImpresso;
//                dto.numSerie = xml.numSerie;
//                dto.origem = (isVendas ? xml.origem : xml.locOrigem);
//                dto.perDesconto = decimal.Parse(xml.perDesconto, ci);
//                dto.poltrona = xml.poltrona;
//                dto.tarifa = decimal.Parse(xml.tarifa, ci);
//                dto.taxaEmbarque = xml.taxaEmbarque;
//                dto.tipoServico = xml.tipoServico;
//                dto.tipoViagem = xml.tipoViagem;
//                dto.valorPedagio = xml.valorPedagio;
//                dto.valorTotal = xml.valorTotal;
//                dto.status = xml.status;
                

//                rep.Add(dto);

//                NewsGPS.BusinessRules.monitriip.WebService ws = new NewsGPS.BusinessRules.monitriip.WebService();
//                ws.vendaPassagem(dto.aliquotaICMS,
//                    dto.cnpj,
//                    "", // Codigo bilhete embarque
//                    dto.categoria,
//                    dto.idLog,
//                    dto.motivoDesconto,
//                    dto.dataEmissao.ToString("YYYYMMDD"),
//                    dto.dataEmissao.ToString("HHmmss"),
//                                dto.dataViagem.ToString("YYYYMMDD"),
//                    dto.dataViagem.ToString("HHmmss"),
//                    dto.tipoServico,
//                    dto.tipoViagem,
//                    dto.linha,
//                    dto.origem,
//                    dto.destino,
//                    dto.numBilheteEmbarque,
//                    dto.poltrona,
//                    dto.numSerie,
//                    dto.perDesconto.ToString(),
//                    "", //dto.plataforma,
//                    dto.valorTotal,
//                    dto.tarifa.ToString(),
//                    dto.taxaEmbarque,
//                    dto.valorTotal,
//                    dto.celularPassageiro,
//                    dto.cpfPassageiro,
//                    dto.docPassageiro,
//                    dto.nomePassageiro);

//            }
//            else if (_passagens.First().status != xml.status)
//            {
//                Passagem dto = _passagens.First();
//                dto.status = xml.status;
//                rep.Update(dto);
//            }

        
//        }


        private XmlDocument BaixaPassagem(DateTime quando, bool isVendas = false, string servico = "")
        {
            string[] detalhe = this.SistemaCliente.userToken.Split(':');

            string requestUrl;
            if (isVendas)
                requestUrl = string.Format("{0}WSMonitriipRJ/busca/buscaVendas/{1}/{2}/{3}/{4}", this.SistemaCliente.URL, detalhe[0], detalhe[1], this.DataUltimaIntegracao.Value.ToString("yyMMdd"), this.DataUltimaIntegracao.Value.ToString("HHmm"));
            else
                requestUrl = string.Format("{0}WSMonitriipRJ/busca/buscaBilhetes/{1}/{3}/{2}/{4}", this.SistemaCliente.URL, detalhe[0], detalhe[1], quando.ToString("yyMMdd"), servico);


            XmlDocument dados = BuscarDados(requestUrl);



            return dados;


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

    public class _Passagem
    {
        public string aliquotaICMS { get; set; }
        public string categoria { get; set; }
        public string celularPassageiro { get; set; }
        public string cnpj { get; set; }
        public string cpfPassageiro { get; set; }
        public string dataEmissao { get; set; }
        public string dataViagem { get; set; }
        public string destino { get; set; }
        public string docPassageiro { get; set; }
        public string horaEmissao { get; set; }
        public string horaVigem { get; set; }
        public string idLog { get; set; }
        public string identificadorBilhete { get; set; }
        public string linha { get; set; }
        public string motivoDesconto { get; set; }

        public string nomePassageiro { get; set; }


        public string numBilheteEmbarque { get; set; }

        public string numBilheteImpresso { get; set; }

        public string numSerie { get; set; }

        public string origem { get; set; }

        public string perDesconto { get; set; }

        public string poltrona { get; set; }

        public string tarifa { get; set; }

        public string taxaEmbarque { get; set; }

        public string tipoPassagem { get; set; }

        public string tipoViagem { get; set; }

        public string valorPedagio { get; set; }

        public string valorTotal { get; set; }



    }

    //private class _Rotas
    //{

    //    public int? TempoViagem { get; set; }
    //    public int IDRota { get; set; }
    //    public int IDTipoRota { get; set; }
    //    public int IDPontoOrigem { get; set; }
    //    public int IDPontoDestino { get; set; }

    //}
}
