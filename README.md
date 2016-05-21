# Sitecore.Support.110927
When RemoteRebuildStrategy is used, full index rebuild job has to be completed on the instance that triggered rebuild before the event broadcasted to other instances. This creates situations when index could be out of sync for a period of time between rebuild initiating instance and all other instances. 

## Main

This repository contains Sitecore patch #110927. The patch implements a custom RemoteRebuildStrategy that subscribes to indexing:start:remote event. Default RemoteRebuildStrategy subscribes to indexing:end event which leaves room for indexes to be out of sync between instance that initiated full index rebuild and all other instances that depend on RemoteRebuildStrategy.
This patch could be used with [Sitecore.Support.97019](https://github.com/SitecoreSupport/Sitecore.Support.97019 "Sitecore.Support.97019 repo") if `Core` database isn't shared across all Sitecore instances. This is a typical situation when `Sitecore Azure` module is used with multiple geo-distributed deployments.

## Deployment

1. Copy `Sitecore.Support.110927.dll` assembly into the `/bin` folder.
2. Copy `z.Sitecore.Support.110927.config` file into the `/App_Config/Include` folder.

## Content

The patch includes the following files:

1. `/bin/Sitecore.Support.110927.dll`
2. `/App_Config/Include/z.Sitecore.Support.110927.config`
