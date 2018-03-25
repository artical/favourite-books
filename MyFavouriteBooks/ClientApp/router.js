import Vue from 'vue'
import VueRouter from 'vue-router'

import { routes } from './routes'

Vue.use(VueRouter);

let router = new VueRouter({
    mode: 'history',
    hashbang: false,
    linkActiveClass: 'active',
    base: __dirname,
    routes
})

export default router