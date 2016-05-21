namespace Sitecore.Support.ContentSearch.Maintenance.Strategies
{
  using System;
  using Diagnostics;
  using Sitecore.ContentSearch;
  using Sitecore.ContentSearch.Abstractions;
  using Sitecore.ContentSearch.Diagnostics;
  using Sitecore.ContentSearch.Maintenance;
  using Sitecore.ContentSearch.Maintenance.Strategies;
  using EventHub = Maintenance.EventHub;

  // NOTE: cannot reuse standard RemoteRebuildStrategy 
  // becuase its Initialize method gets invoked instead of new Initialize method from this class.
  public class RemoteRebuildStrategy : IIndexUpdateStrategy
  {
    // Fields
    private ISearchIndex index;
    private string indexName;

    public void Initialize(ISearchIndex searchIndex)
    {
      Assert.IsNotNull(searchIndex, "index");
      CrawlingLog.Log.Info($"[Index={searchIndex.Name}] Initializing RemoteRebuildStrategy.", null);
      this.index = searchIndex;
      EventHub.OnIndexingStartedRemote += (sender, args) => this.Handle(args);
    }

    protected void Handle(EventArgs args)
    {
      Assert.IsNotNull(args, "event args");
      IEvent instance = this.index.Locator.GetInstance<IEvent>();
      if (instance.ExtractParameters(args).Length != 2)
      {
        CrawlingLog.Log.Fatal($"[Index={this.index.Name}] RemoteRebuildStrategy skipped. Invalid parameters", null);
      }
      else if (!((instance.ExtractParameter(args, 1) is bool) && ((bool)instance.ExtractParameter(args, 1))))
      {
        CrawlingLog.Log.Fatal($"[Index={this.index.Name}] RemoteRebuildStrategy skipped. Full Rebuild was not detected.", null);
      }
      else
      {
        this.indexName = instance.ExtractParameter<string>(args, 0);
        if (string.IsNullOrEmpty(this.indexName))
        {
          CrawlingLog.Log.Fatal($"[Index={this.index.Name}] RemoteRebuildStrategy skipped. Invalid parameters", null);
        }
        else if (this.index.Name.Equals(this.indexName, StringComparison.InvariantCultureIgnoreCase))
        {
          OperationMonitor.Register(new Action(this.Run));
          OperationMonitor.Trigger();
        }
      }
    }

    public void Run()
    {
      CrawlingLog.Log.Debug($"[Index={this.index.Name}] RemoteRebuildStrategy triggered.", null);
      IndexCustodian.FullRebuildRemote(this.index, true);
    }
  }
}
