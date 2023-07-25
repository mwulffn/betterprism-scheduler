<template>
        <div class="card mb-4">
          <div class="card-header d-flex flex-row ">
            <h6 class="m-0 font-weight-bold text-primary">Activity</h6>
          </div>
          <div class="card-body">
            <table class="table table-sm table-striped table-bordered">
              <thead>
                <tr>
                  <th>Resource</th>
                  <th>Task</th>
                  <th>Status</th>
                  <th>Process</th>
                  <th>Master / stop</th>
                  <th>Dispatched</th>
                  <th>Delay</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="item in dashboard" v-bind:key="item.Id">
                  <td>
                    <span v-if="item.Name !== ''">
                      <font-awesome-icon icon="fas fa-circle" v-bind:class="{ 'text-danger': item.DisplayStatus == 'Offline', 'text-success': item.DisplayStatus == 'Working','text-secondary': item.DisplayStatus == 'Logged Out','text-info': item.DisplayStatus == 'Idle' }" />  {{item.Name}}
                    </span>
                  </td>
                  <td>{{item.TaskName}}</td>
                  <td>{{item.SessionState}}</td>
                  <td>{{item.ProcessName}}</td>
                  <td>
                    <span v-if="item.TaskName !==''">{{item.Master ? 'Master' : 'Slave'}} {{item.StopRequested ? '(stop)' : ''}}</span>
                  </td>
                  <td>{{item.Dispatched | shortdate}}</td>
                  <td>{{item.DelayUntil | humanize}}</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
</template>
<script>
import schedulerservice from "@/schedulerservice";
export default {
  name: "ActivityList",
  data() {
    return {
      dashboard: [],
    };
  },
  props: {},
  methods: {
    updateViews() {
      schedulerservice.get("Dashboard").then((response) => {
        var dispatch = response.data;
        schedulerservice.get("Resource").then((response) => {
          var resources = response.data;
          var target = [];
          resources.forEach((a) => {
            var task = dispatch.find((b) => b.ResourceId == a.Id);
            var obj = {
              Id: a.Id,
              Name: a.Name,
              DisplayStatus: a.DisplayStatus,
            };
            if (task !== undefined) {
              obj["TaskName"] = task.TaskName;
              obj["SessionState"] = task.SessionState;
              obj["ProcessName"] = task.ProcessName;
              obj["Master"] = task.Master;
              obj["StopRequested"] = task.StopRequested;
              obj["DelayUntil"] = task.DelayUntil;
              obj["Dispatched"] = task.Dispatched;
            } else {
              obj["TaskName"] = "";
              obj["SessionState"] = "";
              obj["ProcessName"] = "";
              obj["Master"] = false;
              obj["StopRequested"] = false;
              obj["DelayUntil"] = null;
            }
            target.push(obj);
          });
          dispatch.forEach((a) => {
            if (a.ResourceId === "00000000-0000-0000-0000-000000000000") {
              var obj = {
                Id: a.Id,
                Name: "",
                DisplayStatus: "Idle",
                TaskName: a.TaskName,
                SessionState: a.SessionState,
                ProcessName: a.ProcessName,
                Master: a.Master,
                StopRequested: a.StopRequested,
                DelayUntil: a.DelayUntil,
              };
              target.push(obj);
            }
          });
          this.dashboard = target;
        });
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