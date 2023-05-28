using Logistic.DAL;
using Logistic.Models;

namespace Logistic.Core.Services
{
    public class VehicleService
    {
        private readonly IRepository<Vehicle> _repository;

        public VehicleService(IRepository<Vehicle> repository)
        {
            _repository = repository;
        }

        public Vehicle Create(Vehicle vehicle)
        {
            if (vehicle == null)
            {
                throw new ArgumentNullException(nameof(vehicle));
            }

            if (vehicle.Cargos == null)
            {
                vehicle.Cargos = new List<Cargo>();
            }

            _repository.Create(vehicle);
            return vehicle;
        }

        public Vehicle GetById(int vehicleId)
        {
            var vehicle = _repository.ReadById(vehicleId);
            if (vehicle == null)
            {
                throw new ArgumentException($"Vehicle with id {vehicleId} not found");
            }

            return vehicle;
        }

        public IEnumerable<Vehicle> GetAll()
        {
            return _repository.ReadAll();
        }

        public void Delete(int vehicleId)
        {
            //var vehicle = GetById(vehicleId);
            _repository.Delete(vehicleId);
        }

        public void LoadCargo(Cargo cargo, int vehicleId)
        {
            var vehicle = GetById(vehicleId);
            var totalWeight = vehicle.Cargos.Sum(c => c.Weight);
            var totalVolume = vehicle.Cargos.Sum(c => c.Volume);

            if (totalWeight + cargo.Weight > vehicle.MaxCargoWeightKg)
            {
                throw new ArgumentException($"Cargo with weight {cargo.Weight} cannot be loaded into vehicle with id {vehicleId}. The vehicle is already at full capacity");
            }

            if (totalVolume + cargo.Volume > vehicle.MaxCargoVolume)
            {
                throw new ArgumentException($"Cargo with weight {cargo.Volume} cannot be loaded into vehicle with id {vehicleId}. The vehicle has no free space");
            }

            vehicle.Cargos.Add(cargo);
            _repository.Update(vehicleId, vehicle);
        }

        public void UnloadCargo(Guid cargoId, int vehicleId)
        {
            var vehicle = GetById(vehicleId);
            var cargo = vehicle.Cargos.FirstOrDefault(c => c.Id == cargoId);
            if (cargo == null)
            {
                throw new ArgumentException($"Cargo with id {cargoId} not found in vehicle with id {vehicleId}");
            }

            vehicle.Cargos.Remove(cargo);
            _repository.Update(vehicleId, vehicle);
        }
        public void Update(Vehicle updatedVehicle)
        {
            if (updatedVehicle == null)
            {
                throw new ArgumentNullException(nameof(updatedVehicle));
            }

            _repository.Update(updatedVehicle.Id, updatedVehicle);
        }

    }
}