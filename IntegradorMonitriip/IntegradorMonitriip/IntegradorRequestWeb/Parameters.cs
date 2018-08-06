using System;

namespace IntegradorRequestWeb.RequestWeb
{
    public static class Parameters
    {
        public static bool inicializado = false;
        public static bool inicializado1 = false;
        public static DateTime DataUltimaIntegracao;
        public static DateTime DataUltimaIntegracaoVendas;
        public static DateTime DataUltimaImportacao;
        public static DateTime DataUltimaIntegracaoItamarati;
        public static string URL_SERVICO = "http://qslinuxrj.cloudapp.net:9991/";
        public static string URL_SERVICO2 = "http://qslinuxrj.cloudapp.net:9993/";
        //public static string URL_SERVICO_NORDESTE = "http://newsgps.expnordeste.com.br:9991/WSMonitriipRJ/";
        public static string URL_SERVICO_NORDESTE = "http://newsgps.expnordeste.com.br:9991/";
        public static string URL_SERVICO_ANDORINHA = "http://sistema.andorinha.com:9992/";
        public static string URL_SERVICO_MONITRIIPRJ = "http://qslinuxrj.cloudapp.net:9991/WSMonitriipRJ/";
        //public static string URL_ANTT = "http://appservices.antt.gov.br:8956/antt/monitriip/";
        //http://appservices.antt.gov.br:8000/antt/monitriip.validacao/rest/
        //public static string URL_ANTT = "http://appservices.antt.gov.br:8000/antt/monitriip.validacao/";
        public static string URL_ANTT = "http://appservices.antt.gov.br:9000/antt/monitriip.monitoramento/";
        public static string URL_ANTT_LOG = "http://appservices.antt.gov.br:9000/antt/monitriip.monitoramento/rest/";
        public static int SLEEP_TIME = 360000;
        public static int SLEEP_TIME_ITAMARATI = 60000;
        public static int SLEEP_TIME_ATUALIZA_GRADE = 21600000;
        public static int SLEEP_TIME_BENCHMARK = (360000 * 12) * 24;
        public static int SLEEP_TIME_REEENVIO = 1000;
        public static int SLEEP_TIME_REEENVIO_VELOCIDADE_TEMP_LOC = 1000;
        //public static int SLEEP_TIME = 600000;
        public static int TIMEOUT = 120000;
        public static string USERNAME = "newgps";
        public static string PASSWORD = "rjnewgpsrj";
        public static string BUSCA_SERVICO_VIACAO_OURO_PRATA = "{0}monitriip/WSMonitriip/busca/buscaServico/{1}/{3}";
        //public static string BUSCA_SERVICO = "{0}WSMonitriipRJ/busca/buscaServico/{1}/{3}/{2}";
        public static string BUSCA_SERVICO = "{0}WSMonitriipRJ/busca/buscaServicoDetalhado2/{1}/{3}/{2}";
        public static string BUSCA_SERVICO_DETALHADO = "{0}WSMonitriipRJ/busca/buscaServicoDetalhado/{1}/{3}/{2}";
        public static string BUSCA_LOCALIDADE = "{0}WSMonitriipRJ/busca/buscaLocalidade/{1}/{2}";
        public static string BUSCA_SERVICO_UNESUL = "http://sistemas.unesul.com.br/monitriip/buscaServico/{0}/{1}";
        public static string BUSCA_BILHETES_UNESUL = "http://sistemas.unesul.com.br/monitriip/buscaBilhetes/{0}/{1}/{2}";
        public static string CODIGO_EMPRESA_UNESUL = "1";

        public static string BUSCA_SERVICO_ITAMARATI = "http://monitriip.expressoitamarati.com.br/buscaServico";

        public static string BUSCA_VENDAS_GM = "http://200.143.16.42:8084/gm/integrador/I01WCFWEBQUADRISYSTEM/MOR/WS_buscaVendas";
        public static string BUSCA_SERVICO_GM = "http://200.143.16.42:8084/gm/integrador/I01WCFWEBQUADRISYSTEM/CTL/WS_buscaServicoDetalhado";

        public static string TOKEN_PROD_ANTT = "f014dc6f-ec27-4dc5-90e1-b1415e58783d";
        public static string TOKEN = "f014dc6f-ec27-4dc5-90e1-b1415e58783d";

        public static string TOKEN_ORIONSAT_HOMOLOG = "798b0bf2-7741-4d51-aa01-3bce5017967c";
        public static string TOKEN_NEWS_PROD = "db22a8a8-cc60-4ffe-a43b-aa8fdb63c8be";
        public static string TOKEN_ZOOMSAT = "574a74e7-7f26-4ef6-b14f-ac036ba1d1c3";
        public static string TOKEN_SMART = "2fd51cc8-a139-427f-902a-c5f1e463f3e3";
        //public static string TOKEN_SMART = "85e3c36d-d5d0-44f1-a367-608e16264c41";
        //public static string TOKEN_Agadelha = "9ff34687-fa25-4e68-91d5-797746f76f31";
        public static string TOKEN_Agadelha = "e162ecff-fd06-4fc9-a700-a4dc06713796";
        //public static string EUCATUR_URL_HOM = "https://api-teste.eucatur.com.br/api-portais/v3";
        //public static string EUCATUR_URL_PROD = "https://api.eucatur.com.br/api-portais/v3";
        public static string EUCATUR_URL_MONITRIIP = "https://monitriip.eucatur.com.br/v1{0}";
       
        public static int IDENTIFICADOR_UNESUL = 1582;
        public static int IDENTIFICADOR_ITAMARATI = 7937;

        //key="WebApi_ANTT_Token" value="a237fd18-5b70-4dbc-8220-4f802fdf4192"
    }
}
