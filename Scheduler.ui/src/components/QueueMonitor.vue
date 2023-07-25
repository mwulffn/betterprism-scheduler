<template>
        <div class="card mb-4">
          <div class="card-header d-flex flex-row ">
            <h6 class="m-0 font-weight-bold text-primary">Queue Monitor</h6>
          </div>
          <div class="card-body">
            <table class="table table-sm table-striped table-bordered">
              <thead>
                <tr>
                  <th>Queue</th>
                  <th>Pending</th>
                  <th>Handled by</th>
                  <th>Max workers</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="item in queues" v-bind:key="item.Id">
                  <td>
                    <span v-if="item.Running == 0">
                      <font-awesome-icon icon="fas fa-pause" /> 
                    </span>
                    {{item.Name}}
                  </td>
                  <td>{{item.Pending}}</td>
                  <td>{{item.TaskName}}</td>
                  <td>{{item.ScaleLimit}}</td>
                </tr>
                <tr v-if="queues.length == 0">
                    <td colspan="4">No active queues</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
</template>
<script>
import schedulerservice from "@/schedulerservice";
export default {
  name: "QueueMonitor",
  data() {
    return {
      queues: []
    };
  },
  props: {},
  methods: {
      updateViews() {
          schedulerservice.get("Workqueue/ActiveQueues").then(response => {
              this.queues = response.data.filter(a => a.Pending > 0);
          });
      }
  },
  mounted() {
    this.updateViews();
    this.timerId = setInterval(() => {
      this.updateViews();
    }, 2000);
  },
  beforeDestroy() {
    clearInterval(this.timerId);
  }
}
</script>