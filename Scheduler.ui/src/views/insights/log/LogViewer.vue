<template>
  <div>
    <div class="card">
      <div class="card-header">
        <h6 class="m-0 font-weight-bold text-primary">Log</h6>
      </div>
      <div class="card-body">
        <session-log-list
          v-bind:id="this.id"
          v-bind:page="parseInt(this.page)"
        />
      </div>
    </div>
    <router-view />
  </div>
</template>
<script>
import SessionLogList from "../../../components/SessionLogList.vue";
export default {
  components: { SessionLogList },
  props: {
    id: {
      type: String,
      default: "",
    },
    page: {
      type: String,
      default: "1",
    },
  },
  mounted() {    
    if(this.page < 1) {
      this.$router.push({ name: "log-viewer", params: { page: "1" } });
    }      
  },
};
</script>
<style>
</style>