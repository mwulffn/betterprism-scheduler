<template>
  <div class="card mb-4">
    <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
      <h6 class="m-0 font-weight-bold text-primary">BP Sessions</h6>
    </div>
    <div class="card-body">
      <table class="table table-sm table-striped table-bordered">
        <thead>
          <tr>
            <th>Id</th>
            <th>Process</th>
            <th>Resource</th>
            <th>User</th>
            <th>Status</th>
            <th>Start Time</th>
            <th>End Time</th>
          </tr>
        </thead>
        <tbody>
          <tr
            v-bind:class="{ 'table-danger': item.SessionStatusName== 'Terminated', 'table-warning': item.SessionStatusName== 'Warning' }"
            v-for="item in sessions"
            v-bind:key="item.Id"
          >
            <td>{{item.SessionNumber}}</td>
            <td>{{item.ProcessName}}</td>
            <td>{{item.Resource}}</td>
            <td>{{item.User}}</td>
            <td>{{item.SessionStatusName}}</td>
            <td>{{item.Start | shortdate}}</td>
            <td>{{item.End | shortdate}}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>
<script>
import schedulerservice from "@/schedulerservice";
export default { 
  name: "SessionList",
  data() {
    return {
      sessions: []
    };
  },
  props: {},
  methods: {
    updateViews() {
      schedulerservice.get("BPASession").then(response => {
          this.sessions = response.data;
      });
    }
  },
  mounted() {
    this.updateViews();
    this.timerId = setInterval(() => {
      this.updateViews();
    }, 4000);
  },
  beforeDestroy() {
    clearInterval(this.timerId);
  }
};
</script>