using antt.gov.br.monitriip.v1._0;
using EucaturIntegrador.Jobs;
using IntegradorModel.Model;
using IntegradorMonitriip.BeforeRequest;
using IntegradorMonitriip.Jobs;
using IntegradorRepository.DataRepository;
using IntegradorRepository.LocalDatabase.Repository.Entity;
using IntegradorRepositoryAzure;
using IntegradorRequestWeb.RequestWeb;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace IntegradorMonitriip
{

    public partial class Service1 : ServiceBase
    {
        private static bool isRunning = false;
        private static Thread servicoThread;
        private static Thread servicoThreadUnesul;
        private static Thread servicoGMThread;
        private static Thread servicoItamaratiThread;
        private static Thread passagemThread;
        private static Thread vendasThread;
        private static Thread vendasThreadGM;
        private static Thread vendasItamaratiThread;
        private static Thread IntegracaoExternaThread;
        private static Thread BenchmarkThread;
        private static Thread EmailThread;
        private static Thread LocalidadesThread;
        private static Thread MapeamentoViagensThread;
        private static Thread ReenvioThread;
        public static List<ViagemModel> listaReenvio;
        private static Thread ReenvioJornadaMotoristaThread;
        private static Thread ReenvioDetectorParadaThread;
        private static Thread AtualizaGrade;
        private static Thread ReenvioLeitorBilheteEmbarque;
        private static Thread ReenvioLeitorBilheteEmbarque2;
        private static Thread ReenvioSalvarLogsTables;
        private static string QueueName = "iniciofimviagemregular";
        private static string QueueJornadaMotorista = "jornadamotorista";
        private static string QueueDetectorParada = "detectorparada";
        private static string QueueVelocidadeTempoLocalizacao = "velocidadetempolocalizacao";
        private static string QueueLeitorBilhete = "leitorbilheteembarque";
        public static List<ViagemModel> ListaModel = new List<ViagemModel>();
        public static List<Ope_GradeOperacaoOnibus> listaGradeOperacaoOnibus = new List<Ope_GradeOperacaoOnibus>();

        public Service1()
        {
            InitializeComponent();
            //string sSource;
            //string sLog;
            //string sEvent;
            //sSource = "Integrador de dados";
            //sLog = "Application";b
            //sEvent = "Iniciado";
            //if (!EventLog.SourceExists(sSource))
            // EventLog.CreateEventSource(sSource, sLog);
            //EventLog.WriteEntry(sSource, sEvent);
            //EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 777);
            /** PARA TESTAR O PROJETO É PRECISO DESCOMENTAR A LINHA ABAIXO **/
             OnStart(new string[] { "" });
            //Main();

        }

        protected override void OnStop()
        {
            Stop();
        }

        protected override void OnStart(string[] args)
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;
            Main();
        }











        //PASSO SERVIÇO 01
        private static void Main()
        {
            isRunning = true;

            ///////*Consulta de viagens da empresa Unesul*/
            //servicoThreadUnesul = new Thread(CriaServicoUnesul);
            //servicoThreadUnesul.Start();

            ///*Consulta de viagens da empresa GM*/
            //servicoGMThread = new Thread(CriaServicoGM);
            //servicoGMThread.Start();
            //vendasThreadGM = new Thread(GravarVendasGM);
            //vendasThreadGM.Start();

            ///////*Consulta de viagens da empresas vinculadas a RJ*/
            servicoThread = new Thread(CriaServico);
            servicoThread.Start();
            vendasThread = new Thread(GravarVendas);
            vendasThread.Start();

            ///*Consulta de viagens da empresas Itamarati*/
            //servicoItamaratiThread = new Thread(CriaServicoItamarati);
            //servicoItamaratiThread.Start();
            //vendasItamaratiThread = new Thread(GravarVendasItamarati);
            //vendasItamaratiThread.Start();

            ///*Inteligencia que armazena erros de integração no geral*/
            BenchmarkThread = new Thread(CountErrors);
            BenchmarkThread.Start();

            ///*Envio de email com as falhas de integração*/
            //EmailThread = new Thread(checkStatus);
            //EmailThread.Start();

            ///*Consulta de Siglas de localidade Antt com a RJ*/
            LocalidadesThread = new Thread(UpdateLocations);
            LocalidadesThread.Start();

            MapeamentoViagensThread = new Thread(MapeamentoViagens);
            MapeamentoViagensThread.Start();

            ///*Atualiza grade com data chegada real vazia*/
            AtualizaGrade = new Thread(ReescreveChegadaReal);
            AtualizaGrade.Start();

        }



        //PASSO SERVIÇO 02
        private static void CriaServico()
        {
            while (isRunning)
            {
                try
                {
                    /*Criação de serviço - Grade*/
                    ServicoJob.ProcessarServico(Parameters.URL_SERVICO);
                    Thread.Sleep(Parameters.SLEEP_TIME * 5);
                }
                catch (Exception ex)
                {
                }
            }
        }


  

























        static void deleteLogs()
        {
            var rep = new VendasRepository();
            rep.deleteLogs();
        }

        static new void Stop()
        {
            isRunning = false;

            ServiceController sc = new ServiceController("Service1");
            sc.Stop();
        }

        private static void ServicoExterno()
        {
            while (isRunning)
            {
                //RequestWeb.teste();
                ServicoEucaturJob.ProcessarServico(Parameters.URL_SERVICO);
                Thread.Sleep(Parameters.SLEEP_TIME);
            }
        }

        private static void CriaServicoUnesul()
        {
            while (isRunning)
            {
                try
                {
                    /*Criação de serviço - Grade*/
                    ServicoJob.ProcessaServicoUnesul();
                    Thread.Sleep(Parameters.SLEEP_TIME * 5);
                }
                catch (Exception ex)
                {

                    string sSource;
                    string sLog;
                    string sEvent;

                    sSource = "Integrador de dados - Empresa Unesul";
                    sLog = "Application";
                    sEvent = "Erro. Msg:" + ex.Message
                        + " /n  Inner:" + ex.InnerException
                        + " /n StackTrace" + ex.StackTrace;

                    if (!EventLog.SourceExists(sSource))
                        EventLog.CreateEventSource(sSource, sLog);

                    EventLog.WriteEntry(sSource, sEvent);
                    EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 777);
                }
            }
        }



        private static void CriaServicoItamarati()
        {
            while (isRunning)
            {
                try
                {
                    /*Criação de serviço - Grade*/
                    ServicoJob.ProcessarServicoItamarati();
                    Thread.Sleep(Parameters.SLEEP_TIME * 5);
                }
                catch (Exception ex)
                {

                }
            }
        }

        private static void CriaServicoGM()
        {
            while (isRunning)
            {
                try
                {
                    /*Criação de serviço - Grade*/
                    ServicoJob.ProcessarServicoGM();
                    Thread.Sleep(Parameters.SLEEP_TIME * 5);
                }
                catch (Exception ex)
                {

                }
            }
        }

        private static void GravarPassagens()
        {
            while (isRunning)
            {
                /*Gravar Passagens no banco*/
                PassagemJob.ProcessarPassagens(Parameters.URL_SERVICO);
                Thread.Sleep(Parameters.SLEEP_TIME);
            }
        }

        private static void ReescreveChegadaReal()
        {
            while (isRunning)
            {
                AtualizaGradeOperacao();
                AtualizaGradeOperacaoMotorista();
                Thread.Sleep(Parameters.SLEEP_TIME_ATUALIZA_GRADE);
            }
        }

        private static void GravarVendas()
        {
            while (isRunning)
            {
                /*Gravar Passagens no banco*/
                PassagemJob.ProcessarVendas(Parameters.URL_SERVICO);
                Thread.Sleep(Parameters.SLEEP_TIME);
            }
        }

        private static void GravarVendasGM()
        {
            while (isRunning)
            {
                /*Gravar Passagens no banco*/
                PassagemJob.ProcessarVendasGM();
                Thread.Sleep(Parameters.SLEEP_TIME);
            }
        }

        private static void GravarVendasItamarati()
        {
            while (isRunning)
            {
                /*Gravar Passagens no banco*/
                PassagemJob.ProcessarVendasItamarati(Parameters.URL_SERVICO);
                //PassagemJob.ProcessarVendasUnesul( );

                Thread.Sleep(Parameters.SLEEP_TIME_ITAMARATI);
            }
        }

        private static void CountErrors()
        {
            //BenchmarkJob.ProcessarDadosLoop();
            while (isRunning)
            {
                var newday = true;
                while (newday)
                {
                    if (DateTime.UtcNow.AddHours(-3).Hour == 0 /*&& DateTime.Now.Hour <= 1*/)
                        newday = false;
                    else
                        Thread.Sleep(Parameters.SLEEP_TIME * 12);
                }
                BenchmarkJob.ProcessarDados();
                //BenchmarkJob.ProcessarDadosLoop();
                Thread.Sleep(Parameters.SLEEP_TIME_BENCHMARK);
            }
        }

        private static void checkStatus()
        {
            while (isRunning)
            {
                //BenchmarkJob.teste();
                BenchmarkJob.CheckStatusIntegracao();
                Thread.Sleep(Parameters.SLEEP_TIME * 6);
            }
        }

        private static void UpdateLocations()
        {
            while (isRunning)
            {
                LocationJob.Main();
                Thread.Sleep(Parameters.SLEEP_TIME_BENCHMARK);
            }
        }

        private static void MapeamentoViagens()
        {
            while (isRunning)
            {
                var newday = true;
                while (newday)
                {
                    if (DateTime.UtcNow.AddHours(-3).Hour == 0)
                        newday = false;
                    else
                        Thread.Sleep(Parameters.SLEEP_TIME * 12);
                }
                var map = new MapeamentoViagensJob();
                map.formatData();
                Thread.Sleep(Parameters.SLEEP_TIME_BENCHMARK);
            }
        }

        private static void ReenvioLogs()
        {
            while (isRunning)
            {
                //SendMessages();
                ReceiveMessages(QueueName);
                Thread.Sleep(Parameters.SLEEP_TIME_REEENVIO);
            }
        }

        private static void ReenvioJornadaMotoristaLogs()
        {
            while (isRunning)
            {
                //SendMessages();
                ReceiveMessages(QueueJornadaMotorista);
                Thread.Sleep(Parameters.SLEEP_TIME_REEENVIO);
            }
        }

        private static void ReenvioDetectorParadaLogs()
        {
            while (isRunning)
            {
                //SendMessages();
                ReceiveMessages(QueueDetectorParada);
                Thread.Sleep(Parameters.SLEEP_TIME_REEENVIO);
            }
        }

        private static void ReenvioVelocidadeTempoLocalizacaoLogs()
        {
            String teste = "";

            while (isRunning)
            {
                //SendMessages();
                ReceiveMessagesVelocidadeTempoLocalizacao(QueueVelocidadeTempoLocalizacao);
                Thread.Sleep(Parameters.SLEEP_TIME_REEENVIO_VELOCIDADE_TEMP_LOC);
            }
        }

        private static void ReenvioLeitorBilheteEmbarqueLogs()
        {
            while (isRunning)
            {
                //SendMessages();
                ReceiveMessages(QueueLeitorBilhete);
                Thread.Sleep(Parameters.SLEEP_TIME_REEENVIO);
            }
        }

        private static void CreateQueue()
        {
            NamespaceManager namespaceManager = NamespaceManager.Create();

            Console.WriteLine("\nCreating Queue '{0}'...", QueueName);

            // Delete if exists 
            if (namespaceManager.QueueExists(QueueName))
            {
                namespaceManager.DeleteQueue(QueueName);
            }

            namespaceManager.CreateQueue(QueueName);
        }

        private static void ReceiveMessages(string Queue)
        {
            Console.WriteLine("\nReceiving message from Queue...");
            BrokeredMessage message = null;

            NamespaceManager namespaceManager = NamespaceManager.Create();
            QueueClient queueClient = QueueClient.Create(Queue);
            while (true)
            {
                try
                {
                    //receive messages from Queue 
                    message = queueClient.Receive(TimeSpan.FromSeconds(5));
                    if (message != null)
                    {
                        message.Complete();
                        ViagensBR.EnviaANTT(CarregaModel(message), message.Properties["webservice"].ToString(), Queue, (message.Properties["prefixo"] != null ? message.Properties["prefixo"].ToString() : null));

                    }
                    else
                    {
                        //no more messages in the queue 
                        break;
                    }
                }
                catch (MessagingException e)
                {
                    String teste = "";
                }
            }
            queueClient.Close();
        }

        private static void ReceiveMessagesVelocidadeTempoLocalizacao(string Queue)
        {
            Console.WriteLine("\nReceiving message from Queue...");
            BrokeredMessage message = null;

            NamespaceManager namespaceManager = NamespaceManager.Create();
            QueueClient queueClient = QueueClient.Create(Queue);

            while (true)
            {
                try
                {
                    //receive messages from Queue 
                    message = queueClient.Receive(TimeSpan.FromSeconds(5));

                    if (message != null)
                    {
                        message.Complete();
                        ViagensBR.EnviaANTT(CarregaModel(message), message.Properties["webservice"].ToString(), Queue, null);
                    }
                    else
                    {
                        //no more messages in the queue 
                        break;
                    }
                }
                catch (MessagingException e)
                {
                    String teste = "";
                }
            }

            queueClient.Close();
        }

        private static BrokeredMessage CreateSampleMessage(string messageId, string messageBody)
        {
            BrokeredMessage message = new BrokeredMessage(messageBody);
            message.MessageId = messageId;
            return message;
        }

        private static void HandleTransientErrors(MessagingException e)
        {
            //If transient error/exception, let's back-off for 2 seconds and retry 
            Console.WriteLine(e.Message);
            Console.WriteLine("Will retry sending the message in 2 seconds");
            Thread.Sleep(2000);
        }

        public static ViagemModel CarregaModel(BrokeredMessage message)
        {

            ViagemModel model = new ViagemModel();

            model.PartitionKey = message.Properties["partitionKey"].ToString();
            model.cnpjEmpresa = (message.Properties["cnpjEmpresa"] != null ? message.Properties["cnpjEmpresa"].ToString() : null);
            model.autorizacaoViagem = (message.Properties["autorizacaoViagem"] != null ? message.Properties["autorizacaoViagem"].ToString() : null);
            model.cpfMotorista = (message.Properties["cpfMotorista_Passageiro"] != null ? message.Properties["cpfMotorista_Passageiro"].ToString() : null);
            model.placaVeiculo = (message.Properties["placaVeiculo"] != null ? message.Properties["placaVeiculo"].ToString() : null);
            model.identificacaoLinha = (message.Properties["identificacaoLinha"] != null ? message.Properties["identificacaoLinha"].ToString() : null);
            model.dataProgramada = (message.Properties["dataProgramada"] != null ? message.Properties["dataProgramada"].ToString() : null);
            model.horaProgramada = (message.Properties["horaProgramada"] != null ? message.Properties["horaProgramada"].ToString() : null);
            model.codigoSentidoLinha = (message.Properties["codigoSentidoLinha"] != null ? Convert.ToInt32(message.Properties["codigoSentidoLinha"].ToString()) : 0);
            model.latitude = (message.Properties["latitude"] != null ? message.Properties["latitude"].ToString() : null);
            model.longitude = (message.Properties["longitude"] != null ? message.Properties["longitude"].ToString() : null);
            model.pdop = (message.Properties["pdop"] != null ? Convert.ToDecimal(message.Properties["pdop"].ToString()) : 0);
            model.dataHoraEvento = (message.Properties["dataHoraEvento"] != null ? Convert.ToDateTime(message.Properties["dataHoraEvento"].ToString()) : DateTime.UtcNow);
            model.IMEI = (message.Properties["imei"] != null ? message.Properties["imei"].ToString() : null);
            model.codigoTipoLogID = (message.Properties["codigoTipoLogID"] != null ? Convert.ToInt32(message.Properties["codigoTipoLogID"].ToString()) : 0);
            model.codigoTipoRegistroViagem = (message.Properties["codigoTipoRegistroViagem"] != null ? Convert.ToInt32(message.Properties["codigoTipoRegistroViagem"].ToString()) : 0);
            model.tempoViagem = (message.Properties["tempoViagem"] != null ? message.Properties["tempoViagem"].ToString() : null);
            model.tempoDescanso = (message.Properties["tempoDescanso"] != null ? message.Properties["tempoDescanso"].ToString() : null);
            model.KmPercorrido = (message.Properties["KmPercorrido"] != null ? message.Properties["KmPercorrido"].ToString() : null);
            model.TotalParada = (message.Properties["totalParada"] != null ? message.Properties["totalParada"].ToString() : null);
            model.TotalJustificativas = (message.Properties["totalJustificativas"] != null ? message.Properties["totalJustificativas"].ToString() : null);
            model.id_gradeoperacao = (message.Properties["Id_GradeOperacao"] != null ? Convert.ToInt32(message.Properties["Id_GradeOperacao"].ToString()) : 0);
            //message.Properties["rotaOrigem"].ToString();
            model.codigoTipoRegistroEvento = (message.Properties["codigoTipoRegistroEvento"] != null ? Convert.ToInt32(message.Properties["codigoTipoRegistroEvento"].ToString()) : 0);
            model.IDCliente = (message.Properties["idEmpresa"] != null ? Convert.ToInt32(message.Properties["idEmpresa"].ToString()) : 0);
            //message.Properties["Trace"].ToString();
            model.codigoMotivoParada = (message.Properties["codigoMotivoParada"] != null ? Convert.ToInt32(message.Properties["codigoMotivoParada"].ToString()) : 0);
            model.isTransbordo = (message.Properties["isTransbordo"] != null ? Convert.ToBoolean(message.Properties["isTransbordo"].ToString()) : false);

            if (message.Properties["NumeroBilhete"] != null)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                List<bilhete> bilhete = js.Deserialize<List<bilhete>>(message.Properties["NumeroBilhete"].ToString());

                model.NumeroBilheteEmbarque = bilhete;
            }

            //message.Properties["IDLinha"].ToString();
            //message.Properties["IDRota"].ToString();
            //message.Properties["Comentario"].ToString();
            //message.Properties["TipoComentario"].ToString();
            model.velocidadeAtual = (message.Properties["velocidadeAtual"] != null ? Convert.ToInt32(message.Properties["velocidadeAtual"].ToString()) : 0);
            model.distanciaPercorrida = (message.Properties["distanciaPercorrida"] != null ? Convert.ToInt32(message.Properties["distanciaPercorrida"].ToString()) : 0);
            model.codigoSituacaoIgnicaoMotor = (message.Properties["codigoSituacaoIgnicaoMotor"] != null ? Convert.ToInt32(message.Properties["codigoSituacaoIgnicaoMotor"].ToString()) : 0);
            model.codigoSituacaoPortaVeiculo = (message.Properties["codigoSituacaoPortaVeiculo"] != null ? Convert.ToInt32(message.Properties["codigoSituacaoPortaVeiculo"].ToString()) : 0);
            //message.Properties["statusBateria"].ToString();

            return model;
        }

        //private static void SavarLogsinTables()
        //{
        //    while (isRunning)
        //    {
        //        //SendMessages();

        //        for (int i = 0; i < ListaModel.Count; i++)
        //        {
        //            ViagensRepository rep = new ViagensRepository();
        //            ListaModel[i].RowKey = GetRowKey(ListaModel[i].dataHoraEvento, ListaModel[i].placaVeiculo, ListaModel[i].codigoTipoLogID);
        //            rep.UpdateLogViagem(ListaModel[i]);
        //            //rep.UpdateLogViagem(ListaModel[i]);
        //            ListaModel.Remove(ListaModel[i]);
        //        }

        //        Thread.Sleep(Parameters.SLEEP_TIME_REEENVIO_VELOCIDADE_TEMP_LOC);
        //    }
        //}

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

        public static void AtualizaGradeOperacao()
        {
            try
            {

                GradeOperacaoRepository rep = new GradeOperacaoRepository();
                var list = rep.GetAllGradeOperacaoSemDataChegadaReal();

                ViagensRepository anttLog = new ViagensRepository();

                for (int i = 0; i < list.Count; i++)
                {
                    if (!string.IsNullOrEmpty(list[i]))
                    {
                        var viagemLog = anttLog.getUltimoLogFimViagem(list[i].Replace(" ", ""));

                        if (!string.IsNullOrEmpty(viagemLog.PartitionKey))
                        {
                            var model = rep.GetGradeOperacao(list[i].Replace(" ", ""));
                            model.DataChegadaReal = viagemLog.dataHoraEvento;
                            try
                            {
                                rep.UpdateGrade(model);
                            }
                            catch (Exception ex)
                            {

                            }

                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }

        }

        public static void AtualizaGradeOperacaoMotorista()
        {
            try
            {

                GradeOperacaoSeccaoRepository repSeccao = new GradeOperacaoSeccaoRepository();
                GradeOperacaoRepository rep = new GradeOperacaoRepository();

                //var list = repSeccao.GetGradeOperacaoSeccaoMotoristaNull();
                var list = rep.GetListaGradeOperacao();

                ViagensRepository anttLog = new ViagensRepository();

                for (int i = 0; i < list.Count; i++)
                {
                    if (!string.IsNullOrEmpty(list[i].PartitionKey))// && list[i].IDMotorista == null)
                    {

                        var primeiroLog = anttLog.getPrimeiroLogFimViagem(list[i].PartitionKey.Replace(" ", ""));

                        if (!string.IsNullOrEmpty(primeiroLog.PartitionKey))
                        {
                            var model = rep.GetGradeOperacao(list[i].PartitionKey.Replace(" ", ""));
                            var modelSeccao = repSeccao.GetSeccaoByIdGradeOperacao(model.ID);


                            if (modelSeccao != null && modelSeccao.IDMotorista == null && !string.IsNullOrEmpty(primeiroLog.cpfMotorista))
                            {
                                try
                                {
                                    PessoaRepository repPes = new PessoaRepository();
                                    var IdPessoa = repPes.GetIDPessoaByCPF(primeiroLog.cpfMotorista);

                                    modelSeccao.IDMotorista = IdPessoa;

                                    repSeccao.UpdateSeccao(modelSeccao);
                                }
                                catch (Exception ex)
                                {

                                }                         
                            }
                            else if (!string.IsNullOrEmpty(primeiroLog.cpfMotorista) && modelSeccao == null)
                            {
                                try
                                {
                                    PessoaRepository repPes = new PessoaRepository();
                                    var IdPessoa = repPes.GetIDPessoaByCPF(primeiroLog.cpfMotorista);

                                        modelSeccao = new DatabaseContext().GradeOperacaoSeccao.Create();
                                        modelSeccao.IDGradeOperacao = model.ID;
                                        modelSeccao.IDPontoOrigem = model.Ope_GradeOperacaoOnibus.GPS_Linha_Rota.GPS_Linha.GPS_PontoReferencia1.ID;//origem
                                        modelSeccao.IDPontoDestino = model.Ope_GradeOperacaoOnibus.GPS_Linha_Rota.GPS_Linha.GPS_PontoReferencia.ID;
                                        modelSeccao.IDUsuarioCriacao = 103;
                                        modelSeccao.IDMotorista = IdPessoa;
                                   
                                        repSeccao.CreateSeccao(modelSeccao);                                    
                                   
                                }
                                catch (Exception ex)
                                {

                                }
                            }

                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }

        }
    }
}
