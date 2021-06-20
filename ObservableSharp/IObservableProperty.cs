using System;
using System.ComponentModel;

namespace ObservableSharp
{
    public interface IObservableProperty : INotifyPropertyChanged
    {
        object Value
        {
            get;
            set;
        }

    }
}
