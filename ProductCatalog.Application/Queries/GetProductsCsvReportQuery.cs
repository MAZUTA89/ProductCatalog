using MediatR;

namespace ProductCatalog.Application.Queries;

public record GetProductsCsvReportQuery : IRequest<Stream>;

