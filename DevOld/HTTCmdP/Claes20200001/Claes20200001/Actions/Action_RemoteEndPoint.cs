using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.WebServices;

namespace Charlotte.Actions
{
	public static class Action_RemoteEndPoint
	{
		public static void Perform(HTTPServerChannel channel)
		{
			string address = channel.Channel.Handler.RemoteEndPoint.ToString().Split(':')[0];

			if (100 < address.Length) // rough limit
				throw new Exception("Bad IP-address");

			channel.ResStatus = 200;
			channel.ResHeaderPairs.Add(new string[] { "Content-Type", "text/plain; charset=US-ASCII" });
			channel.ResBody = new byte[][] { Encoding.ASCII.GetBytes(address) };
		}
	}
}
