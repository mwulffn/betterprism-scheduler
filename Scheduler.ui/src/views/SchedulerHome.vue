<template>
  <div>
    <h1 class="h3 mb-4 text-gray-800">Dashboard</h1>
    <activity-list />
    <session-monitor />
    <div class="row">
      <div class="col-lg-5 col-sm-12">
        <up-next />
      </div>
      <div class="col-lg-7 col-sm-12">
        <queue-monitor />
      </div>
    </div>
    <session-list />
  </div>
</template>
<script>
import ActivityList from '../components/Activitylist.vue';
import QueueMonitor from '../components/QueueMonitor.vue';
import SessionList from '../components/Sessionlist.vue';
import SessionMonitor from '../components/SessionMonitor.vue';
import UpNext from '../components/UpNext.vue';
export default {
  name: "SchedulerHome",
  data() {
    return {
      logevents: [],
    };
  },
  components: {
    ActivityList,
    SessionList,
    QueueMonitor,
    UpNext,
    SessionMonitor
  },
  methods: {},
  mounted() {},
  beforeDestroy() {}
};
</script>
<style>
</style>