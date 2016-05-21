namespace Sitecore.Support.ContentSearch.Events
{
  using System;
  using Maintenance;

  public class IndexingEventHandler
  {
    public void IndexingStartedRemoteHandler(object sender, EventArgs args)
    {
      EventHub.IndexStartedRemoteHandler(sender, args);
    }
  }
}
