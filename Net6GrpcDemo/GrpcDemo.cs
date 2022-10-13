using Grpc.Core;
using l2_push_Grpc;
using l2_push_Grpc.Client.Impl;
using String = l2PushGrpc.String;

namespace Net6GrpcDemo;

public static class GrpcDemo
{
	public static async Task DemoAsync(CancellationToken cancellationToken)
	{
		try
		{
			//代理服务器监听的地址和端口
			var client = new L2Client(new ClientChannelProvider("localhost", 5000));

			//定义2个订阅主题，订阅主题格式参考文档说明
			var symbol1 = "2_000001_15";
			var symbol2 = "2_002594_15";

			//新增2个订阅主题，根据自身需求设置参数调用
			await AddSubscriptionAsync(client, symbol1, symbol2);

			//查询订阅
			await GetSubscriptionAsync(client);

			//以下代码演示如何获取推送的相关数据
			{
				Console.WriteLine("=================按任意键开始演示接收订阅的推送数据=================");
				Console.ReadKey();
				Console.WriteLine("=================正在接收数据，请等待=================");

				//获取推送逐笔成交行情数据
				_ = GetNewTickRecordStreamAsync(client, cancellationToken);


				//获取推送逐笔委托行情数据
				_ = GetNewOrderRecordStreamAsync(client, cancellationToken);


				//获取推送委托队列行情数据
				_ = GetNewOrderQueueRecordStreamAsync(client, cancellationToken);


				//获取推送股票十档行情行情数据
				_ = GetNewStockQuoteRecordStreamAsync(client, cancellationToken);
			}

			Console.ReadKey();
			Console.WriteLine("=================已停止接收数据，正在演示删除订阅主题=================");
			//结束时演示删除已订阅主题，根据自身需求设置参数调用
			await DelSubscriptionAsync(client, symbol1, symbol2);

		}
		catch (RpcException e)
		{
			Console.WriteLine($"RPC连接失败,请检查是否开启行情工具客户端！\r\n错误信息:{e.Message}");
		}
	}

	/// <summary>
	/// 新增订阅
	/// </summary>
	/// <param name="client"></param>
	/// <param name="values"></param>
	/// <returns></returns>
	public static async Task AddSubscriptionAsync(L2Client client, params string[] values)
	{
		foreach (var value in values)
		{
			//新增订阅 根据自身需求设置参数调用，参数格式参考文档说明
			var addSubscriptionResult = await client.AddSubscriptionAsync(new String { Value = value });
			Console.WriteLine($"【{DateTime.Now:yyyy-MM-dd hh:mm:ss.fff}】 测试演示新增订阅:入参：{value},返回结果：{addSubscriptionResult}");
		}
	}

	/// <summary>
	/// 删除订阅
	/// </summary>
	/// <param name="value"></param>
	/// <param name="client"></param>
	/// <returns></returns>
	public static async Task DelSubscriptionAsync(L2Client client, params string[] values)
	{
		foreach (var value in values)
		{
			//删除订阅 根据自身需求设置参数调用，参数格式参考文档说明
			var delSubscriptionResult = await client.DelSubscriptionAsync(new String { Value = value });
			Console.WriteLine($"【{DateTime.Now:yyyy-MM-dd hh:mm:ss.fff}】 测试演示删除订阅:入参：{value},返回结果：{delSubscriptionResult}");
		}
	}

	/// <summary>
	/// 查询订阅
	/// </summary>
	/// <param name="client"></param>
	/// <param name=""></param>
	/// <returns></returns>
	public static async Task GetSubscriptionAsync(L2Client client)
	{
		var subscription = await client.GetSubscriptionAsync();
		Console.WriteLine($"【{DateTime.Now:yyyy-MM-dd hh:mm:ss.fff}】 当前用户订阅的信息：{subscription}");
	}

	/// <summary>
	///	开启一个新线程接收逐笔成交行情数据
	/// </summary>
	/// <param name="client"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public static Task GetNewTickRecordStreamAsync(L2Client client, CancellationToken cancellationToken)
	{
		var asyncStream = client.NewTickRecordStreamAsync();
		Task.Run(async () =>
		{
			while (await asyncStream.MoveNext(cancellationToken))
			{
				var data = asyncStream.Current;
				Console.WriteLine($"【{DateTime.Now:yyyy-MM-dd hh:mm:ss.fff}】 接收逐笔成交行情数据:{data}");
			}
		});
		return Task.CompletedTask;
	}

	/// <summary>
	/// 开启一个新线程接收逐笔委托行情数据
	/// </summary>
	/// <param name="client"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public static Task GetNewOrderRecordStreamAsync(L2Client client, CancellationToken cancellationToken)
	{
		var asyncStream = client.NewOrderRecordStreamAsync();
		//开启一个新线程接收推送数据
		Task.Run(async () =>
		{
			while (await asyncStream.MoveNext(cancellationToken))
			{
				var data = asyncStream.Current;
				Console.WriteLine($"【{DateTime.Now:yyyy-MM-dd hh:mm:ss.fff}】 接收逐笔委托行情数据:{data}");
			}
		});
		return Task.CompletedTask;
	}

	/// <summary>
	/// 开启一个新线程接收委托队列行情数据
	/// </summary>
	/// <param name="client"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public static Task GetNewOrderQueueRecordStreamAsync(L2Client client, CancellationToken cancellationToken)
	{
		var asyncStream = client.NewOrderQueueRecordStreamAsync();
		//开启一个新线程接收推送数据
		Task.Run(async () =>
		{
			while (await asyncStream.MoveNext(cancellationToken))
			{
				var data = asyncStream.Current;
				Console.WriteLine($"【{DateTime.Now:yyyy-MM-dd hh:mm:ss.fff}】 接收委托队列行情数据:{data}");
			}
		});
		return Task.CompletedTask;
	}

	/// <summary>
	/// 开启一个新线程接收股票十档行情行情数据
	/// </summary>
	/// <param name="client"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public static Task GetNewStockQuoteRecordStreamAsync(L2Client client, CancellationToken cancellationToken)
	{
		var asyncStream = client.NewStockQuoteRecordStreamAsync();
		//开启一个新线程接收推送数据
		Task.Run(async () =>
		{
			while (await asyncStream.MoveNext(cancellationToken))
			{
				var data = asyncStream.Current;
				Console.WriteLine($"【{DateTime.Now:yyyy-MM-dd hh:mm:ss.fff}】 接收股票十档行情行情数据:{data}");
			}
		});
		return Task.CompletedTask;
	}
}