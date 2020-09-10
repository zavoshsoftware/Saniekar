using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IUnitOfWork : System.IDisposable
    {
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        ICityRepository CityRepository { get; }
        IOrderRepository OrderRepository { get; }
        IProductRepository ProductRepository { get; }
        IProvinceRepository ProvinceRepository { get; }
        IOrderDetailRepository OrderDetailRepository { get; }
        IAccountRepository AccountRepository { get; }
        IBranchRepository BranchRepository { get; }
        ISupplierRepository SupplierRepository { get; }
        IProductGroupRepository ProductGroupRepository { get; }
        IPaymentTypeRepository PaymentTypeRepository { get; }
        IPaymentRepository PaymentRepository { get; }
        IBranchUserRepository BranchUserRepository { get; }
        IProductRequestRepository ProductRequestRepository { get; }
        IProductRequestDetailRepository ProductRequestDetailRepository { get; }
        IBranchProductRepository BranchProductRepository { get; }
        IInputDocumentRepository InputDocumentRepository { get; }
        IInputDocumentDetailRepository InputDocumentDetailRepository { get; }
        IInventoryRepository InventoryRepository { get; }
        IProductRequestDetailSupplierRepository ProductRequestDetailSupplierRepository { get; }
        IShipmentTypeRepository ShipmentTypeRepository { get; }
        IRegionRepository RegionRepository { get; }
        IOrderStatusRepository OrderStatusRepository { get; }
        IProductColorRepository ProductColorRepository { get; }
        IMattressRepository MattressRepository { get; }
        IFundRepository FundRepository { get; }
        IFundDetailRepository FundDetailRepository { get; }
        void Save();
    }
}
