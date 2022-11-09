﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Charlotte.Commons;

namespace Charlotte.WebServices
{
	public class HTTPClient
	{
		private HttpWebRequest Inner;
		private string ResFile;

		public HTTPClient(string url, string resFile)
		{
			if (!InitOnceDone)
			{
				InitOnce();
				InitOnceDone = true;
			}

			this.Inner = (HttpWebRequest)HttpWebRequest.Create(url);
			this.SetProxyNone();

			this.ResFile = resFile;
			File.WriteAllBytes(this.ResFile, SCommon.EMPTY_BYTES); // 出力テスト
			SCommon.DeletePath(this.ResFile);
		}

		private static bool InitOnceDone;

		private static void InitOnce()
		{
			// どんな証明書も許可する。
			ServicePointManager.ServerCertificateValidationCallback =
				(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;

			// TLS 1.2
			ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
		}

		/// <summary>
		/// 接続を試みてから応答ヘッダを受信し終えるまでのタイムアウト_ミリ秒
		/// </summary>
		public int ConnectTimeoutMillis = 60000; // 1 min

		/// <summary>
		/// 接続を試みてから全て送受信し終えるまでのタイムアウト_ミリ秒
		/// </summary>
		public int TimeoutMillis = 86400000; // 1 day

		/// <summary>
		/// 応答ヘッダを受信し終えてから全て送受信し終えるまでの間の無通信タイムアウト_ミリ秒
		/// </summary>
		public int IdleTimeoutMillis = 180000; // 3 min

		/// <summary>
		/// 応答ボディ最大サイズ_バイト数
		/// </summary>
		public long ResBodySizeMax = 1500000000000; // 1.5 TB

		/// <summary>
		/// HTTP versions
		/// </summary>
		public enum Version_e
		{
			/// <summary>
			/// HTTP 1.0
			/// </summary>
			V_1_0,

			/// <summary>
			/// HTTP 1.1
			/// </summary>
			V_1_1,
		};

		public void SetVersion(Version_e version)
		{
			switch (version)
			{
				case Version_e.V_1_0:
					this.Inner.ProtocolVersion = HttpVersion.Version10;
					break;

				case Version_e.V_1_1:
					this.Inner.ProtocolVersion = HttpVersion.Version11;
					break;

				default:
					throw null;
			}
		}

		/// <summary>
		/// 100-Continue-を行わないようにする。
		/// </summary>
		public void Disable100Continue()
		{
			this.Inner.ServicePoint.Expect100Continue = false;
		}

		public void SetAuthorization(string user, string password)
		{
			string plain = user + ":" + password;
			string enc = Convert.ToBase64String(Encoding.UTF8.GetBytes(plain));
			this.AddHeader("Authorization", "Basic " + enc);
		}

		public void AddHeader(string name, string value)
		{
			if (SCommon.EqualsIgnoreCase(name, "Accept"))
			{
				this.Inner.Accept = value;
			}
			else if (SCommon.EqualsIgnoreCase(name, "Connection"))
			{
				this.Inner.Connection = value;
			}
			else if (SCommon.EqualsIgnoreCase(name, "Content-Length"))
			{
				throw null; // never
				//this.Inner.ContentLength = long.Parse(value); // 送信時に設定する。
			}
			else if (SCommon.EqualsIgnoreCase(name, "Content-Type"))
			{
				this.Inner.ContentType = value;
			}
			else if (SCommon.EqualsIgnoreCase(name, "Expect"))
			{
				throw null; // never
				//this.Inner.Expect = value; // -> Disable100Continue
			}
			else if (SCommon.EqualsIgnoreCase(name, "Date"))
			{
				this.Inner.Date = SCommon.SimpleDateTime.FromTimeStamp(long.Parse(value)).ToDateTime();
			}
			else if (SCommon.EqualsIgnoreCase(name, "Host"))
			{
				this.Inner.Host = value;
			}
			else if (SCommon.EqualsIgnoreCase(name, "If-Modified-Since"))
			{
				this.Inner.IfModifiedSince = SCommon.SimpleDateTime.FromTimeStamp(long.Parse(value)).ToDateTime();
			}
			else if (SCommon.EqualsIgnoreCase(name, "Range"))
			{
				this.Inner.AddRange(int.Parse(value));
			}
			else if (SCommon.EqualsIgnoreCase(name, "Referer"))
			{
				this.Inner.Referer = value;
			}
			else if (SCommon.EqualsIgnoreCase(name, "Transfer-Encoding"))
			{
				this.Inner.TransferEncoding = value;
			}
			else if (SCommon.EqualsIgnoreCase(name, "User-Agent"))
			{
				this.Inner.UserAgent = value;
			}
			else
			{
				this.Inner.Headers.Add(name, value);
			}
		}

		public void SetProxyNone()
		{
			this.Inner.Proxy = null;
			//this.Inner.Proxy = GlobalProxySelection.GetEmptyWebProxy(); // 古い実装
		}

		public void SetIEProxy()
		{
			this.Inner.Proxy = WebRequest.GetSystemWebProxy();
		}

		public void SetProxy(string host, int port)
		{
			this.Inner.Proxy = new WebProxy(host, port);
		}

		/// <summary>
		/// HEAD-リクエストを実行
		/// </summary>
		public void Head()
		{
			this.Send(null, "HEAD");
		}

		/// <summary>
		/// GET-リクエストを実行
		/// </summary>
		public void Get()
		{
			this.Send(null);
		}

		/// <summary>
		/// POST-リクエストを実行
		/// </summary>
		/// <param name="bodyFile">リクエストボディファイル</param>
		public void Post(string bodyFile)
		{
			this.Send(bodyFile);
		}

		public void Send(string bodyFile)
		{
			this.Send(bodyFile, bodyFile == null ? "GET" : "POST");
		}

		public void Send(string bodyFile, string method)
		{
			DateTime timeoutTime = DateTime.Now + TimeSpan.FromMilliseconds((double)TimeoutMillis);

			this.Inner.Timeout = this.ConnectTimeoutMillis;
			this.Inner.Method = method;

			if (bodyFile != null)
			{
				if (!File.Exists(bodyFile))
					throw new Exception("no bodyFile");

				this.Inner.ContentLength = new FileInfo(bodyFile).Length;

				using (Stream reader = new FileStream(bodyFile, FileMode.Open, FileAccess.Read))
				using (Stream writer = this.Inner.GetRequestStream())
				{
					SCommon.ReadToEnd(reader.Read, writer.Write);
					writer.Flush();
				}
			}
			using (WebResponse res = this.Inner.GetResponse())
			{
				this.ResHeaders = SCommon.CreateDictionaryIgnoreCase<string>();

				// header
				{
					const int RES_HEADERS_LEN_MAX = 612000;
					const int WEIGHT = 1000;

					int roughResHeaderLength = 0;

					foreach (string name in res.Headers.Keys)
					{
						if (RES_HEADERS_LEN_MAX < name.Length)
							throw new Exception("受信ヘッダが長すぎます。");

						roughResHeaderLength += name.Length + WEIGHT;

						if (RES_HEADERS_LEN_MAX < roughResHeaderLength)
							throw new Exception("受信ヘッダが長すぎます。");

						string value = res.Headers[name];

						if (RES_HEADERS_LEN_MAX < value.Length)
							throw new Exception("受信ヘッダが長すぎます。");

						roughResHeaderLength += value.Length + WEIGHT;

						if (RES_HEADERS_LEN_MAX < roughResHeaderLength)
							throw new Exception("受信ヘッダが長すぎます。");

						this.ResHeaders.Add(name, res.Headers[name]);
					}
				}

				// body
				{
					long totalSize = 0L;

					using (Stream reader = res.GetResponseStream())
					using (FileStream writer = new FileStream(this.ResFile, FileMode.Create, FileAccess.Write))
					{
						reader.ReadTimeout = this.IdleTimeoutMillis; // この時間経過すると reader.Read() が例外を投げる。

						byte[] buff = new byte[20000000]; // 20 MB

						for (; ; )
						{
							int readSize = reader.Read(buff, 0, buff.Length);

							if (readSize <= 0)
								break;

							if (timeoutTime < DateTime.Now)
								throw new Exception("受信タイムアウト");

							totalSize += (long)readSize;

							if (this.ResBodySizeMax < totalSize)
								throw new Exception("受信データが長すぎます。");

							writer.Write(buff, 0, readSize);
						}
					}
				}
			}
		}

		public Dictionary<string, string> ResHeaders;
	}
}
