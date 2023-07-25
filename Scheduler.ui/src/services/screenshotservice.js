import schedulerservice from "../schedulerservice";

const screenshotService = {
    async sessiongroups(filter) {
        const response = await schedulerservice
        .get("screenshot/sessiongroups?filter=" + encodeURI(filter), "");

        return response.data;
    },
    async screenshots(sessionId) {
        const response = await schedulerservice
        .get("screenshot/screenshotsForSession/" + encodeURI(sessionId), "");

        return response.data;

    },
    thumbnail(screenshotId) {
        return schedulerservice.baseURL + "screenshot/thumbnail/" + screenshotId;
    },
    full(screenshotId) {
        return schedulerservice.baseURL + "screenshot/full/" + screenshotId;
    }
}

export default screenshotService;