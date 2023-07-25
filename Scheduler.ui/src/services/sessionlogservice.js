import schedulerservice from "../schedulerservice";

const sessionLogService = {
    getPage(id, page) {
        return schedulerservice.get("SessionLog/" + id + "/Page?page=" + page)
    },
    getCollection(logId, direction, name) {
        return schedulerservice.get("SessionLog/" + logId + "/parameter/?direction=" + direction + "&parameter=" + name);
    }
}

export default sessionLogService;