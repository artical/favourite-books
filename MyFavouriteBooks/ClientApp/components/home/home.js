import Vue from 'vue';
import EventBus from '../../eventbus';
import Pagination from '../pagination/pagination.vue.html'

Vue.component('pagination', Pagination);

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
            currentPage: 1,
            isAdded: false,
            isChecked: false,
            query: ''
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
            //isbn = isbn.replace(/[^\dX]/gi, '');
            
            if (isbn.length == 10) {
                var chars = isbn.split('');
                if (chars[9].toUpperCase() == 'X') {
                    chars[9] = 10;
                }
                var sum = 0;
                for (let i = 0; i < chars.length; i++) {
                    sum += ((10 - i) * parseInt(chars[i]));
                }
                return (sum % 11 == 0);
            } else if (isbn.length == 13) {
                var chars = isbn.split('');
                var sum = 0;
                for (let i = 0; i < chars.length; i++) {
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
                        this.checkBook(this.book.ISBN)
                        this.isbn = ''
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
        checkBook: function (isbn) {
            this.isAdded = false;
            this.isChecked = false;
            if (this.$auth.check()) {
                this.$http.get('/api/user/books/' + isbn).
                    then(response => {
                        this.isChecked = true;
                        this.isAdded = response.data ? true : false
                    }).
                    catch(e => {
                        console.log(e)
                    })
            }
            else {
                this.isChecked = true;
            }
        },
        addBook: function () {
            this.log('add', this.book)
            this.$http.post('/api/user/books/', this.book).
                then(response => {
                    this.favBooks.unshift(this.book)
                    this.book = ''
                    this.totalBooks++;
                }).
                catch(e => {
                    console.log(e)
                    console.log(e.response.data)
                    if (e.response.data.error.message && e.response.data.error.message === 'already exists')
                        this.isAdded = true;
                })
        },
        removeBook: function (book) {
            this.log('remove', this.book)
            this.$http.delete('/api/user/books/' + book.ISBN).
                then(response => {
                    let i = this.favBooks.map(item => item.ISBN).indexOf(book.ISBN);
                    if (i > -1)
                        this.favBooks.splice(i, 1)
                    this.totalBooks--;
                    if (this.book.ISBN === book.ISBN)
                        this.isAdded = false
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
        fetchFavBookForPage: function (page) {
            this.currentPage = page;
            this.fetchFavBooks();
        },
        fetchFavBooks: function () {
            if (this.$auth.check()) {
                var options = {
                    params: {
                        page: this.currentPage,
                        per_page: this.perPage,
                        query: this.query
                    }
                }
                this.$http.get('/api/user/books/', options).
                    then(response => {
                        this.favBooks = response.data.books
                        this.totalBooks = response.data.total
                    }).
                    catch(e => {
                        console.log(e)
                        console.log(e.response.data)
                    })
            }
        },
        bookCover: function (coverThumb) {
            return coverThumb ? coverThumb : 'images/book.png'
        },
        containsBook: function (book) {
            return this.favBooks.includes(book)
        },
        searchFavBook: function () {
            this.log("searchName",this.query)
            this.currentPage = 1;
            this.fetchFavBooks();
        },
        reset: function () {
            this.query = '';
            this.currentPage = 1;
            this.fetchFavBooks();
        }
    },
    mounted: function() {
        this.fetchFavBooks(this.currentPage);
        EventBus.$on("login", function (payLoad) {
            this.fetchFavBooks
        });
        EventBus.$on("logout", function (payLoad) {
            this.favBooks = []
        });
    }
    
}