using System;

namespace Admin.Domain.Attributes
{
    /// <summary>
    /// Declaração do atributo contendo o nome da procedure de seleção
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class ProcedureQueryAttribute : Attribute
    {
        public ProcedureQueryAttribute(string stProc)
        {
            _stProc = stProc;
            _funcaoAll = "S";
            _funcaoKey = "K";
            _timeOut = 1;  
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

        private string _funcaoAll;
        /// <summary>
        /// Função All/Grid -
        /// Valor Padrão = S
        /// </summary>
        public string funcaoAll
        {
            get
            {
                return _funcaoAll;
            }
            set
            {
                _funcaoAll = value;
            }
        }

        private string _funcaoKey;
        /// <summary>
        /// Função Key/Form -
        /// Valor Padrão = K
        /// </summary>
        public string funcaoKey
        {
            get
            {
                return _funcaoKey;
            }
            set
            {
                _funcaoKey = value;
            }
        }

        private double _timeOut;
        /// <summary>
        /// Timeout (em minutos)
        /// Valor Padrão = 1
        /// </summary>
        public double timeOut
        {
            get
            {
                return _timeOut;
            }
            set
            {
                _timeOut = value;
            }
        }
    }
}
