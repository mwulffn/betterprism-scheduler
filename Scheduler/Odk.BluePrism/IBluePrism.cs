using PetaPoco;
using System;
using System.Collections.Generic;

namespace Odk.BluePrism
{
    public interface IBluePrism
    {
        bool HasAvailableResources();
        BPResource AllocateResource();
        void FreeResource(BPResource resource);
        BPResourceStatus ResourceStatus(BPResource resource);
        BPResource GetResource(Guid resourceId);
        IEnumerable<BPResource> AllResources();
        BPSessionStatus SessionStatus(Guid sessionId);
        void RequestStop(Guid sessionId);
        Guid LaunchSession(BPProcess process, BPResource resource,string parameters);
        IEnumerable<BPProcess> ListProcesses();
        BPProcess GetProcess(Guid processId);
        int PendingItems(BPWorkqueue queue, bool includePausedQueue = false);
        IEnumerable<BPWorkqueue> ListWorkqueues();
        BPWorkqueue GetWorkqueue(Guid workqueueId);
        IEnumerable<BPSession> LatestSessions(int count);
        BPSession GetSession(Guid sessionId);
        BPLogEntry GetLastLogEntry(Guid sessionId);
        Page<BPLogEntry> GetLogPage(Guid sessionId, int page, int pageSize);
        BPLogEntry GetLogEntry(int logEntryId);
    }
}