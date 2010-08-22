using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DotConsole.Sample
{
    internal class SampleCommandArgs : CommandArguments
    {
        [Required(ErrorMessage = "-name is required")]
        [Argument("name", "n", "Name of the person running the sample.",
            Position = 1,
            ValueName = "persons_name")]
        public string PersonsName { get; set; }
    }
}