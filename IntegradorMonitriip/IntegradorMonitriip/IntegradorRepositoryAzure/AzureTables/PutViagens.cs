﻿using IntegradorModel.Model;
using IntegradorModel.Model.XmlModel;
using IntegradorMonitriip.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using NewsGPS.Logic;
using System.Collections.Generic;
using System.Linq;

namespace IntegradorRepositoryAzure.AzureTables
{
    public class PutViagens
    {

        private ViagensRepository Repository;
        public PutViagens()
        {
            Repository = new ViagensRepository();
        }

        public string converterJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
        }

        public void ReenviarAntt(ViagemModel entitiesList)
        {
          

        }

        public static NewsGPS.Domain.Embarque convertToEmbarque(VendasModel venda)
        {

            try
            {
                //var embarque = new NewsGPS.Domain.Embarque(
                //       venda.numBilheteSistema, venda.numSerie,
                //       venda.numServico, venda.dataHoraViagem,
                //       venda.idCliente, DateTime.UtcNow);

                var embarque = new NewsGPS.Domain.Embarque(
                     venda.idCliente, venda.numBilheteSistema,
                        venda.numSerie, venda.numServico,
                        venda.dataHoraViagem, DateTime.UtcNow);

                //public Embarque(int idCliente, string numBilheteEmbarque, string numSerie, string SRVP, 
                //DateTime dataViagem, DateTime dataEvento);

                //public Embarque(string numBilheteSistema, string numSerie, string SRVP, 
                //DateTime dataViagem, int idCliente, DateTime dataEvento);

                //embarque.CpfMotorista = "";
                //embarque.DatahoraCheckin = "";
                //embarque.ErrosEmbarque = "";
                //embarque.isErrosEmbarque = false;
                //embarque.isErrosVendas = venda.
                //embarque.idCliente = 0;
                //embarque.Placa = ""
                embarque.numServico = venda.numServico;
                embarque.idCliente = venda.idCliente;
                embarque.dataHoraViagem = venda.dataHoraViagem;
                embarque.DataViagem = venda.dataViagem;
                embarque.HoraViagem = venda.horaViagem;
                embarque.Origem = venda.origem;
                embarque.Destino = venda.destino;
                embarque.ErrosVendas = venda.retornoANTT;
                if (string.IsNullOrEmpty(venda.retornoANTT) || venda.retornoANTT.Contains("Timeout")
                    || venda.retornoANTT.Contains("Erro"))
                {
                    embarque.isErrosVendas = true;
                }
                embarque.isVendas = true;
                embarque.Linha = venda.linha;
                embarque.NomePassageiro = venda.nomePassageiro;
                embarque.NumeroBilhete = venda.numBilheteSistema;
                embarque.numSerie = venda.numSerie;
                embarque.Poltrona = venda.poltrona;

                return embarque;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
      

        //public void salvarVendas(vendas passagemRJ, int iDCliente,
        //    ref List<VendasModel> passagensEnvio)
        //{
        //    List<VendasModel> vendas = new List<VendasModel>();
        //    foreach (var item in passagemRJ.passagens)
        //    {
        //        try
        //        {
        //            var model = VendaToModel(item, iDCliente);
        //            if (model != null)
        //            {
        //                vendas.Add(model);
        //                passagensEnvio.Add(model);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //    }
        //    if (vendas.Count > 0)
        //    {
        //        Repository.saveVendas(vendas);
        //    }
        //}

        #region Vendas
        public static VendasModel VendaToModel(PassagemXML bilhete, int iDCliente)
        {
            var model = correcaoDataHoraVendas(bilhete, iDCliente);
            if (model == null)
                return model;

            convertToModel(bilhete, ref model);
            model.isVendas = true;

            return model;
        }

        private static VendasModel correcaoDataHoraVendas(PassagemXML bilhete, int iDCliente)
        {

            var horaViagem = "";
            var dataViagem = "";
            if (bilhete.horaViagem.Length < 6)
            {
                horaViagem = (bilhete.horaViagem.Length < 4) ?
                                        "0" + bilhete.horaViagem.Substring(0, 1) + ":" +
                                        bilhete.horaViagem.Substring(1) + ":00"
                                                : bilhete.horaViagem.Substring(0, 2) + ":" +
                                                bilhete.horaViagem.Substring(2) + ":00";
            }
            else
            {
                horaViagem = bilhete.horaViagem.Substring(0, 2) + ":" +
                                bilhete.horaViagem.Substring(2, 2) + ":" +
                                bilhete.horaViagem.Substring(4);
            }

            if (bilhete.dataViagem.Length < 8)
            {
                dataViagem = "20" + bilhete.dataViagem.Substring(0, 2) + "-"
                                + bilhete.dataViagem.Substring(2, 2) + "-" + bilhete.dataViagem.Substring(4) + " "
                                + horaViagem;
            }
            else
            {
                dataViagem = bilhete.dataViagem.Substring(0, 4) + "-"
                    + bilhete.dataViagem.Substring(4, 2) + "-" + bilhete.dataViagem.Substring(6) + " "
                    + horaViagem;
            }

            var numBilhete = !string.IsNullOrEmpty(bilhete.numBilheteEmbarque) ?
                                                bilhete.numBilheteEmbarque :
                                                !string.IsNullOrEmpty(bilhete.numBilheteEstado) ?
                                                bilhete.numBilheteEstado :
                                                !string.IsNullOrEmpty(bilhete.numBilheteImpresso) ?
                                                bilhete.numBilheteImpresso : "";

            return new VendasModel(
                numBilhete,
                String.IsNullOrEmpty(bilhete.numSerie) ? "000013" : bilhete.numSerie.Trim(),
                bilhete.numServico.Trim(),
                Convert.ToDateTime(dataViagem), iDCliente
                );
        }
        #endregion

        private static void convertToModel(PassagemXML bilhete, ref VendasModel model)
        {
            if (bilhete.docPassageiro.Equals(bilhete.cpfPassageiro))
            {
                model.docPassageiro = bilhete.docPassageiro;
                model.cpfPassageiro = "";
                model.celularPassageiro = "";
            }
            else
            {
                model.cpfPassageiro = bilhete.cpfPassageiro;
                model.docPassageiro = bilhete.docPassageiro;
                model.celularPassageiro = bilhete.celularPassageiro;
            }
            model.aliquotaICMS = bilhete.aliquotaICMS;
            model.categoria = bilhete.categoria;
            model.cnpj = bilhete.cnpj;
            model.dataVenda = bilhete.dataVenda;
            model.dataViagem = bilhete.dataViagem;
            model.horaVenda = bilhete.horaVenda;
            model.horaViagem = bilhete.horaViagem;
            model.idLog = bilhete.idLog;
            model.linha = bilhete.linha;
            //model.locDestino = bilhete.locDestino;
            //model.locOrigem = bilhete.locOrigem;
            model.motivoDesconto = bilhete.motivoDesconto;
            model.nomePassageiro = bilhete.nomePassageiro;
            model.numBilheteImpresso = bilhete.numBilheteImpresso != null ? bilhete.numBilheteImpresso.PadLeft(6, '0') : "0".PadLeft(6, '0');
            model.numBilheteSistema = !string.IsNullOrEmpty(bilhete.numBilheteEmbarque) ?
                                                bilhete.numBilheteEmbarque :
                                                !string.IsNullOrEmpty(bilhete.numBilheteEstado) ?
                                                bilhete.numBilheteEstado :
                                                !string.IsNullOrEmpty(bilhete.numBilheteImpresso) ?
                                                bilhete.numBilheteImpresso : "";
            model.numBilheteEstado = bilhete.numBilheteEstado;
            model.numBilheteEmbarque = model.numBilheteSistema; // bilhete.numBilheteEmbarque != null ? bilhete.numBilheteEmbarque.PadLeft(6, '0') : "0".PadLeft(6, '0');
            model.origem = bilhete.origem;
            model.destino = bilhete.destino;
            model.status = bilhete.status;
            model.numServico = bilhete.numServico;
            model.identificadorBilhete = bilhete.identificadorBilhete;
            model.horaEmissao = bilhete.horaEmissao;
            model.plataformaEmbarque = bilhete.plataformaEmbarque;
            model.dataEmissao = bilhete.dataEmissao;
            model.numSerie = bilhete.numSerie;
            model.perDesconto = bilhete.perDesconto;
            model.poltrona = bilhete.poltrona;
            model.tarifa = bilhete.tarifa;
            model.taxaEmbarque = bilhete.taxaEmbarque;
            model.tipoServico = bilhete.tipoServico;
            model.tipoViagem = bilhete.tipoViagem;
            model.valorPedagio = bilhete.valorPedagio;
            model.valorTotal = bilhete.valorTotal;
            model.numeroNovoBilheteEmbarque = bilhete.numeroNovoBilheteEmbarque;
        }
    }
}
