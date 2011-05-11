using System;

namespace DotConsole.Tests.StubCommands
{
    /// <summary>
    /// Test command for flexing all common parameter value types.
    /// </summary>
    public class ParameterValueTypeTestCommand : ICommand
    {
        public string NamedString { get; set; }

        public bool NamedBool { get; set; }

        public int NamedInt32 { get; set; }

        public decimal NamedDecimal { get; set; }

        public object NamedObject { get; set; }

        public string[] NamedStringArray { get; set; }

        public bool[] NamedBoolArray { get; set; }

        public int[] NamedInt32Array { get; set; }

        public decimal[] NamedDecimalArray { get; set; }

        public object[] NamedObjectArray { get; set; }

        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
}