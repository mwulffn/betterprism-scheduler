<template>
  <div>
    <h1 class="h3 mb-4 text-gray-800">Edit '{{ task.Name }}'</h1>
    <div class="row">
      <div class="col-sm-12">
        <div class="card">
          <div class="card-body">
            <form>
              <div class="form-group">
                <label for="taskName">Name</label>
                <input
                  type="text"
                  class="form-control"
                  id="taskName"
                  placeholder="Task name"
                  v-model="task.Name"
                  required="required"
                />
              </div>
              <div class="form-group">
                <div class="form-check">
                  <input
                    class="form-check-input"
                    type="checkbox"
                    id="enabled"
                    v-model="task.Enabled"
                  />
                  <label class="form-check-label" for="enabled">
                    Enabled
                  </label>
                </div>
              </div>
              <div class="form-group">
                <label for="triggerSelect">Select trigger</label>
                <select
                  class="form-control"
                  v-model="task.Trigger"
                  id="triggerSelect"
                >
                  <option
                    v-for="option in trigger_options"
                    v-bind:key="option.value"
                    v-bind:value="parseInt(option.value)"
                  >
                    {{ option.text }}
                  </option>
                </select>
              </div>
              <!-- Cron group -->
              <div class="form-group" v-if="task.Trigger == 1">
                <label for="cron">Cron pattern</label>
                <input
                  type="text"
                  id="cron"
                  v-model="task.Cron"
                  class="form-control"
                />
                <div class="py-1">
                  <button
                    type="button"
                    class="btn btn-light btn-sm"
                    v-on:click="cronHelp('0 * * * *', $event)"
                  >
                    Hourly
                  </button>
                  <button
                    type="button"
                    class="btn btn-light btn-sm"
                    v-on:click="cronHelp('0 12 * * *', $event)"
                  >
                    Daily
                  </button>
                  <button
                    type="button"
                    class="btn btn-light btn-sm"
                    v-on:click="cronHelp('0 12 * * 1', $event)"
                  >
                    Weekly
                  </button>
                  <button
                    type="button"
                    class="btn btn-light btn-sm"
                    v-on:click="cronHelp('0 12 1 * *', $event)"
                  >
                    Monthly
                  </button>
                  <button
                    type="button"
                    class="btn btn-light btn-sm"
                    v-on:click="cronHelp('0 12 1 1 *', $event)"
                  >
                    Yearly
                  </button>
                </div>
              </div>
              <!-- At dates -->
              <div class="form-group" v-if="task.Trigger == 2">
                <label>Dates</label>
                <ul class="list-group">
                  <li
                    class="list-group-item"
                    v-for="item in atDates"
                    v-bind:key="item.Id"
                  >
                    {{ item.At }}
                    <button
                      class="btn btn-danger btn-small"
                      v-on:click="deleteAtDate(item)"
                    >
                      x
                    </button>
                  </li>
                </ul>
                <div class="input-group mb-3">
                  <input
                    type="datetime-local"
                    v-model="atDate"
                    class="form-control"
                  />
                  <div class="input-group-append">
                    <button
                      class="btn btn-success"
                      :disabled="atDate == ''"
                      v-on:click="addAtDate()"
                    >
                      +
                    </button>
                  </div>
                </div>
              </div>
              <!-- Workqueues -->
              <div v-if="task.Trigger == 3">
                <div class="form-group">
                  <label for="queueSelect">Select workqueue</label>
                  <select
                    class="form-control"
                    v-model="task.Workqueue"
                    id="queueSelect"
                  >
                    <option
                      v-for="option in workqueues"
                      v-bind:key="option.Id"
                      v-bind:value="option.Id"
                    >
                      {{ option.Name }}
                    </option>
                  </select>
                </div>
                <div class="form-group">
                  <label for="scaleLimit">Scalelimit</label>
                  <input
                    type="number"
                    class="form-control"
                    id="scaleLimit"
                    placeholder="Scaling limit (1 at least)"
                    v-model="task.ScaleLimit"
                    required="required"
                  />
                </div>
                <div class="form-group">
                  <label for="scaleThreshold">Scalethreshold</label>
                  <input
                    type="number"
                    class="form-control"
                    id="scaleThreshold"
                    placeholder="Scaling threshold (1 at least)"
                    v-model="task.ScaleThreshold"
                    required="required"
                  />
                </div>
              </div>
              <hr />
              <div class="form-group">
                <label for="launchSelect">Launch</label>
                <select
                  class="form-control"
                  v-model="task.Launch"
                  id="launchSelect"
                >
                  <option value="">-- None</option>
                  <option
                    v-for="option in all_workblocks.filter(
                      (a) => a.Intention == 1
                    )"
                    v-bind:key="option.WorkblockId"
                    v-bind:value="option.WorkblockId"
                  >
                    {{ option.Name }}
                  </option>
                </select>
              </div>
              <div class="form-group">
                <label for="runSelect">Run</label>
                <select class="form-control" v-model="task.Run" id="runSelect">
                  <option value="">-- None</option>
                  <option
                    v-for="option in all_workblocks.filter(
                      (a) => a.Intention == 2
                    )"
                    v-bind:key="option.WorkblockId"
                    v-bind:value="option.WorkblockId"
                  >
                    {{ option.Name }}
                  </option>
                </select>
              </div>
              <div class="form-group">
                <label for="completeSelect">Complete</label>
                <select
                  class="form-control"
                  v-model="task.Complete"
                  id="completeSelect"
                >
                  <option value="">-- None</option>
                  <option
                    v-for="option in all_workblocks.filter(
                      (a) => a.Intention == 3
                    )"
                    v-bind:key="option.WorkblockId"
                    v-bind:value="option.WorkblockId"
                  >
                    {{ option.Name }}
                  </option>
                </select>
              </div>
              <div class="form-group">
                <label for="failSelect">Fail</label>
                <select
                  class="form-control"
                  v-model="task.Fail"
                  id="failSelect"
                >
                  <option value="">-- None</option>
                  <option
                    v-for="option in all_workblocks.filter(
                      (a) => a.Intention == 4
                    )"
                    v-bind:key="option.WorkblockId"
                    v-bind:value="option.WorkblockId"
                  >
                    {{ option.Name }}
                  </option>
                </select>
              </div>
              <div class="row">
                <div class="col-8">
                  <input
                    type="submit"
                    class="btn btn-primary"
                    value="Save"
                    v-on:click="submit($event)"
                  />
                  &nbsp;
                  <router-link
                    :to="{ name: 'tasks-overview' }"
                    class="btn btn-secondary"
                    >Cancel</router-link
                  >
                </div>
                <div class="col-4 text-right">
                  <button
                    class="btn btn-danger"
                    value="delete"
                    v-on:click="delete_task($event)"
                  >
                    Delete
                  </button>
                </div>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import schedulerservice from "@/schedulerservice";
