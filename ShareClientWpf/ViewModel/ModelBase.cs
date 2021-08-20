using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ShareClientWpf
{
    public class ModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new(name));

        protected void SetProperty<T>(ref T prop, T value, Action postCallMethod = null, [CallerMemberName] string name = "") =>
            SetProperty(ref prop, value, postCallMethod != null ? (_) => postCallMethod.Invoke() : null, name);

        protected void SetProperty<T>(ref T prop, T value, Action<T> postCallMethod, [CallerMemberName] string name = "")
        {
            if (!prop?.Equals(value) ?? value != null)
            {
                prop = value;
                OnPropertyChanged(name);
                postCallMethod?.Invoke(prop);
            }
        }

        protected void SetProperty<T>(ref T prop, T value, Func<T, T, bool> valiedate, Action postCallMethod = null, [CallerMemberName] string name = "") =>
            SetProperty(ref prop, value, valiedate, postCallMethod != null ? (_) => postCallMethod.Invoke() : null, name);

        protected void SetProperty<T>(ref T prop, T value, Func<T, T, bool> valiedate, Action<T> postCallMethod, [CallerMemberName] string name = "")
        {
            if ((!prop?.Equals(value) ?? value != null)
                && valiedate(prop, value))
            {
                prop = value;
                OnPropertyChanged(name);
                postCallMethod?.Invoke(prop);
            }
        }

        public static Func<T, T, bool> IntValidate<T>(Action<T> validateCall, bool validateOld = false)
        {
            return (oldValue, newValue) =>
            {
                var result = int.TryParse(newValue.ToString(), out var _)
                                && (!validateOld || int.TryParse(oldValue.ToString(), out var __));
                if (!result)
                {
                    validateCall?.Invoke(newValue);
                }

                return result;
            };
        }

        public static Func<T, T, bool> Validate<T>(Func<T, bool> value, Action<T> validateCall, Func<T, bool> oldvalue = null)
        {
            return (oldValue, newValue) =>
            {
                var result = value.Invoke(newValue) && (oldvalue?.Invoke(oldValue) ?? true);
                if (!result)
                {
                    validateCall?.Invoke(newValue);
                }

                return result;
            };
        }
    }
}
