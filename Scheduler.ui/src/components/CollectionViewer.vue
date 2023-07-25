<template>
  <div>
    <b-modal
      id="parameter-modal"
      @hidden="resetModal"
      scrollable
      :title="parameter"
    >
      <div class="text-center" v-if="loading">
        <b-spinner label="Spinning"></b-spinner>
      </div>
      <table class="table table-bordered table-striped" v-if="!loading">
        <thead>
          <tr>
            <th v-for="(field, index) in fields" v-bind:key="index">
              {{ field }}
            </th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(fields, idx1) in rows" v-bind:key="idx1">
            <td v-for="(val, idx2) in fields" v-bind:key="idx2">
              {{ val }}
            </td>
          </tr>
        </tbody>
      </table>
    </b-modal>
  </div>
</template>
<script>
import sessionLogService from "../services/sessionlogservice";
export default {
  name: "CollectionViewer",
  data() {
    return {
      loading: false,
      fields: [],
      rows: [],
    };
  },
  props: {
    logId: {
      type: Number,
      default: 0,
    },
    direction: {
      type: String,
      default: "",
    },
    parameter: {
      type: String,
      default: "",
    },
  },
  watch: {
    logId(newVal, oldVal) {
      this.loading = false;
      if (newVal != 0 && newVal != this.visiblelogId) {
        this.$bvModal.show("parameter-modal");
        this.loading = true;
        sessionLogService
          .getCollection(newVal, this.direction, this.parameter)
          .then((a) => {
            this.fields = a.data.Fields;
            this.rows = a.data.Rows;
          })
          .finally(() => (this.loading = false));
      }
    },
  },
  mounted() {},
  methods: {
    resetModal() {
      this.$emit("modalClose");
      this.fields = [];
      this.rows = [];
      this.loading = false;
    },
  },
};
</script>
<style>
.modal-dialog {
  max-width: 95% !important;
}
</style>