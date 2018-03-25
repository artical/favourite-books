import Vue from 'vue';
import EventBus from '../../eventbus';

export default {
    name: 'HomePage',
    data() {
        return {
            book: '',
            isbn: '9789000035526',
            favBooks: [],
            error: '',
            totalBooks: 0,
            perPage: 8,
            currentPage: 1
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
            this.log('search', this.isbn);
            this.book = '';
            this.error = '';
            if (this.isValidISBN(this.isbn)) {
                Vue.axios.get('https://www.booknomads.com/api/v0/isbn/' + this.isbn).
                    then(response => {
                        this.book = response.data
                        this.book.Authors = this.book.Authors ? this.book.Authors.map(function (item, index) { return item.Name }).join(): '';
                        this.book.Subjects = this.book.Subjects ? this.book.Subjects.join() : '';
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
            this.log('add', this.book)
            this.$http.post('/api/books/', this.book).
                then(response => {
                    this.favBooks.unshift(this.book)
                    this.book = ''
                }).
                catch(e => {
                    console.log(e)
                    console.log(e.response.data)
                })
        },
        removeBook: function (book) {
            this.log('remove', this.book)
            this.$http.delete('/api/books/' + book.ISBN).
                then(response => {
                    console.log(response.data)
                    this.favBooks.splice(this.favBooks.indexOf(book), 1);
                }).
                catch(e => {
                    console.log(e)
                    console.log(e.response.data)
                })
        },
        log: async function (type, details) {
            this.$http.post('/api/logger/', {
                timestamp: Date.now(),
                logType: type,
                details: JSON.stringify(details)
            }
            ).
                then(response => {
                    console.log('test ok')
                }).
                catch(e => {
                    console.log(e)
                    console.log(e.response.data)
                })
        },
        fetchFavBooks: function () {
            
            if (this.$auth.check()) {
                var options = {
                    params: {
                        page: this.page,
                        per_page: this.perPage
                    }
                }
                this.$http.get('/api/books/', options).
                    then(response => {
                        this.favBooks = response.data.books
                        this.totalBooks = response.data.total
                        this.currentPage = this.page
                    }).
                    catch(e => {
                        console.log(e)
                        console.log(e.response.data)
                    })
            }
        }
    },
    mounted: function() {
        this.fetchFavBooks(this.page);
        EventBus.$on("login", function (payLoad) {
            this.fetchFavBooks
        });
        EventBus.$on("logout", function (payLoad) {
            this.favBooks = []
        });
    }
    
}