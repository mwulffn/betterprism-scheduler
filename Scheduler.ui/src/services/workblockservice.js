import schedulerservice from "../schedulerservice";

const workblockService = {
    delete(workblock_id) {
        return schedulerservice.delete("Workblock/"+ workblock_id, "");
    },
    inUse(workblock_id) {
        return schedulerservice.get("Workblock/inuse/"+ workblock_id, "");
    }
}

export default workblockService;