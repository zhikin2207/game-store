(function($) {
	jQuery.fn.languageControl = function(options) {
		options = $.extend({
			targetDataAttr: 'lang-toggle',
			parentDataAttr: 'parent-toggle'
		}, options);

		function make() {
			$(this).change(checkLanguage);
			checkLanguage.call($(this));

			function checkLanguage() {
				var langEnabled = $(this).prop('checked');
				var $parent = $(this).parents('*[data-' + options.parentDataAttr + ']');
				var $inputs = $parent.find('input[data-' + options.targetDataAttr + ']');

				$inputs.prop('disabled', !langEnabled);

				if (!langEnabled) {
					$parent.find('span.field-validation-error').removeClass('field-validation-error').addClass('field-validation-valid');
					$inputs.removeClass('input-validation-error').addClass('input-validation-valid');
				}
			}
		}

		return this.each(make);
	};
})(jQuery);