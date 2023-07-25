import schedulerservice from "../schedulerservice";

const taskService = {
    schedule(task_id, delay = 0) {
        return schedulerservice
            .post("Task/" + task_id + "/dispatch?delay=" + delay, "");
    },
    schedule(task_id, delay = 0) {
        return schedulerservice
            .post("Task/" + task_id + "/dispatch?delay=" + delay, "");
    },
    setEnabled(task_id, enabled) {
        return schedulerservice.put("Task/" + task_id + "/setenabled?enabled=" + enabled, "");
    },
    delete(task_id) {
        return schedulerservice.delete("Task/" + task_id, "");
    },
    async atDates(task_id) {
        const result = await schedulerservice.get("Task/" + task_id + "/atdates");

        return result.data;
    },
    async tasks() {
        const result = await schedulerservice.get("Task");

        return result.data;
    }
}

export default taskService;