using System;
using System.Collections.Generic;
using System.Linq;
using NewsGPS.Domain;
using NewsGPS.Repository;
using NewsGPS.Contracts.Enums;
using NewsGPS.Contracts.DTO;
using IntegradorMonitriip.DataRepository;
using IntegradorRepositoryAzure;
using System.Data.Entity;
using IntegradorMonitriip.Model;

namespace IntegradorMonitriip.Jobs
{
    public static class BenchmarkJob
    {
        public static void ProcessarDados()
        {
            try
            {
                var repBenchmark = new BenchmarkRepository();
                var rep = new IntegradorRepositoryAzure.ErrosIntegracaoRepository();
                var qry = rep.GetQuery();
                var dtRef = DateTime.Now.AddDays(-1).Date;//new DateTime(2017, 07, 31);
                var dtRef1 = DateTime.Now.Date; // new DateTime(2017, 08, 1);
                var emp = new GetEmpresas();

                var empresas = emp.getNomesDtos();
                var qryR = qry.Where(x => x.DataHoraEvento >= dtRef && x.DataHoraEvento < dtRef1);
                var res = qryR.AsNoTracking().ToList();

                if (res.Count() == 0)
                {
                    return;
                }

                var groups = res.GroupBy(g => g.IDCliente).ToList();
                var list = new List<BenchmarkLog>();

                foreach (var group in groups)
                {
                    var dados = new List<Model.ErrosIntegracaoLog>();

                    var dto = getDadosAnaliticos(group.ToList(), empresas, dtRef);
                    if (dto != null)
                        list.Add(dto);
                }

                var pkGroups = list.GroupBy(x => x.PartitionKey).ToList();
                foreach (var group in pkGroups)
                {
                    repBenchmark.MultiplesAdd(group.ToList());
                }

                MapeamentoViagensJob.Main(res, dtRef);
            }
            catch (Exception ex)
            {
            }
        }

        public static void ProcessarDadosLoop()
        {
            try
            {
                var repBenchmark = new BenchmarkRepository();
                var rep = new IntegradorRepositoryAzure.ErrosIntegracaoRepository();
                var qry = rep.GetQuery();
                var dtRef = DateTime.Now.AddDays(-1).Date;//new DateTime(2017, 07, 31);
                var dtRef1 = DateTime.Now.Date; // new DateTime(2017, 08, 1);
                var emp = new GetEmpresas();

                var empresas = emp.getNomesDtos();
                for (int i = 0; i < 68; i++)
                {
                    var qryR = qry.Where(x => x.DataHoraEvento >= dtRef && x.DataHoraEvento < dtRef1);
                    var res = qryR.AsNoTracking().ToList();

                    if (res.Count() == 0)
                    {
                        return;
                    }

                    var groups = res.GroupBy(g => g.IDCliente).ToList();
                    var list = new List<BenchmarkLog>();

                    foreach (var group in groups)
                    {
                        var dados = new List<Model.ErrosIntegracaoLog>();

                        var dto = getDadosAnaliticos(group.ToList(), empresas, dtRef);
                        if (dto != null)
                            list.Add(dto);
                    }

                    var pkGroups = list.GroupBy(x => x.PartitionKey).ToList();
                    foreach (var group in pkGroups)
                    {
                        repBenchmark.MultiplesAdd(group.ToList());
                    }

                    MapeamentoViagensJob.Main(res, dtRef);

                    dtRef = dtRef.AddDays(-1);
                    dtRef1 = dtRef1.AddDays(-1);
                }

                //var repBenchmark = new BenchmarkRepository();
                //var rep = new IntegradorRepositoryAzure.ErrosIntegracaoRepository();
                //IQueryable<Model.ErrosIntegracaoLog> qry = rep.GetQuery();
                //var dtRef = DateTime.Now.AddDays(-4).Date;//new DateTime(2017, 07, 31);
                //var dtRef1 = DateTime.Now.AddDays(-3).Date; // new DateTime(2017, 08, 1);
                ////var emp = new GetEmpresas();
                ////var empresas = emp.getNomesDtos();
                //for (var i = 0; i < 180; i++)
                //{
                //    var qryR = qry.Where(x => x.DataHoraEvento >= dtRef && x.DataHoraEvento < dtRef1);
                //    var res = qryR.AsNoTracking().ToList();

                //    if (res.Count() == 0)
                //    {
                //        return;
                //    }

                //    MapeamentoViagensJob.Main(res, dtRef);

                //    dtRef = dtRef.AddDays(1);
                //    dtRef1 = dtRef1.AddDays(1);
                //}

            }
            catch (Exception ex)
            {
            }
        }

