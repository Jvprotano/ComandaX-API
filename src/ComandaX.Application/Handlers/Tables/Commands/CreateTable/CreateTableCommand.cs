using ComandaX.Application.DTOs;
using MediatR;

namespace ComandaX.Application.Handlers.Tables.Commands.CreateTable;

public sealed record CreateTableCommand() : IRequest<TableDto>;
