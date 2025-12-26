

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Enums;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                .HasConversion(
                    orderId => orderId.Value,
                    dbId => OrderId.Of(dbId)
                );

            builder.HasOne<Customer>()
                   .WithMany()
                   .HasForeignKey(o => o.CustomerId)
                   .IsRequired();


            builder.HasMany(o => o.OrderItems)
                   .WithOne()
                   .HasForeignKey("OrderId");

            builder.ComplexProperty(o => o.OrderName,
                nameBuilder =>
                {
                    nameBuilder.Property(n => n.Value)
                               .HasColumnName("OrderName")
                               .IsRequired()
                               .HasMaxLength(200);
                });

            builder.ComplexProperty(o => o.ShippingAddress,
                addressBuilder =>
                {
                    addressBuilder.Property(a => a.AddressLine).IsRequired().HasMaxLength(200);
                    addressBuilder.Property(a => a.FirstName).IsRequired().HasMaxLength(100);
                    addressBuilder.Property(a => a.LastName).IsRequired().HasMaxLength(100);
                    addressBuilder.Property(a => a.EmailAddress).IsRequired().HasMaxLength(100);
                    addressBuilder.Property(a => a.State).HasMaxLength(100);
                    addressBuilder.Property(a => a.ZipCode).IsRequired().HasMaxLength(20);
                    addressBuilder.Property(a => a.Country).IsRequired().HasMaxLength(100);
                });

            builder.ComplexProperty(o => o.BillingAddress,
                            addressBuilder =>
                            {
                                addressBuilder.Property(a => a.AddressLine).IsRequired().HasMaxLength(200);
                                addressBuilder.Property(a => a.FirstName).IsRequired().HasMaxLength(100);
                                addressBuilder.Property(a => a.LastName).IsRequired().HasMaxLength(100);
                                addressBuilder.Property(a => a.EmailAddress).IsRequired().HasMaxLength(100);
                                addressBuilder.Property(a => a.State).HasMaxLength(100);
                                addressBuilder.Property(a => a.ZipCode).IsRequired().HasMaxLength(20);
                                addressBuilder.Property(a => a.Country).IsRequired().HasMaxLength(100);
                            });

            builder.ComplexProperty(o => o.Payment, paymentBuilder =>
            {
                paymentBuilder.Property(p => p.PaymentMethod).IsRequired().HasMaxLength(20);
                paymentBuilder.Property(p => p.CardNumber).IsRequired().HasMaxLength(100);
                paymentBuilder.Property(p => p.CardName).IsRequired().HasMaxLength(10);
                paymentBuilder.Property(p => p.CVV).IsRequired().HasMaxLength(5);
                paymentBuilder.Property(p => p.Expiration).IsRequired();
            });

            builder.Property(o => o.Status)
                .HasDefaultValue(OrderStatus.Pending)
                   .HasConversion(
                       status => status.ToString(),
                       dbStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), dbStatus)
                   )
                   .IsRequired();
        }
    }
}
