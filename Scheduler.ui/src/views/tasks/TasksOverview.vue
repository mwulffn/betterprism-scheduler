<template>
  <div>
    <div class="float-right">
      <form class="form-inline">
        <input type="text" class="form-control mr-sm-2" placeholder="Search..." v-model="filter" /> <router-link :to="{ name: 'tasks-new' }" class="btn btn-primary">New task</router-link>
      </form>
    </div>
    <h1 class="h3 mb-4 text-gray-800">Tasks</h1>
    <task-list title="Cron" :tasks="cron" :filter="filter"></task-list>
    <task-list title="Queue" :tasks="queue" :filter="filter"></task-list>
    <task-list title="At" :tasks="at" :filter="filter"></task-list>
    <task-list title="No trigger" :tasks="notrigger" :filter="filter" v-if="notrigger.length > 0"></task-list>
  </div>
</template>
<script>
import workqueueService from "../../services/workqueueservice";
import taskService from "../../services/taskservice";
import TaskList from '../../components/TaskList.vue';
export default {
  components: { TaskList },
  name: "TasksOverview",
  data() {
    return {
      cron: [],
      at: [],
      queue: [],
      notrigger: [],
      filter: ""
    };
  },
  async mounted() {
    //Prime the workqueue cache
    await workqueueService.workqueues()

    const tasks = await taskService.tasks()

    this.notrigger = tasks.filter(a => a.Trigger == 0);
    this.cron = tasks.filter(a => a.Trigger == 1);
    this.at = tasks.filter(a => a.Trigger == 2);
    this.queue = tasks.filter(a => a.Trigger == 3);
  }
};
</script>