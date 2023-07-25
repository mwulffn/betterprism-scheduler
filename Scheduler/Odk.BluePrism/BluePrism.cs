using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using PetaPoco;

namespace Odk.BluePrism
{
    class BluePrism : IBluePrism
    {
        private Database _database;
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly List<Guid> _allocatedResources;
        private readonly int _availableLicenses;

        public BluePrism()
        {
            _database = new PetaPoco.Database("BluePrism");
            _allocatedResources = new List<Guid>();
            _availableLicenses = int.Parse(ConfigurationManager.AppSettings["Licenses"] ?? "1");
        }

        public BPResource AllocateResource()
        {
            var resources = _database.Fetch<BPResource>("SELECT resourceid as Id,name as Name FROM BPAResource WHERE processesrunning = 0 AND AttributeID = 16");
            resources = resources.Where(a => !_allocatedResources.Contains(a.Id)).ToList();

            if (HasAvailableResources() && resources.Count > 0)
            {
                _allocatedResources.Add(resources.First().Id);
                return resources.First();
            }

            return null;
        }

        public void FreeResource(BPResource resource)
        {
            _allocatedResources.Remove(resource.Id);
        }

        public bool HasAvailableResources()
        {
            var resourcesInUse = _database.Fetch<BPResource>("SELECT resourceid as Id,[name],FQDN,DisplayStatus FROM BPAResource WHERE AttributeID = 0");
            var externallyAllocated = resourcesInUse.Where(a => !_allocatedResources.Contains(a.Id));

            return (_availableLicenses - _allocatedResources.Count - externallyAllocated.Count()) > 0;
        }

        public BPResourceStatus ResourceStatus(BPResource resource)
        {
            var status = _database.Single<string>("SELECT DisplayStatus FROM BPAResource WHERE [resourceid]=@0", resource.Id) ?? "";

            switch (status)
            {
                case "Working":
                    return BPResourceStatus.Working;
                case "Idle":
                    return BPResourceStatus.Idle;
                case "Logged Out":
                    return BPResourceStatus.LoggedOut;
                default:
                    return BPResourceStatus.Unavailable;
            }
        }

        public IEnumerable<BPResource> AllResources()
        {
            return _database.Fetch<BPResource>("SELECT resourceid as Id,[name],FQDN,DisplayStatus FROM BPAResource WHERE (AttributeID= 0 or AttributeID = 16) ORDER BY [name] ASC");
        }

        public BPResource GetResource(Guid resourceId)
        {
            return _database.Single<BPResource>("SELECT resourceid as Id,name,FQDN,DisplayStatus FROM BPAResource WHERE [resourceid] = @0", resourceId);
        }


        public BPProcess GetProcess(Guid processId)
        {
            return _database.Single<BPProcess>("SELECT processid as Id,name FROM BPAProcess WHERE [processid] = @0", processId);
        }


        public Guid LaunchSession(BPProcess process, BPResource resource, string parameters)
        {
            var result = AutomateC.LaunchBPASession(process.Name, resource.Name, parameters);

            if (result.HasValue)
                return result.Value;

            throw new Exception($"Unable to launch session {process.Name} on {resource.Name}");
        }

        public void RequestStop(Guid sessionId)
        {
            AutomateC.RequestStop(sessionId);
        }

        public IEnumerable<BPProcess> ListProcesses()
        {
            return _database.Fetch<BPProcess>("SELECT processid as Id,name FROM BPAProcess WHERE ProcessType='P' AND AttributeId=2 ORDER BY name asc;");
        }


        public BPWorkqueue GetWorkqueue(Guid workqueueId)
        {
            return _database.Single<BPWorkqueue>("SELECT [id] AS Id,[name] as Name,[running] as Running FROM [BPAWorkQueue] WHERE id=@0", workqueueId);
        }

        public IEnumerable<BPWorkqueue> ListWorkqueues()
        {
            return _database.Fetch<BPWorkqueue>("SELECT [id] AS Id,[name] as Name,[running] as Running FROM [BPAWorkQueue] ORDER BY [name] ASC");
        }

        public int PendingItems(BPWorkqueue queue, bool includePausedQueue = false)
        {
            var running = _database.ExecuteScalar<bool>("SELECT [running] FROM BPAWorkQueue WHERE id=@0", queue.Id);

            if (!running && !includePausedQueue)
                return 0;

            return _database.Single<int>("SELECT COUNT(*) FROM BPAWorkQueueItem WHERE exception is null AND completed is null AND (deferred is null or deferred < GETUTCDATE()) AND queueid=@0", queue.Id);
        }

