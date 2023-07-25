<template>
  <div class="card mb-4">
    <div class="card-header d-flex flex-row">
      <h6 class="m-0 font-weight-bold text-primary">
        Incidents
        <span
          class="badge"
          v-bind:class="{
            'badge-success': incidents.length == 0,
            'badge-danger': incidents.length > 0,
          }"
          >{{ incidents.length }}</span>
      </h6>
    </div>
    <!--<div class="card-body" v-if="incidents.length > 0">-->
    <table
      class="card-table table table-sm table-striped"
      v-if="incidents.length > 0"
    >
      <thead>
        <tr>
          <th>Task</th>
          <th>Resource</th>
          <th>Start</th>
          <th>Exception</th>
          <th>&nbsp;</th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="item in incidents"
          v-bind:key="item.Incident.SessionIncidentId"
        >
          <td>
            <div>{{ item.Task }}</div>
            <small>{{ item.ProcessName }}</small>
          </td>
          <td>{{ item.Resource }}</td>
          <td>{{ item.Incident.Created | shortdate }}</td>
          <td>
            <div>
              {{ item.BPLogEntry.StageName }}
              <span v-if="item.BPLogEntry.ActionName !== null">
                / {{ item.BPLogEntry.ActionName }}</span
              >
            </div>
            {{ item.BPLogEntry.Result }}
          </td>
          <td class="text-center">
            <div class="dropdown">
              <button
                class="btn btn-outline-primary btn-sm dropdown-toggle"
                type="button"
                :id="item.Incident.SessionIncidentId"
                data-toggle="dropdown"
                aria-haspopup="true"
                aria-expanded="false"
              >
                <i class="fas fa-solid fa-hammer"></i>
              </button>
              <div
                class="dropdown-menu"
                :aria-labelledby="item.Incident.SessionIncidentId"
              >
                <a
                  class="dropdown-item"
                  href="#"
                  @click="
                    reschedule(item, 0);
                    return false;
                  "
                  >Reschedule now</a
                >
                <a
                  class="dropdown-item"
                  href="#"
                  @click="
                    reschedule(item, 2);
                    return false;
                  "
                  >Reschedule +2 hours</a
                >
                <a
                  class="dropdown-item"
                  href="#"
                  @click="
                    disable(item);
                    return false;
                  "
                  >Disable task</a
                >
                <a
                  class="dropdown-item"
                  href="#"
                  @click="
                    log(item);
                    return false;
                  "
                  >Show log</a
                >
                <a
                  class="dropdown-item"
                  href="#"
                  @click="
                    dismiss(item);
                    return false;
                  "
                  >Dismiss</a
                >
                <a
                  class="dropdown-item"
                  href="#"
                  @click="
                    dismissSimilar(item);
                    return false;
                  "
                  >Dismiss similar</a
                >
              </div>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
    <!--  </div>-->
  </div>
</template>
<script>
//import schedulerservice from "@/schedulerservice";
import sessionService from "@/services/sessionservice.js";
import taskService from "@/services/taskservice.js";
export default {
  name: "SessionMonitor",
  data() {
    return {
      incidents: [],
    };
  },
  props: {},
  methods: {
    reschedule(item, delay) {
      taskService.schedule(item.TaskId, delay).then(() => {
        sessionService
          .resolve(item, delay == 0 ? 2 : 3)
          .then(() => this.updateViews());
      });
    },
    dismiss(item) {
      sessionService.resolve(item, 1).then(() => this.updateViews());
    },
    dismissSimilar(item) {
      sessionService.resolveSimilar(item).then(() => this.updateViews());
    },
    disable(item) {
      taskService
        .setEnabled(item.TaskId, false)
        .then(() => sessionService.resolve(item, 4));
    },
    log(item) {
      this.$router.push({
        name: "log-viewer",
        params: { page: "1000000",id: item.Incident.BPSessionId },
      });
    },
    updateViews() {
      sessionService.listPending().then((response) => {
        this.incidents = response.data;
      });
    },
  },
  mounted() {
    this.updateViews();
    this.timerId = setInterval(() => {
      this.updateViews();
    }, 2000);
  },
  beforeDestroy() {
    clearInterval(this.timerId);
  },
};
</script>