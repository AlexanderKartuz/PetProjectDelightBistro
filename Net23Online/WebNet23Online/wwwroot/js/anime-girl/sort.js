$(document).ready(function () {

    $('.girl-row.header .girl-cell a').click(function (evt) {
        evt.preventDefault();

        const newFieldForSortBy = $(this).attr('data-sort-by');

        const searchParams = new URLSearchParams(window.location.search)
        const currentFieldForSort = searchParams.get('sortBy');
        const sortDirection = searchParams.get('direction');
        const direction =
            currentFieldForSort == newFieldForSortBy
                && sortDirection == 'asc'
                ? 'desc'
                : 'asc';

        const sortType = $('[name=filter]').val();
        const valueForFilter = $('[name=valueForFilter]').val();

        const sortQuery = !sortType
            ? ""
            : `&sortType=${sortType}&sortValue=${valueForFilter}`;

        const newUrl = `${window.location.pathname}?sortBy=${newFieldForSortBy}&direction=${direction}${sortQuery}`;

        window.location = newUrl;
    });

});