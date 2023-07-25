<template>
  <div>
    <simple-pager
      v-if="currentPage"
      :current-page="currentPage.CurrentPage"
      :total-pages="currentPage.TotalPages"
      v-on:change-page="setPage($event)"
    />
    <table
      class="table table-bordered table-responsive table-hover"
      v-if="currentPage && currentPage.Items.length > 0"
    >
      <thead>
        <tr>
          <th class="col-sm-2">Page</th>
          <th class="col-sm-2">Stage</th>
          <th class="col-sm-4">In</th>
          <th class="col-sm-4">Out</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item in currentPage.Items" v-bind:key="item.LogId">
          <td>{{ item.PageName }}</td>
          <td>{{ item.StageName }}</td>
          <td>
            <parameter-list
              :items="item.Inputs"
              :log-id="item.LogId"
              v-on:showLog="showLog($event, 'input')"
              v-if="item.Inputs.length > 0"
            />
          </td>
          <td>
            <div>{{ item.Result }}</div>
            <parameter-list
              :items="item.Outputs"
              :log-id="item.LogId"
              v-on:showLog="showLog($event, 'output')"
              v-if="item.Outputs.length > 0"
            />
          </td>
        </tr>
      </tbody>
    </table>
    <collection-viewer
      :log-id="shownLogId"
      :parameter="parameterName"
      :direction="direction"
      v-on:modalClose="shownLogId = 0"
    />
    <simple-pager
      v-if="currentPage"
      :current-page="currentPage.CurrentPage"
      :total-pages="currentPage.TotalPages"
      v-on:change-page="setPage($event)"
    />
  </div>
</template>
<script>
import sessionLogService from "../services/sessionlogservice";
import simplePager from "./SimplePager";
import ParameterList from "./ParameterList.vue";
import CollectionViewer from "./CollectionViewer.vue";
export default {
  name: "SessionLogList",
  components: { simplePager, ParameterList, CollectionViewer },
  data() {
    return {
      currentPage: null,
      shownLogId: 0,
      parameterName: "",
      direction: "",
    };
  },
  props: {
    id: {
      type: String,
      default: "",
    },
    page: {
      type: Number,
      default: 1,
    },
  },
  mounted() {
    this.loadPage(this.page);
  },
  methods: {
    showLog($event, direction) {
      this.shownLogId = 0;
      this.parameterName = $event.name;
      this.direction = direction;
      this.shownLogId = $event.logId;
    },
    setPage(pageno) {
      this.$router.push({
        name: "log-viewer",
        params: { page: "" + pageno },
      });
    },
    loadPage(pageno) {
      sessionLogService.getPage(this.id, pageno).then((result) => {
        if (result.data.CurrentPage > result.data.TotalPages) {
          this.$router.push({
            name: "log-viewer",
            params: { page: "" + result.data.TotalPages },
          });
          return;
        }
        this.currentPage = result.data;
      });
    },
  },
  watch: {
    page(newVal, oldVal) {
      this.loadPage(newVal);
    },
  },
};
</script>
<style>
</style>