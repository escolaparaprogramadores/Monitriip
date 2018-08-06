using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace IntegradorModel.Model
{

    [System.Runtime.Serialization.DataContractAttribute(Name = "passagemCancelada", Namespace = "http://antt.gov.br/monitriip/v1.0/")]
    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    public partial class passagemCancelada : object, System.Runtime.Serialization.IExtensibleDataObject
    {

        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private string idLogField;

        private string numeroBilheteEmbarqueField;

        private string identificacaoLinhaField;

        private string dataViagemField;

        private string horaViagemField;

        private string codigoMotivoCancelamentoField;

        private string dataHoraCancelamentoField;

        private string numeroNovoBilheteEmbarqueField;

        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string idLog
        {
            get
            {
                return this.idLogField;
            }
            set
            {
                this.idLogField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string numeroBilheteEmbarque
        {
            get
            {
                return this.numeroBilheteEmbarqueField;
            }
            set
            {
                this.numeroBilheteEmbarqueField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]

        public string identificacaoLinha
        {
            get
            {
                return this.identificacaoLinhaField;
            }
            set
            {
                this.identificacaoLinhaField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string dataViagem
        {
            get
            {
                return this.dataViagemField;
            }
            set
            {
                this.dataViagemField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public string horaViagem
        {
            get
            {
                return this.horaViagemField;
            }
            set
            {
                this.horaViagemField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public string codigoMotivoCancelamento
        {
            get
            {
                return this.codigoMotivoCancelamentoField;
            }
            set
            {
                this.codigoMotivoCancelamentoField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]

        public string dataHoraCancelamento
        {
            get
            {
                return this.dataHoraCancelamentoField;
            }
            set
            {
                this.dataHoraCancelamentoField = value;
            }
        }


        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]

        public string numeroNovoBilheteEmbarque
        {
            get
            {
                return this.numeroNovoBilheteEmbarqueField;
            }
            set
            {
                this.numeroNovoBilheteEmbarqueField = value;
            }
        }

    }
}
