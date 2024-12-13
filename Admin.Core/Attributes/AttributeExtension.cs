using System.Reflection;

namespace Admin.Domain.Attributes
{
    /// <summary>
    /// Classe para obter todos os atributos informados nas classes de Entidade (DTO) 
    /// </summary>
    public static class AttributeExtensions
    {
        /// <summary>
        /// Busca o conteudo de um atributo especifico de uma entidade especifica.
        /// </summary>
        public static TValue GetAttributeValue<TAttribute, TValue>(
            this Type type,
            Func<TAttribute, TValue> valueSelector)
            where TAttribute : Attribute
        {
            if (type.GetCustomAttributes(
                typeof(TAttribute), true
            ).FirstOrDefault() is TAttribute att)
            {
                return valueSelector(att);
            }
            return default;
        }

        /// <summary>
        /// Busca o conteudo de um atributo de um objeto a partir do nome da propriedade e do tipo do atributo.
        /// </summary>
        public static object GetAttributeValue(Type objectType, string propertyName, Type attributeType, string attributePropertyName)
        {
            var propertyInfo = objectType.GetProperty(propertyName);
            if (propertyInfo != null)
            {
                if (Attribute.IsDefined(propertyInfo, attributeType))
                {
                    var attributeInstance = Attribute.GetCustomAttribute(propertyInfo, attributeType);
                    if (attributeInstance != null)
                    {
                        foreach (PropertyInfo info in attributeType.GetProperties())
                        {
                            if (info.CanRead &&
                              String.Compare(info.Name, attributePropertyName,
                              StringComparison.InvariantCultureIgnoreCase) == 0)
                            {
                                return info.GetValue(attributeInstance, null);
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
