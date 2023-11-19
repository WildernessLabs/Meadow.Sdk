using System;
using Meadow.Cloud;

namespace SampleApp.Commands
{
	public class SampleCommand : IMeadowCommand
    {
        public bool IsOn { get; set; } = false;
    }
}