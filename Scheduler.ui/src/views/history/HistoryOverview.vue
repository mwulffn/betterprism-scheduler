<template>
  <div>
    <h1 class="h3 mb-4 text-gray-800">History</h1>
    <div class="row">
      <div class="col-sm-12">
        <div class="card mb-4">
          <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
            <h6 class="m-0 font-weight-bold text-primary">Sessions</h6>
          </div>
          <div class="card-body">
            <table class="table table-sm thead-dark table-striped">
              <thead>
                <tr>
                  <th>Task</th>
                  <th>Status</th>
                  <th>Run</th>
                  <th>Created</th>
                  <th>Dispatched</th>
                  <th>Closed</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="item in sessions" v-bind:key="item.Id" v-bind:class="{ 'table-danger': item.State== 6, 'table-warning': item.State == 5 }">
                    <td>{{tasks[item.TaskId]}}</td>
                    <td>{{status[item.State]}}</td>
                    <td>{{processes[item.Run]}}</td>
                    <td>{{item.Created | shortdate}}</td>
                    <td>{{item.Dispatched | shortdate}}</td>
                    <td>{{item.Closed | shortdate}}</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
    <router-view />
  </div>
</template>
<script>
import schedulerservice from "@/schedulerservice";
import moment from "moment/moment.js";
export default {
  components: {},
  data() {
    return {
      sessions: [],
      tasks: {},
      processes: {},
      status: {
          0: 'Ready',
          1: 'Launching',
          2: 'Running',
          3: 'Completing',
          4: 'Completed',
          5: 'Failing',
          6: 'Failed'
      }
    };
  },
  filters: {
    shortdate(date) {
      if(date == null){
        return "-";
      }

      return moment(date).format("DD-MM-YY HH:mm");
    },
  },
  mounted() {
    schedulerservice.get("Task").then(result => {
        for(var i = 0; i < result.data.length; i++) {
          this.tasks[result.data[i].TaskId] = result.data[i].Name;
        }            

        schedulerservice.get("Process").then(result => {
            for(var i = 0; i < result.data.length; i++) {
              this.processes[result.data[i].Id] = result.data[i].Name;
            }                

            schedulerservice.get("History").then(result => {
                this.sessions = result.data;
            });
        });
    });
  }
};
</script>