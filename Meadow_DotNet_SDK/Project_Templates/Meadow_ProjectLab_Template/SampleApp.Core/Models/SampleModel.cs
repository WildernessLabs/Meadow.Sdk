using System;
using Meadow.Units;

namespace SampleApp.Models
{
	public class SampleModel
	{
		public Temperature Temperature { get; set; }
		public Pressure Pressure { get; set; }
		public RelativeHumidity Humidity { get; set; }
	}
}