import schedulerservice from "../schedulerservice";

const sessionService = {
    listPending() {
        return schedulerservice.get("SessionMonitor")
    },
    resolve(item,resolution) {
        item.Incident.Resolution = resolution;

        return schedulerservice.post("SessionMonitor", item);
    },
    resolveSimilar(item) {
        return schedulerservice.post("SessionMonitor/resolve-similar", item);
    }
}

export default sessionService;