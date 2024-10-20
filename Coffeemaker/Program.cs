namespace Coffeemaker
{
  public class Program
  {
    public static void Main()
    {
      DeviceManager manager = new();
      manager.Start();
      Console.WriteLine("Press Enter to stop...");
      Console.ReadLine();
      manager.Stop();
    }
  }
}