        public BPSessionStatus SessionStatus(Guid sessionId)
        {
            var status = _database.ExecuteScalar<string>("SELECT [BPAStatus].[description] as SessionStatus FROM [BPVSessionInfo] JOIN BPAStatus on BPAStatus.statusid = BPVSessionInfo.statusid WHERE sessionid = @0; ", sessionId);

            return (BPSessionStatus)Enum.Parse(typeof(BPSessionStatus), status);
        }

        public IEnumerable<BPSession> LatestSessions(int count)
        {
            return _database.Query<BPSession>("SELECT [sessionid] as Id,[sessionnumber] as SessionNumber,[startdatetime] as [Start],[enddatetime] as [End],[processid] as ProcessId,[processname] as ProcessName,[starterusername] as [User],[runningresourcename] as [Resource],[BPAStatus].[description] as SessionStatus FROM [BPVSessionInfo] JOIN BPAStatus on BPAStatus.statusid = BPVSessionInfo.statusid ORDER BY sessionnumber desc; ").Take(count);
        }

        public BPSession GetSession(Guid sessionId)
        {
            return _database.Single<BPSession>("SELECT [sessionid] as Id,[sessionnumber] as SessionNumber,[startdatetime] as [Start],[enddatetime] as [End],[processid] as ProcessId,[processname] as ProcessName,[starterusername] as [User],[runningresourcename] as [Resource],[BPAStatus].[description] as SessionStatus FROM [BPVSessionInfo] JOIN BPAStatus on BPAStatus.statusid = BPVSessionInfo.statusid WHERE [sessionid] = @0;", sessionId);
        }

        public BPLogEntry GetLastLogEntry(Guid sessionId)
        {
            return _database.Single<BPLogEntry>("SELECT TOP (1) [logid] as LogId, BPASessionLog_NonUnicode.[sessionnumber] as SessionNumber,[stageid] as StageId,[stagename] as StageName,[stagetype] as StageType,[processname] as ProcessName,[pagename] as PageName,[objectname] as ObjectName,[actionname] as ActionName,[result] as Result,[resulttype] as ResultType, BPASessionLog_NonUnicode.[startdatetime] as [Start], BPASessionLog_NonUnicode.[enddatetime] as [End],[attributexml] as [AttributeXml] FROM [BPASessionLog_NonUnicode] JOIN BPASession on BPASession.sessionnumber = BPASessionLog_NonUnicode.sessionnumber where BPASession.sessionid = @0 order by logid desc;", sessionId);
        }

        public Page<BPLogEntry> GetLogPage(Guid sessionId, int page, int pageSize)
        {
            var session = GetSession(sessionId);

            return _database.Page<BPLogEntry>(page, pageSize, "SELECT [logid] as LogId, BPASessionLog_NonUnicode.[sessionnumber] as SessionNumber,[stageid] as StageId,[stagename] as StageName,[stagetype] as StageType,[processname] as ProcessName,[pagename] as PageName,[objectname] as ObjectName,[actionname] as ActionName,[result] as Result,[resulttype] as ResultType, BPASessionLog_NonUnicode.[startdatetime] as [Start], BPASessionLog_NonUnicode.[enddatetime] as [End],[attributexml] as [AttributeXml] FROM [BPASessionLog_NonUnicode] WHERE sessionnumber=@0 ORDER BY logid ASC", session.SessionNumber);
        }

        public BPLogEntry GetLogEntry(int logEntryId)
        {
            return _database.Single<BPLogEntry>("SELECT [logid] as LogId, BPASessionLog_NonUnicode.[sessionnumber] as SessionNumber,[stageid] as StageId,[stagename] as StageName,[stagetype] as StageType,[processname] as ProcessName,[pagename] as PageName,[objectname] as ObjectName,[actionname] as ActionName,[result] as Result,[resulttype] as ResultType, BPASessionLog_NonUnicode.[startdatetime] as [Start], BPASessionLog_NonUnicode.[enddatetime] as [End],[attributexml] as [AttributeXml] FROM [BPASessionLog_NonUnicode] WHERE logid = @0;", logEntryId);
        }
    }
}
