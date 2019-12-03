using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Remoting;
using Products;

namespace ClothingProduct
{
    interface IClothingRepository
    {
        Task<IEnumerable<ClothProduct>> GetAllproducts();
        Task AddProduct(ClothProduct CP);
        Task DeleteProduct(int productId);
        Task<ClothProduct> GetProduct(int productId);

    }
    class ClothingRepository : IClothingRepository
    {
        private IReliableStateManager _manager;

        public ClothingRepository(IReliableStateManager manager)
        {
            _manager = manager;
        }

        public async Task AddProduct(ClothProduct CP)
        {
            var dict = await _manager.GetOrAddAsync<IReliableDictionary<int, ClothProduct>>("clothes");

            using( var tx= _manager.CreateTransaction())
            {
                await dict.AddOrUpdateAsync(tx, CP.productID, CP, (id, _CP) => _CP);
                await tx.CommitAsync();
            }
        }

        public async Task DeleteProduct(int productId)
        {
            var dict = await _manager.GetOrAddAsync<IReliableDictionary<int, ClothProduct>>("clothes");

            using (var tx = _manager.CreateTransaction())
            {
                bool exists = await dict.ContainsKeyAsync(tx, productId);
                if (exists)
                {
                    await dict.TryRemoveAsync(tx, productId);
                    await tx.CommitAsync();

                }
           }
        }

        public async Task<IEnumerable<ClothProduct>> GetAllproducts()
        {
            List<ClothProduct> allclothes = new List<ClothProduct>();
            var dict = await _manager.GetOrAddAsync<IReliableDictionary<int, ClothProduct>>("clothes");
            using (var tx = _manager.CreateTransaction())
            {
                var enumerable = await dict.CreateEnumerableAsync(tx, EnumerationMode.Ordered);
                using (var enumerator = enumerable.GetAsyncEnumerator())
                {
                    while(await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        KeyValuePair<int, ClothProduct> instance = enumerator.Current;
                        allclothes.Add(instance.Value);
                    }
                }
            }
            return allclothes;
        }
    

        public async Task<ClothProduct> GetProduct(int productId)
        {
            var dict = await _manager.GetOrAddAsync<IReliableDictionary<int, ClothProduct>>("clothes");
            using (var tx = _manager.CreateTransaction())
            {
               ConditionalValue<ClothProduct> cp =  await dict.TryGetValueAsync(tx, productId);
                return cp.Value;
                }
            }
        }
    }

