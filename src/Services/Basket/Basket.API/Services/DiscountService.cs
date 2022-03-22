using Discount.Grpc.Protos;
using Grpc.Core;
using System.Threading.Tasks;

namespace Basket.API.Services
{
    public interface IDiscountService
    {
        Task<CouponModel> GetDiscount(string productName);
    }

    public class DiscountService : IDiscountService
    {
        private DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient;

        public DiscountService(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
        {
            this.discountProtoServiceClient = discountProtoServiceClient;
        }

        public async Task<CouponModel> GetDiscount(string productName)
        {
            var discountRequest = new GetDiscountRequest() { ProductName = productName };

            return await discountProtoServiceClient.GetDiscountAsync(discountRequest);
        }
    }
}
