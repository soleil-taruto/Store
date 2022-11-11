using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;

namespace Charlotte.Utilities
{
	public class VoyagerVelocity
	{
		private const string NASA_DISTANCE_DATA_URL = "https://voyager.jpl.nasa.gov/assets/javascripts/distance_data.js";

		private static readonly long EPOCH_TIME = SCommon.TimeStampToSec.ToSec(19700101000000);

		public class DistanceInfo
		{
			public long Epoch;
			public double Kilometer;

			public DistanceInfo(long epoch, double kilometer)
			{
				this.Epoch = epoch;
				this.Kilometer = kilometer;
			}
		}

		public class DistancePairInfo
		{
			public DistanceInfo[] Pair = new DistanceInfo[2]; // { Today , Yesterday }

			public double GetKilometerPerSecond() // ret: Velocity
			{
				return (this.Pair[0].Kilometer - this.Pair[1].Kilometer) / (this.Pair[0].Epoch - this.Pair[1].Epoch);
			}

			private double GetKilometer(long epoch) // ret: Distance from Earth or Sun
			{
				return this.Pair[1].Kilometer + (epoch - this.Pair[1].Epoch) * this.GetKilometerPerSecond();
			}

			public double GetKilometer(DateTime dateTime)
			{
				return this.GetKilometer(SCommon.TimeStampToSec.ToSec(dateTime.ToUniversalTime()) - EPOCH_TIME);
			}
		}

		public DistancePairInfo DistanceVoyager1Earth = new DistancePairInfo();
		public DistancePairInfo DistanceVoyager2Earth = new DistancePairInfo();
		public DistancePairInfo DistanceVoyager1Sun = new DistancePairInfo();
		public DistancePairInfo DistanceVoyager2Sun = new DistancePairInfo();

		public VoyagerVelocity()
		{
			HTTPClient hc = new HTTPClient(NASA_DISTANCE_DATA_URL);

			hc.ConnectTimeoutMillis = 10000; // 10 sec
			hc.TimeoutMillis = 15000; // 15 sec
			hc.IdleTimeoutMillis = 5000; // 5 sec
			hc.ResBodySizeMax = 1000000; // 1 MB

			using (WorkingDir wd = new WorkingDir())
			{
				hc.ResFile = wd.MakePath();
				hc.Get();

				Dictionary<string, string> values = SCommon.CreateDictionary<string>();

				foreach (string line in File.ReadAllLines(hc.ResFile, Encoding.ASCII).Where(v => v != ""))
				{
					string[] tokens = line.Split(' ');

					string name = tokens[1];
					string value = tokens[3].Replace(";", "");

					values.Add(name, value);
				}

				long epoch_0 = long.Parse(values["epoch_0"]);
				long epoch_1 = long.Parse(values["epoch_1"]);

				this.DistanceVoyager1Earth.Pair[0] = new DistanceInfo(epoch_0, double.Parse(values["dist_0_v1"]));
				this.DistanceVoyager1Earth.Pair[1] = new DistanceInfo(epoch_1, double.Parse(values["dist_1_v1"]));

				this.DistanceVoyager2Earth.Pair[0] = new DistanceInfo(epoch_0, double.Parse(values["dist_0_v2"]));
				this.DistanceVoyager2Earth.Pair[1] = new DistanceInfo(epoch_1, double.Parse(values["dist_1_v2"]));

				this.DistanceVoyager1Sun.Pair[0] = new DistanceInfo(epoch_0, double.Parse(values["dist_0_v1s"]));
				this.DistanceVoyager1Sun.Pair[1] = new DistanceInfo(epoch_1, double.Parse(values["dist_1_v1s"]));

				this.DistanceVoyager2Sun.Pair[0] = new DistanceInfo(epoch_0, double.Parse(values["dist_0_v2s"]));
				this.DistanceVoyager2Sun.Pair[1] = new DistanceInfo(epoch_1, double.Parse(values["dist_1_v2s"]));
			}
		}
	}
}
