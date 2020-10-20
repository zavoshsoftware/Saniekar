using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public class UnitOfWork : System.Object, IUnitOfWork
    {
        public UnitOfWork()
        {
            IsDisposed = false;
        }
        protected bool IsDisposed { get; set; }

        private Models.DatabaseContext _databaseContext;
        protected virtual Models.DatabaseContext DatabaseContext
        {
            get
            {
                if (_databaseContext == null)
                {
                    _databaseContext =
                        new Models.DatabaseContext();
                }
                return (_databaseContext);
            }
        }
        public void Save()
        {
            try
            {
                DatabaseContext.SaveChanges();
            }
            //catch (System.Exception ex)
            catch
            {
                //Hmx.LogHandler.Report(GetType(), null, ex);
                throw;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _databaseContext.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }


        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Inserting custom Respositories
        //private IRepository _Repository;
        //public IRepository Repository
        //{
        //	get
        //	{
        //		if (_Repository == null)
        //		{
        //			_Repository =
        //				new Repository(DatabaseContext);
        //		}
        //		return (_Repository);
        //	}
        //}

 

        private IUserRepository _userRepository;
        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository =
                        new UserRepository(DatabaseContext);
                }
                return (_userRepository);
            }
        }

      



        private IRoleRepository _roleRepository;
        public IRoleRepository  RoleRepository
        {
            get
            {
                if (_roleRepository == null)
                {
                    _roleRepository =
                        new RoleRepository(DatabaseContext);
                }
                return (_roleRepository);
            }
        }


    
      


        private ICityRepository _cityRepository;
        public ICityRepository CityRepository
        {
            get
            {
                if (_cityRepository == null)
                {
                    _cityRepository =
                        new CityRepository(DatabaseContext);
                }
                return (_cityRepository);
            }
        }


        private IOrderRepository _orderRepository;
        public IOrderRepository OrderRepository
        {
            get
            {
                if (_orderRepository == null)
                {
                    _orderRepository =
                        new OrderRepository(DatabaseContext);
                }
                return (_orderRepository);
            }
        }



        private IProductRepository _productRepository;
        public IProductRepository ProductRepository
        {
            get
            {
                if (_productRepository == null)
                {
                    _productRepository =
                        new ProductRepository(DatabaseContext);
                }
                return (_productRepository);
            }
        }

    
        private IProvinceRepository _provinceRepository;
        public IProvinceRepository ProvinceRepository
        {
            get
            {
                if (_provinceRepository == null)
                {
                    _provinceRepository =
                        new ProvinceRepository(DatabaseContext);
                }
                return (_provinceRepository);
            }
        }
     

        private IOrderDetailRepository _orderDetailRepository;
        public IOrderDetailRepository OrderDetailRepository
        {
            get
            {
                if (_orderDetailRepository == null)
                {
                    _orderDetailRepository =
                        new OrderDetailRepository(DatabaseContext);
                }
                return (_orderDetailRepository);
            }
        }

        private IAccountRepository _accountRepository;
        public IAccountRepository AccountRepository
        {
            get
            {
                if (_accountRepository == null)
                {
                    _accountRepository =
                        new AccountRepository(DatabaseContext);
                }
                return (_accountRepository);
            }
        }

        private IBranchRepository _branchRepository;
        public IBranchRepository BranchRepository
        {
            get
            {
                if (_branchRepository == null)
                {
                    _branchRepository =
                        new BranchRepository(DatabaseContext);
                }
                return (_branchRepository);
            }
        }

        private ISupplierRepository _supplierRepository;
        public ISupplierRepository SupplierRepository
        {
            get
            {
                if (_supplierRepository == null)
                {
                    _supplierRepository =
                        new SupplierRepository(DatabaseContext);
                }
                return (_supplierRepository);
            }
        }

        private IProductGroupRepository _productGroupRepository;
        public IProductGroupRepository ProductGroupRepository
        {
            get
            {
                if (_productGroupRepository == null)
                {
                    _productGroupRepository =
                        new ProductGroupRepository(DatabaseContext);
                }
                return (_productGroupRepository);
            }
        }

        private IPaymentTypeRepository _paymentTypeRepository;
        public IPaymentTypeRepository PaymentTypeRepository
        {
            get
            {
                if (_paymentTypeRepository == null)
                {
                    _paymentTypeRepository =
                        new PaymentTypeRepository(DatabaseContext);
                }
                return (_paymentTypeRepository);
            }
        }

        private IPaymentRepository _paymentRepository;
        public IPaymentRepository PaymentRepository
        {
            get
            {
                if (_paymentRepository == null)
                {
                    _paymentRepository =
                        new PaymentRepository(DatabaseContext);
                }
                return (_paymentRepository);
            }
        }

        private IBranchUserRepository _branchUserRepository;
        public IBranchUserRepository BranchUserRepository
        {
            get
            {
                if (_branchUserRepository == null)
                {
                    _branchUserRepository =
                        new BranchUserRepository(DatabaseContext);
                }
                return (_branchUserRepository);
            }
        }

        private IProductRequestRepository _productRequestRepository;
        public IProductRequestRepository ProductRequestRepository
        {
            get
            {
                if (_productRequestRepository == null)
                {
                    _productRequestRepository =
                        new ProductRequestRepository(DatabaseContext);
                }
                return (_productRequestRepository);
            }
        }

        private IProductRequestDetailRepository _productRequestDetailRepository;
        public IProductRequestDetailRepository ProductRequestDetailRepository
        {
            get
            {
                if (_productRequestDetailRepository == null)
                {
                    _productRequestDetailRepository =
                        new ProductRequestDetailRepository(DatabaseContext);
                }
                return (_productRequestDetailRepository);
            }
        }

        private IBranchProductRepository _branchProductRepository;
        public IBranchProductRepository BranchProductRepository
        {
            get
            {
                if (_branchProductRepository == null)
                {
                    _branchProductRepository =
                        new BranchProductRepository(DatabaseContext);
                }
                return (_branchProductRepository);
            }
        }


        private IInputDocumentRepository _inputDocumentRepository;
        public IInputDocumentRepository InputDocumentRepository
        {
            get
            {
                if (_inputDocumentRepository == null)
                {
                    _inputDocumentRepository =
                        new InputDocumentRepository(DatabaseContext);
                }
                return (_inputDocumentRepository);
            }
        }

        private IInputDocumentDetailRepository _inputDocumentDetailRepository;
        public IInputDocumentDetailRepository InputDocumentDetailRepository
        {
            get
            {
                if (_inputDocumentDetailRepository == null)
                {
                    _inputDocumentDetailRepository =
                        new InputDocumentDetailRepository(DatabaseContext);
                }
                return (_inputDocumentDetailRepository);
            }
        }

        private IInventoryRepository _inventoryRepository;
        public IInventoryRepository InventoryRepository
        {
            get
            {
                if (_inventoryRepository == null)
                {
                    _inventoryRepository =
                        new InventoryRepository(DatabaseContext);
                }
                return (_inventoryRepository);
            }
        }

        private IProductRequestDetailSupplierRepository _productRequestDetailSupplierRepository;
        public IProductRequestDetailSupplierRepository ProductRequestDetailSupplierRepository
        {
            get
            {
                if (_productRequestDetailSupplierRepository == null)
                {
                    _productRequestDetailSupplierRepository =
                        new ProductRequestDetailSupplierRepository(DatabaseContext);
                }
                return (_productRequestDetailSupplierRepository);
            }
        }

        private IShipmentTypeRepository _shipmentTypeRepository;
        public IShipmentTypeRepository ShipmentTypeRepository
        {
            get
            {
                if (_shipmentTypeRepository == null)
                {
                    _shipmentTypeRepository =
                        new ShipmentTypeRepository(DatabaseContext);
                }
                return (_shipmentTypeRepository);
            }
        }

        private IRegionRepository _regionRepository;
        public IRegionRepository RegionRepository
        {
            get
            {
                if (_regionRepository == null)
                {
                    _regionRepository =
                        new RegionRepository(DatabaseContext);
                }
                return (_regionRepository);
            }
        }


        private IOrderStatusRepository _orderStatusRepository;
        public IOrderStatusRepository OrderStatusRepository
        {
            get
            {
                if (_orderStatusRepository == null)
                {
                    _orderStatusRepository =
                        new OrderStatusRepository(DatabaseContext);
                }
                return (_orderStatusRepository);
            }
        }


        private IProductColorRepository _productColorRepository;
        public IProductColorRepository ProductColorRepository
        {
            get
            {
                if (_productColorRepository == null)
                {
                    _productColorRepository =
                        new ProductColorRepository(DatabaseContext);
                }
                return (_productColorRepository);
            }
        }


        private IMattressRepository _mattressRepository;
        public IMattressRepository MattressRepository
        {
            get
            {
                if (_mattressRepository == null)
                {
                    _mattressRepository =
                        new MattressRepository(DatabaseContext);
                }
                return (_mattressRepository);
            }
        }

        private IFundRepository _fundRepository;
        public IFundRepository FundRepository
        {
            get
            {
                if (_fundRepository == null)
                {
                    _fundRepository =
                        new FundRepository(DatabaseContext);
                }
                return (_fundRepository);
            }
        }

        private IFundDetailRepository _fundDetailRepository;
        public IFundDetailRepository FundDetailRepository
        {
            get
            {
                if (_fundDetailRepository == null)
                {
                    _fundDetailRepository =
                        new FundDetailRepository(DatabaseContext);
                }
                return (_fundDetailRepository);
            }
        }

        private IProductRequestStatusRepository _productRequestStatusRepository;
        public IProductRequestStatusRepository ProductRequestStatusRepository
        {
            get
            {
                if (_productRequestStatusRepository == null)
                {
                    _productRequestStatusRepository =
                        new ProductRequestStatusRepository(DatabaseContext);
                }
                return (_productRequestStatusRepository);
            }
        }
        private IInventoryDetailTypeRepository _inventoryDetailTypeRepository;
        public IInventoryDetailTypeRepository InventoryDetailTypeRepository
        {
            get
            {
                if (_inventoryDetailTypeRepository == null)
                {
                    _inventoryDetailTypeRepository =
                        new InventoryDetailTypeRepository(DatabaseContext);
                }
                return (_inventoryDetailTypeRepository);
            }
        }


        private IInventoryDetailRepository _inventoryDetailRepository;
        public IInventoryDetailRepository InventoryDetailRepository
        {
            get
            {
                if (_inventoryDetailRepository == null)
                {
                    _inventoryDetailRepository =
                        new InventoryDetailRepository(DatabaseContext);
                }
                return (_inventoryDetailRepository);
            }
        }
        #endregion Inserting custom Respositories
    }
}
