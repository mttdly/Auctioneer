
using Contracts;

using MassTransit;

namespace AuctionService;

public class AuctionCreatedFaultConsumer : IConsumer<Fault<AuctionCreated>>
{
    public async Task Consume(ConsumeContext<Fault<AuctionCreated>> context)
    {
        Console.WriteLine("Consuming faulty auction created event.");

        var exception = context.Message.Exceptions.First();

        if (exception.ExceptionType == "System.ArgumentException")
        {
            // Handle the fault of AuctionCreatedConsumer for the model "foo" and rename it to "FooBar"
            context.Message.Message.Model = "FooBar";
            // Publishes the fixed faulty message back into the bus
            await context.Publish(context.Message.Message);
        }
        else
        {
            // Log the exception
            Console.WriteLine("Not an argument exception.");
        }
    }
}
