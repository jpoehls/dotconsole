namespace DotConsole
{
    public class ArgumentSingleValue : IArgumentValue
    {
        public string Value { get; private set; }

        public void SetValue(string arg)
        {
            Value = arg;
        }
    }
}