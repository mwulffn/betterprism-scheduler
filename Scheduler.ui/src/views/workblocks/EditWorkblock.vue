<template>
  <div>
    <h1 class="h3 mb-4 text-gray-800">Edit '{{ workblock.Name }}'</h1>
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
                  placeholder="Name"
                  v-model="workblock.Name"
                  required="required"
                />
              </div>
              <div class="form-group">
                <label for="pname">Process</label>
                <input
                  type="text"
                  class="form-control"
                  id="pname"
                  placeholder="Processname"
                  v-model="workblock.ProcessName"
                  required="required"
                  readonly
                />
              </div>
              <div class="form-group">
                <label for="intention">Intention</label>
                <input
                  type="text"
                  class="form-control"
                  id="intention"
                  placeholder="Intention"
                  v-model="intention"
                  required="required"
                  readonly
                />
              </div>
              <div class="form-group">
                <label for="parameters">Parameters</label>
                <input
                  type="text"
                  class="form-control"
                  id="parameters"
                  placeholder="Parameters"
                  v-model="workblock.Parameters"
                />
              </div>
              <div class="form-group">
                <label for="pcd">Post completion delay (PCD)</label>
                <input
                  type="number"
                  class="form-control"
                  id="pcd"
                  placeholder="PCD in seconds"
                  v-model="workblock.PostCompletionDelay"
                />
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
                    :to="{ name: 'workblocks-overview' }"
                    class="btn btn-secondary"
                    >Cancel</router-link
                  >
                </div>
                <div class="col-4 text-right">
                  <button
                    class="btn btn-danger"
                    v-on:click="
                      $event.preventDefault();
                      deleteWorkblock();
                    "
                    v-bind:disabled="inUse"
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
import workblockService from "../../services/workblockservice";
export default {
  name: "EditWorkblock",
  props: {
    id: {
      type: String,
      default: "",
    },
  },
  data() {
    return {
      workblock: {},
      intention: "",
      inUse: true,
    };
  },
  mounted() {
    if (this.id === "") {
      this.$router.push("workblocks-overview");
      return;
    }

    schedulerservice
      .get("Workblock/" + this.id)
      .then((result) => {
        this.workblock = result.data;

        if (this.workblock.Intention == 1) this.intention = "Launch";
        if (this.workblock.Intention == 2) this.intention = "Run";
        if (this.workblock.Intention == 3) this.intention = "Complete";
        if (this.workblock.Intention == 4) this.intention = "Fail";
      })
      .catch(() => {
        this.$router.push({ name: "workblocks-overview" });
      });

    workblockService
      .inUse(this.id)
      .then((result) => (this.inUse = result.data));
  },
  methods: {
    submit(e) {
      if (this.workblock.name == "") {
        return;
      }

      schedulerservice
        .put("Workblock/" + this.workblock.WorkblockId, this.workblock)
        .then(() => {
          this.$router.push({ name: "workblocks-overview" });
        })
        .catch((err) => {
          console.log(err);
          this.$router.push({ name: "workblocks-overview" });
        });

      e.preventDefault();
    },
    deleteWorkblock() {
      if (!confirm("Confirm that you wish to delete this workblock?")) {
        return;
      }

      workblockService
        .delete(this.id)
        .finally(() => this.$router.push({ name: "workblocks-overview" }));
    },
  },
};
</script>