using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ObservableSharp
{
    public class ObservableProperty<T> : IObservableProperty
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableProperty() { }
        public ObservableProperty(T value)
        {
            Value = value;
        }

        private T value;
        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            }
        }

        object IObservableProperty.Value { get => this.Value; set => this.Value = (T)value; }

        public static implicit operator T(ObservableProperty<T> p) => p.Value;

        public override string ToString()
        {
            return Value?.ToString();
        }



        public ComputablePropertyBuilder<TModel> Compute<TModel>(TModel model)
        {
            return new ComputablePropertyBuilder<TModel>(model, this);
        }

        public class ComputableProperty<TModel>
        {
            private readonly TModel _model;
            private readonly ObservableProperty<T> _target;
            private readonly Func<TModel, T> _evalFunc;

            public ComputableProperty(TModel model, ObservableProperty<T> target, IEnumerable<IObservableProperty> triggers, Func<TModel, T> evalFunc)
            {
                _model = model;
                _target = target;
                _evalFunc = evalFunc;
                foreach (IObservableProperty trigger in triggers)
                {
                    trigger.PropertyChanged += Trigger_PropertyChanged;
                }
            }

            private void Trigger_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                Evaluate();
            }


            public void Evaluate()
            {
                T t = _evalFunc.Invoke(_model);
                _target.Value = t;
            }
        }


        public class ComputablePropertyBuilder<TModel>
        {

            private readonly ObservableProperty<T> _observable;
            private readonly TModel _model;
            private readonly List<IObservableProperty> _triggers;

            public ComputablePropertyBuilder(TModel model, ObservableProperty<T> observable)
            {
                _triggers = new List<IObservableProperty>();
                _observable = observable;
                _model = model;
            }
        

            public ComputablePropertyBuilder<TModel>DependsOn(Func<TModel, IObservableProperty> propGetter)
            {
                IObservableProperty observableProperty = propGetter.Invoke(_model);
                if (!_triggers.Contains(observableProperty))
                {
                    _triggers.Add(observableProperty);
                }
                return this;
            }

            public ComputableProperty<TModel> Apply(Func<TModel, T> evalFunc)
            {
                ComputableProperty<TModel> computablePropertyEvaluator = new ComputableProperty<TModel>(_model, _observable, _triggers, evalFunc);
                return computablePropertyEvaluator;
            }

        }

    }
}
