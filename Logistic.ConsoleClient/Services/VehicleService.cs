using System;
using System.Collections.Generic;
using System.Linq;
using Logistic.ConsoleClient.Repositories;

namespace Logistic.ConsoleClient.Services
{
    public class VehicleService
    {
        private readonly InMemoryRepository<Vehicle> _repository;

        public VehicleService(InMemoryRepository<Vehicle> repository)
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
                vehicle.Cargos = new Cargo[] { };
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
            var vehicle = GetById(vehicleId);
            _repository.Delete(vehicle);
        }

        public void LoadCargo(Cargo cargo, int vehicleId)
        {
            var vehicle = GetById(vehicleId);
            var totalWeight = vehicle.Cargos.Sum(c => c.Weight);
            var totalVolume = vehicle.Cargos.Sum(c => c.Volume);

            if (totalWeight + cargo.Weight > vehicle.CargoWeightLeftKg)
            {
                throw new ArgumentException($"Cargo with weight {cargo.Weight} cannot be loaded into vehicle with id {vehicleId}. The vehicle is already at full capacity");
            }

            if (totalVolume + cargo.Volume > vehicle.CargoVolumeLeft)
            {
                throw new ArgumentException($"Cargo with weight {cargo.Volume} cannot be loaded into vehicle with id {vehicleId}. The vehicle has no free space");
            }

            for (int i = 0; i < vehicle.Cargos.Length; i++)
            {
                if (vehicle.Cargos[i] == null)
                {
                    vehicle.Cargos[i] = cargo;
                    break;
                }
            }
            vehicle.CargoWeightLeftKg -= cargo.Weight;
            vehicle.CargoVolumeLeft -= cargo.Volume;

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

            vehicle.Cargos = vehicle.Cargos.Where(n => n != cargo).ToArray();
            vehicle.CargoVolumeLeft += cargo.Volume;
            vehicle.CargoWeightLeftKg += cargo.Weight;
            _repository.Update(vehicleId, vehicle);
        }
    }


}

