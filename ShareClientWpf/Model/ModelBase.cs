using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ShareClientWpf
{
    public class ModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new(name));
        }

        protected void SetProperty<T>(ref T prop, T value, Action postCallMethod = null, [CallerMemberName] string name = "")
        {
            SetProperty(ref prop, value, postCallMethod != null ? (_) => postCallMethod.Invoke() : null, name);
        }

        protected void SetProperty<T>(ref T prop, T value, Action<T> postCallMethod, [CallerMemberName] string name = "")
        {
            if (!prop?.Equals(value) ?? value != null)
            {
                prop = value;
                OnPropertyChanged(name);
                postCallMethod?.Invoke(prop);
            }
        }

        protected void SetProperty<T>(ref T prop, T value, Func<T, T, bool> validate, Action postCallMethod = null, [CallerMemberName] string name = "")
        {
            SetProperty(ref prop, value, validate, postCallMethod != null ? (_) => postCallMethod.Invoke() : null, name);
        }

        protected void SetProperty<T>(ref T prop, T value, Func<T, T, bool> validate, Action<T> postCallMethod, [CallerMemberName] string name = "")
        {
            if ((!prop?.Equals(value) ?? value != null)
                && validate(prop, value))
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

        public static Func<T, T, bool> Validate<T>(Func<T, bool> value, Action<T> validateCall, Func<T, bool> oldValuePredicate = null)
        {
            return (oldValue, newValue) =>
            {
                var result = value.Invoke(newValue) && (oldValuePredicate?.Invoke(oldValue) ?? true);
                if (!result)
                {
                    validateCall?.Invoke(newValue);
                }

                return result;
            };
        }
    }
}
