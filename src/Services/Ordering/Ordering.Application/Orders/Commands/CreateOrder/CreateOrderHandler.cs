

using BuildingBlocks.CQRS;

namespace Ordering.Application.Orders.Commands.CreateOrder;

public class CreateOrderHandler : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public Task<CreateOrderResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // Implementation logic to create an order goes here.
        // This is a placeholder implementation.
        var newOrderId = Guid.NewGuid();
        return Task.FromResult(new CreateOrderResult(newOrderId));
    }
}
