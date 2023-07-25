<template>
  <div>
    <h1 class="h3 mb-4 text-gray-800">New workblock</h1>
    <div class="row">
      <div class="col-sm-12">
        <div class="card">
          <div class="card-body">
            <form>
              <div class="form-group">
                <label for="name">Name</label>
                <input
                  type="text"
                  class="form-control"
                  id="name"
                  placeholder="Enter workblock name"
                  v-model="workblock.Name"
                  required="required"
                  tabindex="0"
                />
              </div>
              <div class="form-group">
                <label for="intention">
                  Intention
                  <small>(warning: can't be changed)</small>
                </label>
                <select class="form-control" v-model="workblock.Intention" id="intention">
                  <option
                    v-for="option in intentions"
                    v-bind:key="option.value"
                    v-bind:value="option.value"
                  >{{option.text}}</option>
                </select>
              </div>
              <div class="form-group">
                <label for="process">
                  Process
                  <small>(warning: can't be changed)</small>
                </label>
                <select class="form-control" v-model="workblock.ProcessId" id="process" required @change="changeProcess($event)">
                  <option
                    v-for="option in processes"
                    v-bind:key="option.Id"
                    v-bind:value="option.Id"
                  >{{option.Name}}</option>
                </select>
              </div>
              <div class="form-group">
                <label for="parameters">Input parameters</label>
                <input
                  type="text"
                  class="form-control"
                  id="parameters"
                  placeholder="Correct xml or something dies"
                  v-model="workblock.Parameters"
                />
              </div>
              <div class="form-group">
                <label for="pcd">Post completion delay (in seconds)</label>
                <input
                  type="number"
                  class="form-control"
                  id="pcd"
                  placeholder="Numbers"
                  v-model="workblock.PostCompletionDelay"
                />
              </div>
              <input type="submit" class="btn btn-primary" value="Save" v-on:click="submit($event)" /> &nbsp;
              <router-link :to="{ name: 'workblocks-overview' }" class="btn btn-secondary">Cancel</router-link>
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
  name: "NewWorkblock",
  data() {
    return {
      workblock: {
        WorkblockId: "00000000-0000-0000-0000-000000000000",
        ProcessId: "00000000-0000-0000-0000-000000000000",
        Name: "",
        Intention: 1,
        IntentionName: "",
        Parameters: "<inputs></inputs>",
        PostCompletionDelay: 0,
        ProcessName: ""
      },
      intentions: [
        { value: 1, text: "Launch" },
        { value: 2, text: "Run" },
        { value: 3, text: "Complete" },
        { value: 4, text: "Fail" }
      ],
      processes: []
    };
  },
  mounted() {
    schedulerservice.get("Process").then(result => {
      this.processes = result.data;
    });
  },
  methods: {
    changeProcess(event) {      
      if(this.workblock.Name == "") {
        this.workblock.Name = event.target.selectedOptions[0].innerText;
      }        
    },
    submit(e) {
      e.preventDefault();

      if (
        this.workblock.Name == "" ||
        this.workblock.ProcessId == "00000000-0000-0000-0000-000000000000"
      ) {
        return;
      }

      schedulerservice.post("Workblock", this.workblock).then(() => {
        this.$router.push({ name: "workblocks-overview" });

        return;
      });
    }
  }
};
</script>