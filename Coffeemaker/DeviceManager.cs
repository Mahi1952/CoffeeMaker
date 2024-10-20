namespace Coffeemaker
{
  public class DeviceManager
  {
    private readonly Thermostat _thermostat;
    private readonly Door _door;
    private readonly Fan _fan;

    public DeviceManager()
    {
      _thermostat = new(HeatSensor.Instance);
      _door = new();
      _fan = new(_door);
    }

    public void Start()
    {
      Task.Run(() => _thermostat.Start());
      Task.Run(() => _door.Start());
      _fan.Start();
    }

    public void Stop()
    {
      _thermostat.Stop();
      _door.Stop();
      _fan.Stop();
    }
  }

}
