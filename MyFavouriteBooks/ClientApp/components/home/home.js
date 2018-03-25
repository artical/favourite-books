import Vue from 'vue';

export default {
    name: 'HomePage',
    data() {
        return {
            credentials: {
                login: "kaka12",
                password: "12b33a1!"
            },
            book: undefined,
            isbn: '9789000035526',
            favBooks: [],
            error: ''
        }
    },
    computed: {
        validation: function () {
            return {
                name: !!this.newUser.name.trim(),
                email: emailRE.test(this.newUser.email)
            }
        },
        isValid: function () {
            var validation = this.validation
            return Object.keys(validation).every(function (key) {
                return validation[key]
            })
        }
    },
    methods: {
        isValidISBN: function (isbn) {
            isbn = isbn.replace(/[^\dX]/gi, '');
            if (isbn.length == 10) {
                var chars = isbn.split('');
                if (chars[9].toUpperCase() == 'X') {
                    chars[9] = 10;
                }
                var sum = 0;
                for (var i = 0; i < chars.length; i++) {
                    sum += ((10 - i) * parseInt(chars[i]));
                }
                return (sum % 11 == 0);
            } else if (isbn.length == 13) {
                var chars = isbn.split('');
                var sum = 0;
                for (var i = 0; i < chars.length; i++) {
                    if (i % 2 == 0) {
                        sum += parseInt(chars[i]);
                    } else {
                        sum += parseInt(chars[i]) * 3;
                    }
                }
                return (sum % 10 == 0);
            } else {
                return false;
            }
        },
        searchBook: function () {
            this.book = null;
            this.error = '';
            if (this.isValidISBN(this.isbn)) {
                Vue.axios.get('https://www.booknomads.com/api/v0/isbn/' + this.isbn).
                    then(response => {
                        this.book = response.data
                    }).
                    catch(e => {
                        console.log(e)
                        console.log(e.response.data.error)
                        this.error = e.response.data.error
                    })
            } else {
                this.error = 'ISBN is invalid';
                console.log(this.error)
            }
            
            
        },
        addBook: function () {
            var favBook = this.book;
            favBook.Authors = favBook.Authors.map(function (item, index) { return item.Name }).join();
            favBook.Subjects = favBook.Subjects.join();
            console.log(favBook)
            this.$http.post('/api/books/', favBook).
                then(response => {
                    console.log(response.data)
                    this.favBooks.unshift(this.book)
                }).
                catch(e => {
                    console.log(e)
                    console.log(e.response.data)
                })
        },
        login: function () {
            this.$auth.login({
                data: this.credentials, // Axios
                rememberMe: true,
                redirect: '/',
                fetchUser: true
            })
                .then(() => {
                    console.log('success ' + this.context);
                }, (res) => {
                    console.log('error ' + this.context);
                    this.error = res.data;
                });
        },
        register: function () {
            
            this.$auth.registerData = { url: '/api/account/register', method: 'POST' };
            var formData = new FormData();
            formData.append('name', this.credentials.login);
            formData.append('password', this.credentials.password);
            this.$auth.register({
                body: formData, // Vue-resoruce
                data: formData, // Axios
                autoLogin: true,
                rememberMe: true,
                success: function () {
                    console.log('success ' + this.context);
                },
                error: function (res) {
                    console.log('error ' + this.context);
                    this.error = res.data;
                }
            });
        }

    }
}