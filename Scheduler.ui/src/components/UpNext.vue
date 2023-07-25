<template>
        <div class="card mb-4">
          <div class="card-header d-flex flex-row ">
            <h6 class="m-0 font-weight-bold text-primary">Up next</h6>
          </div>
          <div class="card-body">
            <table class="table table-sm table-striped table-bordered">
              <thead>
                <tr>
                  <th>Starts</th>
                  <th>Task</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(item,$index) in upNext" v-bind:key="$index">
                  <td>{{item.ExecutionTime | shortdate}}</td>
                  <td>{{item.TaskName}}</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
</template>
<script>
import schedulerservice from "@/schedulerservice";
export default {
    name: "UpNext",
    data() {
        return {
            upNext: []
        };
    },
    methods: {
        updateViews() {
            schedulerservice.get("Dashboard/UpNext").then(response => {
                this.upNext = response.data;
            });
        }
    },
    mounted() {
        this.updateViews();

        this.timerId = setInterval(() => {
            this.updateViews();
        }, 50000);
    },
    beforeDestroy() {
        clearInterval(this.timerId);
    }
}
</script>