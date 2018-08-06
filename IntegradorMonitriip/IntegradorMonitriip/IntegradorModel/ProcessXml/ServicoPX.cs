
using AutoMapper;
using NewsGPS.Contracts.DTO.RJ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace IntegradorModel.Model
{
    public class ServicoPX
    {
        public List<ServicoDTO> TratarRetorno(XmlDocument Servicos, int idCliente)
        {
            if (Servicos != null)
            {
                var xmlReader = new XmlNodeReader(Servicos);
              
                XmlSerializer serializerNew = new XmlSerializer(typeof(servicosDetalhado2));               
                servicosDetalhado2 servicosRJ = servicosRJ = (servicosDetalhado2)serializerNew.Deserialize(xmlReader);               
                var listaServicoDTO = new List<ServicoDTO>();
               
                    //listaServicoDTO = servicosRJ.servicos.Select(row => new ServicoDTO
                   // {
                        //    Data = row.data,
                        //    HoraSaida = row.horarioSaida,
                        //    DataChegadaPrevista = row.data,
                        //    Origem = row.origem,
                        //    Destino = row.destino,
                        //    Linha = row.linha,
                        //    NumServico = row.numServico,
                        //    prefixoLinha = row.prefixoLinha,
                        //    piso = row.piso,
                        //    plataforma = row.plataforma,
                        //    tipoViagem = row.tipoViagem,
                        //    Status = row.status,
                        //    CodOrigem = row.CodOrigem,
                        //    CodDestino = row.CodDestino,
                        //    seccoes = row.Seccao.ToList(),
                        //    ServicosRelacionado = row.ServicosRelacionado.ToList()
      

                    //}).ToList();

               
                   
                    foreach (var item in servicosRJ.servicos)
                    {
                        var model = new ServicoDTO();
                        var horarioSeccao = 0;
                        var horarioSeccaoAtual = 0;
                        var diaSoma = 0;

                        foreach (var item2 in item.Seccao)
                        {
                            if (!item2.hora.Equals(""))
                            {
                                horarioSeccao = Convert.ToInt32(item2.hora);

                                if (horarioSeccao <= horarioSeccaoAtual)
                                {
                                    diaSoma++;
                                    horarioSeccaoAtual = 0;
                                }

                                horarioSeccaoAtual = horarioSeccao;
                            }
                        }

                        model.Data = item.data;
                        model.Motorista = item.motorista;
                        model.Veiculo = item.veiculo;
                        model.HoraSaida = item.horarioSaida;
                        model.DataChegadaPrevista = horarioSeccao != 0 ? formataDataHora(item.data, horarioSeccao.ToString(), diaSoma) : null;
                        model.Origem = item.origem;
                        model.Destino = item.destino;
                        model.Linha = item.linha;
                        model.NumServico = item.numServico;
                        model.prefixoLinha = item.prefixoLinha;
                        model.piso = item.piso;
                        model.plataforma = item.plataforma;
                        model.tipoViagem = item.tipoViagem;
                        model.Status = item.status;
                        model.CodOrigem = item.CodOrigem;
                        model.CodDestino = item.CodDestino;
                        model.seccoes = item.Seccao.ToList();
                        model.ServicosRelacionado = item.ServicosRelacionado.ToList();

                        listaServicoDTO.Add(model);
                    }
               // }               
                
               
               

                return listaServicoDTO;
            }
            return null;
        }

        private static string formataDataHora(string data, string hora, int somaDia)
        {

            if (hora.Length == 1)
                hora = "000" + hora;
            else if (hora.Length == 2)
                hora = "00" + hora;           
            

            var _data = "";        
            var dataAtualizada = new DateTime();
  
            dataAtualizada = new DateTime(Convert.ToInt32("20"+data.Substring(0, 2)), Convert.ToInt32(data.Substring(2, 2)), Convert.ToInt32(data.Substring(4)));
            dataAtualizada = dataAtualizada.Date;
            dataAtualizada = dataAtualizada.AddDays(somaDia);

            if (hora.Length < 4)
            {           
                dataAtualizada = dataAtualizada.AddHours(Convert.ToInt32("0" + hora.Substring(0, 1)));
                dataAtualizada = dataAtualizada.AddMinutes(Convert.ToInt32(hora.Substring(1)));
            }
            else
            {
                dataAtualizada = dataAtualizada.AddHours(Convert.ToInt32(hora.Substring(0, 2)));
                dataAtualizada = dataAtualizada.AddMinutes(Convert.ToInt32(hora.Substring(2)));
            }

            _data = dataAtualizada.ToString("yyyy-MM-dd HH:mm:ss");


            return _data;
        }

        public List<ServicoDTO> TratarRetornoPlacaMotorista(XmlDocument Servicos)
        {
            if (Servicos != null)
            {
                var xmlReader = new XmlNodeReader(Servicos);

                servicoes servicosRJ = null;
                //{"<servicos xmlns=''> não era esperado."}
                XmlRootAttribute xRoot = new XmlRootAttribute();
                xRoot.ElementName = "servicos";
                xRoot.IsNullable = true;

                XmlSerializer serializer = new XmlSerializer(typeof(servicoes), xRoot);
                servicosRJ = (servicoes)serializer.Deserialize(xmlReader);

                var servicos = servicosRJ.servicos.Select(row => new ServicoDTO
                {
                    Data = row.data,
                    HoraSaida = row.horarioSaida,
                    Origem = row.origem,
                    Destino = row.destino,
                    Linha = row.linha,
                    NumServico = row.numServico,
                    Veiculo = row.veiculo,
                    Motorista = row.motorista,
                    DataChegadaPrevista = row.dataChegadaPrevista                   
                }).ToList();

                return servicos;
            }
            return null;
        }

        public List<ServicoDTO> TratarRetornoPlacaMotoristaGM(XmlDocument Servicos)
        {
            if (Servicos != null)
            {
                Servicos.InnerXml = Servicos.InnerXml.Replace("<Servico>", "<servico>").Replace("</Servico>", "</servico>");

                var xmlReader = new XmlNodeReader(Servicos);

                servicoes servicosRJ = null;
                //{"<servicos xmlns=''> não era esperado."}
                XmlRootAttribute xRoot = new XmlRootAttribute();
                xRoot.ElementName = "servicos";
                xRoot.IsNullable = true;

                XmlSerializer serializer = new XmlSerializer(typeof(servicoes), xRoot);
                servicosRJ = (servicoes)serializer.Deserialize(xmlReader);

                var servicos = servicosRJ.servicos.Select(row => new ServicoDTO
                {
                    Data = row.data,
                    HoraSaida = row.horarioSaida,
                    Origem = row.origem,
                    Destino = row.destino,
                    Linha = row.linha,
                    NumServico = row.numServico,
                    Veiculo = row.veiculo,
                    Motorista = row.motorista,
                    DataChegadaPrevista = row.dataChegadaPrevista
                }).ToList();

                return servicos;
            }
            return null;
        }
    }
}
