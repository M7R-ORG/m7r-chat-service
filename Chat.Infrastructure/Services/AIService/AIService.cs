using Chat.Domain.Common;
using Chat.Infrastructure.RabbitMQ;
using Chat.Infrastructure.Services.AIService.Models;
using Chat.Infrastructure.Services.Common;

namespace Chat.Infrastructure.Services.AIService;

public class AIIS : BaseIService, IAIIS
{
    public AIIS(IAppSettings appSettings, IRabbitMQProducer rabbitMQProducer)
        : base(appSettings, rabbitMQProducer) { }

    public void CreateMessage(AIIServiceCreateMessageRequest request)
    {
        _rabbitMQProducer.Emit(
            RMQ.Queue.AI,
            RMQ.AIQueuePattern.CreateMessage,
            new
            {
                request.OriginalMessageId,
                request.ChannelId,
                request.AccountId,
                request.ProfileId,
                request.Messages
            },
            replyQueue: RMQ.Queue.Chat
        );
    }
}
