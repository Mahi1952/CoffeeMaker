namespace Coffeemaker
{
  public class Fan : IDevice
  {
    private int _currentSpeed;
    private readonly HeatSensor _heatSensor;
    private readonly Door _door;

    public Fan(Door door)
    {
      _heatSensor = HeatSensor.Instance;
      _door = door;
      _heatSensor.SafeTemps += OnSafeTemps;
      _heatSensor.HeatedTemps += OnHeatedTemps;
      _heatSensor.OverHeatedTemps += OnOverHeatedTemps;

      _door.Closed += OnDoorClosed;
      _door.Opened += OnDoorOpened;
    }

    private void OnSafeTemps(object? sender, TemperatureArgs e)
    {
      Stop();
    }

    private void OnHeatedTemps(object? sender, TemperatureArgs e)
    {
      SetSpeedBasedOnTemperature(e.Temperature);
    }

    private void OnOverHeatedTemps(object? sender, TemperatureArgs e)
    {
      SetSpeedBasedOnTemperature(e.Temperature);
    }

    private void OnDoorOpened(object? sender, EventArgs e)
    {
      Stop(); // Turn off the fan when the door is opened
    }

    private void OnDoorClosed(object? sender, EventArgs e)
    {
     Console.WriteLine("Fan Keeps Moving");
    }

    private void SetSpeedBasedOnTemperature(int temperature)
    {
      if (_door.IsOpen)
      {
        Stop(); // Ensure fan is off if the door is open
        return;
      }

      if (temperature >= 90 && temperature < 110)
      {
        _currentSpeed = 1; // Low speed
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Fan speed set to Low.");
      }
      else if (temperature >= 110 && temperature < 130)
      {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        _currentSpeed = 2; // Medium speed
        Console.WriteLine("Fan speed set to Medium.");
      }
      else if (temperature >= 130)
      {
        _currentSpeed =3; // High speed
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Fan speed set to High.");
      }
    }

    public void Start()
    {
      Console.WriteLine("Fan is starting.");
      _currentSpeed = 0; // Ensure fan starts off
    }

    public void Stop()
    {
      _currentSpeed = 0;
      Console.WriteLine("Fan is turned off.");
    }
  }
}
