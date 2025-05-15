using MediatR;
using Wonderlust.Application.Features.Communities.Dtos;

namespace Wonderlust.Application.Features.Communities.Queries.GetCommunity;

public record GetCommunityQuery(Guid CommunityId) : IRequest<CommunityDto>;
