using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DotConsole.Sample
{
    [Command("hello")]
    [Description("say's hello to someone")]
    public class SayHelloCommand : ICommand
    {
        [Parameter("recipient", Position = 0)]
        [Required]
        public string Recipient { get; set; }

        public void Execute()
        {
            Console.WriteLine("Greetings, {0}.", Recipient);
        }
    }
}