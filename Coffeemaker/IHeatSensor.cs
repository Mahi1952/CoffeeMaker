
namespace Coffeemaker
{
  public interface IHeatSensor:IDevice
  {
    public event EventHandler<TemperatureArgs> SafeTemps;
    public event EventHandler<TemperatureArgs> HeatedTemps;
    public event EventHandler<TemperatureArgs> OverHeatedTemps;
  }
}
