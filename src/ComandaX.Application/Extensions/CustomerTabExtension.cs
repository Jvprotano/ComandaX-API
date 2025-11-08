using ComandaX.Application.DTOs;
using ComandaX.Domain.Entities;

namespace ComandaX.Application.Extensions;

public static class CustomerTabExtension
{
    public static CustomerTabDto AsDto(this CustomerTab customerTab)
    {
        return new CustomerTabDto(
            customerTab.Id,
            customerTab.Name,
            customerTab.TableId,
            customerTab.Status);
    }

}
