namespace DotConsole
{
    public class SingleValueArgument : IArgumentValue
    {
        public string Value { get; private set; }

        public void SetValue(string arg)
        {
            Value = arg;
        }
    }
}