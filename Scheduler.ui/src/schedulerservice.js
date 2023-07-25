import axios from 'axios';

const schedulerservice = axios.create({
    baseURL: process.env.VUE_APP_SCHEDULER_API,
    timeout: 8000,
});

schedulerservice.baseURL = process.env.VUE_APP_SCHEDULER_API;

export default schedulerservice;