import Vue from 'vue'
import App from './App.vue'
import router from './router'
import moment from "moment/moment.js";
import { BootstrapVue, IconsPlugin } from 'bootstrap-vue'
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'
import { library } from '@fortawesome/fontawesome-svg-core'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'
import { faChessKnight } from '@fortawesome/free-regular-svg-icons'
import { faFileLines, faTachometerAlt, faCog, faHistory, faChartLine, faBackward, faBackwardFast, faForward, faForwardFast, faEdit, faCaretLeft, faCaretRight, faCircle, faPause, faDisplay, faCaretDown, faCheck } from '@fortawesome/free-solid-svg-icons'

/* add icons to the library */
library.add([faChessKnight, faFileLines, faTachometerAlt, faCog, faHistory, faChartLine, faBackward, faBackwardFast, faForward, faForwardFast, faEdit, faCaretLeft, faCaretRight, faCircle, faPause, faDisplay, faCaretDown, faCheck]);

/* add font awesome icon component */
Vue.component('font-awesome-icon', FontAwesomeIcon);
Vue.config.productionTip = false

Vue.filter('shortdate', function (date) {
  if (date == null) return "";

  return moment(date).format("DD-MM-YY HH:mm");
});

Vue.filter('humanize', function (date) {
  if (date === null)
    return '';

  if (moment(date) < new Date()) return "-";

  return moment(date).fromNow();
});

// Make BootstrapVue available throughout your project
Vue.use(BootstrapVue)
// Optionally install the BootstrapVue icon components plugin
Vue.use(IconsPlugin)

new Vue({
  router,
  render: h => h(App)
}).$mount('#app')