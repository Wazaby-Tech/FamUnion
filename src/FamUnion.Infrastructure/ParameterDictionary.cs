using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;

namespace FamUnion.Infrastructure
{
    public class ParameterDictionary
    {
        private IDictionary<string, object> _dictionary;

        #region Constructors

        public ParameterDictionary()
        {
            _dictionary = new Dictionary<string, object>();
        }

        public ParameterDictionary(IDictionary<string, object> dictionary)
            : this()
        {
            foreach (KeyValuePair<string, object> kvp in dictionary)
            {
                _dictionary.Add(kvp.Key, kvp.Value);
            }
        }

        public ParameterDictionary(params object[] parameters) : this()
        {
            if (parameters.Length % 2 != 0)
            {
                throw new ArgumentException("Argument was not in the valid format", nameof(parameters));
            }

            for (int i = 0; i < parameters.Length - 1; i += 2)
            {
                _dictionary.Add(parameters[i].ToString(), parameters[i + 1]);
            }
        }

        #endregion

        #region Static Methods

        public static ParameterDictionary Empty
        {
            get
            {
                return new ParameterDictionary();
            }
        }

        public static ParameterDictionary Single(string parameterName, object parameterValue)
        {
            return new ParameterDictionary(parameterName, parameterValue);
        }

        public static ParameterDictionary FromDictionary(IDictionary<string, object> dictionary)
        {
            return new ParameterDictionary(dictionary);
        }

        #endregion

        public IList<string> ParameterNames { get { return _dictionary.Keys.ToList(); } }

        public object this[string parameterName] { get { return _dictionary.ContainsKey(parameterName) ? _dictionary[parameterName] : null; } }

        public void AddParameter(string parameterName, object value)
        {
            RemoveParameter(parameterName);

            _dictionary[parameterName] = value;
        }

        public void RemoveParameter(string parameterName)
        {
            if (_dictionary.ContainsKey(parameterName))
            {
                _dictionary.Remove(parameterName);
            }
        }

        public void AddToCommand(DbCommand command)
        {
            foreach (KeyValuePair<string, object> kvp in _dictionary)
            {
                command.Parameters.Add(new SqlParameter(kvp.Key, kvp.Value));
            }
        }

        public object GetDynamicObject()
        {
            ExpandoObject eo = new ExpandoObject();
            ICollection<KeyValuePair<string, object>> eoColl = eo;

            foreach (KeyValuePair<string, object> kvp in _dictionary)
            {
                eoColl.Add(kvp);
            }

            dynamic eoDynamic = eo;
            return eoDynamic;
        }
    }

}