import taskService from "../../services/taskservice";

export default {
  name: "EditTask",
  data() {
    return {
      atDate: "",
      atDates: [],
      task: {},
      ready: false,
      workblocks: [],
      trigger_options: [
        { text: "Cron", value: "1" },
        { text: "At", value: "2" },
        { text: "Queue", value: "3" },
      ],
      workqueues: [],
      all_workblocks: [],
    };
  },
  props: {
    id: {
      type: String,
      default: "",
    },
  },
  mounted() {
    if (this.id === "") {
      this.$router.push("tasks-overview");
      return;
    }

    schedulerservice
      .get("Workqueue")
      .then((result) => (this.workqueues = result.data));

    schedulerservice
      .get("Workblock")
      .then((result) => (this.all_workblocks = result.data));

    schedulerservice
      .get("Task/" + this.id)
      .then((result) => {
        this.task = result.data;
      })
      .catch(() => {
        this.$router.push({ name: "tasks-overview" });
      });

    this.refreshDates();
  },
  methods: {
    cronHelp(value, e) {
      this.task.Cron = value;
      e.preventDefault();
    },
    refreshDates() {
      schedulerservice.get("Task/" + this.id + "/atdates").then((result) => {
        this.atDates = result.data;
      });
    },
    addAtDate() {
      var obj = {
        Id: null,
        At: this.atDate,
        TaskId: this.task.TaskId,
      };

      schedulerservice
        .post("Task/" + this.id + "/addatdate", obj)
        .then((result) => {          
          this.refreshDates();
        });
    },
    deleteAtDate(date) {
      schedulerservice
        .delete("Task/" + this.id + "/deleteatdate/" + date.Id)
        .then((result) => {          
          this.refreshDates();
        });
    },
    delete_task(e) {
      e.preventDefault();

      if (!confirm("Are you sure you wish to delete this task?")) {
        return;
      } 

      taskService
        .delete(this.id)
        .finally(() => this.$router.push({ name: "tasks-overview" }));
    },
    submit(e) {
      e.preventDefault();

      schedulerservice
        .put("Task/" + this.id, this.task)
        .then(() => {
          this.$router.push({ name: "tasks-overview" });
        })
        .catch(() => {
          this.$router.push({ name: "tasks-overview" });
        });
    },
  },
};
</script>