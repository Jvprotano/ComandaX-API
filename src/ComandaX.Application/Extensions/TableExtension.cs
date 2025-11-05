using ComandaX.Application.DTOs;
using ComandaX.Domain.Entities;

namespace ComandaX.Application.Extensions;

public static class TableExtension
{
    public static TableDto AsDto(this Table table)
    {
        return new TableDto(table.Id, table.Code, table.Status);
    }
}
