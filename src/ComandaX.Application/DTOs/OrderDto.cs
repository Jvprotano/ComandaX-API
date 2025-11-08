using ComandaX.Domain.Enums;

namespace ComandaX.Application.DTOs;

public sealed class OrderDto
{
    public OrderDto()
    {

    }
    public OrderDto(Guid id, int code, Guid? customerTabId, List<OrderProductDto> products, OrderStatusEnum status)
    {
        this.Id = id;
        this.Code = code;
        this.CustomerTabId = customerTabId;
        this.Products = products;
        this.Status = status;
    }
    public Guid Id { get; set; }
    public int Code { get; set; }
    public Guid? CustomerTabId { get; set; }
    public List<OrderProductDto> Products { get; set; } = [];
    public OrderStatusEnum Status { get; set; }
}
