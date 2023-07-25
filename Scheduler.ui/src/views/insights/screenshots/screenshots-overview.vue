<template>
  <div>
    <h4>Screenshots</h4>
    <div class="row">
      <div class="col-lg-12">
        <div class="form-group">
          <label for="search-input">Filter Sessions:</label>
          <input type="text" class="form-control" id="search-input" v-model="searchQuery"
            placeholder="Search for sessions" />
        </div>
      </div>
    </div>
    <screenshot-session-list :filter="searchQuery"></screenshot-session-list>
  </div>
</template>
<script>
import ScreenshotSessionList from '../../../components/ScreenshotSessionList.vue';
import screenshotService from "../../../services/screenshotservice";
export default {
  components: { ScreenshotSessionList },
  name: "screenshots-overview",
  data() {
    return {
      searchQuery: "",
      sessions: [],
    };
  },
  methods: {
    resetScreenshots(index) {
      this.sessions[index].screenshots = null;
    },
    async loadScreenshots(index) {      
      const screenshots = await screenshotService.screenshots(this.sessions[index].SessionId);      
      this.sessions[index].screenshots = screenshots;
    },
    async fetchSessions(filter = "") {
      // Replace the line below with a call to your API
      const sessions = await screenshotService.sessiongroups(filter);
      this.sessions = sessions;
    },
  },
  created() {
    this.fetchSessions();
  },
};
</script>