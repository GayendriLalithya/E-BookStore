// scripts.js

$(document).ready(function () {
    // Load spotlight books
    $.ajax({
        url: '/api/books/spotlight',
        method: 'GET',
        success: function (data) {
            let spotlightBooks = '';
            data.forEach(book => {
                spotlightBooks += `
                    <div class="col-md-3">
                        <div class="book-card">
                            <img src="${book.coverImage}" alt="${book.title}">
                            <div class="book-card-body">
                                <h5>${book.title}</h5>
                                <p>by ${book.author}</p>
                                <p>$${book.price}</p>
                            </div>
                        </div>
                    </div>
                `;
            });
            $('#spotlight-books').html(spotlightBooks);
        }
    });

    // Load new arrival books
    $.ajax({
        url: '/api/books/newarrivals',
        method: 'GET',
        success: function (data) {
            let newArrivalBooks = '';
            data.forEach(book => {
                newArrivalBooks += `
                    <div class="col-md-3">
                        <div class="book-card">
                            <img src="${book.coverImage}" alt="${book.title}">
                            <div class="book-card-body">
                                <h5>${book.title}</h5>
                                <p>by ${book.author}</p>
                                <p>$${book.price}</p>
                            </div>
                        </div>
                    </div>
                `;
            });
            $('#new-arrival-books').html(newArrivalBooks);
        }
    });

    // Load filtered books
    $.ajax({
        url: '/api/books',
        method: 'GET',
        success: function (data) {
            let filteredBooks = '';
            data.forEach(book => {
                filteredBooks += `
                    <div class="col-md-3">
                        <div class="book-card">
                            <img src="${book.coverImage}" alt="${book.title}">
                            <div class="book-card-body">
                                <h5>${book.title}</h5>
                                <p>by ${book.author}</p>
                                <p>$${book.price}</p>
                            </div>
                        </div>
                    </div>
                `;
            });
            $('#filtered-books').html(filteredBooks);
        }
    });
});
