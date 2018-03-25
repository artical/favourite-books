//import CounterExample from 'components/counter-example'
//import FetchData from 'components/fetch-data'
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


    //{ path: '/', component: HomePage, display: 'Home', style: 'glyphicon glyphicon-home' },
    //{ path: '/counter', component: CounterExample, display: 'Counter', style: 'glyphicon glyphicon-education' },
    //{ path: '/fetch-data', component: FetchData, display: 'Fetch data', style: 'glyphicon glyphicon-th-list' }
]