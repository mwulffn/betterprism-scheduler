import schedulerservice from "../schedulerservice";

const workqueueService = {
    cache: [],
    async workqueue(id) {
        if (this.cache.length == 0) {
            await this.workqueues();
        }            

        let queue = this.cache.filter(a => a.Id == id);

        return queue.length > 0 ? queue[0] : null;
    },
    async workqueues() {
        const response = await schedulerservice
            .get("workqueue", "");

        this.cache = response.data;

        return response.data;
    },
}

export default workqueueService;