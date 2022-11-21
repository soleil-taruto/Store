using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Charlotte.Commons;
using Charlotte.Utilities;

namespace Charlotte.Tests
{
	public class Test0001
	{
		public void Test01()
		{
			VoyagerVelocity vv = new VoyagerVelocity();

			Console.WriteLine("epoch_0 = " + vv.DistanceVoyager1Earth.Pair[0].Epoch);
			Console.WriteLine("epoch_1 = " + vv.DistanceVoyager1Earth.Pair[1].Epoch);

			Console.WriteLine("dist_0_v1 = " + vv.DistanceVoyager1Earth.Pair[0].Kilometer.ToString("F9"));
			Console.WriteLine("dist_1_v1 = " + vv.DistanceVoyager1Earth.Pair[1].Kilometer.ToString("F9"));

			Console.WriteLine("dist_0_v2 = " + vv.DistanceVoyager2Earth.Pair[0].Kilometer.ToString("F9"));
			Console.WriteLine("dist_1_v2 = " + vv.DistanceVoyager2Earth.Pair[1].Kilometer.ToString("F9"));

			Console.WriteLine("dist_0_v1s = " + vv.DistanceVoyager1Sun.Pair[0].Kilometer.ToString("F9"));
			Console.WriteLine("dist_1_v1s = " + vv.DistanceVoyager1Sun.Pair[1].Kilometer.ToString("F9"));

			Console.WriteLine("dist_0_v2s = " + vv.DistanceVoyager2Sun.Pair[0].Kilometer.ToString("F9"));
			Console.WriteLine("dist_1_v2s = " + vv.DistanceVoyager2Sun.Pair[1].Kilometer.ToString("F9"));

			DateTime now = DateTime.Now;

			Console.WriteLine("Distance from Earth - Voyager 1 : " + vv.DistanceVoyager1Earth.GetKilometer(now).ToString("F3"));
			Console.WriteLine("Distance from Earth - Voyager 2 : " + vv.DistanceVoyager2Earth.GetKilometer(now).ToString("F3"));
			Console.WriteLine("Distance from Sun - Voyager 1 : " + vv.DistanceVoyager1Sun.GetKilometer(now).ToString("F3"));
			Console.WriteLine("Distance from Sun - Voyager 2 : " + vv.DistanceVoyager2Sun.GetKilometer(now).ToString("F3"));

			Console.WriteLine("v1s -3 day : " + vv.DistanceVoyager1Sun.GetKilometer(now - new TimeSpan(3 * 24, 0, 0)).ToString("F3"));
			Console.WriteLine("v1s -2 day : " + vv.DistanceVoyager1Sun.GetKilometer(now - new TimeSpan(2 * 24, 0, 0)).ToString("F3"));
			Console.WriteLine("v1s -1 day : " + vv.DistanceVoyager1Sun.GetKilometer(now - new TimeSpan(1 * 24, 0, 0)).ToString("F3"));
			Console.WriteLine("v1s +0 day : " + vv.DistanceVoyager1Sun.GetKilometer(now + new TimeSpan(0 * 24, 0, 0)).ToString("F3"));
			Console.WriteLine("v1s +1 day : " + vv.DistanceVoyager1Sun.GetKilometer(now + new TimeSpan(1 * 24, 0, 0)).ToString("F3"));
			Console.WriteLine("v1s +2 day : " + vv.DistanceVoyager1Sun.GetKilometer(now + new TimeSpan(2 * 24, 0, 0)).ToString("F3"));
			Console.WriteLine("v1s +3 day : " + vv.DistanceVoyager1Sun.GetKilometer(now + new TimeSpan(3 * 24, 0, 0)).ToString("F3"));

			Console.WriteLine("v1e velocity : " + vv.DistanceVoyager1Earth.GetKilometerPerSecond().ToString("F9"));
			Console.WriteLine("v2e velocity : " + vv.DistanceVoyager2Earth.GetKilometerPerSecond().ToString("F9"));
			Console.WriteLine("v1s velocity : " + vv.DistanceVoyager1Sun.GetKilometerPerSecond().ToString("F9"));
			Console.WriteLine("v2s velocity : " + vv.DistanceVoyager2Sun.GetKilometerPerSecond().ToString("F9"));

			{
				double KM_LIGHT_DAY = 2.59e+10; // 1光日をキロメートルで
				double remKm = KM_LIGHT_DAY - vv.DistanceVoyager1Sun.GetKilometer(now);
				double remSec = remKm / vv.DistanceVoyager1Sun.GetKilometerPerSecond();
				double remDay = remSec / 86400.0;
				double remYear = remDay / 365.25;

				Console.WriteLine("ボイジャー1号が今の速度のまま進むとしたら太陽から1光日の距離に達するまであと " + remYear.ToString("F3") + " 年くらい。");
			}
		}

		public void Test02()
		{
			VoyagerStatus vs = new VoyagerStatus(@"C:\temp\Test0001_Test02_VoyagerStatus.txt");

			Console.WriteLine(vs.TimeStamp);
			Console.WriteLine(vs.V1S_Kilometer.ToString("F20"));
			Console.WriteLine(vs.V2S_Kilometer.ToString("F20"));
			Console.WriteLine(vs.V1S_KilometerPerSecond.ToString("F20"));
			Console.WriteLine(vs.V2S_KilometerPerSecond.ToString("F20"));
		}
	}
}
