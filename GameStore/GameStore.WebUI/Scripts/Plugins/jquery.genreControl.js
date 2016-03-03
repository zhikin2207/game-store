(function ($) {
	jQuery.fn.genreControl = function (options) {
		options = $.extend({
			allGenresListAttr: 'all-genres',
			selectedGenresListAttr: 'selected-genres',
			selectBtnAttr: 'select',
			removeBtnAttr: 'remove',
			selectAllBtnAttr: 'select-all',
			removeAllBtnAttr: 'remove-all'
		}, options);


		function moveGenreOptions($to, $options) {
			$to.append($options);
		}

		function make() {
			var $allGenresList = $('select[data-' + options.allGenresListAttr + ']');
			var $selectedGenresList = $('select[data-' + options.selectedGenresListAttr + ']');

			// select genres
			$('button[data-' + options.selectBtnAttr + ']').click(function () {
				moveGenreOptions($selectedGenresList, $allGenresList.children('option:selected'));
				event.preventDefault();
			});

			// remove genres
			$('button[data-' + options.removeBtnAttr + ']').click(function () {
				moveGenreOptions($allGenresList, $selectedGenresList.children('option:selected'));
				event.preventDefault();
			});

			// select all genres
			$('button[data-' + options.selectAllBtnAttr + ']').click(function () {
				moveGenreOptions($selectedGenresList, $allGenresList.children('option'));
				event.preventDefault();
			});

			// remove all genres
			$('button[data-' + options.removeAllBtnAttr + ']').click(function () {
				moveGenreOptions($allGenresList, $selectedGenresList.children('option'));
				event.preventDefault();
			});
		}

		return this.each(make);
	};
})(jQuery);