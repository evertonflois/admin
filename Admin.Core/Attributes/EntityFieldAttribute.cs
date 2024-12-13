using System;

namespace Admin.Domain.Attributes
{
    /// <summary>
    /// Declaração dos atributos de entidade
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class EntityField : Attribute
    {
        public EntityField()
        {
            _persists = true;
            _isKey = false;
        }

        private bool _persists;
        /// <summary>
        /// Persiste no banco de dados
        /// </summary>
        public bool Persists
        {
            get
            {
                return _persists;
            }
            set
            {
                _persists = value;
            }
        }

        private bool _ignoreOnUpdate;
        /// <summary>
        /// Ignorar no comando Update
        /// </summary>
        public bool IgnoreOnUpdate
        {
            get
            {
                return _ignoreOnUpdate;
            }
            set
            {
                _ignoreOnUpdate = value;
            }
        }

        private bool _isKey;
        /// <summary>
        /// É chave da tabela
        /// </summary>
        public bool IsKey
        {
            get
            {
                return _isKey;
            }
            set
            {
                _isKey = value;
            }
        }
    }
}
