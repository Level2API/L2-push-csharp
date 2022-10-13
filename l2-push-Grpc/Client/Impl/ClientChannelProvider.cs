using Grpc.Core;

namespace l2_push_Grpc.Client.Impl
{
	public class ClientChannelProvider : IClientChannelProvider
	{
		private readonly string _host;
		private readonly int _port;

		public ClientChannelProvider(string host, int port)
		{
			_host = host;
			_port = port;
		}

		public Channel GetChannel()
		{
			return new Channel(_host, _port, ChannelCredentials.Insecure);
		}
	}
}