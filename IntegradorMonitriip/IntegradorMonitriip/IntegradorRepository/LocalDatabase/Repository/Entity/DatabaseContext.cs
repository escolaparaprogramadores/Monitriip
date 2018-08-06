namespace IntegradorRepository.LocalDatabase.Repository.Entity
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext()
            : base("name=prodCon")
        {
        }

        public virtual DbSet<Codigo_Conexao> CodCon { get; set; }
        public virtual DbSet<Com_Empresa> Pessoa { get; set; }
        public virtual DbSet<Com_Empresa_Conexao> PessoaCon { get; set; }
        public virtual DbSet<Com_Empresa_Func> Pessoa_Func { get; set; }
        public virtual DbSet<Com_Empresa_PF> Pessoa_PF { get; set; }
        public virtual DbSet<Com_Empresa_PJ> Pessoa_PJ { get; set; }
        public virtual DbSet<GPS_Linha> Linha { get; set; }
        public virtual DbSet<GPS_Linha_Ponto> LinhaPonto { get; set; }
        public virtual DbSet<GPS_Linha_Rota> LinhaRota { get; set; }
        public virtual DbSet<GPS_PontoReferencia> PontoReferencia { get; set; }
        public virtual DbSet<GPS_PontoReferenciaIntegracao> RefIntegracao { get; set; }
        public virtual DbSet<GPS_Prefixo_Linha> PrefixoLinha { get; set; }
        public virtual DbSet<GPS_Rota> Rota { get; set; }
        public virtual DbSet<Ope_GradeOperacao> GradeOperacao { get; set; }
        public virtual DbSet<Ope_GradeOperacaoOnibus> GradeOperacaoOnibus { get; set; }
        public virtual DbSet<Ope_GradeOperacaoSeccao> GradeOperacaoSeccao { get; set; }
        public virtual DbSet<Ope_GradeOperacaoFretamento> GradeOperacaoFretamento { get; set; }
        public virtual DbSet<Tbl_Veiculo> Veiculo { get; set; }
        public virtual DbSet<Ope_Eventos> Eventos { get; set; }
        public virtual DbSet<ServicosRelacionados> ServicosRelacionados { get; set; }

        public virtual DbSet<Logs_ServicosRelacionados> Logs_ServicosRelacionados { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Codigo_Conexao>()
                .Property(e => e.Codigo1)
                .IsFixedLength();

            modelBuilder.Entity<Codigo_Conexao>()
                .Property(e => e.Codigo2)
                .IsFixedLength();

            modelBuilder.Entity<Com_Empresa>()
                .Property(e => e.CnpjCpf)
                .IsFixedLength();

            modelBuilder.Entity<Com_Empresa>()
                .Property(e => e.IM_RG)
                .IsFixedLength();

            modelBuilder.Entity<Com_Empresa>()
                .Property(e => e.ChaveMaps)
                .IsUnicode(false);

            modelBuilder.Entity<Com_Empresa>()
                .HasMany(e => e.Ope_GradeOperacao)
                .WithOptional(e => e.Com_Empresa)
                .HasForeignKey(e => e.IDContratante);

            modelBuilder.Entity<Com_Empresa>()
                .HasMany(e => e.GPS_Linha)
                .WithOptional(e => e.Com_Empresa)
                .HasForeignKey(e => e.IDCliente);

            modelBuilder.Entity<Com_Empresa>()
                .HasMany(e => e.GPS_Linha1)
                .WithOptional(e => e.Com_Empresa1)
                .HasForeignKey(e => e.IDUsuarioAlteracao);

            modelBuilder.Entity<Com_Empresa>()
                .HasMany(e => e.GPS_Linha2)
                .WithRequired(e => e.Com_Empresa2)
                .HasForeignKey(e => e.IDUsuarioInclusao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Com_Empresa>()
                .HasMany(e => e.GPS_PontoReferencia)
                .WithOptional(e => e.Com_Empresa)
                .HasForeignKey(e => e.IDEmpresa);

            modelBuilder.Entity<Com_Empresa>()
                .HasMany(e => e.GPS_PontoReferencia1)
                .WithRequired(e => e.Com_Empresa1)
                .HasForeignKey(e => e.IDUsuarioInclusao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Com_Empresa>()
                .HasMany(e => e.GPS_PontoReferencia2)
                .WithOptional(e => e.Com_Empresa2)
                .HasForeignKey(e => e.IDUsuarioAlteracao);

            modelBuilder.Entity<Com_Empresa>()
                .HasMany(e => e.GPS_Rota)
                .WithRequired(e => e.Com_Empresa)
                .HasForeignKey(e => e.IDCliente)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Com_Empresa>()
                .HasMany(e => e.GPS_Rota1)
                .WithOptional(e => e.Com_Empresa1)
                .HasForeignKey(e => e.IDUsuarioAlteracao);

            modelBuilder.Entity<Com_Empresa>()
                .HasMany(e => e.GPS_Rota2)
                .WithRequired(e => e.Com_Empresa2)
                .HasForeignKey(e => e.IDUsuarioInclusao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Com_Empresa>()
                .HasMany(e => e.Ope_GradeOperacao1)
                .WithRequired(e => e.Com_Empresa1)
                .HasForeignKey(e => e.IDCliente)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Com_Empresa>()
                .HasMany(e => e.Ope_GradeOperacao2)
                .WithOptional(e => e.Com_Empresa2)
                .HasForeignKey(e => e.IDUsuarioCancelou);

            modelBuilder.Entity<Com_Empresa>()
                .HasMany(e => e.Ope_GradeOperacao3)
                .WithRequired(e => e.Com_Empresa3)
                .HasForeignKey(e => e.IDUsuarioCriacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Com_Empresa>()
                .HasMany(e => e.Ope_GradeOperacaoSeccao)
                .WithOptional(e => e.Com_Empresa)
                .HasForeignKey(e => e.IDCobrador);

            modelBuilder.Entity<Com_Empresa>()
                .HasMany(e => e.Ope_GradeOperacaoSeccao1)
                .WithOptional(e => e.Com_Empresa1)
                .HasForeignKey(e => e.IDMotorista);

            modelBuilder.Entity<Com_Empresa>()
                .HasMany(e => e.Ope_GradeOperacaoSeccao2)
                .WithRequired(e => e.Com_Empresa2)
                .HasForeignKey(e => e.IDUsuarioCriacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Com_Empresa>()
                .HasMany(e => e.Tbl_Veiculo)
                .WithRequired(e => e.Com_Empresa)
                .HasForeignKey(e => e.IDCliente)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Com_Empresa>()
                .HasMany(e => e.Tbl_Veiculo1)
                .WithRequired(e => e.Com_Empresa1)
                .HasForeignKey(e => e.IDContratante)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Com_Empresa>()
                .HasMany(e => e.Tbl_Veiculo2)
                .WithOptional(e => e.Com_Empresa2)
                .HasForeignKey(e => e.IDUsuarioAlteracao);

            modelBuilder.Entity<Com_Empresa>()
                .HasMany(e => e.Tbl_Veiculo3)
                .WithRequired(e => e.Com_Empresa3)
                .HasForeignKey(e => e.IDUsuarioInclusao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Com_Empresa>()
                .HasMany(e => e.Com_Empresa1)
                .WithOptional(e => e.Com_Empresa2)
                .HasForeignKey(e => e.IDUsuarioAlteracao);

            modelBuilder.Entity<Com_Empresa>()
                .HasMany(e => e.Com_Empresa11)
                .WithRequired(e => e.Com_Empresa3)
                .HasForeignKey(e => e.IDUsuarioInclusao);

            modelBuilder.Entity<Com_Empresa_Conexao>()
                .Property(e => e.IPFila)
                .IsFixedLength();

            modelBuilder.Entity<Com_Empresa_Func>()
                .Property(e => e.Comissao)
                .HasPrecision(6, 2);

            modelBuilder.Entity<Com_Empresa_Func>()
                .Property(e => e.Salario)
                .HasPrecision(9, 2);

            modelBuilder.Entity<Com_Empresa_PF>()
                .Property(e => e.OrgaoExpedidor)
                .IsFixedLength();

            modelBuilder.Entity<Com_Empresa_PF>()
                .HasMany(e => e.Com_Empresa_Func)
                .WithOptional(e => e.Com_Empresa_PF)
                .HasForeignKey(e => e.IDGerente);

            modelBuilder.Entity<Com_Empresa_PF>()
                .HasOptional(e => e.Com_Empresa_Func1)
                .WithRequired(e => e.Com_Empresa_PF1);

            modelBuilder.Entity<Com_Empresa_PJ>()
                .Property(e => e.IE)
                .IsFixedLength();

            modelBuilder.Entity<GPS_Linha>()
                .Property(e => e.Numero)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<GPS_Linha>()
                .Property(e => e.AutorizacaoANTT)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<GPS_Linha>()
                .Property(e => e.CodigoANTT)
                .IsUnicode(false);

            modelBuilder.Entity<GPS_Linha>()
                .HasMany(e => e.GPS_Linha_Rota)
                .WithRequired(e => e.GPS_Linha)
                .HasForeignKey(e => e.IDLinha)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GPS_Linha>()
                .HasMany(e => e.GPS_Prefixo_Linha)
                .WithRequired(e => e.GPS_Linha)
                .HasForeignKey(e => e.IDLinha)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GPS_Linha_Rota>()
                .HasMany(e => e.GPS_Linha_Ponto)
                .WithRequired(e => e.GPS_Linha_Rota)
                .HasForeignKey(e => e.IDLinhaRota)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GPS_Linha_Rota>()
                .HasMany(e => e.Ope_GradeOperacaoOnibus)
                .WithRequired(e => e.GPS_Linha_Rota)
                .HasForeignKey(e => e.IdLinhaRota)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GPS_PontoReferencia>()
                .HasMany(e => e.GPS_Linha)
                .WithRequired(e => e.GPS_PontoReferencia)
                .HasForeignKey(e => e.IDPontoDestino)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GPS_PontoReferencia>()
                .HasMany(e => e.GPS_Linha1)
                .WithRequired(e => e.GPS_PontoReferencia1)
                .HasForeignKey(e => e.IDPontoOrigem)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GPS_PontoReferencia>()
                .HasMany(e => e.GPS_Linha_Ponto)
                .WithRequired(e => e.GPS_PontoReferencia)
                .HasForeignKey(e => e.IDPontoReferencia)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GPS_PontoReferencia>()
                .HasMany(e => e.GPS_PontoReferenciaIntegracao)
                .WithOptional(e => e.GPS_PontoReferencia)
                .HasForeignKey(e => e.IDPontoReferencia);

            modelBuilder.Entity<GPS_PontoReferencia>()
                .HasMany(e => e.GPS_Rota)
                .WithRequired(e => e.GPS_PontoReferencia)
                .HasForeignKey(e => e.IDPontoDestino)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GPS_PontoReferencia>()
                .HasMany(e => e.GPS_Rota1)
                .WithRequired(e => e.GPS_PontoReferencia1)
                .HasForeignKey(e => e.IDPontoOrigem)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GPS_PontoReferencia>()
                .HasMany(e => e.Ope_GradeOperacaoSeccao)
                .WithRequired(e => e.GPS_PontoReferencia)
                .HasForeignKey(e => e.IDPontoDestino)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GPS_PontoReferencia>()
                .HasMany(e => e.Ope_GradeOperacaoSeccao1)
                .WithRequired(e => e.GPS_PontoReferencia1)
                .HasForeignKey(e => e.IDPontoOrigem)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GPS_PontoReferenciaIntegracao>()
                .Property(e => e.CodIntegracao)
                .IsFixedLength();

            modelBuilder.Entity<GPS_PontoReferenciaIntegracao>()
                .Property(e => e.CodOrgaoIntegracao)
                .IsFixedLength();

            modelBuilder.Entity<GPS_Prefixo_Linha>()
                .Property(e => e.Prefixo)
                .IsFixedLength();

            modelBuilder.Entity<GPS_Rota>()
                .HasMany(e => e.GPS_Linha_Rota)
                .WithRequired(e => e.GPS_Rota)
                .HasForeignKey(e => e.IDRota)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ope_GradeOperacao>()
                .Property(e => e.AutorizacaoANTT)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Ope_GradeOperacao>()
                .Property(e => e.IMEI)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Ope_GradeOperacao>()
                .HasOptional(e => e.Ope_GradeOperacaoOnibus)
                .WithRequired(e => e.Ope_GradeOperacao);

            modelBuilder.Entity<Ope_GradeOperacaoFretamento>()
                .HasOptional(e => e.Ope_GradeOperacaoOnibus)
                .WithRequired(e => e.Ope_GradeOperacao);

            modelBuilder.Entity<Ope_GradeOperacaoOnibus>()
                .Property(e => e.CodigoSRVP)
                .IsFixedLength();

            modelBuilder.Entity<Ope_GradeOperacaoOnibus>()
                .Property(e => e.CodFretamento)
                .IsUnicode(false);

            modelBuilder.Entity<Ope_GradeOperacaoOnibus>()
                .HasMany(e => e.Ope_GradeOperacaoSeccao1)
                .WithRequired(e => e.Ope_GradeOperacaoOnibus1)
                .HasForeignKey(e => e.IDGradeOperacao);

            modelBuilder.Entity<Ope_GradeOperacaoOnibus>()
                .HasMany(e => e.Ope_GradeOperacaoSeccao2)
                .WithRequired(e => e.Ope_GradeOperacaoOnibus2)
                .HasForeignKey(e => e.IDGradeOperacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ope_GradeOperacaoSeccao>()
                .HasMany(e => e.Ope_GradeOperacaoOnibus)
                .WithOptional(e => e.Ope_GradeOperacaoSeccao)
                .HasForeignKey(e => e.IDSeccaoAtual);

            modelBuilder.Entity<Tbl_Veiculo>()
                .Property(e => e.Identificacao)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_Veiculo>()
                .Property(e => e.Placa)
                .IsFixedLength();

            modelBuilder.Entity<Tbl_Veiculo>()
                .Property(e => e.Chassi)
                .IsFixedLength();

            modelBuilder.Entity<Tbl_Veiculo>()
                .Property(e => e.Bateria)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_Veiculo>()
                .Property(e => e.IPServer)
                .IsFixedLength();

            modelBuilder.Entity<Tbl_Veiculo>()
                .Property(e => e.RPMFATOR)
                .HasPrecision(7, 3);

            modelBuilder.Entity<Tbl_Veiculo>()
                .Property(e => e.IDRioCard)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_Veiculo>()
                .Property(e => e.TensaoLigado)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Tbl_Veiculo>()
                .Property(e => e.Rfid)
                .IsFixedLength();

            modelBuilder.Entity<Tbl_Veiculo>()
                .HasMany(e => e.Ope_GradeOperacao)
                .WithOptional(e => e.Tbl_Veiculo)
                .HasForeignKey(e => e.IDVeiculo);

            modelBuilder.Entity<Tbl_Veiculo>()
                .HasMany(e => e.Ope_GradeOperacaoSeccao)
                .WithOptional(e => e.Tbl_Veiculo)
                .HasForeignKey(e => e.IDVeiculo);

            modelBuilder.Entity<ServicosRelacionados>()
              .HasRequired(e => e.GradeOperacao)              
              .WithMany()
              .HasForeignKey(e => e.idGradeOperacao);

            modelBuilder.Entity<Logs_ServicosRelacionados>()
              .HasRequired(e => e.ServicosRelacionados)
              .WithMany()
              .HasForeignKey(e => e.ID_ServicoRelacionado);

            modelBuilder.Entity<Ope_GradeOperacao>()
              .HasMany(e => e.ServicosRelacionados)
              .WithRequired(e => e.GradeOperacao)
              .HasForeignKey(e => e.idGradeOperacao);
        }
    }
}
