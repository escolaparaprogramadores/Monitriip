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
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace IntegradorMonitriip
{
    partial class AnttLog : ServiceBase
    {
        private static bool isRunning = false;
        private static Thread servicoThread;
        private static Thread servicoGMThread;
        private static Thread passagemThread;
        private static Thread vendasThread;
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

        public AnttLog()
        {
            InitializeComponent();
            //OnStart(new string[] { });

        }

        protected override void OnStop()
        {
            Stop();
        }

        protected override void OnStart(string[] args)
        {
            Main();
        }

        private static void Main()
        {
            isRunning = true;
            InciarThreadsReenvioLogsAntt();    
        }

        static void InciarThreadsReenvioLogsAntt()
        {
            ReenvioThread = new Thread(ReenvioLogs);
            ReenvioThread.Start();

            ReenvioJornadaMotoristaThread = new Thread(ReenvioJornadaMotoristaLogs);
            ReenvioJornadaMotoristaThread.Start();

            ReenvioDetectorParadaThread = new Thread(ReenvioDetectorParadaLogs);
            ReenvioDetectorParadaThread.Start();

            for (int i = 0; i < 50; i++)
            {
                Thread a = new Thread(ReenvioVelocidadeTempoLocalizacaoLogs);
                a.Start();
                Thread.Sleep(100);
            }

            ReenvioLeitorBilheteEmbarque = new Thread(ReenvioLeitorBilheteEmbarqueLogs);
            ReenvioLeitorBilheteEmbarque.Start();

            ReenvioLeitorBilheteEmbarque2 = new Thread(ReenvioLeitorBilheteEmbarqueLogs);
            ReenvioLeitorBilheteEmbarque2.Start();

        }

        static void deleteLogs()
        {
            var rep = new VendasRepository();
            rep.deleteLogs();
        }

        static new void Stop()
        {
            isRunning = false;

            while (ReenvioThread.IsAlive || ReenvioJornadaMotoristaThread.IsAlive
                  || ReenvioDetectorParadaThread.IsAlive || ReenvioLeitorBilheteEmbarque.IsAlive || ReenvioLeitorBilheteEmbarque2.IsAlive || ReenvioLeitorBilheteEmbarque.IsAlive)
            {
                Thread.Sleep(100);
            }

            ServiceController sc = new ServiceController("AnttLog");
            sc.Stop();
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

    }
}

