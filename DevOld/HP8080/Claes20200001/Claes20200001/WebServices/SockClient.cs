using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using Charlotte.Commons;

namespace Charlotte.WebServices
{
	public class SockClient : SockChannel, IDisposable
	{
		public void Connect(string domain, int portNo)
		{
			IPHostEntry hostEntry = Dns.GetHostEntry(domain);
			IPAddress address = GetFairAddress(hostEntry.AddressList);
			IPEndPoint endPoint = new IPEndPoint(address, portNo);

			this.Handler = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			this.Handler.Connect(endPoint);
			this.Handler.Blocking = false;
		}

		private static IPAddress GetFairAddress(IPAddress[] addresses)
		{
			foreach (IPAddress address in addresses)
			{
				if (address.AddressFamily == AddressFamily.InterNetwork) // ? IPv4
				{
					return address;
				}
			}
			return addresses[0];
		}

		/// <summary>
		/// 送信
		/// 送信し終えるまでブロックする。
		/// </summary>
		/// <param name="data">送信データ</param>
		public void B_Send(byte[] data)
		{
			int waitMillis = 0;

			foreach (int ret in this.Send(data))
			{
				if (ret == 0)
					throw null; // never

				if (ret < 0)
				{
					if (waitMillis < 100)
						waitMillis++;

					Thread.Sleep(waitMillis);
				}
				else
					waitMillis = 0; // reset
			}
		}

		/// <summary>
		/// 受信
		/// 受信し終えるまでブロックする。
		/// </summary>
		/// <param name="size">受信サイズ</param>
		/// <returns>受信データ</returns>
		public byte[] B_Recv(int size)
		{
			byte[] data = null;
			int waitMillis = 0;

			foreach (int ret in this.Recv(size, ret => data = ret))
			{
				if (ret == 0)
					throw null; // never

				if (ret < 0)
				{
					if (waitMillis < 100)
						waitMillis++;

					Thread.Sleep(waitMillis);
				}
				else
					waitMillis = 0; // reset
			}
			if (data == null)
				throw null; // never

			return data;
		}

		/// <summary>
		/// 例外を投げないこと。
		/// </summary>
		public void Dispose()
		{
			if (this.Handler != null)
			{
				try
				{
					this.Handler.Disconnect(false);
				}
				catch (Exception e)
				{
					SockCommon.WriteLog(SockCommon.ErrorLevel_e.NETWORK, e);
				}

				try
				{
					this.Handler.Dispose();
				}
				catch (Exception e)
				{
					SockCommon.WriteLog(SockCommon.ErrorLevel_e.NETWORK, e);
				}

				this.Handler = null;
			}
		}
	}
}
