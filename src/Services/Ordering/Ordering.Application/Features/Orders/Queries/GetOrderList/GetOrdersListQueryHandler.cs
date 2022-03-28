using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Queries.GetOrderList
{
    public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, List<OrderVm>>
    {
        private IOrderRepository orderRepository;
        private IMapper mapper;

        public GetOrdersListQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
        }

        public async Task<List<OrderVm>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
        {
            var orders = await orderRepository.GetOrdersByUserName(request.UserName);
            var ordersVm = mapper.Map<List<OrderVm>>(orders);
            return ordersVm;
        }
    }
}
