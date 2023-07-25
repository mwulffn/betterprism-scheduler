<template>
  <div class="d-none d-sm-inline-block form-inline ml-auto ml-md-3 my-2 my-md-0 mw-100">
    <div class="input-group">
      <select class="form-control" v-model="selectedTask">
        <option value>Schedule now</option>
        <option v-for="item in tasks" v-bind:key="item.TaskId" v-bind:value="item.TaskId">{{item.Name}}</option>
      </select>
      <div class="input-group-append">
        <button
          class="btn btn-sm btn-primary"
          v-bind:disabled="selectedTask == ''"
          v-on:click="submit($event)"
        >+</button>
      </div>
    </div>
  </div>
</template>
<script>
import schedulerservice from "@/schedulerservice.js";
import taskService from "@/services/taskservice.js";
export default {
  data() {
    return {
      selectedTask: "",
      tasks: [],
      timerId: 0
    };
  },
  mounted() {
    schedulerservice.get("Task").then(result => {
      this.tasks = result.data;
    });
    this.timerId = setInterval(() => {
      schedulerservice.get("Task").then(result => {
        this.tasks = result.data;
      });
    }, 30000);
  },
  beforeDestroy() {
    clearInterval(this.timerId);
  },
  methods: {
    submit(e) {
      e.preventDefault();
      taskService.schedule(this.selectedTask,0);
      this.selectedTask = "";
    }
  }
};
</script>