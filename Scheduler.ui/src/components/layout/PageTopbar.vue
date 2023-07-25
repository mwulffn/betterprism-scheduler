<template>  
  <nav class="navbar navbar-expand navbar-light bg-white topbar mb-4 static-top shadow">    
    <button id="sidebarToggleTop" class="btn btn-link d-md-none rounded-circle mr-3">
      <i class="fa fa-bars"></i>
    </button>    
    <ul class="navbar-nav ml-auto">
      <div class="topbar-divider d-none d-sm-block"></div>
    </ul>
    <QuickDispatcher />
  </nav>  
</template>
<script>
import QuickDispatcher from "@/components/QuickDispatcher.vue";
export default {
  components: {
    QuickDispatcher: QuickDispatcher
  },
  name: "PageTopbar"
};
</script>