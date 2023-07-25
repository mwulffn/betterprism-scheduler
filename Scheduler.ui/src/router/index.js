import Vue from 'vue'
import VueRouter from 'vue-router'
import SchedulerHome from '../views/SchedulerHome.vue'
import AbstractView from '../views/AbstractView.vue'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'Dashboard',
    component: SchedulerHome,
    icon: 'fa-solid fa-tachometer-alt'
  },  
  {
    path: '/tasks',
    name: 'Tasks',
    icon: 'fas fa-fw fa-cog',
    component: AbstractView,
    props: true,
    children: [
      {
        name: 'tasks-overview',
        path: '',
        component: () => import('../views/tasks/TasksOverview.vue')
      },
      {
        name: 'tasks-new',
        path: 'new',
        props: true,
        component: () => import('../views/tasks/NewTask.vue')        
      },
      {
        name: 'tasks-edit',
        path: ':id',
        props: true,
        component: () => import('../views/tasks/EditTask.vue')        
      },

    ]
  },
  {
    path: '/workblocks',
    name: 'Workblocks',
    icon: 'fas fa-fw fa-cog',
    component: AbstractView,
    props: true,
    children: [
      {
        name: 'workblocks-overview',
        path: '',
        component: () => import('../views/workblocks/WorkblocksOverview.vue'),

      },
      {
        name: 'workblocks-new',
        path: 'new',
        props: true,
        component: () => import('../views/workblocks/NewWorkblock.vue')        
      },
      {
        name: 'workblocks-edit',
        path: ':id',
        props: true,
        component: () => import('../views/workblocks/EditWorkblock.vue')        
      },
    ]
  },
  {
    path: '/history',
    name: 'History',
    icon: 'fas fa-history',
    component: AbstractView,
    props: true,
    children: [
      {
        name: 'history-overview',
        path: '',
        component: () => import('../views/history/HistoryOverview.vue'),

      },]
  },
  {
    path: '/insights',
    name: 'Insights',
    /*icon: 'fa-chart-line',*/
    component: AbstractView,
    props: true,
    children: [
      {
        name: 'insights-overview',
        path: '',
        component: () => import('../views/insights/insights-overview.vue'),
      },
      {
        name: 'screenshots-overview',
        path: 'screenshots',
        component: () => import('../views/insights/screenshots/screenshots-overview.vue'),
      },
      {
        name: 'log-viewer',
        path: 'log/:id/:page?',
        component: () => import('../views/insights/log/LogViewer.vue'),
        props: true
      },]
  }
]

const router = new VueRouter({
  routes
})

export default router;
export { routes };