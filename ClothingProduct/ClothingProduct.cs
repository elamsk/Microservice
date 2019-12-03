using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Products;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;

namespace ClothingProduct
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class ClothingProduct : StatefulService, IClothingService
    {
        private IClothingRepository _repo ;
        public ClothingProduct(StatefulServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return this.CreateServiceRemotingReplicaListeners();
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            _repo = new ClothingRepository(this.StateManager);
        }

        public async Task<IEnumerable<ClothProduct>> allClothesGet()
        {
            var item = await _repo.GetAllproducts();
            return item;
        }

        public async Task AddCloth(ClothProduct CP)
        {
            await _repo.AddProduct(CP);
        }

        public async Task DeleteCloth(int productId)
        {
            await _repo.DeleteProduct(productId);
        }

        public async Task<ClothProduct> GetSingleCloth(int productId)
        {
            var item = await _repo.GetProduct(productId);
            return item;
        }
    }
}
