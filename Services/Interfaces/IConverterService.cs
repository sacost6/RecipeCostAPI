using RecipeCost.Shared;

namespace RecipeCostAPI.Services.Interfaces
{
    // Used to handle unit conversions and cost calculations, but we can abstract it for now
    public interface IConverterService
    {
            // Convert a quantity from one unit to another (e.g., grams to cups)
        decimal Convert(decimal quantity, UnitType fromUnit, UnitType toUnit);
        UnitType GetBaseUnit(UnitType unit); 
        decimal ConvertToBaseUnit(decimal quantity, UnitType fromUnit);
        bool IsVolumeUnit(UnitType unit);
        bool IsMassUnit(UnitType unit);

    }
}

