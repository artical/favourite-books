import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'

import Vue from 'vue';
//import VueRouter from 'vue-router';
import BootstrapVue from 'bootstrap-vue'
import axios from 'axios'
import VueAxios from 'vue-axios'
import VueAuth from '@websanova/vue-auth'
import router from './router'
import App from './components/app/app.vue.html'
import HomePage from './components/home/home.vue.html'
//Vue.use(VueRouter);
Vue.use(BootstrapVue);
Vue.use(VueAxios, axios)
Vue.router = router


Vue.use(VueAuth, {
    auth: require('@websanova/vue-auth/drivers/auth/bearer.js'),
    http: require('@websanova/vue-auth/drivers/http/axios.1.x.js'),
    router: require('@websanova/vue-auth/drivers/router/vue-router.2.x.js'),
    registerData: { url: 'api/account/register', method: 'POST', redirect: '/' },
    loginData: { url: 'api/account/token', method: 'POST', redirect: '/' },
    tokenStore: ['localStorage'],
    token: [{ name: 'token', authType: 'bearer', foundIn: 'response' }]
});

var bus = new Vue();

new Vue({
    el: '#app-root',
    router: router,
    render: h => h(App)
});