        public static BenchmarkLog getDadosAnaliticos(List<Model.ErrosIntegracaoLog> entities, List<IdNomeDTO> empresas, DateTime dtRef)
        {
            try
            {
                var entity = entities.OrderByDescending(x => x.DataHoraEvento).FirstOrDefault();
                var idCliente = entity.IDCliente;
                var dto = new BenchmarkLog();

                dto.PartitionKey = entity.PartitionKey;
                dto.RowKey = entity.RowKey;
                var nomeEmpresa = empresas.Where(x => x.Id == idCliente).Select(x => x.Nome).FirstOrDefault();
                dto.NomeEmpresa = string.IsNullOrEmpty(nomeEmpresa)
                    ? GetEmpresas.getbyId(idCliente) : nomeEmpresa;
                dto.Grades = entities.Where(x => x.idTpErro == (int)ETipoErro.Grades).Count();
                dto.Vendas = entities.Where(x => x.idTpErro == (int)ETipoErro.Vendas).Count();
                dto.Passagens = entities.Where(x => x.idTpErro == (int)ETipoErro.ListaPassagem).Count();
                dto.ValidacaoPassagem = entities.Where(x => x.idTpErro == (int)ETipoErro.ValidaPassagem).Count();
                dto.BilheteLido = entities.Where(x => x.idTpErro == (int)ETipoErro.BilheteLido).Count();
                dto.BilheteDigitado = entities.Where(x => x.idTpErro == (int)ETipoErro.BilheteDigitado).Count();
                dto.Total = entities.Count();
                dto.dataReferencia = Convert.ToInt32(dtRef.ToString("yyyyMMdd"));
                //dto.ultimoErro = entity.DataHoraEvento.AddHours(-3).ToString("yyyy/MM/dd HH:mm:ss");
                dto.idCliente = idCliente;

                return dto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        static List<Email> lista = new List<Email>();
        public static void CheckStatusIntegracao()
        {
            try
            {
                var rep = new StatusRequestRepository();

                var qry = rep.GetQuery();

                var res = qry.AsNoTracking().ToList();

                var groups = res.GroupBy(x => x.IDCliente).ToList();

                foreach (var group in groups)
                {
                    var tpErroGroup = group.GroupBy(g => g.idTpErro).ToList();

                    foreach (var tp in tpErroGroup)
                    {
                        var ultInt = tp.Where(x => x.erro == false).LastOrDefault();
                        if (ultInt == null)
                            continue;

                        if (
                            (ultInt.idTpErro == 2 && ultInt.DataHoraEvento <= DateTime.UtcNow.AddHours(-2))
                            || (ultInt.idTpErro == 1 && ultInt.DataHoraEvento <= DateTime.UtcNow.AddHours(-6))
                            )
                        {

                            var ultErr = tp.Where(x => x.erro == true).LastOrDefault();


                            if (ultErr != null && ultInt.DataHoraEvento < ultErr.DataHoraEvento)
                            {
                                var email = new Email();

                                email = new Email()
                                {

                                    DescricaoErro = ultErr.descricao,
                                    Empresa = GetEmpresas.getbyId(ultErr.IDCliente),
                                    Url = ultErr.url,
                                    DataErro = ultErr.DataHoraEvento.AddHours(-3).ToString("dd/MM/yyyy HH:mm:ss"),
                                    DataSucesso = ultInt.DataHoraEvento.AddHours(-3).ToString("dd/MM/yyyy HH:mm:ss")
                                };

                                lista.Add(email);

                                // StatusRequestRepository repe = new StatusRequestRepository();                                
                                // repe.Delete(ultErr);
                            }

                        }
                    }
                }
                if (lista.Count() > 0)
                {
                    sendError();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void checkFalhaIntegracao()
        {
            try
            {
                var rep = new StatusRequestRepository();
                var qry = rep.GetQuery();
                var res = qry.AsNoTracking().ToList();
                var groups = res.GroupBy(x => x.IDCliente).ToList();

                foreach (var group in groups)
                {
                    var tpErroGroup = group.GroupBy(g => g.idTpErro).ToList();

                    foreach (var tp in tpErroGroup)
                    {
                        var ultInt = tp.Where(x => x.erro == false).FirstOrDefault();
                        if (ultInt == null)
                            continue;

                        if (
                            (ultInt.idTpErro == 2 && ultInt.DataHoraEvento <= DateTime.UtcNow.AddHours(-2))
                            || (ultInt.idTpErro == 1 && ultInt.DataHoraEvento <= DateTime.UtcNow.AddHours(-12))
                            )
                        {
                            var ultErr = tp.Where(x => x.erro == true).FirstOrDefault();
                        }

                    }
                }
            }
            catch
            {

            }
        }

        public static void sendError()
        {
            using (System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient())
            {
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("qsstatusreport@gmail.com", "cadeadoAZ");

                using (System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage())
                {
                    mail.From = new System.Net.Mail.MailAddress("qsstatusreport@gmail.com");
                    mail.To.Add(new System.Net.Mail.MailAddress("brunna.freire@quadrisystems.com.br"));
                    mail.To.Add(new System.Net.Mail.MailAddress("claudio.marcio@quadrisystems.com.br"));
                    mail.To.Add(new System.Net.Mail.MailAddress("MARCIELE@newsgps.com.br"));
                    mail.To.Add(new System.Net.Mail.MailAddress("BRENDA@newsgps.com.br"));
                    mail.To.Add(new System.Net.Mail.MailAddress("lucas.almeida@quadrisystems.com.br"));
                    mail.To.Add(new System.Net.Mail.MailAddress("suporte@quadrisystems.com.br"));
                    mail.To.Add(new System.Net.Mail.MailAddress("joao.vianna@quadrisystems.com.br"));
                    mail.To.Add(new System.Net.Mail.MailAddress("mark.silva@quadrisystems.com.br"));
                    mail.To.Add(new System.Net.Mail.MailAddress("rogerio@rjconsultores.com.br"));
                    mail.Subject = "Falha na integração";
                    mail.IsBodyHtml = true;


                    var inicio = @"<p>
	 </p>
<p>
    Prezado, <br>
	Ocorreu um erro ao realizar a integração. As informações estão listadas na tabela abaixo.<p>
<br>
<table border='1' cellpadding='1' cellspacing='1' style='width: 700px'>
	<tbody>
		<tr>
			<td>
				<strong>Empresa</strong></td>
			<td>
				<strong>URL</strong></td>
			<td>
				<strong>Última Falha</strong></td>
            <td>
				<strong>Última integração</strong></td>
            <td>
				<strong>Descrição do Erro</strong></td>
		</tr>";

                    var body = "";
                    foreach (var item in lista)
                    {
                        body = body + @"<tr>
			                    <td>
				                    " + item.Empresa + @"
			                    </td>
			                    <td>
                                    " + item.Url + @"
				                     </td>
			                    <td>
                                    " + item.DataErro + @"
				                     </td>
                                <td>
                                    " + item.DataSucesso + @"
				                     </td>
                                <td>
                                    " + item.DescricaoErro + @"
				                     </td>
		                    </tr>";
                    }

                    var fim = @"</tbody>
</table>
<br>
<p>
Att
	 </p>
<p>
	 </p>
";
                    ////.Substring(3, 2) + "/" + item.DataSucesso.Substring(0, 2) + "/" + item.DataSucesso.Substring(6, 4) + " " + item.DataSucesso.Substring(11) + @"
                    mail.Body = inicio + body + fim;

                    //foreach (string file in listBoxAttachments.Items)
                    //{
                    //    mail.Attachments.Add(new System.Net.Mail.Attachment(file));
                    //}

                    //await smtp.SendMailAsync(mail);
                    smtp.Send(mail);
                    lista = new List<Email>();
                }
            }
        }

        public static void teste()
        {
            var rep = new StatusRequestRepository();
            rep.CheckStatusIntegracao();
        }

        private class Email
        {
            public string Empresa { get; set; }
            public string Url { get; set; }
            public string DataErro { get; set; }
            public string DataSucesso { get; set; }

            public string DescricaoErro { get; set; }
        }
    }
}
