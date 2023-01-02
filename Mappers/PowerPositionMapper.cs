using CsvHelper.Configuration;
using Microsoft.VisualBasic;
using PowerPosition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPosition.Mappers
{
	public class PowerPositionMapper : ClassMap<PowerPositionModel>
	{
		public PowerPositionMapper()
		{
			Map(x => x.LocalTime).Name("Local Time");
			Map(x => x.Volume).Name("Volume");
		}
	}
}