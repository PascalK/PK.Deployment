using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PK.Test.MSBuild
{
    public abstract class MSBuildParametersBase : Dictionary<string, string>
    {
        protected string Get(string key)
        {
            if (ContainsKey(key))
            {
                return base[key];
            }
            else
            {
                return null;
            }
        }
        protected void Set(string key, string value)
        {
            base[key] = value;
        }
        protected bool? GetBool(string key)
        {
            return Get(key, (s) => bool.Parse(s));
        }
        protected void SetBool(string key, bool? value)
        {
            if (value.HasValue)
            {
                Set(key, value.ToString());
            }
            else
            {
                Set(key, null);
            }
        }

        protected int? GetInt(string key)
        {
            return Get(key, (s) => int.Parse(s, CultureInfo.InvariantCulture));
        }
        protected void SetInt(string key, int? value)
        {
            if (value.HasValue)
            {
                Set(key, value.Value.ToString("F1", CultureInfo.InvariantCulture));
            }
            else
            {
                Set(key, null);
            }
        }
        protected TType Get<TType>(string key, Func<string, TType> constructPredicate)
        {
            string strValue = Get(key);
            if (!string.IsNullOrEmpty(strValue))
            {
                return constructPredicate(strValue);
            }
            else
            {
                return default(TType);
            }
        }
    }
}