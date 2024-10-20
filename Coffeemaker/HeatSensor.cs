
namespace Coffeemaker
{
  public class HeatSensor:IHeatSensor
  {
    private static readonly Lazy<HeatSensor> _instance = new(() => new HeatSensor());
    private readonly TemperatureGenerator _tempGen = new();
    private CancellationTokenSource? _cancelationTokenSource;

    public static HeatSensor Instance => _instance.Value;

    public event EventHandler<TemperatureArgs>? SafeTemps;
    public event EventHandler<TemperatureArgs>? HeatedTemps;
    public event EventHandler<TemperatureArgs>? OverHeatedTemps;

    private HeatSensor() { }

    private async void MonitorTemeratures(CancellationTokenSource token)
    {
      while (!token.IsCancellationRequested)
      {
        var temperature = _tempGen.GetNewTemp();
        switch (temperature)
        {
          case int temp when temp < 90:
            SafeTemps?.Invoke(this, new TemperatureArgs { Temperature = temp, CurrentTime = DateTime.Now });
            break;
          case int temp when temp >= 90 && temp < 110:
            HeatedTemps?.Invoke(this, new TemperatureArgs { Temperature = temp, CurrentTime = DateTime.Now });
            break;
          case int temp when temp >= 110:
            OverHeatedTemps?.Invoke(this, new TemperatureArgs { Temperature = temp, CurrentTime = DateTime.Now });
            break;
        }
        await Task.Delay(1500);
      }
    }
    public void Start()
    {
      _cancelationTokenSource = new CancellationTokenSource();
      Task.Run(()=>MonitorTemeratures(_cancelationTokenSource));
      
    }
    public void Stop()
    {
     _cancelationTokenSource?.Cancel();
    }
  }  
  
  public class Thermostat
  {
    private readonly HeatSensor _heatSensor;
    public Thermostat(HeatSensor heatSensor)
    {
      _heatSensor = HeatSensor.Instance;
      _heatSensor.SafeTemps += OnSafeTemps;
      _heatSensor.HeatedTemps += OnHeatedTemps;
      _heatSensor.OverHeatedTemps += OnOverHeatedTemps;
    }
    private void OnSafeTemps(object? sender, TemperatureArgs e)
    {
      Console.ForegroundColor=ConsoleColor.Green;
      Console.WriteLine($"\nSafe temperature of {e.Temperature} at {e.CurrentTime}");
    }
    private void OnHeatedTemps(object? sender, TemperatureArgs e)
    {
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine($"\nHeated temperature of {e.Temperature} at {e.CurrentTime}");
    }
    private void OnOverHeatedTemps(object? sender, TemperatureArgs e)
    {
      Console.ForegroundColor = ConsoleColor.DarkRed;
      Console.WriteLine($"\nOver heated temperature of {e.Temperature} at {e.CurrentTime}");
    }

    public void Start()
    {
      _heatSensor.Start();
    }

    public void Stop()
    {
      _heatSensor?.Stop();
    }
  }
  public class TemperatureArgs { 
  public int Temperature { get; set; }
    public DateTime CurrentTime { get; set; }
  }
  public class TemperatureGenerator
  {
    private readonly Random _random = new();
    public int GetNewTemp()=> _random.Next(50, 150);
  }
}
