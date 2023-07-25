<template>  
  <ul
    class="navbar-nav bg-gradient-primary sidebar sidebar-dark accordion"
    v-bind:class="[expanded ? '' : 'toggled']"
    id="accordionSidebar"
  >    
    <a
      class="sidebar-brand d-flex align-items-center justify-content-center"
      href="index.html"
    >
      <div class="sidebar-brand-icon">
        <font-awesome-icon icon="fa-regular fa-chess-knight" />
      </div>
      <div class="sidebar-brand-text mx-3">{{ title }}</div>
    </a>    
    <hr class="sidebar-divider my-0" />
    <li
      class="nav-item"
      v-bind:class="[item.path == currentRoute ? 'active' : '']"
      v-for="item in routes"
      v-bind:key="item.path"
    >
      <router-link class="nav-link" :to="item.path" v-if="item.icon" 
        ><font-awesome-icon :icon="item.icon"/> <span>{{ item.name }}</span>
      </router-link>
    </li>
    <li class="nav-item">
      <router-link class="nav-link" to="/insights/screenshots"
        ><font-awesome-icon icon="fas fa-display" /> <span>Screenshots</span>
      </router-link>
    </li>    
    <hr class="sidebar-divider d-none d-md-block" />        
    <div class="text-center d-none d-md-inline">
      <button
        class="rounded-circle border-0"
        id=""
        v-on:click="expanded = !expanded"
      >
        <font-awesome-icon icon="fas fa-caret-left" v-if="expanded" />
        <font-awesome-icon icon="fas fa-caret-right" v-if="!expanded" />
      </button>
    </div>
  </ul>  
</template>
<script>
import { routes } from "@/router/index";
export default {
  name: "PageSidebar",
  watch: {
    $route(to) {
      this.currentRoute = to.path;
    },
  },
  data() {
    return {
      routes: routes,
      currentRoute: this.$route.path,
      expanded: true,
      title: 'Better Scheduler'
    };
  },
  mounted() {
    this.title = process.env.VUE_APP_APP_TITLE;
  },
};
</script>