$(document).ready(function () {

    scrollToHeroesSection();
    init();

    $('article.media-card').click(function () {
        const self = $(this);

        // bad way to do it
        // $(this).css('border', '3px red solid'); //inline style

        // if (self.hasClass('active')) {
        //     self.removeClass('active');
        // } else {
        //     self.addClass('active');
        // }

        self.toggleClass('active');

        const activeCount = $('article.media-card.active').length;
        const atLeastOneItemForRemove = activeCount > 0;

        if (atLeastOneItemForRemove) {
            $('.section-heroes .remove-image').removeAttr('disabled');
        } else {
            $('.section-heroes .remove-image').attr('disabled', 'disabled');
        }

        window.dispatchEvent(new CustomEvent('animeGirlSelectionChanged', {
            detail: { count: activeCount }
        }));
    });

    $('.section-heroes .remove-image').click(function () {
        const ids = [];

        $('article.media-card.active').each((x, item) => {
            const id = $(item).attr('data-id')
            ids.push(id);
        });

        const idsStr = ids.join('&ids=');

        $('article.media-card.active').remove();

        const url = `/api/AnimeGirl/delete?ids=${idsStr}`;
        $.get(url);
    });

    $('.mode-view').click(function () {
        $(this).hide();
        const editBlock = $(this).parent().find('.mode-edit');
        const oldValue = $(this).text();
        editBlock.val(oldValue);
        editBlock.show();
    });

    $('.new-anime-name-input').on('keypress', function (e) {
        // 13 == Enter
        if (e.which == 13) {
            const newName = $(this).val();
            $(this).hide();
            const viewBlock = $(this).parent().find('.mode-view');
            viewBlock.show();

            const animeId = $(this)
                .closest('.anime-catalog-card')
                .attr('data-id');
            const url = `/api/anime/updateName?id=${animeId}&name=${newName}`;
            $.get(url)
                .done(function (answer) {
                    if (answer) {
                        viewBlock.text(newName);
                    }
                });
        }
    })

    $('.create-movie-button').click(function () {
        const requestUrl = `https://localhost:7142/CreateMovie`;
        const name = $('.movie-name-input').val();
        const url = $('.movie-url-input').val();

        const data = { name, url };

        $.ajax({
            type: 'POST',
            url: requestUrl,
            contentType: 'application/json',
            data: JSON.stringify(data)
        }).done(function (movie) {
            drawMovie(movie);
        });
    });

    function init() {
        const url = `https://localhost:7142/GetMovies`;
        $.get(url)
            .done(function (movies) {
                movies.forEach((movie) => {
                    drawMovie(movie);
                });
            });
    }

    function drawMovie(movie) {
        const movieContainer = $('.section-movie-catalog .anime-catalog-grid');
        const divForMovie = $('.section-movie-catalog .anime-catalog-card.template').clone();
        divForMovie.removeClass('template');
        divForMovie.find('.anime-catalog-card__title').text(movie.name);
        divForMovie.find('.anime-catalog-card__cover img').attr('src', movie.url);
        movieContainer.append(divForMovie);
    }

    function scrollToHeroesSection() {
        const params = new URLSearchParams(window.location.search);
        const hasPaginationParams = params.has('page') || params.has('pageSize');
        const hasHeroesHash = window.location.hash === '#heroes';

        if (!hasPaginationParams && !hasHeroesHash) {
            return;
        }

        const heroesSection = document.getElementById('heroes');
        if (heroesSection) {
            heroesSection.scrollIntoView({ behavior: 'smooth', block: 'start' });
        }
    }

});