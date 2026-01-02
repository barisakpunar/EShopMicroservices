

using BuildingBlocks.CQRS;
using FluentValidation;
using Ordering.Application.Dtos;
using System.Windows.Input;

namespace Ordering.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand(OrderDto Order)
: ICommand<CreateOrderResult>;

public record CreateOrderResult(Guid Id);

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Order).NotNull().WithMessage("Order cannot be null");
        RuleFor(x => x.Order.OrderItems)
            .NotEmpty().WithMessage("Order must contain at least one item")
            .Must(items => items != null && items.Count > 0)
            .WithMessage("Order must contain at least one item");
        RuleFor(x => x.Order.Payment).NotNull().WithMessage("Payment information is required");
        RuleFor(x => x.Order.ShippingAddress).NotNull().WithMessage("Shipping address is required");
        RuleFor(x => x.Order.BillingAddress).NotNull().WithMessage("Billing address is required");
    }

}