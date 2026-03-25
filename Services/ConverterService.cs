using RecipeCost.Shared;
using RecipeCostAPI.Services.Interfaces;
using UnitsNet;
using UnitsNet.Units;

namespace RecipeCostAPI.Services
{
    public class ConverterService : IConverterService
    { 
        public decimal Convert(decimal quantity, UnitType fromUnit, UnitType toUnit)
        {
            if (quantity == 0) return 0;
            if (fromUnit == toUnit) return quantity;

            // Handle Mass to Mass
            if(IsMassUnit(fromUnit) && IsMassUnit(toUnit))
            {
                var fromUnitsNet = MapToMass(fromUnit);
                var toUnitsNet = MapToMass(toUnit);
                return (decimal)UnitConverter.Convert(quantity, fromUnitsNet, toUnitsNet);
            }

            // Handle Volume to Volume
            if(IsVolumeUnit(fromUnit) && IsVolumeUnit(toUnit))
            {
                var fromUnitsNet = MapToVolume(fromUnit);
                var toUnitsNet = MapToVolume(toUnit);
                return (decimal)UnitConverter.Convert(quantity, fromUnitsNet, toUnitsNet);
            }

            throw new ArgumentException($"Cannot convert from '{fromUnit}' to '{toUnit}'. Units must be of the same category (both mass or both volume).");
        }

        public UnitType GetBaseUnit(UnitType unit)
        {
            if (IsMassUnit(unit)) return UnitType.Gram;
            if (IsVolumeUnit(unit)) return UnitType.Milliliter;
            throw new ArgumentException($"Unit {unit} category not recognized.");
        }


        public decimal ConvertToBaseUnit(decimal quantity, UnitType fromUnit)
        {
            // Separate by category BEFORE calling the library
            if (IsVolumeUnit(fromUnit))
            {
                var from = MapToVolume(fromUnit);
                // We MUST use a VolumeUnit as the destination (toUnit)
                var baseQuantity = UnitConverter.Convert((double)quantity, from, VolumeUnit.Milliliter);
                // Calculate the base unit cost by dividing the user unit cost by the converted quantity in base units
                var baseUnitCost = quantity / (decimal)baseQuantity;
                return baseUnitCost;
            } 
            else if (IsMassUnit(fromUnit))
            {
                var from = MapToMass(fromUnit);
                // We MUST use a MassUnit as the destination (toUnit)
                var baseQuantity = UnitConverter.Convert((double)quantity, from, MassUnit.Gram);
                var baseUnitCost = quantity / (decimal)baseQuantity;
                return baseUnitCost;
            }
            else if(IsPieceUnit(fromUnit))
            {
                // For piece units, we can treat them as a 1:1 conversion to a "base" piece unit
                return quantity; // No conversion needed for piece units
            }

            throw new ArgumentException($"Unit {fromUnit} is not a valid mass or volume unit.");
        }

        public bool IsMassUnit(UnitType unit) => unit switch
        {
            UnitType.Gram or UnitType.Kilogram or UnitType.Ounce or UnitType.Pound => true,
            _ => false
        };

        public bool IsVolumeUnit(UnitType unit) => unit switch
        {
            UnitType.Milliliter or UnitType.Liter or UnitType.Cup or UnitType.Gallon 
            or UnitType.Quart or UnitType.Teaspoon or UnitType.Tablespoon => true,
            _ => false
        };

        public bool IsPieceUnit(UnitType unit) => unit switch
        {
            UnitType.Piece or UnitType.Each => true,
            _ => false
        };

        private MassUnit MapToMass(UnitType unit) => unit switch
        {
            UnitType.Gram => MassUnit.Gram,
            UnitType.Kilogram => MassUnit.Kilogram,
            UnitType.Ounce => MassUnit.Ounce,
            UnitType.Pound => MassUnit.Pound,
            _ => throw new ArgumentException($"{unit} is not a mass unit.")
        };

        private VolumeUnit MapToVolume(UnitType unit) => unit switch
        {
            UnitType.Milliliter => VolumeUnit.Milliliter,
            UnitType.Liter => VolumeUnit.Liter,
            UnitType.Cup => VolumeUnit.UsCustomaryCup,
            UnitType.Teaspoon => VolumeUnit.UsTeaspoon,
            UnitType.Tablespoon => VolumeUnit.UsTablespoon,
            UnitType.Gallon => VolumeUnit.UsGallon,
            UnitType.Quart => VolumeUnit.UsQuart,
            _ => throw new ArgumentException($"{unit} is not a volume unit.")
        }; 
    }
}
