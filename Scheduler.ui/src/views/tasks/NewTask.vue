<template>
  <div>
    <h1 class="h3 mb-4 text-gray-800">New task</h1>
    <div class="row">
      <div class="col-sm-12">
        <div class="card">
          <div class="card-body">
            <form>
              <div class="form-group">
                <label for="taskName">Taskname</label>
                <input
                  type="text"
                  class="form-control"
                  id="taskName"
                  placeholder="enter task name"
                  v-model="name"
                  required="required"
                  tabindex="0"
                />
              </div>
              <input type="submit" class="btn btn-primary" value="Save" v-on:click="submit($event)" /> &nbsp;
              <router-link :to="{ name: 'tasks-overview' }" class="btn btn-secondary">Cancel</router-link>
            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import schedulerservice from "@/schedulerservice";
export default {
  name: "NewTask",
  data() {
    return {
      name: ""
    };
  },
  methods: {
    submit(e) {
      if (this.name == "") {
        return;
      } 

      var obj = {
        Id: "00000000-0000-0000-0000-000000000000",
        Name: this.name,
        Trigger: 0,
        TriggerName: null,
        Cron: null,
        WorkQueue: null,
        ScaleLimit: 1,
        ScaleThreshold: 1
      };

      schedulerservice.post("Task",obj).then(result => {        
        this.$router.push({ name: "tasks-edit", params: { id: result.data.TaskId } });
      }).catch(error => {
          console.log(error);
      });

      e.preventDefault();
    }
  }
};
</script>