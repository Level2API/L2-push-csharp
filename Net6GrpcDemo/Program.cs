/*
 * 此Demo只是演示简单接入过程,代码严谨性未做处理,请根据自己实际情况调整代码
 * 此Demo是 "NET6" 版本控制台演示程序，其他net版本程序只需要引用项目l2-push-Grpc即可调用Grpc相关操作
 */

using Net6GrpcDemo;

await GrpcDemo.DemoAsync(CancellationToken.None);