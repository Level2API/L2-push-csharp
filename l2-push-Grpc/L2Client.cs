using Grpc.Core;
using l2_push_Grpc.Client;
using System;
using System.Threading.Tasks;
using l2PushGrpc;
using String = l2PushGrpc.String;

namespace l2_push_Grpc
{
	/// <summary>
	/// Level2 GRPC客户端操作类
	/// </summary>
	public class L2Client
	{
		private readonly Channel _channel;
		private Proxy.ProxyClient client => new Proxy.ProxyClient(_channel);

		public L2Client(IClientChannelProvider channelProvider)
		{
			if (channelProvider == null) throw new ArgumentNullException(nameof(channelProvider));
			_channel = channelProvider.GetChannel();
		}

		/// <summary>
		/// 查询订阅
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public Task<SubscriptionRsp> GetSubscriptionAsync() => client.GetSubscriptionAsync(new l2PushGrpc.Void()).ResponseAsync;

		/// <summary>
		/// 新增订阅
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public Task<Rsp> AddSubscriptionAsync(String input) => client.AddSubscriptionAsync(input).ResponseAsync;

		/// <summary>
		/// 删除订阅
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public Task<Rsp> DelSubscriptionAsync(String input) => client.DelSubscriptionAsync(input).ResponseAsync;

		/// <summary>
		/// 推送逐笔成交行情数据
		/// </summary>
		/// <returns></returns>
		public IAsyncStreamReader<TickRecord> NewTickRecordStreamAsync() => client.NewTickRecordStream(new l2PushGrpc.Void()).ResponseStream;

		/// <summary>
		/// 推送逐笔委托行情数据
		/// </summary>
		/// <returns></returns>
		public IAsyncStreamReader<OrderRecord> NewOrderRecordStreamAsync() => client.NewOrderRecordStream(new l2PushGrpc.Void()).ResponseStream;

		/// <summary>
		/// 推送委托队列行情数据
		/// </summary>
		/// <returns></returns>
		public IAsyncStreamReader<OrderQueueRecord> NewOrderQueueRecordStreamAsync() => client.NewOrderQueueRecordStream(new l2PushGrpc.Void()).ResponseStream;

		/// <summary>
		/// 推送股票十档行情行情数据
		/// </summary>
		/// <returns></returns>
		public IAsyncStreamReader<StockQuoteRecord> NewStockQuoteRecordStreamAsync() => client.NewStockQuoteRecordStream(new l2PushGrpc.Void()).ResponseStream;
	}
}