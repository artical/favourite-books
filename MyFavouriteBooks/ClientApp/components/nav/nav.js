import Vue from 'vue';
import { error } from 'util';
export default {
    data() {
        return {
            credentials: {
                username: "kaka12",
                password: "12b33a1!"
            },
            error: ''
        }
    },
    methods: {
        login: function (evt) {
            evt.preventDefault();
            if (!this.credentials.username || !this.credentials.password) {
                this.error = 'Please enter your username and password'
            } else {
                this.handleLogin()
            }
            
        },
        handleLogin: function () {
            this.$auth.login({
                data: this.credentials,
                rememberMe: true,
                redirect: '/',
                fetchUser: true
            })
                .then(() => {
                    this.$refs.modalL.hide()
                }, (error) => {
                    this.error = error.response.data;
                });
        },
        register: function (evt) {
            evt.preventDefault();
            if (!this.credentials.username || !this.credentials.password) {
                this.error = 'Please enter your username and password'
            } else {
                this.handleRegister()
            }
        },
        handleRegister: function () {
            var formData = new FormData();
            formData.append('username', this.credentials.username);
            formData.append('password', this.credentials.password);
            this.$auth.register({
                data: formData, // Axios
                autoLogin: true,
                rememberMe: true,
                success: function () {
                    this.$refs.modalR.hide()
                },
                error: function (error) {
                    console.log(error)
                    this.error = error.response.data.error;
                }
            });
        },
        logout: function () {
            this.$auth.logout({
                redirect: { name: 'default' }
            });
        },
        clear: function () {

        },
        clearName: function() {
            this.credentials.username = '';
            this.credentials.password = '';
            this.error = '';
        }
    }
}