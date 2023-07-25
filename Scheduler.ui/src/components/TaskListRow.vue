<template>
    <router-link :to="{ path: '/tasks/' + task.TaskId, params: { id: task.TaskId } }" role="button" tag="tr">
        <td>{{ task.Name }}</td>
        <td v-if="task.Trigger == 0">&nbsp;</td>
        <td v-if="task.Trigger == 1">{{ task.Cron }}</td>
        <td v-if="task.Trigger == 2">{{ atDate | shortdate }}</td>
        <td v-if="task.Trigger == 3">{{ workqueue.Name }}</td>
        <td class="text-center"><font-awesome-icon icon="fas fa-check" v-if="task.Enabled" /></td>
        <td>{{ task.LastTriggered | shortdate }}</td>
    </router-link>
</template>

<script>
import taskService from '../services/taskservice';
import workqueueService from '../services/workqueueservice';
export default {
    name: "TaskListRow",
    data() {
        return {
            workqueue: '',
            atDate: null
        };
    },
    props: {
        task: {
            type: Object,
            default: () => null,
        }
    },
    async mounted() {
        if (this.task.Trigger == 2) {
            const dates = await taskService.atDates(this.task.TaskId);
            if (dates.length > 0) {
                this.atDate = dates[0].At;
            }
        }
        //Load workqueue name
        if (this.task.Trigger == 3) {
            this.workqueue = await workqueueService.workqueue(this.task.Workqueue);
        }
    }

}
</script>
<style>
</style>