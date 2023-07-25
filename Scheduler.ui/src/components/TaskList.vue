<template>
    <div class="card mb-4">
        <div class="card-header d-flex flex-row ">
            <h6 class="m-0 font-weight-bold text-primary">{{ title }}</h6>
        </div>
        <div class="card-body">
            <table class="table table-hover table-bordered table-striped table-sm">
                <thead>
                    <tr>
                        <th style="width: 40%">Task</th>
                        <th style="width: 40%">Settings</th>
                        <th style="width: 10%" class="text-center">Enabled</th>
                        <th style="width: 10%">Last triggered</th>
                    </tr>
                </thead>
                <tbody>
                    <task-list-row :task="item" v-for="item in filteredTasks" v-bind:key="item.TaskId"></task-list-row>
                </tbody>
            </table>
        </div>
    </div>
</template>
<script>
import TaskListRow from './TaskListRow.vue';
export default {
    name: "TaskList",
    components: { TaskListRow },
    props: {
        title: {
            type: String,
            default: ""
        },
        tasks: {
            type: Array,
            default: () => [],
        },
        filter: {
            type: String,
            default: ""
        },
    },
    computed: {
        filteredTasks() {
            if (this.filter == '') {
                return this.tasks;
            }
                
            return this.tasks.filter(a => a.Name.toLowerCase().indexOf(this.filter.toLowerCase()) != -1);
        }
    }
}
</script>
<style>
</style>