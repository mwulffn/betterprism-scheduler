<template>
  <div>
    <div class=" clearfix">
    <div class="float-right">
      <router-link :to="{ name: 'workblocks-new' }" class="btn btn-primary my-3">New workblock</router-link>
    </div>
    <h1 class="h3 mb-4 text-gray-800">Workblocks</h1>
  </div>
    <div class="row" >
      <div class="col-sm-12">
        <workblock-list title="Run blocks" v-bind:workblocks="run"></workblock-list>
        <workblock-list title="Launch blocks" v-bind:workblocks="launch"></workblock-list>
        <workblock-list title="Completion blocks" v-bind:workblocks="complete"></workblock-list>
        <workblock-list title="Failing blocks" v-bind:workblocks="fail"></workblock-list>
      </div>
    </div>
    <router-view />
  </div>
</template>
<script>
import schedulerservice from "@/schedulerservice";
import WorkblockList from "@/components/Workblocklist.vue";
export default {
  components: {
    WorkblockList: WorkblockList
  },
  data() {
    return {
      workblocks: [],
      launch: [],
      run: [],
      complete: [],
      fail: []
    };
  },
  mounted() {
    schedulerservice.get("Workblock").then(result => {
      this.workblocks = result.data;
      this.launch = this.workblocks.filter(a => a.Intention == 1);
      this.run = this.workblocks.filter(a => a.Intention == 2);
      this.complete = this.workblocks.filter(a => a.Intention == 3);
      this.fail = this.workblocks.filter(a => a.Intention == 4);
    });
  }
};
</script>