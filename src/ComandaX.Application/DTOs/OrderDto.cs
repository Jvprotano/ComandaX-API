using ComandaX.Domain.Enums;

namespace ComandaX.Application.DTOs;

public sealed class OrderDto
{
    public OrderDto() { }
    public OrderDto(
        Guid id,
        int code,
        Guid? customerTabId,
        OrderStatusEnum status,
        DateTime createdAt,
        DateTime? updatedAt)
    {
        Id = id;
        Code = code;
        CustomerTabId = customerTabId;
        Status = status;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
    public Guid Id { get; set; }
    public int Code { get; set; }
    public Guid? CustomerTabId { get; set; }
    public OrderStatusEnum Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
