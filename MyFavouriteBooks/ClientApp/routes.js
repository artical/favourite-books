import HomePage from './components/home/home.vue.html'
import LoginPage from './components/auth/Login.vue.html'
import RegisterPage from './components/auth/Register.vue.html'

export const routes = [
    {
        path: '/',
        name: 'default',
        component: HomePage//require('./components/home/home.vue.html').default
    }, {
        path: '/login',
        name: 'login',
        component: LoginPage,
        meta: { auth: false }
    },  {
        path: '/register',
        name: 'register',
        component: RegisterPage,
        meta: { auth: false }
    }
]