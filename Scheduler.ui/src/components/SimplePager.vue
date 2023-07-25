<template>
  <div class="row">
    <ul class="pagination col-8" v-if="totalPages > 0">
      <li
        class="paginate_button page-item"
        v-bind:class="{ disabled: currentPage == 1 }"
      >
        <a href="#" v-on:click="changePage(1, $event)" class="page-link">
          <font-awesome-icon icon="fas fa-backward-fast"/>
        </a>
      </li>
      <li
        class="paginate_button page-item"
        v-bind:class="{ disabled: currentPage == 1 }"
      >
        <a
          href="#"
          v-on:click="changePage(currentPage - 1, $event)"
          class="page-link"
          ><font-awesome-icon icon="fas fa-backward"/>
        </a>
      </li>
      <li class="paginate_button disabled" v-if="start > 1">
        <a class="page-link">...</a>
      </li>
      <li
        class="paginate_button page-item"
        v-for="n in pages"
        v-bind:key="n"
        v-bind:class="{ active: n == currentPage }"
      >
        <a href="#" v-on:click="changePage(n, $event)" class="page-link">{{
          n
        }}</a>
      </li>
      <li class="paginate_button disabled" v-if="stop < totalPages">
        <a class="page-link">...</a>
      </li>
      <li
        class="paginate_button page-item"
        v-bind:class="{ disabled: currentPage == totalPages }"
      >
        <a
          href="#"
          v-on:click="changePage(currentPage + 1, $event)"
          class="page-link"
          ><font-awesome-icon icon="fas fa-forward"/>
        </a>
      </li>
      <li
        class="paginate_button page-item"
        v-bind:class="{ disabled: currentPage == totalPages }"
      >
        <a
          href="#"
          v-on:click="changePage(totalPages, $event)"
          class="page-link"
          ><font-awesome-icon icon="fas fa-forward-fast"/>
        </a>
      </li>
    </ul>
    <div class="col-4">
      <form class="form" v-on:submit="changePage(parseInt(gotoPage),$event)">
        <div class="row">
          <div class="col-4">
            <input
              type="text"
              name="page"
              v-model="gotoPage"
              class="form-control"
              placeholder="#"                
            />
          </div>
          <div class="col-auto">
            <span class="form-text">/ {{ totalPages }}</span>
          </div>
        </div>
      </form>
    </div>
</div>
</template>
<script>
export default {
  props: {
    currentPage: {
      type: Number,
      default: 0,
    },
    totalPages: {
      type: Number,
      default: 0,
    },
  },
  data: function () {
    return {
      start: 0,
      stop: 0,
      pages: [],
      gotoPage: ""
    };
  },
  mounted() {
    this.computeRange();
  },
  methods: {   
    changePage(n, $event) {
      this.$emit("change-page", n);
      $event.preventDefault();
    },
    computeRange() {
      this.stop = Math.min(this.totalPages, this.currentPage + 5);
      this.start = Math.max(1, this.stop - 10);
      
      if (this.totalPages < 11 || this.currentPage < 6) {
        this.start = 1;
        this.stop = Math.min(this.totalPages, 10);
      }
      var res = [];

      for (var i = this.start; i <= this.stop; i++) {
        res.push(i);
      }
      this.pages = res;
      /*if (currentPage >= 5) {
        start = Math.max(1, currentPage - 5);
        stop = Math.min(this.totalPages, currentPAge + 5);
      }*/
    },
  },
  watch: {
    currentPage(newVal, oldVal) {
      this.computeRange();
    },
    totalPages(newVal, oldVal) {
      this.computeRange();
    },
  },
};
</script>
<style>
</style>