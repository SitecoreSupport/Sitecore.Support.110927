namespace Sitecore.Support.ContentSearch.Maintenance
{
  using System;
  using Diagnostics;

  public class EventHub
  {
    public static event EventHandler OnIndexingStartedRemote;

    public static void IndexStartedRemoteHandler(object sender, EventArgs args)
    {
      Assert.IsNotNull(args, "event args");
      OnIndexingStartedRemote?.Invoke(sender, args);
    }
  }
}
