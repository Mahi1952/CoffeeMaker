namespace Coffeemaker
{
  public class Door : IDevice
  {
    private bool _isOpen; 
    private CancellationTokenSource _cancelationTokenSource = new();
    private readonly Random _random = new();
    private int _openTime;

    public event EventHandler<DoorStateArgs>? Opened;
    public event EventHandler<DoorStateArgs>? Closed;

    private async Task RandomlyOpenOrClose(CancellationToken token)
    {
      while (!token.IsCancellationRequested)
      {
        bool _openDoor = _random.Next(0, 2) == 1;
        if (_openDoor) Open();
        else if (_isOpen) Close();

        int waitingTime = _random.Next(3000, 12000);
        await Task.Delay(waitingTime, token);
      }
    }

    private void Open()
    {
      if (_isOpen) return;

      _isOpen = true;
      _openTime = 0;
      Console.WriteLine("Door is open");
      Opened?.Invoke(this, new DoorStateArgs()); // Trigger Opened event
      Task.Run(() => LogDoorState(_cancelationTokenSource.Token));
    }

    private void Close()
    {
      if (!_isOpen) return;

      _isOpen = false;
      Console.WriteLine("Door is closed");
      Closed?.Invoke(this, new DoorStateArgs());
    }

    private async Task LogDoorState(CancellationToken token)
    {
      while (_isOpen && !token.IsCancellationRequested)
      {
        Console.WriteLine($"Door is open for {_openTime} seconds");
        if (_openTime < 8)
        {
          await Task.Delay(1000, token);
        }
        else
        {
          await Task.Delay(500, token);
        }
        _openTime++;
      }
    }

    public void Start()
    {
      if (_cancelationTokenSource != null)
        _cancelationTokenSource = new CancellationTokenSource(); 

      Task.Run(() => RandomlyOpenOrClose(_cancelationTokenSource.Token));
    }

    public void Stop()
    {
      _cancelationTokenSource?.Cancel();
    }

    public bool IsOpen => _isOpen;
  }
  public class DoorStateArgs : EventArgs
  {
    public bool IsOpen { get; set; }
  }
}
