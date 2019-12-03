using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Products
{
    public interface IClothingService : IService
    {
        Task<IEnumerable<ClothProduct>> allClothesGet();
        Task AddCloth(ClothProduct CP);
        Task DeleteCloth(int productId);
        Task<ClothProduct> GetSingleCloth(int productId);

    }
    public class ClothProduct
    {
        public int productID { get; set; }
        public string name { get; set; }
        public string desc { get; set; }
        public decimal price { get; set; }
    }
}
