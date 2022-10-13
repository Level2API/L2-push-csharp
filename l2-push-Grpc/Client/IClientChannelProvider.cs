using Grpc.Core;

namespace l2_push_Grpc.Client
{
	public interface IClientChannelProvider
	{
		Channel GetChannel();
	}
}