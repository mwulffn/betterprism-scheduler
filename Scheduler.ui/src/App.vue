<template>
  <div id="wrapper">
    <page-sidebar />
    <div id="content-wrapper" class="d-flex flex-column">
      <div id="content">
        <page-topbar />
        <div class="container-fluid">
          <router-view></router-view>
        </div>
      </div>
      <page-footer />
    </div>
  </div>
</template>
<script>
import '../node_modules/jquery/dist/jquery.min.js';
import '../node_modules/bootstrap/dist/js/bootstrap.min.js';
import PageSidebar from "@/components/layout/PageSidebar.vue";
import PageTopbar from "@/components/layout/PageTopbar.vue";
import PageFooter from "@/components/layout/PageFooter.vue";
export default {
  name: "App",
  components: {
    PageSidebar,
    PageTopbar,
    PageFooter
  }
};
</script>
<style>
@import "~startbootstrap-sb-admin-2/css/sb-admin-2.css";
</style>