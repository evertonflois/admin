using System;

namespace Admin.Domain.Attributes
{
    /// <summary>
    /// Declaração do atributo contendo o nome da procedure de manutenção
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class ProcedureMaintenanceAttribute : Attribute
    {
        public ProcedureMaintenanceAttribute(string stProc)
        {
            _stProc = stProc;
            _funcaoInsert = "I";
            _funcaoUpdate = "U";
            _funcaoDelete = "D";
        }

        protected string _stProc;
        /// <summary>
        /// Nome da Stored Procedure
        /// </summary>
        public string stProc 
        {
            get
            {
                return _stProc;
            }
            set
            {
                _stProc = value;
            }
        }

        private string _funcaoInsert;
        /// <summary>
        /// Função Insert -
        /// Valor Padrão = I
        /// </summary>
        public string funcaoInsert
        {
            get
            {
                return _funcaoInsert;
            }
            set
            {
                _funcaoInsert = value;
            }
        }

        private string _funcaoUpdate;
        /// <summary>
        /// Função Update -
        /// Valor Padrão = U
        /// </summary>
        public string funcaoUpdate
        {
            get
            {
                return _funcaoUpdate;
            }
            set
            {
                _funcaoUpdate = value;
            }
        }

        private string _funcaoDelete;
        /// <summary>
        /// Função Delete -
        /// Valor Padrão = D
        /// </summary>
        public string funcaoDelete
        {
            get
            {
                return _funcaoDelete;
            }
            set
            {
                _funcaoDelete = value;
            }
        }
    }
}
