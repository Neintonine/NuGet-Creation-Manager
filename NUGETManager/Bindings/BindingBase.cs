using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using NUGETManager.Annotations;

namespace NUGETManager.Bindings
{
    public class BindingBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnPropertyChanged(params string[] propertyNames)
        {
            foreach (string name in propertyNames) OnPropertyChanged(name);
        }
    }
}