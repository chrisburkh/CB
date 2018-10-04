using Events;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Photo.Api.Messaging.Consumer
{
    public class PhotoDeleteConsumer : IConsumer<PhotoDeleteEvent>
    {
        public Task Consume(ConsumeContext<PhotoDeleteEvent> context)
        {
            Console.WriteLine("Consumer works");

            return Task.CompletedTask;
        }
    }
}
