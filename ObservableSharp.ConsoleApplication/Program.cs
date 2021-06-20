using System;

namespace ObservableSharp.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            new TestClass().Run();
        }
    }
    public class TestClass
    {
        public ObservableProperty<string> Email { get; set; } = new();
        public ObservableProperty<string> FirstName { get; set; } = new();
        public ObservableProperty<string> LastName { get; set; } = new();
        public ObservableProperty<bool> IsValidForm { get; set; } = new();

        public void Run()
        {
            IsValidForm.PropertyChanged += PropertyChanged;
            IsValidForm.Compute(this)
                .DependsOn(x => x.Email)
                .DependsOn(x => x.FirstName)
                .DependsOn(x => x.LastName)
                .Apply(x => x.FirstName.Value.IsNotNullOrEmpty()
                            && x.LastName.Value.IsNotNullOrEmpty()
                            && x.Email.Value.IsValidEmail())
                .Evaluate();

            FirstName.Value = "Westley";
            LastName.Value = "Rotolo";
            Email.Value = "westley@outlook.it";



        }


        private void PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.WriteLine($"Email:{Email}");
            Console.WriteLine($"FirstName:{FirstName}");
            Console.WriteLine($"LastName:{LastName}");
            Console.WriteLine($"IsValidForm:{IsValidForm}");
            Console.WriteLine("--------");

        }
    }
}
