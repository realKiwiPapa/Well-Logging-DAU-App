using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

using System.ComponentModel.DataAnnotations;

namespace Logging_App.Model
{

    [Serializable]
    public class ModelBase : IDataErrorInfo, INotifyPropertyChanged, ICloneable
    {
        #region 数据更新通知

        [field: NonSerializedAttribute()]
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        [NonSerialized]
        private Dictionary<string, bool> Errors = new Dictionary<string, bool>();
        [NonSerialized]
        private string _error;

        public string Error
        {
            get { return _error; }
        }

        public bool HasError()
        {
            return !string.IsNullOrEmpty(_error);
        }

        public string this[string columnName]
        {
            get
            {
                Type tp = this.GetType();
                PropertyInfo pi = tp.GetProperty(columnName);
                var value = pi.GetValue(this, null);
                object[] Attributes = pi.GetCustomAttributes(false);
                if (Attributes != null && Attributes.Length > 0)
                {
                    Errors.Remove(columnName);
                    foreach (object attribute in Attributes)
                    {
                        if (attribute is ValidationAttribute)
                        {
                            ValidationAttribute vAttribute = attribute as ValidationAttribute;
                            if (!vAttribute.IsValid(value))
                            {
                                Errors[columnName] = true;
                                _error = vAttribute.ErrorMessage;
                                return _error;
                            }
                        }
                    }
                }
                if (Errors.Count == 0) _error = null;
                return null;
            }
        }
    }

    public enum CharUsedType
    {
        Byte, Char
    }

    public class OracleNumberLength : ValidationAttribute
    {
        public int MaximumLength { get; set; }

        public int MinimumLength { get; set; }

        public override bool IsValid(object value)
        {
            string str = string.Empty;
            if (value != null) str = value.ToString();
            int index = str.IndexOf('.');
            if (index >= 0) str = str.Remove(index);
            if (str.Length >= MinimumLength && str.Length <= MaximumLength) return true;
            ErrorMessage = string.Format("整数部分长度在{0}到{1}之间！", MinimumLength, MaximumLength);
            return false;
        }
    }

    public class OracleStringLength : OracleNumberLength
    {
        public override bool IsValid(object value)
        {
            string str = string.Empty;
            if (value != null) str = value.ToString();
            int strLength = CharUsed == CharUsedType.Byte ? Encoding.GetEncoding("GB2312").GetByteCount(str) : str.Length;
            if (strLength >= MinimumLength && strLength <= MaximumLength) return true;
            string msg = CharUsed == CharUsedType.Byte ? "字节长度" : "字符长度";
            ErrorMessage = string.Format("{0}在{1}到{2}之间！", msg, MinimumLength, MaximumLength);
            return false;
        }

        public CharUsedType CharUsed { get; set; }
    }

    public class Required : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            ErrorMessage = "不能为空";
            if (value == null || string.IsNullOrEmpty(value.ToString())) return false;
            return true;
        }
    }
    public class Range : ValidationAttribute
    {
        private double _minimum;
        private double _maximum;

        public double Minimum { get { return _minimum; } }
        public double Maximum { get { return _maximum; } }

        public Range(double minimum, double maximum)
        {
            _minimum = minimum;
            _maximum = maximum;
        }

        public override bool IsValid(object value)
        {
           var range= new RangeAttribute(Minimum, Maximum);
            ErrorMessage = string.Format("数字范围在{0}到{1}之间！", Minimum, Maximum);
            return range.IsValid(value);
        }
    }

}
