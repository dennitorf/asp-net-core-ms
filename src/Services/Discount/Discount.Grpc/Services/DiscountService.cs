using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository repository;
        private readonly ILogger<DiscountService> logger;

        public DiscountService(IDiscountRepository repository, ILogger<DiscountService> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            logger.LogInformation("Product Discount Get {productName}", request.ProductName);
            var coupon = await repository.GetDiscount(request.ProductName);

            if (coupon == null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Product with discount ProductName={request.ProductName} not found"));

            return new CouponModel() 
                { Id = coupon.Id, Amount = coupon.Amount, Description = coupon.Description, ProductName = coupon.ProductName };
        }

        public async override Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            logger.LogInformation("Product Discount Create {productName}", request.Coupon.ProductName);

            var coupon = new Coupon()
            {
                Id = request.Coupon.Id,
                Amount = request.Coupon.Amount,
                Description = request.Coupon.Description,
                ProductName = request.Coupon.ProductName
            };

            var ent = await repository.CreateDiscount(coupon);


            return new CouponModel()
            {
                Id = request.Coupon.Id,
                Amount = request.Coupon.Amount,
                Description = request.Coupon.Description,
                ProductName = request.Coupon.ProductName
            };
        }


        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            logger.LogInformation("Product Discount Update {productName}", request.Coupon.ProductName);

            var coupon = new Coupon()
            {
                Id = request.Coupon.Id,
                Amount = request.Coupon.Amount,
                Description = request.Coupon.Description,
                ProductName = request.Coupon.ProductName
            };

            var ent = await repository.UpdateDiscount(coupon);


            return new CouponModel()
            {
                Id = request.Coupon.Id,
                Amount = request.Coupon.Amount,
                Description = request.Coupon.Description,
                ProductName = request.Coupon.ProductName
            };
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            logger.LogInformation("Product Discount Delete {productName}", request.ProductName);

            bool sucess = await repository.DeleteDiscount(request.ProductName);

            return new DeleteDiscountResponse() { Success = sucess };
        }

    }
}
