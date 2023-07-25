<template>
  <div>
    <screenshot-session :session="session" v-for="session in filteredSessions" v-bind:key="session.SessionId">{{ session.Name }}</screenshot-session>
    <div v-if="filteredSessions.length == 0 && filter !== ''">No results matched the search</div>
  </div>
</template>
<script>
import screenshotService from "../services/screenshotservice";
import ScreenshotSession from './ScreenshotSession.vue';
export default {
  components: { ScreenshotSession },
    name: "screenshot-session-list",
    props: {
        filter: String
    },
    data() {
        return {
            sessions: []
        }
    },
    computed: {
        filteredSessions() {
            if(this.filter == "") {
                return this.sessions;
            }                

            return this.sessions.filter((session,index) => session.Name.includes(this.filter));
        }
    },
    async created() {
        this.sessions = await screenshotService.sessiongroups("");
    }
}
</script>
<style>
</style>