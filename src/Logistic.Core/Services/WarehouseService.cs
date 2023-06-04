using Logistic.DAL;
using Logistic.Models;

namespace Logistic.Core.Services
{
    public class WarehouseService :IService<Warehouse>
    {
        private readonly IRepository<Warehouse> _repository;

        public WarehouseService(IRepository<Warehouse> repository)
        {
            _repository = repository;
        }

        public Warehouse Create(Warehouse warehouse)
        {
            return _repository.Create(warehouse);
        }

        public Warehouse GetById(int warehouseId)
        {
            return _repository.ReadById(warehouseId);
        }

        public IEnumerable<Warehouse> GetAll()
        {
            return _repository.ReadAll();
        }

        public void Delete(int warehouseId)
        {
            _repository.Delete(warehouseId);
        }

        public void LoadCargo(Cargo cargo, int warehouseId)
        {
            var warehouse = GetById(warehouseId);
            warehouse.CargoList?.Add(cargo);
            _repository.Update(warehouseId, warehouse);
        }

        public void UnloadCargo(Guid cargoId, int warehouseId)
        {
            var warehouse = GetById(warehouseId);
            var cargo = warehouse.CargoList?.FirstOrDefault(c => c.Id == cargoId);
            if (cargo == null)
            {
                throw new ArgumentException($"Cargo with id {cargoId} not found in warehouse with id {warehouseId}");
            }

            warehouse.CargoList?.Remove(cargo);
            _repository.Update(warehouseId, warehouse);
        }

        public void Update(Warehouse updatedWarehouse)
        {
            if (updatedWarehouse == null)
            {
                throw new ArgumentNullException(nameof(updatedWarehouse));
            }

            _repository.Update(updatedWarehouse.Id, updatedWarehouse);
        }
    }
}