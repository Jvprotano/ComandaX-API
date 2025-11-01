using ComandaX.Application.DTOs;
using MediatR;

namespace ComandaX.Application.Tables.Commands.CreateTable;

public sealed record CreateTableCommand() : IRequest<TableDto>;
