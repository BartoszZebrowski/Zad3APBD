namespace Zad3.Controllers
{
    internal class WarehouseService
    {
        private readonly WarehouseRepository _warehouseRepository;

        public WarehouseService(WarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        internal void X()
        {
             _warehouseRepository.Get();
        }
    }
}