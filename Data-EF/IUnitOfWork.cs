using Data_EF.Repositories;
using System.Threading.Tasks;

namespace Data_EF
{
    public interface IUnitOfWork
    {
        #region Repositories

        public IAttachmentRepository Attachment { get; }

        public IAuditLogRepository AuditLog { get; }

        public IAuthTokenRepository AuthToken { get; }

        public IBeginningVoucherRepository BeginningVoucher { get; }

        public IBeginningVoucherDetailRepository BeginningVoucherDetail { get; }

        public ICategoryRepository Category { get; }

        public ICheckingVoucherRepository CheckingVoucher { get; }

        public ICheckingVoucherDetailRepository CheckingVoucherDetail { get; }

        public ICustomerRepository Customer { get; }

        public IPermissionRepository Permission { get; }

        public IProductRepository Product { get; }

        public IUserRepository User { get; }

        public IUserGroupRepository UserGroup { get; }

        public IVoucherPrefixCodeRepository VoucherPrefixCode { get; }

        public IWarehouseRepository Warehouse { get; }

        public ITestRepository TestSetting { get; }

        public IReceiveRequestVoucherRepository ReceiveRequestVoucher { get; }

        public IReceiveRequestVoucherDetailRepository ReceiveRequestVoucherDetail { get; }

        public IDeliveryRequestVoucherRepository DeliveryRequestVoucher { get; }

        public IDeliveryRequestVoucherDetailRepository DeliveryRequestVoucherDetail { get; }

        public IFixingVoucherRepository FixingVoucher { get; }
        
        public ITransferRequestVoucherRepository TransferRequestVoucher { get; }
        
        public IDeliveryVoucherRepository DeliveryVoucher { get; }

        public IReceiveVoucherRepository ReceiveVoucher { get; }

        public ITransferVoucherRepository TransferVoucher { get; }

        public IDocumentTypeRepository DocumentType { get; }
        
        public ISumProductRepository SumProduct { get; }

        #endregion

        #region DB method

        Task<int> SaveChanges();

        #endregion
    }
}