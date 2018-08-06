namespace IntegradorMonitriip.Jobs
{
    internal class TDerros
    {
        private int cliente;
        private int sDtRef;

        public TDerros(int sDtRef, int cliente)
        {
            this.sDtRef = sDtRef;
            this.cliente = cliente;
        }

        public string Descricao { get; internal set; }
        public int TipoErro { get; internal set; }
        public int TotalN { get; internal set; }
    }
}